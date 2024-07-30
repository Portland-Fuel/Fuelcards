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
                    transactions.transactionId = item.TransactionId;
                    transactions.controlId = item.ControlId;
                    transactions.portlandId = item.PortlandId;
                    transactions.transactionNumber = item.TransactionNumber;
                    transactions.transactionDate = item.TransactionDate;
                    transactions.transactionTime = item.TransactionTime;
                    transactions.siteCode = item.SiteCode;
                    transactions.cardNumber = item.CardNumber.ToString();
                    transactions.customerCode = item.CustomerCode;
                    transactions.customerAc = item.CustomerAc;
                    transactions.primaryRegistration = item.PrimaryRegistration;
                    transactions.mileage = item.Mileage;
                    transactions.fleetNumber = item.FleetNumber;
                    transactions.productCode = item.ProductCode;
                    transactions.quantity = item.Quantity;
                    transactions.sign = item.Sign;
                    transactions.cost = item.Cost;
                    transactions.costSign = item.CostSign;
                    transactions.accurateMileage = item.AccurateMileage;
                    transactions.cardRegistration = item.CardRegistration;
                    transactions.transactonRegistration = item.TransactonRegistration;
                    transactions.invoiced = item.Invoiced;
                    transactions.network = 0;
                    TotalTransactions.Add(transactions);
                }
            }
            if (uk is not null)
            {
                foreach (var item in uk)
                {
                    GenericTransactionFile transactions = new();
                    transactions.transactionId = item.TransactionId;
                    transactions.controlId = item.ControlId;
                    transactions.portlandId = item.PortlandId;
                    transactions.transactionNumber = item.TranNoItem;
                    transactions.transactionDate = item.TranDate;
                    transactions.transactionTime = item.TranTime;
                    transactions.siteCode = (int)item.Site;
                    transactions.cardNumber = item.PanNumber.ToString();
                    transactions.customerCode = (int)item.Customer;
                    transactions.customerAc = (short)item.Customer;
                    transactions.primaryRegistration = item.Registration;
                    transactions.mileage = item.Mileage;
                    transactions.productCode = item.ProdNo;
                    transactions.quantity = item.Quantity;
                    transactions.cost = item.Price;
                    transactions.accurateMileage = item.Mileage.ToString();
                    transactions.cardRegistration = item.Registration;
                    transactions.invoiced = null;
                    transactions.network = 1;
                    TotalTransactions.Add(transactions);
                }
            }
            if (tx is not null)
            {
                foreach (var item in tx)
                {
                    GenericTransactionFile transactions = new();
                    transactions.transactionId = item.TransactionId;
                    transactions.controlId = (int)item.ControlId;
                    transactions.portlandId = item.PortlandId;
                    transactions.transactionNumber = item.TranNoItem;
                    transactions.transactionDate = item.TranDate;
                    transactions.transactionTime = item.TranTime;
                    transactions.siteCode = (int)item.Site;
                    transactions.cardNumber = item.IsoNumber.ToString() + item.Customer.ToString() + item.CardNo.ToString();
                    transactions.customerCode = (int)item.Customer;
                    transactions.primaryRegistration = item.Registration;
                    transactions.mileage = item.Mileage;
                    transactions.productCode = (short)item.ProdNo;
                    transactions.quantity = item.Quantity;
                    transactions.cost = item.Price;
                    transactions.accurateMileage = item.Mileage.ToString();
                    transactions.cardRegistration = item.Registration;
                    transactions.transactonRegistration = item.Registration;
                    transactions.invoiced = null;
                    transactions.network = 2;
                    TotalTransactions.Add(transactions);
                }
            }
            if (fg is not null)
            {
                foreach (var item in fg)
                {
                    GenericTransactionFile transactions = new();
                    transactions.transactionId = item.TransactionId;
                    transactions.controlId = 0;
                    transactions.portlandId = item.PortlandId;
                    transactions.transactionNumber = item.TransactionId;
                    transactions.transactionDate = item.TransactionDate;
                    transactions.transactionTime = item.TransactionTime;
                    transactions.siteCode = (int)item.MerchantId;
                    transactions.cardNumber = item.PanNumber;
                    transactions.customerCode = (int)item.PortlandId;
                    transactions.customerAc = (short)item.CustomerNumber;
                    transactions.primaryRegistration = item.RegNo;
                    transactions.mileage = item.Mileage;
                    transactions.productCode = item.ProductCode;
                    transactions.quantity = item.ProductQuantity;
                    transactions.cost = item.NetAmount;
                    transactions.accurateMileage = item.Mileage.ToString();
                    transactions.cardRegistration = item.RegNo;
                    transactions.transactonRegistration = item.RegNo;
                    transactions.invoiced = null;
                    transactions.network = 3;
                    TotalTransactions.Add(transactions);
                }
            }

            return TotalTransactions;
        }

    }
}
