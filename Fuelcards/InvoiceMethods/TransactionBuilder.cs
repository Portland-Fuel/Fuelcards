using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;

namespace Fuelcards.InvoiceMethods
{
    public class TransactionBuilder
    {
        private readonly List<SiteNumberToBand> _sites;
        public TransactionBuilder(List<SiteNumberToBand> sites)
        {
                _sites = sites;
        }
        public string getSiteName(int? siteCode, EnumHelper.Network network)
        {
            if (siteCode == null) throw new ArgumentNullException("The site code should not be null. now that it is - This needs to be coded...");
            string? name = _sites.Where(e => e.SiteNumber == siteCode && e.NetworkId == (int)network && e.Active != false).FirstOrDefault()?.Name;
            return name;
        }
    }
}
