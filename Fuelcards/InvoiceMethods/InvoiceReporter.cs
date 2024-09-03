using DataAccess.Fuelcards;
using Fuelcards.GenericClassFiles;
using Microsoft.Graph;

namespace Fuelcards.InvoiceMethods
{
    public class InvoiceReporter
    {
        public InvoiceReport CreateNewInvoiceReport(InvoicePDFModel invoice)
        {
            var report = new InvoiceReport();

            EnumHelper.Network ReportType = EnumHelper.NetworkEnumFromString(invoice.CustomerDetails.Network);

            PopulateInvoiceDetails(report, invoice, ReportType);
            CalculateVolumes(report, invoice, ReportType);
            CalculatePrices(report, invoice, ReportType);
            CalculateSummary(report, invoice, ReportType);

            //CalculatePrices(report, invoice, ReportType);
            //CalculateAdditionalDetails(report, invoice, ReportType);
            return report;
        }

        private void CalculateSummary(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network reportType)
        {
            switch (reportType)
            {
                case EnumHelper.Network.Keyfuels:
                    report.RollAvailable = invoice.fixedBox?.FixedPriceRemaining ?? 0;
                    report.Current = invoice.fixedBox?.FixedPriceVolumeForThisPeriod ?? 0;
                    report.Rolled = invoice.fixedBox?.FixedPriceVolumeFromPreviousPeriods ?? 0;
                    report.DieselLifted = InvoiceSummary.Round2(invoice.transactions.Where(e => e.product == EnumHelper.Products.Diesel.ToString()).Sum(e => e.Volume));
                    report.Fixed = invoice.fixedBox?.FixedPriceVolumeUsedOnThisinvoice ?? 0;
                    report.Floating = report.DieselLifted - report.Fixed ?? 0;
                    report.NetTotal = InvoiceSummary.Round2(invoice.totals.Goods);
                    report.Vat = InvoiceSummary.Round2(invoice.totals.VAT);
                    report.Total = InvoiceSummary.Round2(invoice.totals.Total);
                    report.InvNo = invoice.CustomerDetails.InvoiceNumber;
                    report.PayDate = invoice.CustomerDetails.paymentDate;
                    report.Commission = InvoiceSummary.Round2(invoice.transactions.Sum(e => e.Commission));

                    break;
                case EnumHelper.Network.UkFuel:
                    report.RollAvailable = invoice.fixedBox?.FixedPriceRemaining ?? 0;
                    report.Current = invoice.fixedBox?.FixedPriceVolumeForThisPeriod ?? 0;
                    report.Rolled = InvoiceSummary.Round2((invoice.fixedBox?.FixedPriceVolumeFromPreviousPeriods ?? 0) - (invoice.fixedBox?.FixedPriceRemaining ?? 0));
                    report.DieselLifted = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName.ToLower().Contains("diesel")).Sum(e => e.Quantity) + report.Rolled);
                    report.TescoSainsburys = InvoiceSummary.Round2(invoice.rows
           .Where(e => e.productName.ToLower().Contains("tesco") || e.productName.ToLower().Contains("sainsbur"))
           .Sum(e => e.Quantity));
                    if (report.PremDieselVol > 0)report.DieselLifted = report.DieselLifted - report.PremDieselVol;
                    report.Fixed = invoice.fixedBox?.FixedPriceVolumeUsedOnThisinvoice ?? 0;
                    report.Floating = InvoiceSummary.Round2(report.DieselLifted - report.Fixed - report.TescoSainsburys ?? 0);
                    if (report.Floating < 0) report.Floating = 0;
                    report.NetTotal = InvoiceSummary.Round2(invoice.totals.Goods);
                    report.Vat = InvoiceSummary.Round2(invoice.totals.VAT);
                    report.Total = InvoiceSummary.Round2(invoice.totals.Total);
                    report.InvNo = invoice.CustomerDetails.InvoiceNumber;
                    report.PayDate = invoice.CustomerDetails.paymentDate;
                    report.Commission = InvoiceSummary.Round2(invoice.transactions.Sum(e => e.Commission));

