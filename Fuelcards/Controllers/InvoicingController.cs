using DataAccess.Fuelcards;
using Fuelcards.CustomExceptions;
using Fuelcards.GenericClassFiles;
using Fuelcards.InvoiceMethods;
using Fuelcards.Models;
using Fuelcards.Repositories;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.SecurityNamespace;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using MigraDoc.DocumentObjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Xero.NetStandard.OAuth2.Model.Accounting;
using DataAccess.Cdata;
using System.Numerics;
using System.Threading;
using System.Transactions;
using Fuelcards.CustomExceptions;

namespace Fuelcards.Controllers
{
    public class InvoicingController : Controller
    {
        private readonly IQueriesRepository _db;
        private readonly List<SiteNumberToBand> _sites;
        public static List<InvoicePDFModel> invoices = new();
        public static InvoiceReporter _report = new();
        public static List<InvoiceReport> reportList = new();
        public InvoicingController(IQueriesRepository db)
        {
            _db = db;
            _sites = _db.GetAllSiteInformation();
        }


        public IActionResult Invoicing()
        {
            // Return the view without the model, as the model will be fetched via AJAX if needed
            return View("/Views/Invoicing/RealInvoicing.cshtml");
        }
        [HttpGet]
        public async Task<JsonResult> GetInvoicePreCheckModel()
        {
            try
            {
                InvoicePreCheckModels checks = new();
                checks.InvoiceDate = Transactions.GetMostRecentMonday(DateOnly.FromDateTime(DateTime.Now.AddDays(-15)));
                checks.BasePrice = _db.GetBasePrice(checks.InvoiceDate);
                checks.PlattsPrice = checks.BasePrice - 52.95;
                checks.KeyfuelImports = _db.GetTotalEDIs(0);
                checks.UkfuelImports = _db.GetTotalEDIs(1);
                checks.TexacoImports = _db.GetTotalEDIs(2);
                try
                {
                    checks.KeyfuelsInvoiceList = await _db.GetCustomersToInvoice(0, checks.InvoiceDate, checks.BasePrice);
                    checks.UkFuelInvoiceList = await _db.GetCustomersToInvoice(1, checks.InvoiceDate, checks.BasePrice);
                    checks.TexacoInvoiceList = await _db.GetCustomersToInvoice(2, checks.InvoiceDate, checks.BasePrice);
                }
                catch (Exception e )
                {
                    throw new SiteErrorException($"There was a issue getting the network invoice list. Possibly due to a site error The issue was - {e}");
                }
                checks.FailedKeyfuelsSites = await _db.GetFailedSiteBanding(0);
                checks.FailedUkfuelSites = await _db.GetFailedSiteBanding(1);
                checks.FailedTexacoSites = await _db.GetFailedSiteBanding(2);
                checks.TexacoVolume = new();
                checks.TexacoVolume.DieselBand7 = _db.GetDieselBand7Texaco();
                checks.TexacoVolume.Unleaded = _db.GetProductVolume(EnumHelper.Products.ULSP);
                checks.TexacoVolume.Adblue = _db.GetProductVolume(EnumHelper.Products.Adblue);
                checks.TexacoVolume.SuperUnleaded = _db.GetProductVolume(EnumHelper.Products.SuperUnleaded);
                checks.TexacoVolume.Diesel = _db.GetProductVolume(EnumHelper.Products.Diesel);
                checks.KeyfuelsDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.Keyfuels);
                checks.UkFuelDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.UkFuel);
                checks.TexacoDuplicates = _db.CheckForDuplicateTransactions(EnumHelper.Network.Texaco);
                InvoicingController.reportList = new();
                InvoicingController.invoices = new();
                return Json(checks);
            }
            catch (SiteErrorException e)
            {
                var res = new
                {
                    exceptionType = "SiteErrorException",
                    message = "Site Error",
                    description = e.Message
                };
                Response.StatusCode = 500;
                return Json(res);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpPost]
        public JsonResult GetEmailBody([FromBody] CustomerInvoice customerInvoice)
        {
            try
            {
                var emailInfo = _db.AllEmail((int)customerInvoice.account);
                // Define variables for to, cc, bcc, and subject
                string to = emailInfo.To; // Replace with actual customer email
                string cc = emailInfo.Cc; // Replace with actual cc recipients
                string bcc = emailInfo.Bcc; // Replace with actual bcc recipients
                string subject = $"Portland Fuelcard Invoice for {customerInvoice.name}"; // Replace with actual subject

                // Define the HTML email body
                string html = $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333 !important;
                            }}
      
