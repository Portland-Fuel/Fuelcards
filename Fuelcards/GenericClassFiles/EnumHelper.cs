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
            Floating
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

    }
}
