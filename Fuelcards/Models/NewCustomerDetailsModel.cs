namespace Fuelcards.Models
{
    public class NewCustomerDetailsModel
    {
        public class AddEditCustomerFormData
        {
            //Do not touch this class with a 10ft pole it is used in js and is strongly typed in js so if you change it will break
            public string? customerName { get; set; }
            public bool? isUpdateCustomer { get; set; }
            public NetworkInfo? keyFuelsInfo { get; set; }
            public NetworkInfo? texacoInfo { get; set; }
            public NetworkInfo? uKFuelsInfo { get; set; }
            public NetworkInfo? fuelGenieInfo { get; set; }


        }

        public class NetworkInfo
        {
            //Do not touch this class with a 10ft pole it is used in js and is strongly typed in js so if you change it will break
            public List<Fix>? newFixesForCustomer { get; set; }
            public List<AccountInfo>? newAccountInfo { get; set; }
            public List<AddonData>? newAddons { get; set; }
        }
        public class AddonData
        {
            public string? addon { get; set; }
            public string? effectiveFrom { get; set; }
        }
        public class AccountInfo
        {
            public string? account { get; set; }
            public string? toEmail { get; set; }
            public string? ccEmail { get; set; }
            public string? BccEmail { get; set; }
            public string? paymentTerm { get; set; }
            public string? invoiceFormatType { get; set; }


        }
        public class Fix
        {
            public string? selectedNetwork { get; set; }
            public string? account { get; set; }
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
