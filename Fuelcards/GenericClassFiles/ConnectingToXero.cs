using PortlandXeroLib;
using System.Net.Http;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace Fuelcards.GenericClassFiles
{
    public class ConnectingToXero
    {
        public static async Task GetFuelcardCustomers(List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> PFLXeroContacts, List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> FTCXeroContacts,XeroConnector xero)
        {
            string FuelcardGroupId = "13cfb40d-5427-4b0d-8c9c-1ea4ff6c4c79";
            string FuelgenieGroupId = "7e3f263a-caec-4c34-8737-12462e74a298";
            //HttpClient client = new();
            //var xero = new XeroConnector(client);
            //await xero.ConnectToXero();
            await xero.GetTokens();
            await xero.GetTenantsAsync();
            List<string> XeroIds = new();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    XeroIds = await xero.GetContactsInGroup(FuelcardGroupId, i, PFLXeroContacts,XeroIds);
                    await xero.ContactAddresses(i, PFLXeroContacts, XeroIds);
                    XeroIds = new();
                }

                if (i == 1)
                {
                    XeroIds = await xero.GetContactsInGroup(FuelgenieGroupId, i, FTCXeroContacts, XeroIds);
                    await xero.ContactAddresses(i, FTCXeroContacts, XeroIds);
                    XeroIds = new();
                }
            }
        }
    }
}