                    break;
                case EnumHelper.Network.Texaco:
                    report.RollAvailable = invoice.fixedBox?.FixedPriceRemaining ?? 0;
                    report.Current = invoice.fixedBox?.FixedPriceVolumeForThisPeriod ?? 0;
                    report.Rolled = InvoiceSummary.Round2((invoice.fixedBox?.FixedPriceVolumeFromPreviousPeriods ?? 0) - (invoice.fixedBox?.FixedPriceRemaining ?? 0));
                    report.DieselLifted = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName.ToLower().Contains("diesel")).Sum(e => e.Quantity) + report.Rolled);
                    report.TescoSainsburys = InvoiceSummary.Round2(invoice.rows
           .Where(e => e.productName.ToLower().Contains("tesco") || e.productName.ToLower().Contains("sainsbur"))
           .Sum(e => e.Quantity));
                    if (report.PremDieselVol > 0) report.DieselLifted = report.DieselLifted - report.PremDieselVol;
                    report.Fixed = invoice.fixedBox?.FixedPriceVolumeUsedOnThisinvoice ?? 0;
                    report.Floating = InvoiceSummary.Round2(report.DieselLifted - report.Fixed - report.TescoSainsburys ?? 0);
                    if (report.Floating < 0) report.Floating = 0;
                    report.NetTotal = InvoiceSummary.Round2(invoice.totals.Goods);
                    report.Vat = InvoiceSummary.Round2(invoice.totals.VAT);
                    report.Total = InvoiceSummary.Round2(invoice.totals.Total);
                    report.InvNo = invoice.CustomerDetails.InvoiceNumber;
                    report.PayDate = invoice.CustomerDetails.paymentDate;
                    report.Commission = InvoiceSummary.Round2(invoice.transactions.Sum(e => e.Commission));
                    break;
            }
        }

        private void CalculatePrices(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network reportType)
        {
            switch (reportType)
            {
                case EnumHelper.Network.Keyfuels:
                    report.DieselPrice = invoice.rows.FirstOrDefault(e => e.productName == "Diesel - ")?.NetTotal;
                    report.PetrolPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.NetTotal;
                    report.LubesPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.NetTotal;
                    report.GasoilPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.NetTotal;
                    report.AdbluePrice = invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.NetTotal);
                    report.PremDieselPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.NetTotal;
                    report.SuperUnleadedPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.NetTotal;
                    report.TescoPrice = InvoiceSummary.Round2(invoice.transactions.Where(e => e.productCode == 77 && e.Band == "1").Sum(e => e.Value));
                    break;
                case EnumHelper.Network.UkFuel:
                    report.DieselPrice = invoice.rows.Where(e => e.productName == "Diesel - ").Sum(e => e.NetTotal);
                    report.PetrolPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.NetTotal;
                    report.LubesPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.NetTotal;
                    report.GasoilPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.NetTotal;
                    report.AdbluePrice = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.NetTotal));
                    report.PremDieselPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.NetTotal;
                    report.SuperUnleadedPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.NetTotal;
                    report.TescoPrice = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Tesco").Sum(e => e.NetTotal));
                    report.SainsburysPrice = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Sainsburys").Sum(e => e.NetTotal));
                    break;
                case EnumHelper.Network.Texaco:
                    report.DieselPrice = invoice.rows.Where(e => e.productName == "Diesel - ").Sum(e => e.NetTotal);
                    report.PetrolPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.NetTotal;
                    report.LubesPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.NetTotal;
                    report.GasoilPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.NetTotal;
                    report.AdbluePrice = invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.NetTotal);
                    report.PremDieselPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.NetTotal;
                    report.SuperUnleadedPrice = invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.NetTotal;
                    report.TescoPrice = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Tesco").Sum(e => e.NetTotal));
                    report.SainsburysPrice = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Sainsburys").Sum(e => e.NetTotal));
                    break;
                case EnumHelper.Network.Fuelgenie:
                    break;
                default:
                    break;
            }
        }

        private void PopulateInvoiceDetails(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network InvoiceType)
        {
            report.InvoiceDate = invoice.InvoiceDate;
            report.AccountNo = (int)invoice.CustomerDetails.account;
            report.InvNo = invoice.CustomerDetails.InvoiceNumber;
            report.PayDate = invoice.CustomerDetails.paymentDate;
            report.Network = (int)InvoiceType;
        }
        private void CalculateVolumes(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network reportType)
        {
            switch (reportType)
            {
                case EnumHelper.Network.Keyfuels:
                    report.DieselVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - ").Sum(e => e.Quantity));
                    report.PetrolVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.Quantity);
                    report.LubesVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.Quantity);
                    report.GasoilVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.Quantity);
                    report.AdblueVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.Quantity));
                    report.PremDieselVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.Quantity);
                    report.SuperUnleadedVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.Quantity);
                    report.TescoVol = InvoiceSummary.Round2(invoice.transactions.Where(e => e.productCode == 77 && e.Band == "1").Sum(e => e.Volume));
                    report.Customer = invoice.CustomerDetails.CompanyName;

                    break;
                case EnumHelper.Network.UkFuel:
                    report.DieselVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - " || e.productName == "Diesel").Sum(e => e.Quantity));
                    report.PetrolVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.Quantity);
                    report.LubesVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.Quantity);
                    report.GasoilVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.Quantity);
                    report.AdblueVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.Quantity));
                    report.PremDieselVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.Quantity);
                    report.SuperUnleadedVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.Quantity);
                    report.TescoVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Tesco").Sum(e => e.Quantity));
                    report.SainsburysVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Sainsburys").Sum(e => e.Quantity));
                    report.Customer = invoice.CustomerDetails.CompanyName;

                    break;
                case EnumHelper.Network.Texaco:
                    report.DieselVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - " || e.productName == "Diesel").Sum(e => e.Quantity));
                    report.PetrolVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.ULSP.ToString())?.Quantity);
                    report.LubesVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Lube.ToString())?.Quantity);
                    report.GasoilVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.Gasoil.ToString())?.Quantity);
                    report.AdblueVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == EnumHelper.Products.Adblue.ToString() || e.productName == EnumHelper.Products.AdblueCan.ToString() || e.productName == EnumHelper.Products.PackagedAdblue.ToString())?.Sum(e => e.Quantity));
                    report.PremDieselVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.PremiumDiesel.ToString())?.Quantity);
                    report.SuperUnleadedVol = InvoiceSummary.Round2(invoice.rows.FirstOrDefault(e => e.productName == EnumHelper.Products.SuperUnleaded.ToString())?.Quantity);
                    report.Customer = invoice.CustomerDetails.CompanyName;
                    report.TescoVol = InvoiceSummary.Round2(invoice.rows.Where(e => e.productName == "Diesel - Tesco").Sum(e => e.Quantity));
                    break;
                case EnumHelper.Network.Fuelgenie:
                    break;
                default:
                    break;
            }

        }
    }
}
//private void CalculateVolumes(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network InvoiceType)
//{