                            .header {{
                                margin-bottom: 20px;
                            }}
                            .header p {{
                                margin: 2px 0;
                            }}
                            .footer {{
                                font-size: 0.9em;
                                color: #777;
                            }}
                            .footer p {{
                                margin: 5px 0;
                            }}
                            .footer img {{
                                max-width: 50%;
                                height: auto;
                            }}
                            .email-details {{
                                margin-bottom: 20px;
                                padding: 10px;
                                background-color: #e8f0fe;
                                border-left: 4px solid #4285f4;
                                border-radius: 4px;
                            }}
                            .email-details p {{
                                margin: 5px 0;
                                font-size: 0.9em;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='header'>
                            <h1>Email Details</h1>
                            <div class='email-details'>
                                <p id='EmailTo'><strong>To:</strong> {to}</p>
                                <p id='EmailCC'><strong>CC:</strong> {cc}</p>
                                <p id='EmailBCC'><strong>BCC:</strong> {bcc}</p>
                                <p id='EmailSubject'><strong>Subject:</strong> {subject}</p>
                            </div>
                        </div>

                        <p>Please find your latest Portland Fuelcard invoice attached.</p>
                        <p>Kind regards,</p>
                        <p><strong>Portland Fuel Cards</strong></p>
                        <hr>
                        <div class='footer'>
                            <p>Portland Fuel Ltd</p>
                            <p>Company Reg No: 07020627</p>
                            <p>1 Toft Green | York | YO1 6JT</p>
                            <p>Tel: 01904 570021 | Fax: 01904 652082</p>
                            <p><a href='http://www.portland-fuel.co.uk'>www.portland-fuel.co.uk</a></p>
                            <img src='\lib\Portland Fuel Card Footer.png' alt='Portland Fuel Card Footer'>
                            <p><em>Did you know we now also supply Ad Blue? Contact us to find out more.</em></p>
                        </div>
                    </body>
                    </html>";


                // Return the JSON object
                var JsonToReturn = new
                {
                    to = to,
                    cc = cc,
                    bcc = bcc,
                    subject = subject,
                    html = html,
                };
                return Json(JsonToReturn);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }


        public struct ItemCodeAndPorductData
        {
            public string description { get; set; }
            public string itemCode { get; set; }
        }
        [HttpPost]
        public JsonResult ConfirmInvoicing([FromBody] string Network)
        {
            try
            {
                try
                {
                    var ListOfProducts = _db.GetListOfProducts();
                    InvoiceGenerator.GenerateXeroCSV(invoices, ListOfProducts);
                }
                catch (Exception e)
                {
                    throw new InventoryItemCodeNotInDb(e.Message.Split(':')[1]);
                }
                _db.ConfirmChanges(Network, reportList.Where(e => e.Network == (int)EnumHelper.NetworkEnumFromString(Network)).ToList(), invoices.Where(e => e.network == EnumHelper.NetworkEnumFromString(Network)).ToList(), _db);
                InvoicingController.reportList = new();
                return Json("Success");
            }
            catch (InventoryItemCodeNotInDb e)
            {
                var res = new
                {
                    exceptionType = "InventoryItemCodeNotInDb",
                    message = "Inventory Item Code not in database",
                    description = e.Message
                };
                Response.StatusCode = 500;
                return Json(res);
            }


            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }

        }
        [HttpPost]
        public JsonResult GetInvoicePng([FromBody] CustomerInvoice customerInvoice)
        {
            try
            {
                if (customerInvoice == null || string.IsNullOrEmpty(customerInvoice.name))
                {
                    Response.StatusCode = 400;
                    return Json("Error: Invalid input.");
                }

                var invoice = invoices.FirstOrDefault(e => e.CustomerDetails.CompanyName == customerInvoice.name);
                if (invoice == null)
                {
                    Response.StatusCode = 404;
                    return Json("Error: Invoice not found.");
                }

                string fileName = FileHelperForInvoicing.BuildingFileNameForInvoicing(invoice, invoice.CustomerDetails.CompanyName);
                string path = Path.Combine(
                    FileHelperForInvoicing._startingDirectory.TrimEnd(Path.DirectorySeparatorChar),
                    FileHelperForInvoicing._year.ToString().TrimEnd(Path.DirectorySeparatorChar),
                    invoice.InvoiceDate.ToString("yyMMdd").TrimEnd(Path.DirectorySeparatorChar),
                    GetNetworkName(invoice.CustomerDetails.Network).TrimEnd(Path.DirectorySeparatorChar),
                    "PDFImages",
                    fileName + ".png"
                );

                if (!System.IO.File.Exists(path))
                {
                    Response.StatusCode = 404;
                    return Json("Error: Image not found.");
                }

                // Load the image and convert it to a base64 string
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                string base64Image = Convert.ToBase64String(imageBytes);
                string imageSrc = $"data:image/png;base64,{base64Image}";

                return Json(imageSrc);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error: Could not get PNG. " + e.Message);
            }
        }



