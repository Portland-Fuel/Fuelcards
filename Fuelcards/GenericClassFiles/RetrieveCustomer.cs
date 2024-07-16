using DataAccess.Cdata;
using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.Repositories;
using Microsoft.Graph.TermStore;
using RestSharp;
using System.Runtime.CompilerServices;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Model.Accounting;

namespace Fuelcards.GenericClassFiles
{
    public class RetrieveCustomer
    {
        private readonly IQueriesRepository _db;

        public RetrieveCustomer(IQueriesRepository db)
        {
            _db = db;
        }

        public List<CustomerList> CustomerDetailsLoadData()
        {
            List<CustomerList> customers = new();
            foreach (var item in HomeController.PFLXeroCustomersData)
            {
                CustomerList model = new()
                {
                    Name = item.Name,
                    xeroId = item.ContactID.ToString(),
                };
                customers.Add(model);
            }
            return customers;
        }

        private string GenerateAddress(Xero.NetStandard.OAuth2.Model.Accounting.Contact? item)
        {
            if (item is null) throw new ArgumentException("Xero Id was null therefore no address can be found");
                string? AddressLine = item.Addresses[0].AddressLine1;
                string? AddressLine2 = item.Addresses[0].AddressLine2;
                string? AddressLine3 = item.Addresses[0].AddressLine3;
                string? City = item.Addresses[0].City;
                string? Postcode = item.Addresses[0].PostalCode;
                string? Region = item.Addresses[0].Region;
                string address = $"{AddressLine},{AddressLine2},{AddressLine3},{City},{Postcode},{Region}";
                if (address == ",,,,," && item.Addresses.Count() > 1)
                {
                    AddressLine = item.Addresses[1].AddressLine1;
                    AddressLine2 = item.Addresses[1].AddressLine2;
                    AddressLine3 = item.Addresses[1].AddressLine3;
                    City = item.Addresses[1].City;
                    Postcode = item.Addresses[1].PostalCode;
                    Region = item.Addresses[1].Region;
                    address = $"{AddressLine},{AddressLine2},{AddressLine3},{City},{Postcode},{Region}";

                }
                AddressLine = null;
                City = null;
                Postcode = null;
                Region = null;

                if (item.Name == "Portland Fuel Ltd") item.EmailAddress = "info@portland-fuel.co.uk";
                if (address == ",,,,,")
                {
                    throw new Exception("Address could not be established.");
                }
            return address;
            }

        public CustomerModel GetCustomerInformation(string xeroId)
        {
            var XeroCustomer = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == xeroId).FirstOrDefault();
            if (XeroCustomer == null) throw new ArgumentException("Xero ID passed in from the view does not match a customer in the list produced from xero");
            CustomerModel Customers = new()
            {
                xeroID = ValidateXeroId(XeroCustomer.ContactID.ToString()),
                name = ValidateCustomerName(XeroCustomer.Name, XeroCustomer.ContactID.ToString()),
                portlandId = ValidatePortlandId(xeroId),
                address = GenerateAddress(HomeController.PFLXeroCustomersData.Where(e=>e.ContactID.ToString() == xeroId).FirstOrDefault()),
            };
            Customers.networks = new();
            int[]? Accounts = _db.GetAccounts(Customers.portlandId);
            foreach (var account in Accounts)
            {
                Network network = new();
                network.networkName = _db.getNetworkFromAccount(account);
                network.allAddons = GetAddon(Customers.portlandId, network.networkName);
                network.paymentTerms = ValidatePaymentTerms(Customers.xeroID, Customers.name);
                network.Fixed = getAllFixedData(account);
                network.email = _db.AllEmail(account);
                network.account = account;
                Customers.networks.Add(network);
            }
            return Customers;
        }

        

        private List<FixedPriceContract>? getAllFixedData(int account)
        {
            List<FixedPriceContract>? contracts = _db.AllFixContracts(account);
            if (contracts is null || contracts.Count == 0) return null;
            return contracts.ToList();
        }

        private int ValidatePaymentTerms(string xeroID, string name)
        {
            int? paymentTerms = _db.GetPaymentTerms(xeroID);
            if (paymentTerms != null) return (int)paymentTerms;
            throw new ArgumentException($"No Payment Terms have been found for the following customer {name}");
        }
        private List<HistoricAddon> GetAddon(int portlandId, EnumHelper.Network network)
        {
            List<CustomerPricingAddon>? AllAddons = _db.GetListOfAddonsForCustomer(portlandId, network);
            List<HistoricAddon> historicAddons = new();
            if (AllAddons != null && AllAddons.Count > 0)
            {
                foreach (var addon in AllAddons)
                {
                    HistoricAddon model = new()
                    {
                        effectiveDate = addon.EffectiveDate,
                        Addon = addon.Addon,
                    };
                    historicAddons.Add(model);
                }
            }
            historicAddons = historicAddons.OrderByDescending(e => e.effectiveDate).ToList();
            return historicAddons;
        }

        private int ValidatePortlandId(string xeroId)
        {
            int? PortlandId = _db.GetPortlandIdFromXeroId(xeroId);
            if (PortlandId == null) throw new ArgumentException($"Portland ID cannot be null for XeroID = {xeroId}");
            return (int)PortlandId;
        }

        private string ValidateCustomerName(string? name, string? id)
        {
            if (name is null) throw new ArgumentNullException($"Error setting the customer name. Name was null. Xero ID Was {id}");
            return name;
        }

        private string ValidateXeroId(string? xeroID)
        {
            if (xeroID is null) throw new ArgumentNullException("Error setting the xeroID name. Name was null. Xero ID Was");
            return xeroID;
        }
    }

    public struct CustomerModel
    {
        // IF ANYONE TOUCHES THIS WITHOUT INFORMING CHUCKLES. BIGGGG TROUBLE
        public string? name { get; set; }
        public string? xeroID { get; set; }
        public int portlandId { get; set; }
        public string? address { get; set; }
        public List<Network> networks { get; set; }
        
    }
    public class Network
    {
        public EnumHelper.Network networkName { get; set; }
        public int account { get; set; }
        public List<HistoricAddon>? allAddons { get; set; }
        public List<FixedPriceContract>? Fixed { get; set; }
        public int? paymentTerms { get; set; }
        public Email email { get; set; }

    }
    public struct CustomerList
    {
        public string? Name { get; set; }
        public string? xeroId { get; set; }
    }

    public struct HistoricAddon
    {
        public double? Addon { get; set; }
        public DateOnly? effectiveDate { get; set; }
    }
    public  struct Email
    {
        public string? To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
    }
}
