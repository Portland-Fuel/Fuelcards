using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;

namespace Fuelcards.Models
{
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
        public string? networkName { get; set; }
        public int account { get; set; }
        public List<HistoricAddon>? allAddons { get; set; }
        public List<FixedPriceContract>? Fixed { get; set; }
        public int? paymentTerms { get; set; }
        public Email email { get; set; }
        public EnumHelper.InvoiceFormatType? invoiceFormatType { get; set; } 

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
    public struct Email
    {
        public string? To { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
    }
}
