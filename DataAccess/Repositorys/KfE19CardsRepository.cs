using DataAccess.Fuelcards;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE19CardsRepository : Repository<KfE19Card>, IKfE19CardsRepository
    {
		private readonly FuelcardsContext _db;

		public KfE19CardsRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}
		public void Update(KfE19Card source)
		{
			var dbObj = _db.KfE19Cards.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
		public async Task UpdateAsync(KfE19Card source)
		{
			var dbObj = _db.KfE19Cards.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.KfE19Cards.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public IEnumerable<KfE19Card> Where(Func<KfE19Card, bool> predicate)
        {
            return _db.KfE19Cards.Where(predicate).AsQueryable();
        }
        public List<KfE19Card> RawSQL(string Where1, string Where2)
        {
            if(Where1 != "IS NOT NULL" && !Where1.Contains("="))Where1 = "=" + Where1;  
            string Query = $"SELECT * FROM public.kf_e19_cards WHERE customer_account_code {Where1} AND pan_number {Where2}";
            var ReleventCards = _db.KfE19Cards.FromSqlRaw(Query).ToList();
            return ReleventCards;
        }
        public IEnumerable<KfE19Card> First(Func<KfE19Card, bool> predicate)
        { 
            return (IQueryable<KfE19Card>)_db.KfE19Cards.First(predicate);
        }
        private void UpdateDbObject(KfE19Card dbObj, KfE19Card source)
		{
            dbObj.PortlandId = source.PortlandId;
            dbObj.CustomerAccountCode = source.CustomerAccountCode;
            dbObj.CustomerAccountSuffix = source.CustomerAccountSuffix;
            dbObj.PanNumber = source.PanNumber;
            dbObj.Date = source.Date;
            dbObj.Time = source.Time;
            dbObj.ActionStatus = source.ActionStatus;
            dbObj.OdometerUnit = source.OdometerUnit;
            dbObj.VehicleReg = source.VehicleReg;
            dbObj.EmbossingDetails = source.EmbossingDetails;
            dbObj.CardGrade = source.CardGrade;
            dbObj.MileageEntryFlag = source.MileageEntryFlag;
            dbObj.PinRequired = source.PinRequired;
            dbObj.PinNumber = source.PinNumber;
            dbObj.TelephoneRequired = source.TelephoneRequired;
            dbObj.ExpiryDate = source.ExpiryDate;
            dbObj.European = source.European;
            dbObj.Smart = source.Smart;
            dbObj.SingleTransFuelLimit = source.SingleTransFuelLimit;
            dbObj.DailyTransFuelLimit = source.DailyTransFuelLimit;
            dbObj.WeeklyTransFuelLimit = source.WeeklyTransFuelLimit;
            dbObj.NumberTransPerDay = source.NumberTransPerDay;
            dbObj.NumberTransPerWeek = source.NumberTransPerWeek;
            dbObj.NumberFalsePinEntries = source.NumberFalsePinEntries;
            dbObj.PinLockoutMinutes = source.PinLockoutMinutes;
            dbObj.MondayAllowed = source.MondayAllowed;
            dbObj.TuesdayAllowed = source.TuesdayAllowed;
            dbObj.WednesdayAllowed = source.WednesdayAllowed;
            dbObj.ThursdayAllowed = source.ThursdayAllowed;
            dbObj.FridayAllowed = source.FridayAllowed;
            dbObj.SaturdayAllowed = source.SaturdayAllowed;
            dbObj.SundayAllowed = source.SundayAllowed;
            dbObj.ValidStartTime = source.ValidStartTime;
            dbObj.ValidEndTime = source.ValidEndTime;
        }
    }
}