//    switch (InvoiceType)
//    {
//        case EnumHelper.Network.Keyfuels:
//            report.DieselVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.PetrolVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.LubesVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.GasoilVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.AdblueVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.PremDieselVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.SuperUnleadedVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.OtherVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.BrushTollVol = CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            double? TotalRetailDieselVolume = CalculateSiteVolume(invoice, EnumHelper.Products.Diesel, InvoiceType);
//            report.TescoVol = CalculateSiteVolume(invoice);
//            if (TotalRetailDieselVolume > report.TescoVol) report.OtherVol = RoundToTwoDecimalPlaces(report.OtherVol + (TotalRetailDieselVolume - report.TescoVol));
//            return;
//        //case EnumHelper.Network.UkFuel:
//        //    report.DieselVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType));
//        //    report.PetrolVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.ULSP, InvoiceType));
//        //    report.LubesVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Lube, InvoiceType));
//        //    report.GasoilVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Gasoil, InvoiceType));
//        //    report.AdblueVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Adblue, InvoiceType));
//        //    report.PremDieselVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.PremiumDiesel, InvoiceType));
//        //    report.SuperUnleadedVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.SuperUnleaded, InvoiceType));
//        //    report.OtherVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Other, InvoiceType));
//        //    report.BrushTollVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Tolls, InvoiceType));
//        //    report.SainsburysVol = RoundToTwoDecimalPlaces(CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.Diesel, InvoiceType));
//        //    //double? TotalRetailDieselVolume2 = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.All, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//        //    report.TescoVol = RoundToTwoDecimalPlaces(CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType));
//        //    //if (TotalRetailDieselVolume2 > report.TescoVol) report.OtherVol = RoundToTwoDecimalPlaces(report.OtherVol + (TotalRetailDieselVolume2 - report.TescoVol));
//        //    return;
//        //case EnumHelper.Network.Texaco:
//        //    report.DieselVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Diesel, InvoiceType));
//        //    report.PetrolVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.ULSP, InvoiceType));
//        //    report.LubesVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Lube, InvoiceType));
//        //    report.GasoilVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Gasoil, InvoiceType));
//        //    report.AdblueVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Adblue, InvoiceType));
//        //    report.PremDieselVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.PremiumDiesel, InvoiceType));
//        //    report.SuperUnleadedVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.SuperUnleaded, InvoiceType));
//        //    report.OtherVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Other, InvoiceType));
//        //    report.BrushTollVol = RoundToTwoDecimalPlaces(CalculateProductVolume(invoice, EnumHelper.Products.Tolls, InvoiceType));
//        //    report.SainsburysVol = RoundToTwoDecimalPlaces(CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.Diesel, InvoiceType));
//        //    //double? TotalRetailDieselVolume2 = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.All, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//        //    report.TescoVol = RoundToTwoDecimalPlaces(CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType));
//        //    //if (TotalRetailDieselVolume2 > report.TescoVol) report.OtherVol = RoundToTwoDecimalPlaces(report.OtherVol + (TotalRetailDieselVolume2 - report.TescoVol));
//        //    break;
//        //case EnumHelper.Network.Fuelgenie:
//        //    break;
//        default:
//            break;
//    }





