using System;

namespace Fuelcards.GenericClassFiles
{

    public class EnumHelper
    {
        public enum Products
        {
            Diesel,
            Gasoil,
            ULSP,
            Lube,
            Adblue,
            SuperUnleaded,
            TescoDieselNewDiesel,
            PackagedAdblue,
            PremiumDiesel,
            Tolls,
            Unknown,
            Other,
            AdblueCan,
            LPG,
            Goods,
            HVO,
            Card,
            CardStopManagementFee,
            EDISTDSignle,
            StockNotification,
            EmailPinCharge,
            Brush,
            CarWash,
            MinimumStockCharge
        };
        public enum RetailDiesel
        {
            Morrisons,
            Tesco,
            Sainsburys,
            All,
            None
        }
        public enum Network
        {
            Keyfuels,
            UkFuel,
            Texaco,
            Fuelgenie
        }
        public enum CustomerType
        {
            Fix,
            Floating,
            ExpiredFixWithVolume
        };
        public static Network NetworkEnumFromString(string network)
        {
            switch (network.ToLower())
            {
                case "0": return Network.Keyfuels;
                case "1": return Network.UkFuel;
                case "2": return Network.Texaco;
                case "3": return Network.Fuelgenie;
                case "keyfuel": return Network.Keyfuels;
                case "keyfuels": return Network.Keyfuels;
                case "uk fuel": return Network.UkFuel;
                case "ukfuel": return Network.UkFuel;
                case "ukfuels": return Network.UkFuel;
                case "uk fuels": return Network.UkFuel;
                case "uk": return Network.UkFuel;
                case "texaco": return Network.Texaco;
                case "fuelgenie": return Network.Fuelgenie;
                default: throw new ArgumentException($"The network passed '{network}' did not match any of the options in the NetworkEnumFromString.");
            }
        }
        public static string NetworkToNumericalString(Network network)
        {
            switch (network)
            {
                case Network.Keyfuels:
                    return "0";
                case Network.UkFuel:
                    return "1";
                case Network.Texaco:
                    return "2";
                case Network.Fuelgenie:
                    return "3";
                default:
                    throw new ArgumentException($"Network to numerical string failed.");
            }
        }
        public static Network NetworkEnumFromInt(int? value)
        {
            switch (value)
            {
                case 0: return Network.Keyfuels;
                case 1: return Network.UkFuel;
                case 2: return Network.Texaco;
                case 3: return Network.Fuelgenie;
            }
            throw new ArgumentException($"network id {value} is not a valid network. Only integer values 0-3 currently are attached to networks");
        }
        public static string RetailDieselToString(RetailDiesel market)
        {
            switch (market)
            {
                case RetailDiesel.Tesco:
                    return "tesco";
                case RetailDiesel.Morrisons:
                    return "morrisons";
                case RetailDiesel.Sainsburys:
                    return "sainsburys";
                default:
                    throw new ArgumentException($"Retail diesel to string only caters for Tesco,morrisons and sainsburys. You have provided a value of {market.ToString()}");
            }
        }
        public static string ProductsToString(Products product)
        {
            switch (product)
            {
                case Products.Diesel:
                    return "Diesel";
                case Products.Gasoil:
                    return "Gasoil";
                case Products.ULSP:
                    return "ULSP";
                case Products.Lube:
                    return "Lube";
                case Products.Adblue:
                    return "Adblue";
                case Products.SuperUnleaded:
                    return "SuperUnleaded";
                case Products.TescoDieselNewDiesel:
                    return "TescoDieselNewDiesel";
                case Products.PackagedAdblue:
                    return "PackagedAdblue";
                case Products.PremiumDiesel:
                    return "PremiumDiesel";
                case Products.Tolls:
                    return "Tolls";
                case Products.Unknown:
                    return "Unknown";
                case Products.Other:
                    return "Other";
                case Products.AdblueCan:
                    return "AdblueCan";
                default:
                    throw new ArgumentException("Invalid enum value");
            }
        }
        public static RetailDiesel BandMeaning(EnumHelper.Network network, int Band, int productcode)
        {
            switch (network)
            {
                case Network.Keyfuels:
                    if (productcode == 77) return RetailDiesel.Tesco;
                    switch (Band)
                    {
                        case 4:
                            return RetailDiesel.Morrisons;
                    }
                    return RetailDiesel.None;
                case Network.UkFuel:
                    switch (Band)
                    {
                        case 3:
                            return RetailDiesel.Morrisons;
                        case 8:
                            return RetailDiesel.Sainsburys;
                        case 9:
                            return RetailDiesel.Tesco;
                    }
                    return RetailDiesel.None;
                case Network.Texaco:
                    switch (Band)
                    {
                        case 3:
                            return RetailDiesel.Morrisons;
                        case 8:
                            return RetailDiesel.Sainsburys;
                        case 9:
                            return RetailDiesel.Tesco;
                    }
                    return RetailDiesel.None;
                case Network.Fuelgenie:
                    return RetailDiesel.None;
                default:
                    return RetailDiesel.None;
            }
        }
        public static EnumHelper.Products? GetProductFromProductCode(int? productCode, EnumHelper.Network network)
        {
            switch (network)
            {
                case EnumHelper.Network.Keyfuels:
                    switch (productCode)
                    {
                        case 1:
                            return EnumHelper.Products.Diesel;
                        case 52:
                        case 334:
                        case 351:
                        case 352:
                        case 469:
                        case 470:
                        case 471:
                        case 472:
                        case 473:
                        case 474:
                            return EnumHelper.Products.ULSP;
                        case 3:
                        case 244:
                        case 343:
                        case 349:
                        case 350:
                            return EnumHelper.Products.Gasoil;
                        case 2:
                        case 15:
                        case 246:
                        case 331:
                        case 347:
                        case 348:
                        case 428:
                        case 455:
                        case 456:
                            return EnumHelper.Products.Lube;
                        case 279:
                        case 361:
                        case 362:
                        case 425:
                        case 489:
                            return EnumHelper.Products.Adblue;

                        case 54:
                        case 74:
                        case 108:
                        case 332:
                        case 336:
                        case 338:
                        case 339:
                        case 368:
                        case 415:
                        case 453:
                        case 476:
                        case 477:
                            return EnumHelper.Products.SuperUnleaded;
                        case 77:
                        case 276:
                            return EnumHelper.Products.TescoDieselNewDiesel;
                        case 364:
                        case 365:
                        case 459:
                        case 536:
                            return EnumHelper.Products.Unknown;
                        case 260:
                        case 435:
                        case 487:
                        case 488:
                            return EnumHelper.Products.PremiumDiesel;

                        case 465:
                            return EnumHelper.Products.Tolls;
                        case 448: return EnumHelper.Products.AdblueCan;
                        case 5: return EnumHelper.Products.Card;
                        case 208: return EnumHelper.Products.CardStopManagementFee;
                        case 261: return EnumHelper.Products.EDISTDSignle;
                        case 241: return EnumHelper.Products.StockNotification;
                        case 280: return EnumHelper.Products.EmailPinCharge;
                        case 7: return EnumHelper.Products.Brush;
                        case 576: return EnumHelper.Products.MinimumStockCharge;
                        default: return EnumHelper.Products.Other;

                    }
                case EnumHelper.Network.UkFuel:
                    switch (productCode)
                    {
                        case 1:
                        case 70:
                            return EnumHelper.Products.Diesel;
                        case 7: return EnumHelper.Products.LPG;
                        case 24:
                            return EnumHelper.Products.Goods;
                        case 45:
                            return EnumHelper.Products.CarWash;
                        case 42: return EnumHelper.Products.HVO;
                        case 2: return EnumHelper.Products.ULSP;
                        case 5: return EnumHelper.Products.Gasoil;
                        case 6: return EnumHelper.Products.Lube;
                        case 8: return EnumHelper.Products.Adblue;
                        case 18: return EnumHelper.Products.PackagedAdblue;
                        case 3:
                        case 31:
                            return EnumHelper.Products.SuperUnleaded;
                        case 30: return EnumHelper.Products.PremiumDiesel;
                        default: return EnumHelper.Products.Other;

                    }
                case EnumHelper.Network.Texaco:
                    switch (productCode)
                    {
                        case 1:
                        case 70:
                            return EnumHelper.Products.Diesel;
                        case 3: return EnumHelper.Products.SuperUnleaded;

                        case 2: return EnumHelper.Products.ULSP;
                        case 5: return EnumHelper.Products.Gasoil;
                        case 6: return EnumHelper.Products.Lube;
                        case 8:
                        case 18:
                            return EnumHelper.Products.Adblue;
                        case 30: return EnumHelper.Products.PremiumDiesel;
                        default: return EnumHelper.Products.Other;
                    }
                case EnumHelper.Network.Fuelgenie:
                    switch (productCode)
                    {
                        case 5:
                            return EnumHelper.Products.Diesel;
                        case 1:
                        case 20:
                        case 21:
                        case 22:
                        case 24:
                            return EnumHelper.Products.ULSP;
                        case 28:
                            return EnumHelper.Products.Gasoil;
                        case 0:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            return EnumHelper.Products.Lube;
                        case 3:
                            return EnumHelper.Products.Adblue;
                        case 2:
                            return EnumHelper.Products.PackagedAdblue;

                        default: return EnumHelper.Products.Other;
                    }

                default:
                    break;
            }
            return null;
        }
        public enum InvoiceFormatType
        {
            Default,
            Pan,
        }
        public enum InvoiceFrequency
        {
            Weekly,
            Monthly
        };

    }
}