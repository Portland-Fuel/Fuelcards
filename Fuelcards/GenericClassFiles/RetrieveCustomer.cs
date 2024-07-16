using DataAccess.Cdata;
using DataAccess.Fuelcards;
using Fuelcards.Controllers;
using Fuelcards.Repositories;
using Microsoft.Graph.TermStore;
using RestSharp;

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
        public CustomerModel GetCustomerInformation(string xeroId)
        {
            var XeroCustomer = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == xeroId).FirstOrDefault();
            if (XeroCustomer == null) throw new ArgumentException("Xero ID passed in from the view does not match a customer in the list produced from xero");
            CustomerModel Customers = new()
            {
                xeroID = ValidateXeroId(XeroCustomer.ContactID.ToString()),
                name = ValidateCustomerName(XeroCustomer.Name, XeroCustomer.ContactID.ToString()),
                portlandId = ValidatePortlandId(xeroId),
            };
            Customers.allAddons = GetAddon(Customers.portlandId);
            Customers.paymentTerms = ValidatePaymentTerms(Customers.xeroID, Customers.name);
            Customers = getAllFixedData(Customers);
            return Customers;
        }

        private CustomerModel getAllFixedData(CustomerModel customers)
        {
            List<FixedPriceContract>? contracts = _db.AllFixContracts(customers.portlandId);
            if (contracts is null || contracts.Count == 0) return customers;
            customers.keyfuelsFixedData = new();
            customers.ukfuelFixedData = new();
            customers.texacoFixedData = new();
            customers.fuelgenieFixedData = new();
            customers.keyfuelsFixedData = contracts.Where(e => e.Network is not null && e.Network[0] == 0).ToList();
            customers.ukfuelFixedData = contracts.Where(e => e.Network is not null && e.Network[0] == 1).ToList();
            customers.texacoFixedData = contracts.Where(e => e.Network is not null && e.Network[0] == 2).ToList();
            customers.fuelgenieFixedData = contracts.Where(e => e.Network is not null && e.Network[0] == 3).ToList();
            return customers;
        }

        private int ValidatePaymentTerms(string xeroID, string name)
        {
            int? paymentTerms = _db.GetPaymentTerms(xeroID);
            if (paymentTerms != null) return (int)paymentTerms;
            throw new ArgumentException($"No Payment Terms have been found for the following customer {name}");
        }

        private List<HistoricAddon> GetAddon(int portlandId)
        {
            List<CustomerPricingAddon>? AllAddons = _db.GetListOfAddonsForCustomer(portlandId);
            List<HistoricAddon> historicAddons = new();
            if (AllAddons != null && AllAddons.Count > 0)
            {
                foreach (var addon in AllAddons)
                {
                    HistoricAddon model = new()
                    {
                        effectiveDate = addon.EffectiveDate,
                        Addon = addon.Addon,
                        network = EnumHelper.NetworkEnumFromInt(addon.Network).ToString(),
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
        public List<HistoricAddon>? allAddons { get; set; }
        public int? paymentTerms { get; set; }
        public List<FixedPriceContract> keyfuelsFixedData { get; set; }
        public List<FixedPriceContract> ukfuelFixedData { get; set; }
        public List<FixedPriceContract> texacoFixedData { get; set; }
        public List<FixedPriceContract> fuelgenieFixedData { get; set; }
    }

    public struct CustomerList
    {
        public string? Name { get; set; }
        public string? xeroId { get; set; }
    }

    public struct HistoricAddon
    {
        public string network { get; set; }
        public double? Addon { get; set; }
        public DateOnly? effectiveDate { get; set; }
    }
}