        public string GetNetworkName(string Network)
        {
            switch (Network.ToLower())
            {
                case "texaco":
                    return "FastFuel";

                case "ukfuel":
                    return "UK Fuels";
                case "keyfuels":
                    return "Keyfuels";
                case "keyfuel":
                    return "Keyfuels";
                default:
                    return Network;
            }

        }
        [HttpPost]
        public JsonResult CompleteInvoicing([FromBody] CustomerInvoice customerInvoice)
        {
            InvoiceSummary summary = new();
            try
            {
                InvoicePDFModel newInvoice = new();
                customerInvoice = _db.OrderTransactions(customerInvoice);
                newInvoice.network = _db.getNetworkFromAccount((int)customerInvoice.account);

                newInvoice.InvoiceDate = customerInvoice.invoiceDate;
                if (customerInvoice.CustomerType != EnumHelper.CustomerType.Floating)
                {
                    newInvoice.fixedBox = summary.GetFixDetails(customerInvoice, newInvoice.network, customerInvoice.CustomerType);
                    newInvoice.fixedBox.TradeId = customerInvoice.fixedInformation?.CurrentTradeId;
                }
                newInvoice.rows = summary.ProductBreakdown(customerInvoice, newInvoice.network);

                newInvoice.transactions = summary.TurnsTransactionsToPdf(newInvoice.network, customerInvoice, _db);

                newInvoice.totals = summary.GetInvoiceTotal(newInvoice.rows);
                if (customerInvoice.name != "The Fuel Trading Company")
                {
                    newInvoice.CustomerDetails = summary.GetCustomerDetails(customerInvoice, _db, HomeController.PFLXeroCustomersData.Where(e => e.Name == customerInvoice.name).FirstOrDefault().ContactID.ToString(), (int)customerInvoice.CustomerTransactions[0].network);
                }

                else
                {
                    newInvoice.CustomerDetails = summary.GetCustomerDetails(customerInvoice, _db, "FTC", (int)customerInvoice.CustomerTransactions[0].network);
                }

                invoices.Add(newInvoice);

                reportList.Add(_report.CreateNewInvoiceReport(newInvoice));
                try
                {
                    InvoiceGenerator invoiceGenerator = new(newInvoice);
                    FileHelperForInvoicing.CheckOrCorrectDirectorysBeforePDFCreation();
                    invoiceGenerator.generatePDF(newInvoice);
                    invoiceGenerator.generatePDFImage(newInvoice);
                    invoiceGenerator.generateCSV(newInvoice);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return Json("");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpPost]
        public JsonResult SendEmail([FromBody] SendEmailInformation sendEmailInformation)
        {
            try
            {
                var Invoice = invoices.FirstOrDefault(e => e.CustomerDetails.account == sendEmailInformation.CustomerInvoice.account);
                Fuelcards.InvoiceMethods.Email.SendEmail email = new("prices@portland-fuel.co.uk", "fuJeXU5BgLAM69Tqch3&#mQ%4");
                email.message.AddTo("connor@portland-fuel.co.uk");

                //email.message.AddTo(sendEmailInformation.EmailDetails.emailTo);
                //email.message.AddCC(sendEmailInformation.EmailDetails.emailCc);
                //email.message.AddBCC(sendEmailInformation.EmailDetails.emailBcc);
                email.message.AddSubject(sendEmailInformation.EmailDetails.emailSubject);
                string GeneralPath = FileHelperForInvoicing.BuidlingPDFFilePath(Invoice, sendEmailInformation.CustomerInvoice.invoiceDate);
                string file = FileHelperForInvoicing.BuildingFileNameForInvoicing(Invoice, sendEmailInformation.CustomerInvoice.name);
                string PDF = Path.Combine(GeneralPath, file);
                string CSVPathInfo = FileHelperForInvoicing.BuildingFilePathForCSV(Invoice, sendEmailInformation.CustomerInvoice.invoiceDate);
                email.message.AddAttachment(PDF);
                email.message.AddAttachment(CSVPathInfo);
                email.message.SendEmail(); //DO NOT UNCOMMENT THIS UNDER ANY CIRCUMSTANCES.UNLESS THOSE CIRCUMSTANCES ARE A WORKING APP!

                var jsonToReturn = new
                {
                    success = true,
                };
                return Json(jsonToReturn);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpGet]
        public JsonResult GetInvoiceReport()
        {
            try
            {

                List<InvoiceReport> reports = reportList;
                InvoiceReport totalsRow = InvoiceReportController.CalculateTotals(reports);
                reports.Add(totalsRow);

                return Json(reports);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }

        [HttpPost]
        public JsonResult ProcessTransactionFromPage([FromBody] TransactionDataFromView transactionDataFromView)
        {
            try
            {
                EnumHelper.Network network = _db.getNetworkFromAccount((int)transactionDataFromView.account);
                TransactionBuilder tb = new(_sites, _db);
                DataToPassBack model = tb.processTransaction(transactionDataFromView, network);

                return Json(model);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }
        [HttpPost]
        public JsonResult UploadNewItemInventoryCode([FromBody] ItemCodeAndPorductData itemCodeAndPorductData)
        {
            try
            {
                _db.UploadNewItemInventoryCode(itemCodeAndPorductData.itemCode, itemCodeAndPorductData.description);
                return Json("Success");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json("Error:" + e.Message);
            }
        }

        public struct DataToPassBack
        {
            public string? SiteName { get; set; }
            public string? InvoicePrice { get; set; }
            public string? UnitPrice { get; set; }
            public string? Product { get; set; }
        }
        public struct TransactionDataFromView
        {
            public DateOnly invoiceDate { get; set; }
            public string? name { get; set; }
            public double? addon { get; set; }
            public int? account { get; set; }
            public EnumHelper.CustomerType customerType { get; set; }
            public bool IfuelsCustomer { get; set; }
            public FixedInformation? fixedInformation { get; set; }
            public GenericTransactionFile? transaction { get; set; }
        }
        public struct SendEmailInformation
        {
            public EmailDetails EmailDetails { get; set; }
            public CustomerInvoice CustomerInvoice { get; set; }
        }
        public struct EmailDetails
        {
            public string? htmlbody { get; set; }
            public string emailTo { get; set; }
            public string emailCc { get; set; }
            public string emailBcc { get; set; }
            public string emailSubject { get; set; }

        }
    }
}