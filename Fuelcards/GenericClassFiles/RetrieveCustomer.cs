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
        private static IQueriesRepository _db;
        public RetrieveCustomer(IQueriesRepository db)
        {
            _db = db;
        }
        public static List<CustomerList> CustomerDetailsLoadData()
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
        public static CustomerModel GetCustomerInformation(string xeroId)
        {
            var XeroCustomer = HomeController.PFLXeroCustomersData.Where(e => e.ContactID.ToString() == xeroId).FirstOrDefault();
            if (XeroCustomer == null) throw new ArgumentException("Xero ID passed in from the view does not match a customer in the list produced from xero");

            CustomerModel Customers = new()
            {
                xeroID = ValidateXeroId(XeroCustomer.ContactID.ToString()),
                name = ValidateCustomerName(XeroCustomer.Name, XeroCustomer.ContactID.ToString()),
                portlandId = ValidatePortlandId(xeroId)
            };
            return Customers;
        }

        private static int ValidatePortlandId(string xeroId)
        {
            int? PortlandId = _db.GetPortlandIdFromXeroId(xeroId);
            if (PortlandId == null) throw new ArgumentException($"Portland ID cannot be null for XeroID = {xeroId}");
            return (int)PortlandId;
        }

        private static string ValidateCustomerName(string? name, string? id)
        {
            if (name is null) throw new ArgumentNullException($"Error setting the customer name. Name was null. Xero ID Was {id}");
            return name;
        }
        private static string ValidateXeroId(string? xeroID)
        {
            if (xeroID is null) throw new ArgumentNullException("Error setting the xeroID name. Name was null. Xero ID Was");
            return xeroID;
        }

    }
    public struct CustomerModel
    {
        public string? name { get; set; }
        public string? xeroID { get; set; }
        public int portlandId { get; set; }
        public HistoricAddon? allAddons { get; set; }

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
        public DateOnly effectiveDate { get; set; }
    }
}
