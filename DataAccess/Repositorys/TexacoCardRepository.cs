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

    public class TexacoCardRepository : Repository<TexacoCard>, ITexacoCardRepository
    {
        private readonly FuelcardsContext _db;

        public TexacoCardRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TexacoCard source)
        {
            var dbObj = _db.TexacoCards.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) _db.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public IQueryable<TexacoCard> Where(Func<TexacoCard, bool> predicate)
        {
            return _db.TexacoCards.Where(predicate).AsQueryable();
        }
        public async Task UpdateAsync(TexacoCard source)
        {
            var dbObj = _db.TexacoCards.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) await _db.TexacoCards.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        public List<TexacoCard> RawSQL(string Where1, string Where2)
        {
            if (Where2 != "IS NOT NULL")
            {
                string PanPart1 = Where2.Substring(2, 6);
                string PanPart2;
                string PanPart3;
                if (PanPart1 == "708251")
                {
                    PanPart2 = Where2.Substring(8, 5);
                    PanPart3 = Where2.Substring(13, Where2.Length - 13);

                }
                else
                {
                    PanPart2 = Where2.Substring(8, 6);
                    PanPart3 = Where2.Substring(14, Where2.Length - 13);
                }
                string Query = $"SELECT * FROM public.texaco_cards WHERE customer_number = '{PanPart2}' AND uid = '{PanPart1}'";
                var ReleventCards = _db.TexacoCards.FromSqlRaw(Query).ToList();
                return ReleventCards;
            }
            else
            {
                string Query = $"SELECT * FROM public.texaco_cards WHERE customer_number {Where1}";
                var ReleventCards = _db.TexacoCards.FromSqlRaw(Query).ToList();
                return ReleventCards;
            }
        }
        private void UpdateDbObject(TexacoCard dbObj, TexacoCard source)
        {
            dbObj.Id = dbObj.Id;
            dbObj.Company = source.Company;
            dbObj.Division = source.Division;
            dbObj.DivisionName = source.DivisionName;
            dbObj.ClientNo = source.ClientNo;
            dbObj.InvoiceCentreNumber = source.InvoiceCentreNumber;
            dbObj.CustomerNumber = source.CustomerNumber;
            dbObj.CustomerName = source.CustomerName;
            dbObj.CustomerCountry = source.CustomerCountry;
            dbObj.Pan = source.Pan;
            dbObj.Embossed3 = source.Embossed3;
            dbObj.Embossed4 = source.Embossed4;
            dbObj.IssueDate = source.IssueDate;
            dbObj.IssueNumber = source.IssueNumber;
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
            dbObj.CostCenter = source.CostCenter;
            dbObj.CardLinked = source.CardLinked;
            dbObj.Uid = source.Uid;

        }
    }
}