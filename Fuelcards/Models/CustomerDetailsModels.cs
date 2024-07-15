using Fuelcards.GenericClassFiles;

namespace Fuelcards.Models
{
    public class CustomerDetailsModels
    {
        public List<CustomerList>? CustomerLists { get; set; }



        public class AddEditCustomerFormData
        {
            //Do not touch this class with a 10ft pole it is used in js and is strongly typed in js so if you change it will break
            public string? customerName { get; set; }   
            public string? emailTo { get; set; }
            public string? emailCc { get; set; }
            public string? emailBcc { get; set; }
            public CheckBoxInfo? keyFuelsInfo { get; set; }
            public CheckBoxInfo? texacoInfo { get; set; }
            public CheckBoxInfo? uKFuelsInfo { get; set; }
            public CheckBoxInfo? fuelGenieInfo { get; set; }
             
            public string? invoiceOrderType { get; set; }
            public string? paymentTerm { get; set; }


        }



        public class CheckBoxInfo
        {
            //Do not touch this class with a 10ft pole it is used in js and is strongly typed in js so if you change it will break
            public List<NewFix>? NewFixesForCustomer  { get; set; }
            public string? accountNumber { get; set; }
            public string? addon { get; set; }
            public string? date { get; set; }
        }

        public class NewFix
        {
            public string? selectedNetwork { get; set; }
            public string? effectiveFrom { get; set; }
            public string? endDate { get; set; }
            public string? period { get; set; }
            public string? grade { get; set; }
            public string? fixedPrice { get; set; }
            public string? fixedPriceIncDuty { get; set; }
            public string? fixedVolume { get; set; }
            public string? periods { get; set; }
            public string? tradeReference { get; set; }
        }

    }
}