//        }
//        public void CalculateSupermarkets(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network InvoiceType)
//        {
//            report.TescoVol = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);

//            if (invoice.InvoiceSummary.Networks[0] == EnumHelper.Network.Keyfuels.ToString())
//            {
//                var totalRetail = (double)CalculateProductPrice(invoice, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                if (totalRetail > report.TescoPrice)
//                {
//                    report.OthersPrice = RoundToTwoDecimalPlaces((double)totalRetail - (double)report.TescoPrice);
//                    report.OtherVol = RoundToTwoDecimalPlaces((double)CalculateOtherVol(invoice, EnumHelper.Products.TescoDieselNewDiesel, report.TescoVol));
//                }
//            }
//            else if (invoice.InvoiceSummary.Networks[0] == EnumHelper.Network.UkFuel.ToString())
//            {
//                report.DieselVol = report.DieselVol - report.TescoVol;
//            }
//            else
//            {
//                report.SainsburysVol = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                if (report.SainsburysVol > 0)
//                {
//                    report.SainsburysPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);

//                }
//            }



//        }

//        private void CalculatePrices(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network InvoiceType)
//        {
//            switch (InvoiceType)
//            {
//                case EnumHelper.Network.Keyfuels:
//                    report.DieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Diesel, InvoiceType);
//                    report.PetrolPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.ULSP, InvoiceType);
//                    report.LubesPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Lube, InvoiceType);
//                    report.GasoilPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Gasoil, InvoiceType);
//                    report.AdbluePrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Adblue, InvoiceType);
//                    report.PremDieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.PremiumDiesel, InvoiceType);
//                    report.SuperUnleadedPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.SuperUnleaded, InvoiceType);
//                    report.OthersPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Other, InvoiceType);
//                    report.BrushTollPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Tolls, InvoiceType);
//                    report.TescoPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    var AllRetailDieselPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.All, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    if (AllRetailDieselPrice > report.TescoPrice)
//                    {
//                        report.OthersPrice = RoundToTwoDecimalPlaces(AllRetailDieselPrice - report.TescoPrice);
//                    }
//                    break;
//                case EnumHelper.Network.UkFuel:
//                    report.DieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Diesel, InvoiceType);
//                    report.PetrolPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.ULSP, InvoiceType);
//                    report.LubesPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Lube, InvoiceType);
//                    report.GasoilPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Gasoil, InvoiceType);
//                    report.AdbluePrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Adblue, InvoiceType);
//                    report.PremDieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.PremiumDiesel, InvoiceType);
//                    report.SuperUnleadedPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.SuperUnleaded, InvoiceType);
//                    report.OthersPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Other, InvoiceType);
//                    report.BrushTollPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Tolls, InvoiceType);
//                    report.TescoPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    report.SainsburysPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    //var AllRetailDieselPrice2 = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.All, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    //if(AllRetailDieselPrice2 > 0)
//                    //{
//                    //    string Theory = "Turns out i use this. If you see this and its never hit then remove it in Uk Fuels";
//                    //}
//                    //if (AllRetailDieselPrice2 > report.TescoPrice)
//                    //{
//                    //    report.OthersPrice = RoundToTwoDecimalPlaces(AllRetailDieselPrice2 - report.TescoPrice);
//                    //}
//                    break;
//                case EnumHelper.Network.Texaco:
//                    report.DieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Diesel, InvoiceType);
//                    report.PetrolPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.ULSP, InvoiceType);
//                    report.LubesPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Lube, InvoiceType);
//                    report.GasoilPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Gasoil, InvoiceType);
//                    report.AdbluePrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Adblue, InvoiceType);
//                    report.PremDieselPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.PremiumDiesel, InvoiceType);
//                    report.SuperUnleadedPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.SuperUnleaded, InvoiceType);
//                    report.OthersPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Other, InvoiceType);
//                    report.BrushTollPrice = (double)CalculateProductPrice(invoice, EnumHelper.Products.Tolls, InvoiceType);
//                    report.TescoPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    report.SainsburysPrice = (double)CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                    break;
//                case EnumHelper.Network.Fuelgenie:
//                    break;
//                default:
//                    break;
//            }
//        }

