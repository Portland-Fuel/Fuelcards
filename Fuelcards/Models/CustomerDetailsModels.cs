using Fuelcards.GenericClassFiles;

namespace Fuelcards.Models
{
    public class CustomerDetailsModels
    {
        public List<CustomerList>? CustomerLists { get; set; }



        public class AddEditCustomerFormData
        {
            public string? customerName { get; set; }   
            public string? emailTo { get; set; }
            public string? emailCc { get; set; }
            public string? emailBcc { get; set; }

        }
    }
}
