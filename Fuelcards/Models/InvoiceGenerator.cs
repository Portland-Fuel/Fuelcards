using Fuelcards.InvoiceMethods;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System.Reflection.Metadata;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.Globalization;
using System.Net;
using System.Xml.XPath;
using System.Xml;
using Color = MigraDoc.DocumentObjectModel.Color;
using Column = MigraDoc.DocumentObjectModel.Tables.Column;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;
using System.Collections.Generic;
using System.IO;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components.Forms;
using static System.Collections.Specialized.BitVector32;
using Xero.NetStandard.OAuth2.Model.Accounting;
using Microsoft.Graph;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Directory = System.IO.Directory;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.IO;
using File = System.IO.File;
namespace Fuelcards.Models
{
    public class InvoiceGenerator
    {
        public static DateOnly InvoiceDate;
        public static string FileName;
        private MigraDoc.DocumentObjectModel.Document document;
        private MigraDoc.DocumentObjectModel.Section section;
        private Table table;
        readonly XPathNavigator navigator;
        public readonly XmlDocument invoice;
        //Colors
        readonly static Color TableBorder = new(81, 125, 192);
        readonly static Color TableBlue = new(235, 240, 249);
        readonly static Color TableGray = new(242, 242, 242);
        /*readonly static Color TableGreen = new Color(101, 235, 114);*/
        readonly static Color TableGreen = new(99, 171, 113);
        readonly static Color Black = new(0, 0, 0, 0);
        readonly static Color Blue = new(30, 144, 255);


        public InvoiceGenerator(InvoicePDFModel invoicePDFModel)
        {
            FileName = InvoiceFileHelper.BuildingFileName(invoicePDFModel, invoicePDFModel.CustomerDetails.CompanyName);
            XmlUrlResolver resolver = new();
            resolver.Credentials = CredentialCache.DefaultCredentials;
            InvoiceDate = invoicePDFModel.InvoiceDate;

            // Create invoice
            invoice = new XmlDocument();
            invoice.XmlResolver = resolver;

            //var stream = new FileStream(path, FileMode.OpenOrCreate);

            //invoice.Load(stream);
            navigator = invoice.CreateNavigator();
        }

