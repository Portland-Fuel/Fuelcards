using System;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Model;
using System.Threading.Tasks;
using static PortlandXeroLib.Constants.XeroUrls;
using Xero.NetStandard.OAuth2.Model.PayrollAu;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xero.NetStandard.OAuth2.Models;
using Xero.NetStandard.OAuth2.Model.Accounting;
using PortlandCredentials;
using System.Collections;
using Microsoft.Extensions.Configuration;
namespace PortlandXeroLib
{
    public class XeroConnector
    {
        private readonly VerifierCreator _verifier;
        private readonly string _state;
        private string _scopes;
        private readonly string _clientId;
        private readonly string _redirectUri;
        public static ReturnedAuthCodes _authCodes;
        public XeroTokens _tokens;
        private HttpClient _client;
        private List<Tenant>? _tenants;
        public static bool listnercompleted;
        //private IConfiguration _configuration;
        static XeroConnector()
        {
            _authCodes = new ReturnedAuthCodes();
        }
        public XeroConnector(HttpClient Client)
        {
            _clientId = "2B3A74ECAC7544F3AF513DC912933DE5";
            //_configuration = configuration;
            //_redirectUri = "https://localhost:44312/Fuelcard/Authorization/Callback";
            //_redirectUri = "https://fueltrading.uk/FuelCard/Authorization/Callback";
            //_redirectUri = "https://192.168.0.17:443/Fuelcard/Authorization/Callback";
            _redirectUri = GetRedirectUriFromAppSettings();
            _verifier = new VerifierCreator();
            _state = new VerifierCreator(25).Verifier;
            _scopes = GetScopes();
            _tokens = new();
            //_authCodes.PropertyChanged += AuthCodes_PropertyChanged;
            _client = Client;
        }

