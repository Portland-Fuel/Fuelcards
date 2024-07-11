using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.NetStandard.OAuth2.Model.Accounting;

namespace PortlandXeroLib
{
    public class ContactsReturns : XeroCallReturns
    {
        public List<Contact> Contacts { get; set; }
    }

    public class XeroCallReturns 
    {
        public string Id { get; set; }
        public string? Status { get; set; }
        public required string ProviderName { get; set; }
        public DateTime DateTimeUTC { get; set; }
    }
}