        #region PDF Generation
        public void generatePDF(InvoicePDFModel PDFModel)
        {
            try
            {
                InvoiceFileHelper.CheckOrCorrectDirectorysBeforePDFCreation();

                DateOnly InvoiceDate = PDFModel.InvoiceDate;
                string SavePath = InvoiceFileHelper.BuildingFilePath(PDFModel, InvoiceDate);

                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(SavePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Set the custom font resolver for PDFsharp, if needed

                // Create the document
                document = createDocument(PDFModel, InvoiceDate.ToString("dd/MM/yyyy"));

                // Prepare and render the document
                var docRenderer = new DocumentRenderer(document);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                docRenderer.PrepareDocument();

                PdfDocumentRenderer renderer = new(true)
                {
                    Document = document
                };
                renderer.PrepareRenderPages();
                renderer.RenderDocument();
                string FullSavePath = Path.Combine(SavePath, FileName);
                renderer.PdfDocument.Save(FullSavePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access denied: " + ex.Message);
                // Handle the error or inform the user
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                // Handle other errors
            }
        }
        // Define the custom font resolver as shown previously


        public MigraDoc.DocumentObjectModel.Document createDocument(InvoicePDFModel PDFModel, string dateFormatted)
        {
            document = new MigraDoc.DocumentObjectModel.Document();
            document.Info.Title = "Invoice From Portland Fuel";
            document.Info.Subject = "Invoice Month: " + DateTime.Today.Date;
            document.Info.Author = "Portland Fuel";
            defineStyles();
            createPage(PDFModel, dateFormatted);
            fillContent(PDFModel, dateFormatted);


            return document;
        }

        private void fillContent(InvoicePDFModel pDFModel, string dateFormatted)
        {
            Row row = table.AddRow();
            row.Borders.Visible = false;

            printVAT(pDFModel);

            // Add an invisible row as a space line to the table
            row = table.AddRow();
            row.Borders.Visible = false;

            printDrawings(pDFModel, dateFormatted);
        }
        private void printDrawings(InvoicePDFModel invoiceModelCustomerDetails, string dateFormatted)
        {
            if (invoiceModelCustomerDetails.CustomerDetails.Network.ToLower().Contains("keyfuel"))
            {
                PrintForKeyfuels(invoiceModelCustomerDetails, dateFormatted);
            }
            else
            {
                PrintForUkFuels(invoiceModelCustomerDetails, dateFormatted);
            }
        }
        private void PrintForUkFuels(InvoicePDFModel invoiceModelCustomerDetails, string dateFormatted)
        {
            var table = section.AddTable();
            table.Style = "InvoiceDetailsStyle";
            table.Format.Font.Name = "Calibri";
            table.Format.Font.Size = 7;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Borders.Visible = false;

            var column = table.AddColumn();
            column = table.AddColumn("2.5cm");
            column = table.AddColumn("2cm");
            column = table.AddColumn("3cm");
            column = table.AddColumn();
            column = table.AddColumn();
            column = table.AddColumn("1.8cm");
            column = table.AddColumn("1.8cm");
            column = table.AddColumn("1.8 cm");

            Row DocumentNoAndDateRow = table.AddRow();
            DocumentNoAndDateRow.Borders.Visible = false;

            var FirstCellInRowThereAsPlaceholder = DocumentNoAndDateRow[0];
            FirstCellInRowThereAsPlaceholder.MergeRight = 5;
            FirstCellInRowThereAsPlaceholder.Borders.Visible = false;
            var CellToDisplayText = DocumentNoAndDateRow[6];
            CellToDisplayText.MergeRight = 2;

            CellToDisplayText.AddParagraph($"Document No: {invoiceModelCustomerDetails.CustomerDetails.InvoiceNumber} Date: {dateFormatted}");
            CellToDisplayText.Format.Font.Size = 6.5;
            CellToDisplayText.Format.Font.Color = TableGreen;
            CellToDisplayText.Borders.Visible = false;


            Row headerRow = table.AddRow();
            headerRow.Format.Font.Color = TableGreen;
            headerRow.Cells[0].AddParagraph("Card No/Cd");
            headerRow.Cells[1].AddParagraph("Reg No");
            headerRow.Cells[2].AddParagraph("Mileage");
            headerRow.Cells[3].AddParagraph("Site Details ");
            headerRow.Cells[4].AddParagraph("Date/Time");
            headerRow.Cells[5].AddParagraph("Product");
            headerRow.Cells[6].AddParagraph("Unit Price");
            headerRow.Cells[7].AddParagraph("Volume");
            headerRow.Cells[8].AddParagraph("Value");

            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;

            if (invoiceModelCustomerDetails.CustomerDetails.InvoiceType == 1)
            {
                var edis = invoiceModelCustomerDetails.transactions;
                string previousPanNumber = null;
                double VolumeTotalForEachTransactionGroup = 0;
                double TotalValueForEachTransactionGroup = 0;
                foreach (var edi in edis)
                {
                    //if (!string.IsNullOrEmpty(edi.Pan) && edi.Pan.Length >= 19)
                    //{

                    string currentPanNumber = edi.TransactionNumber;

                    if (previousPanNumber != null && previousPanNumber != currentPanNumber)
                    {
                        int countForPreviousPan = edis.Count(e => e.TransactionNumber == previousPanNumber);
                        var breakRow = table.AddRow();
                        breakRow.Cells[0].MergeRight = 3;
                        breakRow.Cells[0].AddParagraph($"Sub Total for Card No {previousPanNumber}");
                        breakRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                        breakRow.Cells[0].Format.Font.Bold = true;
                        breakRow.Cells[4].MergeRight = 2;
                        breakRow.Cells[4].AddParagraph($"No of Drawings: {countForPreviousPan}");
                        breakRow.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                        breakRow.Cells[4].Format.Font.Bold = true;
                        breakRow.Cells[7].Format.Font.Bold = true;
                        VolumeTotalForEachTransactionGroup = Math.Round(VolumeTotalForEachTransactionGroup, 2);
                        breakRow.Cells[7].AddParagraph(VolumeTotalForEachTransactionGroup.ToString());
                        breakRow.Cells[8].Format.Font.Bold = true;
                        TotalValueForEachTransactionGroup = Math.Round(TotalValueForEachTransactionGroup, 2);
                        breakRow.Cells[8].AddParagraph(TotalValueForEachTransactionGroup.ToString());

                        VolumeTotalForEachTransactionGroup = 0;

                        TotalValueForEachTransactionGroup = 0;

                    }
                    if (edi.Value == 0 || edi.Value == 0.0)
                    {
                        edi.Value = 0.00;
                    }
                    previousPanNumber = currentPanNumber;
                    var _row = table.AddRow();
                    _row.Borders.Visible = false;
                    _row.Cells[0].AddParagraph(currentPanNumber);
                    _row.Cells[1].AddParagraph(edi.RegNo);
                    _row.Cells[2].AddParagraph(edi.Mileage.ToString());
                    _row.Cells[3].AddParagraph(edi.SiteName);
                    _row.Cells[3].Format.Font.Size = 5;
                    _row.Cells[4].AddParagraph(edi.TranDate.ToString() + " " + edi.TranTime);
                    _row.Cells[5].AddParagraph(edi.product);
                    _row.Cells[5].Format.Font.Size = 6.5;
                    _row.Cells[6].AddParagraph(edi.UnitPrice.ToString());
                    _row.Cells[7].AddParagraph(edi.Volume.ToString());
                    VolumeTotalForEachTransactionGroup += (double)edi.Volume;
                    _row.Cells[8].AddParagraph("£" + edi.Volume.ToString());
                    TotalValueForEachTransactionGroup += (double)edi.Value;


                }
                if (!string.IsNullOrEmpty(previousPanNumber))
                {
                    int countForPreviousPan = edis.Count(e => e.TransactionNumber == previousPanNumber);
                    var FinalRow = table.AddRow();
                    FinalRow.Cells[0].MergeRight = 3;
                    FinalRow.Cells[0].AddParagraph($"Sub Total for Card No {previousPanNumber}");
                    FinalRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    FinalRow.Cells[0].Format.Font.Bold = true;
                    FinalRow.Cells[4].MergeRight = 2;
                    FinalRow.Cells[4].AddParagraph($"No of Drawings: {countForPreviousPan}");
                    FinalRow.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    FinalRow.Cells[4].Format.Font.Bold = true;
                    FinalRow.Cells[7].Format.Font.Bold = true;
                    VolumeTotalForEachTransactionGroup = Math.Round(VolumeTotalForEachTransactionGroup, 2);
                    FinalRow.Cells[7].AddParagraph(VolumeTotalForEachTransactionGroup.ToString());
                    FinalRow.Cells[8].Format.Font.Bold = true;
                    TotalValueForEachTransactionGroup = Math.Round(TotalValueForEachTransactionGroup, 2);
                    FinalRow.Cells[8].AddParagraph(TotalValueForEachTransactionGroup.ToString());

                    VolumeTotalForEachTransactionGroup = 0;

                    TotalValueForEachTransactionGroup = 0;


                }
            }
            if (invoiceModelCustomerDetails.CustomerDetails.InvoiceType == 0)
            {
                var edis = invoiceModelCustomerDetails.transactions;

                // Populate the cells
                foreach (var edi in edis)
                {
                    if (edi.Value == 0 || edi.Value == 0.0)
                    {
                        edi.Value = 0.00;
                    }
                    var _row = table.AddRow();
                    _row.Cells[0].AddParagraph(edi.TransactionNumber);
                    _row.Cells[1].AddParagraph(edi.RegNo);
                    _row.Cells[2].AddParagraph(edi.Mileage.ToString());
                    _row.Cells[3].AddParagraph(edi.SiteName);
                    _row.Cells[3].Format.Font.Size = 5;
                    _row.Cells[4].AddParagraph(edi.TranDate.ToString() + " " + edi.TranTime);
                    _row.Cells[5].AddParagraph(edi.product);
                    _row.Cells[5].Format.Font.Size = 6.5;
                    _row.Cells[6].AddParagraph(edi.UnitPrice.ToString());
                    _row.Cells[7].AddParagraph(edi.Volume.ToString());
                    _row.Cells[8].AddParagraph(edi.Value.ToString());
                }
            }

        }
        private void PrintForKeyfuels(InvoicePDFModel invoiceModelCustomerDetails, string dateFormatted)
        {
            var table = section.AddTable();
            table.Style = "InvoiceDetailsStyle";
            table.Format.Font.Name = "Calibri";
            table.Format.Font.Size = 7;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Borders.Visible = false;

            // Add columns to the table without specifying sizes
            var column = table.AddColumn();
            column = table.AddColumn("2.5cm");
            column = table.AddColumn("2cm");
            column = table.AddColumn("3cm");
            column = table.AddColumn();
            column = table.AddColumn();
            column = table.AddColumn("1.8cm");
            column = table.AddColumn("1.8cm");
            column = table.AddColumn("1.8 cm");

            Row DocumentNoAndDateRow = table.AddRow();
            DocumentNoAndDateRow.Borders.Visible = true;

            var FirstCellInRowThereAsPlaceholder = DocumentNoAndDateRow[0];
            FirstCellInRowThereAsPlaceholder.MergeRight = 5;
            FirstCellInRowThereAsPlaceholder.Borders.Visible = false;
            var CellToDisplayText = DocumentNoAndDateRow[6];
            CellToDisplayText.MergeRight = 2;
            string EmptyStr = "     ";
            CellToDisplayText.AddParagraph($"Document No: {invoiceModelCustomerDetails.CustomerDetails.InvoiceNumber} {EmptyStr} Date: {dateFormatted}");
            CellToDisplayText.Format.Font.Size = 6.5;
            CellToDisplayText.Format.Font.Color = TableGreen;
            CellToDisplayText.Borders.Visible = false;


            Row headerRow = table.AddRow();
            headerRow.Format.Font.Color = TableGreen;
            headerRow.Cells[0].AddParagraph("Card No");
            headerRow.Cells[1].AddParagraph("Transactio No.");
            headerRow.Cells[2].AddParagraph("Reg No ");
            headerRow.Cells[3].AddParagraph("Fuel Site");
            headerRow.Cells[4].AddParagraph("Date/Time");
            headerRow.Cells[5].AddParagraph("Product");
            headerRow.Cells[6].AddParagraph("Unit Price");
            headerRow.Cells[7].AddParagraph("Volume");
            headerRow.Cells[8].AddParagraph("Value");

            // Enable repeating header row on new pages
            headerRow.HeadingFormat = true;
            headerRow.Format.Font.Bold = true;

            if (invoiceModelCustomerDetails.CustomerDetails.InvoiceType == 1)
            {
                var edis = invoiceModelCustomerDetails.transactions;
                string previousPanNumber = null; // Initialize with null or empty string
                double VolumeTotalForEachTransactionGroup = 0;
                double TotalValueForEachTransactionGroup = 0;
                foreach (var edi in edis)
                {
                    //if (!string.IsNullOrEmpty(edi.Pan) && edi.Pan.Length >= 19)
                    //{
                    string currentPanNumber = edi.TransactionNumber;

                    if (previousPanNumber != null && previousPanNumber != currentPanNumber)
                    {
                        int countForPreviousPan = edis.Count(e => e.TransactionNumber == previousPanNumber);
                        var breakRow = table.AddRow();
                        breakRow.Cells[0].MergeRight = 3;
                        breakRow.Cells[0].AddParagraph($"Sub Total for Card No {previousPanNumber}");
                        breakRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                        breakRow.Cells[0].Format.Font.Bold = true;
                        breakRow.Cells[4].MergeRight = 2;
                        breakRow.Cells[4].AddParagraph($"No of Drawings: {countForPreviousPan}");
                        breakRow.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                        breakRow.Cells[4].Format.Font.Bold = true;
                        breakRow.Cells[7].Format.Font.Bold = true;
                        VolumeTotalForEachTransactionGroup = Math.Round(VolumeTotalForEachTransactionGroup, 2);
                        breakRow.Cells[7].AddParagraph(VolumeTotalForEachTransactionGroup.ToString());
                        breakRow.Cells[8].Format.Font.Bold = true;
                        TotalValueForEachTransactionGroup = Math.Round(TotalValueForEachTransactionGroup, 2);
                        breakRow.Cells[8].AddParagraph(TotalValueForEachTransactionGroup.ToString());

                        VolumeTotalForEachTransactionGroup = 0;

                        TotalValueForEachTransactionGroup = 0;

                    }
                    if (edi.Value == 0)
                    {
                        edi.Value = 0.00;
                    }
                    previousPanNumber = currentPanNumber;
                    var _row = table.AddRow();
                    _row.Borders.Visible = false;
                    _row.Cells[0].AddParagraph(currentPanNumber);
                    _row.Cells[1].AddParagraph(edi.TransactionNumber.ToString());
                    _row.Cells[2].AddParagraph(edi.RegNo);
                    _row.Cells[3].AddParagraph(edi.SiteName);
                    _row.Cells[3].Format.Font.Size = 5;
                    _row.Cells[4].AddParagraph(edi.TranDate.ToString() + " " + edi.TranTime);
                    _row.Cells[5].AddParagraph(edi.product);
                    _row.Cells[5].Format.Font.Size = 6.5;
                    _row.Cells[6].AddParagraph(edi.UnitPrice.ToString());
                    _row.Cells[7].AddParagraph(edi.Volume.ToString());
                    _row.Cells[8].AddParagraph("£" + edi.Value.ToString());


                }

                if (!string.IsNullOrEmpty(previousPanNumber))
                {
                    int countForPreviousPan = edis.Count(e => e.TransactionNumber == previousPanNumber);
                    var FinalBreakRow = table.AddRow();
                    FinalBreakRow.Cells[0].MergeRight = 3;
                    FinalBreakRow.Cells[0].AddParagraph($"Sub Total for Card No {previousPanNumber}");
                    FinalBreakRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    FinalBreakRow.Cells[0].Format.Font.Bold = true;
                    FinalBreakRow.Cells[4].MergeRight = 2;
                    FinalBreakRow.Cells[4].AddParagraph($"No of Drawings: {countForPreviousPan}");
                    FinalBreakRow.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    FinalBreakRow.Cells[4].Format.Font.Bold = true;
                    FinalBreakRow.Cells[7].Format.Font.Bold = true;
                    VolumeTotalForEachTransactionGroup = Math.Round(VolumeTotalForEachTransactionGroup, 2);
                    FinalBreakRow.Cells[7].AddParagraph(VolumeTotalForEachTransactionGroup.ToString());
                    FinalBreakRow.Cells[8].Format.Font.Bold = true;
                    TotalValueForEachTransactionGroup = Math.Round(TotalValueForEachTransactionGroup, 2);
                    FinalBreakRow.Cells[8].AddParagraph(TotalValueForEachTransactionGroup.ToString());

                    VolumeTotalForEachTransactionGroup = 0;

                    TotalValueForEachTransactionGroup = 0;
                }
            }
            if (invoiceModelCustomerDetails.CustomerDetails.InvoiceType == 0)
            {
                var edis = invoiceModelCustomerDetails.transactions;

                // Populate the cells
                foreach (var edi in edis)
                {
                    if (edi.Value == 0)
                    {
                        edi.Value = 0.00;
                    }
                    var _row = table.AddRow();
                    _row.Borders.Visible = false;
                    _row.Cells[0].AddParagraph(edi.TransactionNumber.ToString());
                    _row.Cells[1].AddParagraph(edi.TransactionNumber.ToString());
                    _row.Cells[2].AddParagraph(edi.RegNo);
                    _row.Cells[3].AddParagraph(edi.SiteName);
                    _row.Cells[3].Format.Font.Size = 5;
                    _row.Cells[4].AddParagraph(edi.TranDate.ToString() + " " + edi.TranTime);
                    _row.Cells[5].AddParagraph(edi.product);
                    _row.Cells[6].AddParagraph(edi.UnitPrice.ToString());
                    _row.Cells[7].AddParagraph(edi.Volume.ToString());
                    _row.Cells[8].AddParagraph("£" + edi.Value.ToString());

                }
            }

        }
        private void printVAT(InvoicePDFModel invoiceModelCustomerDetails)
        {
            table = section.AddTable();
            table.Style = "VATTable";
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 7;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            var Column = new Column();
            Column = table.AddColumn("1cm");
            Column = table.AddColumn("2cm");
            Column = table.AddColumn("2cm");
            Column = table.AddColumn("2cm");
            Column = table.AddColumn("2.4cm");// Text DebtCol 
            Column = table.AddColumn("2.2cm");
            Column = table.AddColumn("1cm");
            Column = table.AddColumn("1.5cm"); // new new
            Column = table.AddColumn("1.2cm");
            Column = table.AddColumn("3.45cm");
            /* var Column = new Column();
             Column = table.AddColumn("1cm");
             Column = table.AddColumn("2cm");
             Column = table.AddColumn("2cm");
             Column = table.AddColumn("2cm");
             Column = table.AddColumn("2.4cm");// Text DebtCol 
             Column = table.AddColumn("2.2cm");
             Column = table.AddColumn("1cm");
             Column = table.AddColumn("2cm");
             Column = table.AddColumn("1cm");
             Column = table.AddColumn("2cm");*/
            #region RowStuff
            var Row1 = table.AddRow();
            Row1.Format.Alignment = ParagraphAlignment.Center;
            Row1.Cells[0].Format.Font.Color = TableGreen;
            Row1.Cells[0].AddParagraph("VAT");
            Row1.Cells[1].Format.Font.Color = TableGreen;
            Row1.Cells[1].AddParagraph("Vat Rate");
            Row1.Cells[2].Format.Font.Color = TableGreen;
            Row1.Cells[2].AddParagraph("Goods Amount");
            Row1.Cells[3].Format.Font.Color = TableGreen;
            Row1.Cells[3].AddParagraph("VAT Amount");
            //Adding Text
            Row1.Cells[4].Format.Font.Color = Black;
            Row1.Cells[4].Borders.Visible = false;
            Row1.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            Row1.Cells[4].AddParagraph("Vat Number");

            Row1.Cells[5].Format.Font.Color = Black;
            Row1.Cells[5].Borders.Visible = false;
            Row1.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            Row1.Cells[5].AddParagraph("299755033");

            var Row2 = table.AddRow();
            #region Formatting
            double FormatGoods = (double)invoiceModelCustomerDetails.totals.Goods;
            string FormaattedGoods = FormatGoods.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

            double FormatVatAmount = (double)invoiceModelCustomerDetails.totals.VAT;
            string FormattedVat = FormatVatAmount.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

            #endregion
            Row2.Format.Alignment = ParagraphAlignment.Center;
            Row2.Cells[0].AddParagraph("1");
            Row2.Cells[1].AddParagraph("20.00%");
            Row2.Cells[2].AddParagraph(FormaattedGoods);
            Row2.Cells[3].AddParagraph(FormattedVat);
            Row2.Cells[4].Borders.Visible = false;
            Row2.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            Row2.Cells[4].AddParagraph("Bank");
            Row2.Cells[5].Borders.Visible = false;
            Row2.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            Row2.Cells[5].AddParagraph("Santander UK");

            Row2.Cells[6].Format.Borders.Visible = false;
            Row2.Cells[7].Format.Borders.Visible = false;

            var Row3 = table.AddRow();
            for (int i = 0; i < 4; i++)
            {
                Row3.Cells[i].Borders.Visible = false;
            }
            Row3.Cells[4].Borders.Visible = false;
            Row3.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            Row3.Cells[4].AddParagraph("Account Name:");
            Row3.Cells[5].Borders.Visible = false;
            Row3.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            Row3.Cells[5].AddParagraph("Portland Fuel Ltd");

            var Row4 = table.AddRow();
            for (int i = 0; i < 4; i++)
            {
                Row4.Cells[i].Borders.Visible = false;
            }
            Row4.Cells[4].Borders.Visible = false;
            Row4.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            Row4.Cells[4].AddParagraph("Account Number");
            Row4.Cells[5].Borders.Visible = false;
            Row4.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            Row4.Cells[5].AddParagraph("11084796");

            Row4.Cells[6].Borders.Visible = false;
            Row4.Cells[7].Borders.Visible = false;
            Row4.Cells[8].Borders.Visible = false;
            Row4.Cells[9].Borders.Visible = false;

            var Row5 = table.AddRow();
            for (int i = 0; i < 4; i++)
            {
                Row5.Cells[i].Borders.Visible = false;
            }
            Row5.Cells[4].Borders.Visible = false;
            Row5.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            Row5.Cells[4].AddParagraph("Account Sort Code");
            Row5.Cells[5].Borders.Visible = false;
            Row5.Cells[5].Format.Alignment = ParagraphAlignment.Left;
            Row5.Cells[5].AddParagraph("09-02-22");
            Row5.Cells[6].Borders.Visible = false;
            Row5.Cells[7].Borders.Visible = false;
            Row5.Cells[9].Borders.Visible = false;
            #endregion
            //GOODS VAR TOTAL TABLE


            /*Row1.Cells[8].Format.Alignment = ParagraphAlignment.Center;
            Row1.Cells[8].Borders.Visible = false;
            Row1.Cells[8].Borders.Visible = true;
            Row1.Cells[8].Format.Font.Bold = true;
            Row1.Cells[8].Format.Font.Color = Blue;
            Row1.Cells[8].AddParagraph("Goods");*/
            Row1.Cells[6].Borders.Visible = false;
            Row1.Cells[7].Borders.Visible = false;


            Row2.Cells[6].Borders.Visible = false;
            Row2.Cells[7].Borders.Visible = false;



            #region Formatting
            double GoodsDouble = (double)invoiceModelCustomerDetails.totals.Goods;
            string GoodsDoubleFormatted = GoodsDouble.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

            double VatDouble = (double)invoiceModelCustomerDetails.totals.VAT;
            string VatFormatted = VatDouble.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

            double TotalDouble = (double)invoiceModelCustomerDetails.totals.Total;
            string TotalFormatted = TotalDouble.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

            #endregion
            // Goods
            Cell GoodsMerged = Row1.Cells[8];
            GoodsMerged.MergeDown = 1;
            var GoodsTxt = GoodsMerged.AddParagraph("Goods");
            GoodsTxt.Format.Font.Color = Blue;
            GoodsTxt.Format.Font.Bold = true;
            GoodsTxt.Format.Font.Size = 12;

            var mergedCellBorders = GoodsMerged.Borders;

            mergedCellBorders.Left.Width = 1.5;
            mergedCellBorders.Top.Width = 1.5;



            Cell GoodsVal = Row1.Cells[9];
            GoodsVal.MergeDown = 1;
            GoodsVal.AddParagraph(GoodsDoubleFormatted);
            GoodsVal.Format.Alignment = ParagraphAlignment.Center;
            GoodsVal.Format.Font.Size = 10;
            var GoodsMergedBorders = GoodsVal.Borders;
            GoodsMergedBorders.Right.Width = 1.5;
            GoodsMergedBorders.Top.Width = 1.5;







            Cell VatMerged = Row3.Cells[8];
            VatMerged.MergeDown = 1;
            var VatTxt = VatMerged.AddParagraph("VAT");
            VatMerged.Format.Alignment = ParagraphAlignment.Center;
            VatTxt.Format.Font.Color = Blue;
            VatTxt.Format.Font.Bold = true;
            Row3.Cells[6].Borders.Visible = false;
            Row3.Cells[7].Borders.Visible = false;
            VatTxt.Format.Font.Size = 12;

            var VatBorderes = VatMerged.Borders;
            VatBorderes.Left.Width = 1.5;


            Cell VatVal = Row3.Cells[9];
            VatVal.MergeDown = 1;
            VatVal.AddParagraph(VatFormatted);
            VatVal.Format.Alignment = ParagraphAlignment.Center;
            VatVal.Format.Font.Size = 10;
            var VatValBorder = VatVal.Borders;
            VatValBorder.Right.Width = 1.5;

            // Assuming invoiceModelCustomerDetails.DateOfInvoice is a DateTime object
            DateOnly invoiceDateOnly = invoiceModelCustomerDetails.InvoiceDate;

            // Convert DateOnly to DateTime
            DateTime dateTime = new(invoiceDateOnly.Year, invoiceDateOnly.Month, invoiceDateOnly.Day);

            // Add 7 days
            DateTime newDate = dateTime.AddDays(14);

            // Get the date string without the time part
            string DateToShow = newDate.ToString("dd-MM-yyyy"); // Adjust the format to your preference


            var Row6 = table.AddRow();
            for (int i = 0; i < 8; i++)
            {
                Row6.Cells[i].Borders.Visible = false;
            }
            var Longtext = Row6.Cells[1];
            Row6.Cells[1].Format.Font.Bold = true;
            Longtext.MergeRight = 5;
            var Text = Longtext.AddParagraph($"THIS INVOICE WILL BE DEBITED FROM YOUR ACCOUNT ON OR SHORTLY AFTER {DateToShow}");
            Text.Format.Font.Bold = true;

            Cell TotalMerged = Row5.Cells[8];
            TotalMerged.MergeDown = 1;
            var TxtTotal = TotalMerged.AddParagraph("Total");
            TxtTotal.Format.Font.Color = Blue;
            TxtTotal.Format.Font.Bold = true;
            TotalMerged.Format.Alignment = ParagraphAlignment.Center;

            var TotalMergedBorders = TotalMerged.Borders;
            TxtTotal.Format.Font.Size = 12;
            TotalMergedBorders.Left.Width = 1.5;
            TotalMergedBorders.Bottom.Width = 1.5;




            Cell TotalVal = Row5.Cells[9];
            TotalVal.MergeDown = 1;
            TotalVal.Format.Borders.Visible = false;
            Row5.Cells[9].Borders.Visible = true;
            TotalVal.AddParagraph(TotalFormatted);
            TotalVal.Format.Alignment = ParagraphAlignment.Center;
            TotalVal.Format.Font.Size = 10;
            TotalVal.Format.Font.Bold = true;
            var TotalBorders = TotalVal.Borders;
            TotalBorders.Right.Width = 1.5;
            TotalBorders.Bottom.Width = 1.5;


            var _Row7 = table.AddRow();
            for (int i = 0; i < 10; i++)
            {
                _Row7.Cells[i].Borders.Visible = false;
            }




            table = section.AddTable();
            table.Style = "DebitedMEssageTable";
            table.Format.Font.Name = "Calibri";
            table.Format.Font.Size = 7;
            table.Format.SpaceAfter = 2;
            table.Borders.Color = TableBorder;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;
            table.Borders.Color = Black;
            var DebtCol = new Column();
            DebtCol = table.AddColumn("1cm");
            DebtCol = table.AddColumn("12.5cm");

            var Row7 = table.AddRow();
            for (int i = 0; i < 2; i++)
            {
                Row7.Cells[i].Borders.Visible = false;
            }
            //DOSENT WORK YET BUT NEED TO INCLUDE THIS DO NOT DELETE
            if (invoiceModelCustomerDetails.fixedBox != null)
            {
                table = section.AddTable();
                table.Style = "InfoTableOptional";
                table.Format.Font.Name = "Calibri";
                table.Format.Font.Size = 7;
                table.Format.SpaceAfter = 2;
                table.Borders.Color = TableBorder;
                table.Borders.Width = 0.25;
                table.Borders.Left.Width = 0.5;
                table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 0;
                table.Borders.Color = Black;
                var InfoTableCol = new Column();
                InfoTableCol = table.AddColumn("7cm");
                InfoTableCol = table.AddColumn("4cm");
                #region Formatting
                double TotalDieselLiftedOnInvoiceInt = Convert.ToDouble(invoiceModelCustomerDetails.fixedBox.TotalDieselVolumeLiftedOnThisInvoice);
                string TotalDieselLiftedOnInvoiceFormatted = TotalDieselLiftedOnInvoiceInt.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                TotalDieselLiftedOnInvoiceFormatted = TotalDieselLiftedOnInvoiceFormatted.Replace("£", string.Empty);


                double FixedPriceVolumeForPeriodint = Convert.ToDouble(invoiceModelCustomerDetails.fixedBox.FixedPriceVolumeForThisPeriod);
                string FixedPriceVolumeForPeriodFormatted = FixedPriceVolumeForPeriodint.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                FixedPriceVolumeForPeriodFormatted = FixedPriceVolumeForPeriodFormatted.Replace("£", string.Empty);

                double FixedVolumeFromPreviousInt = Convert.ToDouble(invoiceModelCustomerDetails.fixedBox.FixedPriceVolumeFromPreviousPeriods);
                string FixedVolumeFromPreviousString = FixedVolumeFromPreviousInt.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                FixedVolumeFromPreviousString = FixedVolumeFromPreviousString.Replace("£", string.Empty);

                double FixedVolumeUsedOnCurrentInt = Convert.ToDouble(invoiceModelCustomerDetails.fixedBox.FixedPriceVolumeUsedOnThisinvoice);
                string FixedVolumeUsedOnCurrentString = FixedVolumeUsedOnCurrentInt.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                FixedVolumeUsedOnCurrentString = FixedVolumeUsedOnCurrentString.Replace("£", string.Empty);

                double FixedPriceLitresRemainingInt = Convert.ToDouble(invoiceModelCustomerDetails.fixedBox.FixedPriceRemaining);
                string FixedPriceLitresRemainingString = FixedPriceLitresRemainingInt.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                FixedPriceLitresRemainingString = FixedPriceLitresRemainingString.Replace("£", string.Empty);
                #endregion

                var Sumrow = table.AddRow();
                Sumrow[0].AddParagraph("Total diesel volume lifted on this invoice");
                Sumrow[1].Format.Alignment = ParagraphAlignment.Right;
                Sumrow[1].AddParagraph(TotalDieselLiftedOnInvoiceFormatted);

                var Sumrow2 = table.AddRow();
                Sumrow2[0].AddParagraph("Fixed price volume for this period");
                Sumrow2[1].Format.Alignment = ParagraphAlignment.Right;
                Sumrow2[1].AddParagraph(FixedPriceVolumeForPeriodFormatted);

                var Sumrow3 = table.AddRow();
                Sumrow3[0].AddParagraph("Fixed price volume from previous periods");
                Sumrow3[1].Format.Alignment = ParagraphAlignment.Right;
                Sumrow3[1].AddParagraph(FixedVolumeFromPreviousString);

                var Sumrow4 = table.AddRow();
                Sumrow4[0].AddParagraph("Fixed price volume used on this invoice");
                Sumrow4[1].Format.Alignment = ParagraphAlignment.Right;
                Sumrow4[1].AddParagraph(FixedVolumeUsedOnCurrentString);

                var Sumrow5 = table.AddRow();
                Sumrow5[0].AddParagraph("Fixed price litres remaining");
                Sumrow5[0].Format.Font.Bold = true;
                Sumrow5[1].Format.Font.Bold = true;
                Sumrow5[1].Format.Alignment = ParagraphAlignment.Right;
                Sumrow5[1].AddParagraph(FixedPriceLitresRemainingString);
            }


        }
        private void createPage(InvoicePDFModel CustomerInvoiceModel, string dateFormatted)
        {
            section = document.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(0.4);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            // Put Logo In Header



            //Testing Out doing the header in a table way and not by the sections
            var HeaderTable = section.AddTable();
            HeaderTable.Style = "";
            HeaderTable.Borders.Color = Black;
            HeaderTable.Borders.Width = 0.25;
            HeaderTable.Borders.Left.Width = 0.5;
            HeaderTable.Borders.Right.Width = 0.5;
            HeaderTable.Borders.Visible = false;

            Column column = HeaderTable.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = HeaderTable.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = HeaderTable.AddColumn("2cm");
            column.Format.Borders.Visible = true;

            column = HeaderTable.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            string[] AddressArr = CustomerInvoiceModel.CustomerDetails.AddressArr;
            string Customername = CustomerInvoiceModel.CustomerDetails.CompanyName;





            var HeaderRow1 = HeaderTable.AddRow();
            Paragraph CompanyDetails = HeaderRow1.Cells[0].AddParagraph();
            var Comp = CompanyDetails.AddFormattedText("\n" + Customername + "\n");
            Comp.Font.Bold = true;
            Comp.Font.Size = 9;
            CompanyDetails.Format.Alignment = ParagraphAlignment.Left;
            CompanyDetails.Format.Font.Size = 7;
            CompanyDetails.AddFormattedText("\n");
            for (int i = 0; i < AddressArr.Length; i++)
            {
                if (AddressArr[i] != string.Empty || AddressArr[i] != "")
                {
                    CompanyDetails.AddText(AddressArr[i] + "\n");
                }
            }

            Paragraph FooterPara = section.Footers.Primary.AddParagraph();
            FooterPara.Format.Alignment = ParagraphAlignment.Right;
            FooterPara.AddText("Portland Fuel Ltd, 1 Toft Green, York, YO1 6JT");
            FooterPara.Format.Font.Size = 7;
            // Adding multiple lines of text with line breaks in HeaderRow1.Cells[1]
            Paragraph portlandInfo = HeaderRow1.Cells[1].AddParagraph();
            portlandInfo.Format.Font.Bold = true;
            portlandInfo.Format.Font.Size = 9;
            if (CustomerInvoiceModel.CustomerDetails.Network.ToLower() == "fuelgenie")
            {
                portlandInfo.AddFormattedText("The Fuel Trading Company Limited\n", TextFormat.Bold);

            }
            else
            {
                portlandInfo.AddFormattedText("Portland Fuel Limited\n", TextFormat.Bold);

            }
            portlandInfo.AddText("1 Toft Green\n");
            portlandInfo.AddText("York\n");
            portlandInfo.AddText("YO1 6JT\n");
            portlandInfo.AddText("\n");
            portlandInfo.Format.Alignment = ParagraphAlignment.Center;

            var tele = portlandInfo.AddFormattedText("Telephone: 0044 (0) 1904 570021 \n");
            tele.Font.Size = 7;
            tele.Font.Bold = false;
            var web = portlandInfo.AddFormattedText("www.portland-fuel.co.uk");
            web.Font.Size = 7;
            web.Font.Bold = false;

            HeaderRow1.Cells[2].Borders.Visible = false;

            var fuelCardImage = HeaderRow1.Cells[3].AddImage(@"C:\Portland\Fuel Trading Company\Portland - Portland\Marketing\Marketing Materials\Logos\New Logos\Short Logos\Colour\PNG\Fuel Cards.png");
            fuelCardImage.Width = "4.2cm";
            fuelCardImage.Height = "1.8cm";

            HeaderRow1.Cells[3].VerticalAlignment = VerticalAlignment.Center;

            // Create footer


            table = section.AddTable();
            var Column = new Column();
            Column = table.AddColumn("1cm");
            Row row = table.AddRow();
            row.Borders.Visible = false;

            Row row2 = table.AddRow();
            row.Borders.Visible = false;
            Row row3 = table.AddRow();
            row.Borders.Visible = false;

            // Print the header
            PrintSecondSetOfContent(CustomerInvoiceModel, dateFormatted);

            // Print the summary table
            printSummary(CustomerInvoiceModel);

            //table.SetEdge(1, 1, 6, 2, Edge.Box , BorderStyle.Single, 1, Color.Empty);
        }
        private void printSummary(InvoicePDFModel CustomerInvoiceModel)
        {
            // Create the address table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = Black;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;



            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.7cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.0cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("2.42cm");
            column.Format.Alignment = ParagraphAlignment.Center;



            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Format.Font.Color = TableGreen;

            row.Cells[0].AddParagraph("Card Type");
            row.Cells[1].AddParagraph("Product Code");
            row.Cells[2].AddParagraph("Description");
            row.Cells[3].AddParagraph("Quantity");
            row.Cells[4].AddParagraph("Net Total");
            row.Cells[5].AddParagraph("Net VAT");
            row.Cells[6].AddParagraph("VAT Rate");


            string DisplayedNetwork = "";
            int count = 0;
            foreach (var sumrow in CustomerInvoiceModel.rows)
            {
                Row NewSumRow = table.AddRow();
                if (count == 0 && CustomerInvoiceModel.CustomerDetails != null)
                {
                    NewSumRow.Cells[0].AddParagraph(CustomerInvoiceModel.CustomerDetails.Network);
                }
                else
                {
                    NewSumRow.Cells[0].AddParagraph("");
                }
                double netTotal = (double)sumrow.NetTotal;
                string formattedCurrency = netTotal.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

                double netVat = (double)sumrow.VAT;
                string NetVatFormatted = netVat.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

                NewSumRow.Cells[1].AddParagraph(sumrow.productCode.ToString());
                NewSumRow.Cells[2].AddParagraph(sumrow.productName);
                NewSumRow.Cells[3].AddParagraph(sumrow.Quantity.ToString());
                NewSumRow.Cells[4].AddParagraph(formattedCurrency);
                NewSumRow.Cells[5].AddParagraph(NetVatFormatted);
                NewSumRow.Cells[6].AddParagraph(sumrow.VAT.ToString() + "%");




                count++;
            }


            /*  List<string> Networks = CustomerInvoiceModel.InvoiceSummary.Networks.ToList();

              foreach (var network in Networks)
              {
                  DisplayedNetwork = network;
                  if (DisplayedNetwork == "Uk Fuel") DisplayedNetwork = "Commercial";
              }
              row2.Cells[0].AddParagraph(DisplayedNetwork);

              foreach (var item in CustomerInvoiceModel.InvoiceSummary.ProductSummaries)
              {
                  var matchedProduct = CustomerInvoiceModel.InvoiceSummary.ProductsAndCodes.FirstOrDefault(p => p.Key == item.product);

                  if (matchedProduct.Value != null)
                  {
                      #region Formatting
                      string ProductCodeSTR = string.Join(",", matchedProduct.Value);

                      double netTotal = item.NetTotal;
                      string formattedCurrency = netTotal.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

                      double netVat = item.NetVat;
                      string NetVatFormatted = netVat.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));

                      #endregion

                      row2.Cells[1].AddParagraph(ProductCodeSTR);
                      if (matchedProduct.Key == "Diesel" && DisplayedNetwork == "Commercial") row2.Cells[2].AddParagraph("ULSD UN1202");
                      else
                      {
                          row2.Cells[2].AddParagraph(matchedProduct.Key);
                      }
                      row2.Cells[3].AddParagraph(item.quantity.ToString());
                      row2.Cells[4].AddParagraph(formattedCurrency);
                      row2.Cells[5].AddParagraph(NetVatFormatted);
                      row2.Cells[6].AddParagraph(item.VATRate + ".0%");

                  }
                  else
                  {
                      var egg = 0;
                  }
              }*/

        }
        private void PrintSecondSetOfContent(InvoicePDFModel CustomerInvoiceModel, string dateFormatted)
        {
            var rand = new Random(1000);

            // Create the table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = Black;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Define the Columns
            var col = table.AddColumn("5cm");

            col = table.AddColumn("3.2cm");
            col.Borders.Visible = false;
            col = table.AddColumn("2.7cm");
            col = table.AddColumn("3.8cm");
            col = table.AddColumn("3.8cm");

            // Define the rows
            var row = table.AddRow();
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Font.Size = 8;
            row.Cells[0].Format.Font.Color = Black;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("SALES INVOICE");

            row.Format.Alignment = ParagraphAlignment.Center;
            row.Cells[2].AddParagraph("Account No");
            row.Cells[2].Format.Font.Color = TableGreen;
            row.Cells[3].AddParagraph("Document Date");
            row.Cells[3].Format.Font.Color = TableGreen;
            row.Cells[4].AddParagraph("Sales Invoice No");
            row.Cells[4].Format.Font.Color = TableGreen;

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Cells[0].Borders.Visible = false;
            row.Cells[2].AddParagraph(CustomerInvoiceModel.CustomerDetails.account.ToString());
            row.Cells[3].AddParagraph(dateFormatted);
            row.Cells[4].AddParagraph($"{CustomerInvoiceModel.CustomerDetails.InvoiceNumber.ToString()}");



            // Add a space line
            row = table.AddRow();
            row.Borders.Visible = false;
        }
        private void defineStyles()
        {
            Style style = document.Styles["Normal"];
            //style.Font.Name = "Verdana";

            style = document.Styles[StyleNames.Header];
            // style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Size = 7;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        #endregion

        public void GenerateCSV(InvoicePDFModel invoicePDFModel)
        {
            try
            {
                string Filename = InvoiceFileHelper.BuildingFileName(invoicePDFModel, invoicePDFModel.CustomerDetails.CompanyName);
                string FilePath = InvoiceFileHelper.BuildingFilePath(invoicePDFModel, invoicePDFModel.InvoiceDate);
                string CSVPath = Path.Combine(FilePath, "CSV", Filename.Replace(".pdf", ".csv"));
                using (StreamWriter sw = new StreamWriter(CSVPath))
                {
                    sw.WriteLine(",,,,,,,,,Portland");
                    sw.WriteLine($",{invoicePDFModel.CustomerDetails.CompanyName}");
                    for (int i = 0; i < invoicePDFModel.CustomerDetails.AddressArr.Length; i++)
                    {
                        sw.WriteLine($",{invoicePDFModel.CustomerDetails.AddressArr[i]}");
                    }
                    sw.WriteLine(",SALES INVOICE,,,,,,,,Account No,,,Document Date,,,Sales Invoice No");
                    sw.WriteLine($",,,,,,,,,122542,,,29-Jul-2024,,,TX32734");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }


    public static class InvoiceFileHelper
    {
        public static string _startingDirectory = @"C:\Portland\Fuel Trading Company\Fuelcards - Fuelcards\Invoices\FuelcardApp";
        public static decimal _year = DateTime.Now.Year;
        public static string BuildingFileName(InvoicePDFModel newInvoice, string CustomerName)
        {
            string prefix = string.Empty;
            switch (newInvoice.CustomerDetails.Network.ToString())
            {
                case "UkFuel":
                    prefix = "PF";
                    break;
                case "Uk Fuel":
                    prefix = "PF";
                    break;
                case "Texaco":
                    prefix = "TX";
                    break;
                default:
                    break;
            }   
            newInvoice.CustomerDetails.InvoiceNumber = prefix + newInvoice.CustomerDetails.InvoiceNumber;
            return CustomerName + " Inv" + " " + newInvoice.CustomerDetails.InvoiceNumber + ".pdf";
        }

        public static void CheckOrCorrectDirectorysBeforePDFCreation()
        {

            try
            {
                List<string> ListOfNetworks = new List<string>();
                ListOfNetworks.Add("Keyfuels");
                ListOfNetworks.Add("UK Fuels");
                ListOfNetworks.Add("Fuel Genie");
                ListOfNetworks.Add("FastFuel");
                //TopLevelCheck
                if (!Directory.Exists(_startingDirectory))
                {
                    Directory.CreateDirectory(_startingDirectory);
                }


                //YearCheck
                if (!Directory.Exists(_startingDirectory + "\\" + _year))
                {
                    Directory.CreateDirectory(_startingDirectory + "\\" + _year);
                }

                //DateCheck
                if (!Directory.Exists(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd")))
                {
                    Directory.CreateDirectory(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd"));
                }

                //NetworkCheck
                foreach (var Network in ListOfNetworks)
                {
                    if (!Directory.Exists(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network))
                    {
                        Directory.CreateDirectory(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network);
                    }
                }

                //CsvAndPDFImagesCheck
                foreach (var Network in ListOfNetworks)
                {
                    if (!Directory.Exists(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network + "\\" + "CSV"))
                    {
                        Directory.CreateDirectory(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network + "\\" + "CSV");
                    }
                    if (!Directory.Exists(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network + "\\" + "PDFImages"))
                    {
                        Directory.CreateDirectory(_startingDirectory + "\\" + _year + "\\" + InvoiceGenerator.InvoiceDate.ToString("yyyy-MM-dd") + "\\" + Network + "\\" + "PDFImages");
                    }
                }



            }
            catch (Exception e)
            {
                throw new Exception("Error in CheckOrCorrectDirectorysBeforePDFCreation: " + e.Message);
            }
           
        }

        public static string BuildingFilePath(InvoicePDFModel newInvoice,DateOnly InvoiceDate)
        {
            string? network = newInvoice.CustomerDetails.Network;
            if(network == null)
            {
                throw new InvalidOperationException("Network is null");
            }
            else
            {
                network = network.ToLower();
            }
            string strdate = InvoiceDate.ToString("yyyy-MM-dd");
            strdate = strdate.Replace("/", "-");
            string baseDirectory = Path.Combine(_startingDirectory, _year.ToString(), strdate);


            switch (network)
            {
                case "keyfuels":
                    return Path.Combine(baseDirectory, "KeyFuels");
                case "texaco":
                    return Path.Combine(baseDirectory, "FastFuel");
                case "fuelgenie":
                    return Path.Combine(baseDirectory, "Fuel Genie");
                case "ukfuel":
                    return Path.Combine(baseDirectory, "UK Fuels");
                case "uk fuel":
                    return Path.Combine(baseDirectory, "UK Fuels");

                default:
                    throw new InvalidOperationException($"Unknown network: {network}");
            }
        }
    }

}