//        private void CalculateAdditionalDetails(InvoiceReport report, InvoicePDFModel invoice, EnumHelper.Network InvoiceType)
//        {
//            switch (InvoiceType)
//            {
//                case EnumHelper.Network.Keyfuels:
//                    report.DieselLifted = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.InvoiceDetail.Where(e => e.Product == EnumHelper.Products.Diesel.ToString()).Sum(e => e.Quantity)), 2));
//                    report.Commission = RoundToTwoDecimalPlaces(invoice.InvoiceDetail.Sum(e => e.Commission));
//                    if (invoice.RolloverVolumeTotal is not null)
//                    {
//                        report.PrevRolled = RoundToTwoDecimalPlaces((double)invoice.RolloverVolumeTotal);
//                    }
//                    report.Rolled = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedVolumeFromPrevious));
//                    report.Current = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedPriceVolumeForPeriod));
//                    report.Fixed = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedVolumeUsedOnCurrent));
//                    report.Floating = RoundToTwoDecimalPlaces((double)report.DieselLifted - (double)report.Fixed);
//                    report.NetTotal = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Goods);
//                    report.Vat = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Vat);
//                    report.Total = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Total);
//                    if (report.Commission > 0) report.ComPayable = "I-fuelcards";
//                    break;
//                case EnumHelper.Network.UkFuel:
//                    report.DieselLifted = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.InvoiceDetail.Where(e => e.Product == EnumHelper.Products.Diesel.ToString()).Sum(e => e.Quantity)), 2));
//                    report.Commission = RoundToTwoDecimalPlaces(invoice.InvoiceDetail.Sum(e => e.Commission));
//                    if (invoice.RolloverVolumeTotal is not null)
//                    {
//                        report.PrevRolled = RoundToTwoDecimalPlaces((double)invoice.RolloverVolumeTotal);
//                    }
//                    double? DieselForFixOrFloating = report.DieselLifted - report.TescoVol - report.SainsburysVol;

