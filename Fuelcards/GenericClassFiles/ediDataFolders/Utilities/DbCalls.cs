using FuelcardModels.DataTypes;
using FuelcardModels.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Portland.Data.Repository.IRepository;
//using Portland.DataAccess.Fuelcards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace FuelCardModels.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DbCalls
    {

        /// <summary>
        /// Retrieves the customer account numbers from the NetworkAccNoToPortlandId table, sets the _accNumbers
        /// </summary>
        /// <param name="_db"></param>
        /// <param name="network"></param>
        /// <returns></returns>
        public static IQueryable<FcNetworkAccNoToPortlandId> SetAccountNumbers(IFuelcardUnitOfWork fuelcardRepo, Network network)
        {
            return fuelcardRepo.Set<FcNetworkAccNoToPortlandId>().Where(f => f.Network == (short)network);
        }

        /// <summary>
        /// Returns a list of the introducers (_ediAccounts) for the given Network 0 - KeyFuels, 1 - UKFuels, 2 - Texaco, 3 - FuelGenie
        /// </summary>
        /// <param name="_db"></param>
        /// <param name="Network"></param>
        /// <returns></returns>
        public static IQueryable<FcRequiredEdiReport> SetEdiAccounts(IFuelcardUnitOfWork _db, Network Network)
        {
            return Network switch
            {
                Network.KeyFuels => _db.FcRequiredEdiReports.Where(f => f.Keyfuels == true),
                Network.UkFuels => _db.FcRequiredEdiReports.Where(f => f.UkFuels == true),
                Network.Texaco => _db.FcRequiredEdiReports.Where(f => f.Texaco == true),
                Network.FuelGenie => _db.FcRequiredEdiReports.Where(f => f.Fuelgenie == true),
                _ => throw new NotImplementedException(),
            };
        }



        /// <summary>
        /// Checks to see if the control record is already in the db
        /// <para>If it is already in this returns false, if it is not in and</para>
        /// <para>needs adding it returns true</para>
        /// </summary>
        /// <param name="c"></param>
        /// <param name="_db"></param>
        /// <returns>true if the control needs adding</returns>
        /// <returns>false if it already in the database</returns>
        public static bool CompareControlAgainstDb(FcControl c, IFuelcardUnitOfWork _db)
        {
            FcControl t = _db.FcControl.GetFirstOrDefault(p =>
                //   p.CreationDate == c.CreationDate &&   // These need to be taken out as the file will have a new Creation time and date if it has been copied accross from L Drive on a different day / time
                //   p.CreationTime == c.CreationTime &&
                p.RecordCount == c.RecordCount &&
                p.TotalQuantity == c.TotalQuantity &&
                p.TotalCost == c.TotalCost &&
                p.CustomerCode == c.CustomerCode &&
                p.CustomerAc == c.CustomerAc &&
                p.BatchNumber == c.BatchNumber &&
                p.CostSign == c.CostSign &&
                p.QuantitySign == c.QuantitySign);
            return (t == null);
        }

        /// <summary>
        /// Checks to see if the control record is already in the db
        /// <para>If it is already in this returns false, if it is not in and</para>
        /// <para>needs adding it returns true</para>
        /// </summary>
        /// <param name="c"></param>
        /// <param name="_db"></param>
        /// <returns>true if the control needs adding</returns>
        /// <returns>false if it already in the database</returns>
        public static int? GetControlIdForDummies(FcControl c, IFuelcardUnitOfWork _db)
        {
            FcControl t = _db.FcControl.GetFirstOrDefault(p =>
                //   p.CreationDate == c.CreationDate &&   // These need to be taken out as the file will have a new Creation time and date if it has been copied accross from L Drive on a different day / time
                //   p.CreationTime == c.CreationTime &&
                p.RecordCount == c.RecordCount &&
                p.TotalQuantity == c.TotalQuantity &&
                p.TotalCost == c.TotalCost &&
                p.CustomerCode == c.CustomerCode &&
                p.CustomerAc == c.CustomerAc &&
                p.BatchNumber == c.BatchNumber &&
                p.CostSign == c.CostSign &&
                p.QuantitySign == c.QuantitySign);

            if (t is not null) return t.ControlId;
            return null;
        }



        /// <summary>
        /// Get a list of the required Introducer accounts from the RequiredEdiReport table
        /// </summary>
        /// <param name="_ediAccounts"></param>
        /// <returns></returns>
        public static List<int> GetListOfIntroducers(IQueryable<FcRequiredEdiReport> _ediAccounts)
        {
            List<int> introducers = new();
            foreach (var i in _ediAccounts)
            {
                if (!introducers.Contains(i.IntroducerId)) introducers.Add(i.IntroducerId);
            }

            return introducers;
        }

        /// <summary>
        /// Get a list of the required Introducer accounts from the RequiredEdiReport table
        /// </summary>
        /// <param name="_ediAccounts"></param>
        /// <returns></returns>
        public static List<int> GetListOfAllIntroducerIds(FuelcardsContext _db)
        {
            List<int> introducers = _db.FcRequiredEdiReports.Select(i => i.IntroducerId).Distinct().ToList();

            return introducers;
        }

        /// <summary>
        /// Returns the PortlandId from a given Network account number
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="_accNumbers"></param>
        /// <returns></returns>
        public static int? GetPortlandIdFromNetworkCustCode(int customerCode, IQueryable<FcNetworkAccNoToPortlandId> _accNumbers)
        {
            var dbEntry = _accNumbers.FirstOrDefault(p => p.FcAccountNo == customerCode);
            if (dbEntry is null) return null;
            return dbEntry.PortlandId;
        }

        /// <summary>
        /// Gets the Portland Id for the given card number
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="mc"></param>
        /// <returns>int PortlandId</returns>
        public static int? GetPortlandIdFromMaskedCardNumber(ICardNumber pan, IQueryable<FcMaskedCard> mc)
        {
            var dbEntry = mc.FirstOrDefault(m => m.CardNumber == pan.Value);
            if (dbEntry is null) return null;
            return dbEntry.PortlandId;
        }

        /// <summary>
        /// Returns the list of masked cards for the given network
        /// </summary>
        /// <param name="network"></param>
        /// <param name="_db"></param>
        /// <returns>IQueryable list of Masked Cards</returns>
        public static IQueryable<FcMaskedCard> GetMaskedCardsForNetwork(Network network, IFuelcardUnitOfWork _db)
        {
            try
            {
                return _db.FcMaskedCards.Where(m => m.Network == (short)network);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// <param name="_ediAccounts"></param>
        /// <returns></returns>
        /// Returns a list containing all the customers PortlandId's of a given introducer's PortlandId
        /// </summary>
        /// <param name="introId"></param>
        public static List<int> GetListOfIntroCustomersFromIntroId(int introId, IQueryable<FcRequiredEdiReport> _ediAccounts)
        {
            return _ediAccounts.Where(p => p.IntroducerId == introId).Select(p => p.CustomerId).ToList();
        }
    }
}