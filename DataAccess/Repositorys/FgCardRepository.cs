//using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;

namespace DataAccess.Repositorys
{
    class FgCardRepository : Repository<FgCard>, IFgCardRepository
    {
        private readonly FuelcardsContext _db;

        public FgCardRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }
        public void Update(FgCard source)
        {
            var dbObj = _db.FgCards.FirstOrDefault(s => s.PanNumber == source.PanNumber);
            if (dbObj is null) _db.FgCards.Add(source);
            else UpdateDbObject(dbObj, source);
        }

        public async Task UpdateAsync(FgCard source)
        {
            var dbObj = _db.FgCards.FirstOrDefault(s => s.PanNumber == source.PanNumber);
            if (dbObj is null) await _db.FgCards.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        public List<FgCard> RawSQL(int? Where1, string Where2)
        {
            if (Where1 is null)
            {
                try
                {
                    string Query = $"SELECT * FROM public.fg_cards WHERE pan_number '{Where2}';";
                    var ReleventCards = _db.FgCards.FromSqlRaw(Query).ToList();
                    return ReleventCards;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    string Query = $"SELECT * FROM public.fg_cards WHERE portland_id = {Where1} AND pan_number {Where2}";
                    var ReleventCards = _db.FgCards.FromSqlRaw(Query).ToList();
                    return ReleventCards;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public IQueryable<FgCard> Where(Func<FgCard, bool> predicate)
        {
            return _db.FgCards.Where(predicate).AsQueryable();
        }

        private void UpdateDbObject(FgCard dbObj, FgCard source)
        {
            dbObj.AccountId = source.AccountId;
            dbObj.CaptureMileagePos = source.CaptureMileagePos;
            dbObj.CardId = source.CardId;
            dbObj.CardLimit = source.CardLimit;
            dbObj.CarWash = source.CarWash;
            dbObj.CostCentre = source.CostCentre;
            dbObj.Diesel = source.Diesel;
            dbObj.DisplayName = source.DisplayName;
            dbObj.EmployeeNumber = source.EmployeeNumber;
            dbObj.ExpiryDate = source.ExpiryDate;
            dbObj.IsActivated = source.IsActivated;
            dbObj.IsPoolCard = source.IsPoolCard;
            dbObj.IsTest = source.IsTest;
            dbObj.Oil = source.Oil;
            dbObj.PortlandId = source.PortlandId;
            dbObj.RegionalLocation = source.RegionalLocation;
            dbObj.RegNo = source.RegNo;
            dbObj.Status = source.Status;
            dbObj.ThirdPartyName = source.ThirdPartyName;
            dbObj.Unleaded = source.Unleaded;
            dbObj.VehicleType = source.VehicleType;
        }
    }
}