//                    report.Rolled = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedVolumeUsedOnCurrent));
//                    report.Current = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedPriceVolumeForPeriod));
//                    report.Fixed = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedVolumeUsedOnCurrent));
//                    report.Floating = RoundToTwoDecimalPlaces((double)DieselForFixOrFloating - (double)report.Fixed);
//                    report.TescoSainsburys = RoundToTwoDecimalPlaces(report.TescoVol + report.SainsburysVol);
//                    report.NetTotal = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Goods);
//                    report.Vat = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Vat);
//                    report.Total = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Total);
//                    if (report.Commission > 0) report.ComPayable = "I-fuelcards";
//                    break;
//                case EnumHelper.Network.Texaco:
//                    report.DieselLifted = Convert.ToDouble(Math.Round(Convert.ToDecimal(invoice.InvoiceDetail.Where(e => e.Product == EnumHelper.Products.Diesel.ToString()).Sum(e => e.Quantity)), 2));
//                    report.Commission = RoundToTwoDecimalPlaces(invoice.InvoiceDetail.Sum(e => e.Commission));
//                    if (invoice.RolloverVolumeTotal is not null)
//                    {
//                        report.PrevRolled = RoundToTwoDecimalPlaces((double)invoice.RolloverVolumeTotal);
//                    }
//                    double? DieselForFixOrFloating3 = report.DieselLifted - report.TescoVol - report.SainsburysVol;

//                    report.Rolled = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedVolumeFromPrevious));
//                    report.Current = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedPriceVolumeForPeriod));
//                    report.Fixed = RoundToTwoDecimalPlaces(Convert.ToDouble(invoice.InvoiceSummary.FixedPriceVolumeForPeriod));
//                    report.Floating = RoundToTwoDecimalPlaces((double)DieselForFixOrFloating3 - (double)report.Fixed);
//                    report.TescoSainsburys = RoundToTwoDecimalPlaces(report.TescoVol + report.SainsburysVol);
//                    report.NetTotal = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Goods);
//                    report.Vat = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Vat);
//                    report.Total = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Total);
//                    if (report.Commission > 0) report.ComPayable = "I-fuelcards";
//                    break;
//                case EnumHelper.Network.Fuelgenie:
//                    break;
//                default:
//                    break;
//            }

//        }

//        private double CalculateProductVolume(InvoicePDFModel invoice, EnumHelper.Products product, EnumHelper.Network InvoiceType)
//        {

//            if (product == EnumHelper.Products.Adblue || product == EnumHelper.Products.AdblueCan || product == EnumHelper.Products.PackagedAdblue)
//            {
//                return RoundToTwoDecimalPlaces(invoice.rows.Where(e => e.productName == EnumHelper.ProductsToString(EnumHelper.Products.Adblue) || e.productName == EnumHelper.ProductsToString(EnumHelper.Products.AdblueCan) || e.productName == EnumHelper.ProductsToString(EnumHelper.Products.PackagedAdblue))
//               .Sum(e => Convert.ToDouble(e.Quantity)));
//            }
//            if (InvoiceType == EnumHelper.Network.UkFuel && product == EnumHelper.Products.Diesel)
//            {
//                var TotalDiesel = RoundToTwoDecimalPlaces(invoice.rows.Where(e => e.productName == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.Quantity)));