        private string? GetRedirectUriFromAppSettings()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                return "https://localhost:7110/Authorization/Callback";
            }
            else
            {
                return "https://192.168.0.17:443/Authorization/Callback";
            }
        }

        public async Task<string> GetLinkAndOpen()
        {
            Console.WriteLine("Connecrting to Xero now...");
            string scopes = Uri.EscapeDataString(_scopes);
            string authLink = $"{AuthorisationUrl}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope={scopes}&state={_state}&code_challenge={_verifier.CodeChallenge}&code_challenge_method=S256";
            return authLink;
        }
        public async Task ConnectToXero()
        {
            Console.WriteLine("Connecrting to Xero now...");
            string scopes = Uri.EscapeDataString(_scopes);
            string authLink = $"{AuthorisationUrl}?response_type=code&client_id={_clientId}&redirect_uri={_redirectUri}&scope={scopes}&state={_state}&code_challenge={_verifier.CodeChallenge}&code_challenge_method=S256";
            //open web browser with the link generated

            OpenBrowser(authLink);

            //start webserver to listen for the callback
            Console.WriteLine("Callback listening started...");
            await LoginListenerAsync(_authCodes);

        }

        public async Task GetTokens()
        {
            if (await GetTokensAsync())
            {
                Console.WriteLine($"\nID token = {_tokens.Id}\n\nAccess Token : {_tokens.Access}\n\nRefresh Token : {_tokens.Refresh}\n");
            }
            else
            {
                Console.WriteLine("There was a problem retrieving the tokens");
            }
        }

        private async Task<bool> GetTokensAsync()
        {
            //exchange the code for a set of tokens
            const string url = "https://identity.xero.com/connect/token";

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("code", _authCodes.AuthorisationCode),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri),
                new KeyValuePair<string, string>("code_verifier", _verifier.Verifier),
            });

            var response = await _client.PostAsync(url, formContent);

            //read the response and populate the boxes for each token
            //could also parse the expiry here if required
            var content = await response.Content.ReadAsStringAsync();
            var tokens = JObject.Parse(content);

            try
            {
                _tokens.Id = tokens["id_token"].ToString();
                _tokens.Access = tokens["access_token"].ToString();
                _tokens.Refresh = tokens["refresh_token"].ToString();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task GetTenantsAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.Access);

            var response = await _client.GetAsync(ConnectionsUrl);
            var content = await response.Content.ReadAsStringAsync();

            //fill the dropdown box based on the results 
            var tenants = JsonConvert.DeserializeObject<List<Tenant>>(content);

            if (tenants is not null) _tenants = tenants;
            foreach (var tenant in _tenants)
            {
                Console.WriteLine($"Tenant Name : {tenant.TenantName}\n\nID : {tenant.TenantId}\n");
            }

        }

        public async Task<string> CallXeroAsync(string builder)
        {
            int tenant = ScratchPad.PickTenant(_tenants);
            var url = $"{XeroApiUrlBase}/{builder}";
            _client.DefaultRequestHeaders.Remove("xero-tenant-id");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.Access);
            _client.DefaultRequestHeaders.Add("xero-tenant-id", tenant.ToString());
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;

        }
        public async Task ContactAddresses(int Tenant, List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> XeroContacts, List<string> XeroId)
        {
            string Appender = "?IDs=";
            int i = 0;
            foreach (string Id in XeroId)
            {

                if (i == 50)
                {
                    await RunRequest(Appender, Tenant, XeroContacts);
                    Appender = "?IDs=";
                    i = 0;
                }
                Appender = Appender + Id + ",";
                i++;
            }
            if (i > 1) await RunRequest(Appender, Tenant, XeroContacts);

        }
        public async Task RunRequest(string Appender, int Tenant, List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> XeroContacts)
        {

            var url = $"{XeroApiUrlBase}/contacts/{Appender.Substring(0, Appender.Length - 1)}";
            _client.DefaultRequestHeaders.Remove("xero-tenant-id");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.Access);
            _client.DefaultRequestHeaders.Add("xero-tenant-id", _tenants[Tenant].TenantId.ToString());

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var IndividualContact = JsonConvert.DeserializeObject<Contacts>(content);
            foreach (var contact in IndividualContact._Contacts)
            {
                XeroContacts.Add(contact);
            }
        }


        public async Task<List<string>> GetContactsInGroup(string id, int tenant, List<Xero.NetStandard.OAuth2.Model.Accounting.Contact> XeroContacts, List<string> XeroIds)
        {
            var url = $"{XeroApiUrlBase}/ContactGroup/{id}";
            _client.DefaultRequestHeaders.Remove("xero-tenant-id");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.Access);
            _client.DefaultRequestHeaders.Add("xero-tenant-id", _tenants[tenant].TenantId.ToString());

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var Groups = JsonConvert.DeserializeObject<ContactGroups>(content);

            foreach (var item in Groups._ContactGroups)
            {

                foreach (var item2 in item.Contacts.OrderBy(e => e.Name))
                {
                    
                    if (item2.ContactID.ToString() != "cb369ae9-2361-41e8-b9ec-c33715fe76be" && item2.ContactID.ToString() != "418a8417-0451-478d-86e1-9cbcd6f55513")
                    {
                        XeroIds.Add(item2.ContactID.ToString());
                        string? EmailAddress = item2.EmailAddress;
                    }
                }
            }
            return XeroIds;
        }

        private void AuthCodes_PropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            //if(e is not null) Console.WriteLine($"Property changed : {e.PropertyName}");

            // AuthorisationCode   State
        }

        private static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }

        }

        public static async Task LoginListenerAsync(ReturnedAuthCodes authCodes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            string[] prefixes = new string[] { "http://localhost:8888/" };

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");

            // Note: The GetContext method blocks while waiting for a request.

            //HttpListenerContext context = listener.GetContext();
            HttpListenerContext context = await listener.GetContextAsync();


            //HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;


            response.ContentType = "text/html";

            //Write it to the response stream
            var query = context.Request.Url.Query;
            var code = "";
            var state = "";

            if (query.Contains("?"))
            {
                query = query.Substring(query.IndexOf('?') + 1);
            }

            foreach (var vp in Regex.Split(query, "&"))
            {
                var singlePair = Regex.Split(vp, "=");

                if (singlePair.Length == 2)
                {
                    if (singlePair[0] == "code")
                    {
                        code = singlePair[1];
                        authCodes.AuthorisationCode = code;
                    }

                    if (singlePair[0] == "state")
                    {
                        state = singlePair[1];
                        authCodes.State = state;
                    }
                }
            }

            var buffer = Encoding.UTF8.GetBytes("All done you can close this window now");
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);

            listener.Stop();
        }

        static string GetScopes()
        {
            return "offline_access openid profile email files accounting.transactions accounting.settings accounting.contacts";
            //return "openid profile email offline_access files accounting.transactions accounting.contacts";
        }
    }

}
