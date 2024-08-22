using DataAccess.Fuelcards;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class UkFuelCardRepository : Repository<UkfuelCard>, IUkFuelCardRepository
	{
		private readonly FuelcardsContext _db;

		public UkFuelCardRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(UkfuelCard source)
		{
			var dbObj = _db.UkfuelCards.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public IQueryable<UkfuelCard> Where(Func<UkfuelCard, bool> predicate)
        {
            return _db.UkfuelCards.Where(predicate).AsQueryable();
        }
        public async Task UpdateAsync(UkfuelCard source)
		{
			var dbObj = _db.UkfuelCards.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.UkfuelCards.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public List<UkfuelCard> RawSQL(string Where1, string Where2)
        {
            string Query = $"SELECT * FROM public.ukfuel_cards WHERE customer_number {Where1} AND pan_number {Where2}";
            var ReleventCards = _db.UkfuelCards.FromSqlRaw(Query).ToList();
            return ReleventCards;
        }
		private void UpdateDbObject(UkfuelCard dbObj, UkfuelCard source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.ComapnyNo = source.ComapnyNo;
            dbObj.CompanyName = source.CompanyName;
            dbObj.DivisionNo = source.DivisionNo;
            dbObj.DivisionName = source.DivisionName;
            dbObj.CustomerNumber = source.CustomerNumber;
            dbObj.CustomerName = source.CustomerName;
            dbObj.CutomerCountry = source.CutomerCountry;
            dbObj.PanNumber = source.PanNumber;
            dbObj.Embossed3 = source.Embossed3;
            dbObj.Embossed4 = source.Embossed4;
            dbObj.IssueDate = source.IssueDate;
            dbObj.ExpiryDate = source.ExpiryDate;
            dbObj.StopFlag = source.StopFlag;
            dbObj.StopDate = source.StopDate;
            dbObj.Diesel = source.Diesel;
            dbObj.Unleaded = source.Unleaded;
            dbObj.SuperUnleaded = source.SuperUnleaded;
            dbObj.Lpg = source.Lpg;
            dbObj.CarWash = source.CarWash;
            dbObj.Lrp = source.Lrp;
            dbObj.Goods = source.Goods;
            dbObj.GasOil = source.GasOil;
            dbObj.LubeOil = source.LubeOil;
            dbObj.PinNo = source.PinNo;
            dbObj.CardType = source.CardType;
            dbObj.LastTransaction = source.LastTransaction;
        }
    }
}