//                var TotalTesco = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                var TotalSainsburys = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                return TotalDiesel - TotalTesco - TotalSainsburys;
//            }
//            if (InvoiceType == EnumHelper.Network.Texaco && product == EnumHelper.Products.Diesel)
//            {
//                var TotalDiesel = RoundToTwoDecimalPlaces(invoice.rows.Where(e => e.productName == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.Quantity)));

//                var TotalTesco = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                var TotalSainsburys = CalculateSiteVolume(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                return TotalDiesel - TotalTesco - TotalSainsburys;
//            }
//            return RoundToTwoDecimalPlaces(invoice.rows.Where(e => e.productName == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.Quantity)));
//        }

//        private double CalculateSiteVolume(InvoicePDFModel invoice, EnumHelper.RetailDiesel retailMerchant, EnumHelper.Products product, EnumHelper.Network InvoiceType)
//        {
//            switch (InvoiceType)
//            {
//                case EnumHelper.Network.Keyfuels:
//                    if (retailMerchant == EnumHelper.RetailDiesel.All)
//                    {
//                        return RoundToTwoDecimalPlaces(invoice.rows.Where(e => e.productName == "Retail Diesel")
//                        .Sum(e => Convert.ToDouble(e.Quantity)));
//                    }
//                    return RoundToTwoDecimalPlaces(invoice.transactions.Where(e => e.productCode == 77).Sum(e => e.Volume));


//                case EnumHelper.Network.UkFuel:
//                    product = EnumHelper.Products.Diesel;
//                    break;
//                case EnumHelper.Network.Texaco:
//                    product = EnumHelper.Products.Diesel;
//                    break;
//                case EnumHelper.Network.Fuelgenie:
//                    break;
//                default:
//                    break;
//            }
//            return RoundToTwoDecimalPlaces(invoice.rows
//                .Where(e => e.Brand != null && e.Brand.ToLower() == retailMerchant.ToString().ToLower() && e.Product == EnumHelper.ProductsToString(product))
//                .Sum(e => e.Quantity));

//            //return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//            //    .Where(e => e.Brand.ToLower() == retailMerchant.ToString().ToLower() && e.Product == EnumHelper.ProductsToString(product))
//            //    .Sum(e => e.Quantity));

//            //return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//            //    .Where(e => e.NameOfSite.ToLower().Contains(siteName.ToLower()) && e.Product == EnumHelper.ProductsToString(product))
//            //    .Sum(e => e.Quantity));
//        }


//        private double? CalculateProductPrice(InvoicePDFModel invoice, EnumHelper.Products product, EnumHelper.Network InvoiceType)
//        {
//            if (product == EnumHelper.Products.Adblue || product == EnumHelper.Products.AdblueCan || product == EnumHelper.Products.PackagedAdblue)
//            {
//                return RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//               .Where(e => e.product == EnumHelper.ProductsToString(EnumHelper.Products.Adblue) || e.product == EnumHelper.ProductsToString(EnumHelper.Products.AdblueCan) || e.product == EnumHelper.ProductsToString(EnumHelper.Products.PackagedAdblue))
//               .Sum(e => Convert.ToDouble(e.NetTotal)));
//            }
//            if (InvoiceType == EnumHelper.Network.UkFuel && product == EnumHelper.Products.Diesel)
//            {
//                var TotalDiesel = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//                .Where(e => e.product == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.NetTotal)));

//                var TotalTesco = CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                var TotalSainsburys = CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                return RoundToTwoDecimalPlaces(TotalDiesel - TotalTesco - TotalSainsburys);
//            }
//            if (InvoiceType == EnumHelper.Network.Texaco && product == EnumHelper.Products.Diesel)
//            {
//                var TotalDiesel = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//                .Where(e => e.product == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.NetTotal)));

