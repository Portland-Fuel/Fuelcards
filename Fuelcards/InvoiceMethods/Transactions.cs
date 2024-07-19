using DataAccess.Fuelcards;
using Fuelcards.Models;
using Fuelcards.Repositories;
using Xero.NetStandard.OAuth2.Client;

namespace Fuelcards.InvoiceMethods
{
    public class Transactions
    {
       
        
        public static DateOnly GetMostRecentMonday(DateOnly currentDate)
        {
            //if (network == "Fuelgenie") return currentDate;
            int daysUntilMonday = ((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateOnly mostRecentMonday = currentDate.AddDays(-daysUntilMonday);
            return mostRecentMonday;
        }
        public static List<GenericTransactionFile> TurnTransactionIntoGeneric(List<KfE1E3Transaction>? kf, List<UkfTransaction>? uk, List<TexacoTransaction>? tx, List<FgTransaction>? fg)
        {

            List<GenericTransactionFile> TotalTransactions = [];
            if (kf is not null)
            {
                foreach (var item in kf)
                {
                    GenericTransactionFile transactions = new();
                    transactions.TransactionId = item.TransactionId;
                    transactions.ControlId = item.ControlId;
                    transactions.PortlandId = item.PortlandId;
                    transactions.TransactionNumber = item.TransactionNumber;
                    transactions.TransactionDate = item.TransactionDate;
                    transactions.TransactionTime = item.TransactionTime;
                    transactions.SiteCode = item.SiteCode;
                    transactions.CardNumber = item.CardNumber;
                    transactions.CustomerCode = item.CustomerCode;
                    transactions.CustomerAc = item.CustomerAc;
                    transactions.PrimaryRegistration = item.PrimaryRegistration;
                    transactions.Mileage = item.Mileage;
                    transactions.FleetNumber = item.FleetNumber;
                    transactions.ProductCode = item.ProductCode;
                    transactions.Quantity = item.Quantity;
                    transactions.Sign = item.Sign;
                    transactions.Cost = item.Cost;
                    transactions.CostSign = item.CostSign;
                    transactions.AccurateMileage = item.AccurateMileage;
                    transactions.CardRegistration = item.CardRegistration;
                    transactions.TransactonRegistration = item.TransactonRegistration;
                    transactions.Invoiced = item.Invoiced;
                    transactions.network = 0;
                    TotalTransactions.Add(transactions);
                }
            }
            if (uk is not null)
            {
                foreach (var item in uk)
                {
                    GenericTransactionFile transactions = new();
                    transactions.TransactionId = item.TransactionId;
                    transactions.ControlId = item.ControlId;
                    transactions.PortlandId = item.PortlandId;
                    transactions.TransactionNumber = item.TranNoItem;
                    transactions.TransactionDate = item.TranDate;
                    transactions.TransactionTime = item.TranTime;
                    transactions.SiteCode = (int)item.Site;
                    transactions.CardNumber = item.PanNumber;
                    transactions.CustomerCode = (int)item.Customer;
                    transactions.CustomerAc = (short)item.Customer;
                    transactions.PrimaryRegistration = item.Registration;
                    transactions.Mileage = item.Mileage;
                    transactions.ProductCode = item.ProdNo;
                    transactions.Quantity = item.Quantity;
                    transactions.Cost = item.Price;
                    transactions.AccurateMileage = item.Mileage.ToString();
                    transactions.CardRegistration = item.Registration;
                    transactions.Invoiced = null;
                    transactions.network = 1;
                    TotalTransactions.Add(transactions);
                }
            }
            if (tx is not null)
            {
                foreach (var item in tx)
                {
                    GenericTransactionFile transactions = new();
                    transactions.TransactionId = item.TransactionId;
                    transactions.ControlId = (int)item.ControlId;
                    transactions.PortlandId = item.PortlandId;
                    transactions.TransactionNumber = item.TranNoItem;
                    transactions.TransactionDate = item.TranDate;
                    transactions.TransactionTime = item.TranTime;
                    transactions.SiteCode = (int)item.Site;
                    transactions.CardNumber = Convert.ToDecimal(item.IsoNumber.ToString() + item.Customer.ToString() + item.CardNo.ToString());
                    transactions.CustomerCode = (int)item.Customer;
                    transactions.PrimaryRegistration = item.Registration;
                    transactions.Mileage = item.Mileage;
                    transactions.ProductCode = (short)item.ProdNo;
                    transactions.Quantity = item.Quantity;
                    transactions.Cost = item.Price;
                    transactions.AccurateMileage = item.Mileage.ToString();
                    transactions.CardRegistration = item.Registration;
                    transactions.TransactonRegistration = item.Registration;
                    transactions.Invoiced = null;
                    transactions.network = 2;
                    TotalTransactions.Add(transactions);
                }
            }
            if (fg is not null)
            {
                foreach (var item in fg)
                {
                    GenericTransactionFile transactions = new();
                    transactions.TransactionId = item.TransactionId;
                    transactions.ControlId = 0;
                    transactions.PortlandId = item.PortlandId;
                    transactions.TransactionNumber = item.TransactionId;
                    transactions.TransactionDate = item.TransactionDate;
                    transactions.TransactionTime = item.TransactionTime;
                    transactions.SiteCode = (int)item.MerchantId;
                    transactions.CardNumber = Convert.ToDecimal(item.PanNumber);
                    transactions.CustomerCode = (int)item.PortlandId;
                    transactions.CustomerAc = (short)item.CustomerNumber;
                    transactions.PrimaryRegistration = item.RegNo;
                    transactions.Mileage = item.Mileage;
                    transactions.ProductCode = item.ProductCode;
                    transactions.Quantity = item.ProductQuantity;
                    transactions.Cost = item.NetAmount;
                    transactions.AccurateMileage = item.Mileage.ToString();
                    transactions.CardRegistration = item.RegNo;
                    transactions.TransactonRegistration = item.RegNo;
                    transactions.Invoiced = null;
                    transactions.network = 3;
                    TotalTransactions.Add(transactions);
                }
            }

            return TotalTransactions;
        }

    }
}
