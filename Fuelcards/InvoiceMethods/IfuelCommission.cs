using Fuelcards.GenericClassFiles;
using Fuelcards.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.AccessControl;
using static Fuelcards.GenericClassFiles.EnumHelper;

namespace Fuelcards.InvoiceMethods
{
    public class IfuelCommission
    {
        public static double CalculateCommission(EnumHelper.Network network, CustomerInvoice invoice, GenericTransactionFile transaction, double? addon)
        {
            if (!invoice.IfuelsCustomer) return 0;
            double commission = 0;
            switch (network)
            {
                case EnumHelper.Network.Keyfuels:
                    if (transaction.band == "9") return commission; // No Commission on band 9
                    switch (transaction.product)
                    {
                        case "Diesel":
                            if (transaction.productCode == 77)
                            {
                                commission = Convert.ToDouble(((transaction.quantity * 0.01) * 0.6));
                                return commission;
                            }
                            else
                            {
                                commission = Convert.ToDouble(Math.Round(Convert.ToDecimal((addon / 100) * transaction.quantity * 0.6), 5));
                                return commission;
                            }

                        case "ULSP":
                            double? profit = transaction.invoicePrice - transaction.cost;
                            commission = Convert.ToDouble(Math.Round(Convert.ToDecimal(profit * 0.6), 5));
                            return commission;
                    }
                    return 0;
                case EnumHelper.Network.UkFuel:
                    switch (transaction.product)
                    {
                        case "Diesel":
                            if (transaction.band == "9" || transaction.band == "8")
                            {
                                commission = Convert.ToDouble((transaction.quantity * 0.01) * 0.6);
                                return commission;
                            }
                            else
                            {
                                commission = Convert.ToDouble(Math.Round(Convert.ToDecimal(((addon-2.95) / 100) * (transaction.quantity / 100) * 0.6), 5));


                                return commission;
                            }


                        case "ULSP":
                            double? profit = transaction.invoicePrice - transaction.cost;
                            commission = Convert.ToDouble(Math.Round(Convert.ToDecimal(profit * 0.6), 5));
                            return commission;
                    }
                    return 0;
                case EnumHelper.Network.Texaco:
                    switch (transaction.product)
                    {
                        case "Diesel":
                            if (transaction.band == "9")
                            {
                                commission = Math.Round(Convert.ToDouble((((transaction.quantity / 100) * 0.01) * 0.6)),5);
                                return commission;
                            }
                            else
                            {
                                // 3,86 is the ifuels cost price. This is added on to the addon in the db call so removing it for the commisison calculation
                                commission = Convert.ToDouble(Math.Round(Convert.ToDecimal(((addon - 3.86) / 100) * (transaction.quantity/100) * 0.6), 5));
                                return commission;
                            }
                        case "ULSP":
                            double? ediValue = (transaction.cost * 1.02) / 100;
                            double? profit = transaction.invoicePrice - ediValue;
                            commission = Convert.ToDouble(Math.Round(Convert.ToDecimal(profit * 0.6), 5));
                            return commission;
                    }
                    return 0;
                case EnumHelper.Network.Fuelgenie:
                    break;
                default:
                    break;
            }
            return 0;

        }
    }
}