//                var TotalTesco = CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Tesco, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                var TotalSainsburys = CalculateSitePrice(invoice, EnumHelper.RetailDiesel.Sainsburys, EnumHelper.Products.TescoDieselNewDiesel, InvoiceType);
//                return RoundToTwoDecimalPlaces(TotalDiesel - TotalTesco - TotalSainsburys);
//            }
//            return RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//                .Where(e => e.product == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.NetTotal)));
//        }
//        private double? CalculateOtherVol(InvoicePDFModel invoice, EnumHelper.Products product, double? TescoVol)
//        {
//            var TotalVol = RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//                .Where(e => e.product == EnumHelper.ProductsToString(product))
//                .Sum(e => Convert.ToDouble(e.Quantity)));
//            if (TotalVol > TescoVol)
//            {
//                return TotalVol - TescoVol;
//            }
//            return 0;
//        }

//        private double? CalculateSitePrice(InvoicePDFModel invoice, EnumHelper.RetailDiesel retailmerchant, EnumHelper.Products product, EnumHelper.Network InvoiceType)
//        {
//            switch (InvoiceType)
//            {
//                case EnumHelper.Network.Keyfuels:
//                    if (retailmerchant == EnumHelper.RetailDiesel.All)
//                    {
//                        return RoundToTwoDecimalPlaces(invoice.InvoiceSummary.Productsummaries
//                        .Where(e => e.product == EnumHelper.Products.TescoDieselNewDiesel.ToString())
//                        .Sum(e => Convert.ToDouble(e.NetTotal)));
//                    }
//                    else if (retailmerchant == EnumHelper.RetailDiesel.Tesco)
//                    {
//                        return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//                    .Where(e => e.Brand != null && e.Brand == EnumHelper.RetailDiesel.Tesco.ToString())
//                    .Sum(e => Convert.ToDouble(e.TotalPrice)));
//                    }
//                    else
//                    {
//                        throw new ArgumentException($"Retail Merchant - '{retailmerchant}' is not an expected merchant for Keyfuels.");
//                    }


//                case EnumHelper.Network.UkFuel:

//                    if (product == EnumHelper.Products.TescoDieselNewDiesel) product = EnumHelper.Products.Diesel;

//                    //return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//                    //    .Where(e => e.Brand.ToLower().Contains(retailmerchant.ToString().ToLower()) && e.Product == EnumHelper.ProductsToString(product))
//                    //    .Sum(e => e.TotalPrice));
//                    return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//                        .Where(e => e.Brand != null && e.Brand.ToLower().Contains(retailmerchant.ToString().ToLower()) && e.Product == EnumHelper.ProductsToString(product))
//                        .Sum(e => e.TotalPrice));

//                case EnumHelper.Network.Texaco:
//                    if (product == EnumHelper.Products.TescoDieselNewDiesel) product = EnumHelper.Products.Diesel;
//                    return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//                        .Where(e => e.Brand != null && e.Brand.ToLower().Contains(retailmerchant.ToString().ToLower()) && e.Product == EnumHelper.ProductsToString(product))
//                        .Sum(e => e.TotalPrice));

//                    //return RoundToTwoDecimalPlaces(invoice.InvoiceDetail
//                    //    .Where(e => e.Brand.ToLower().Contains(retailmerchant.ToString().ToLower()) && e.Product == EnumHelper.ProductsToString(product))
//                    //    .Sum(e => e.TotalPrice));
//                    break;
//                case EnumHelper.Network.Fuelgenie:
//                    break;
//                default:
//                    break;
//            }
//            return null;
//        }

//        private double RoundToTwoDecimalPlaces(double? value)
//        {
//            if (!value.HasValue) return 0;
//            return Math.Round((double)value, 2);
//        }
//    }
//}
