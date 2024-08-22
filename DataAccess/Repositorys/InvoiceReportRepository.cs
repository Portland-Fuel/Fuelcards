using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

    public class InvoiceReportRepository : Repository<InvoiceReport>, IInvoiceReportRepository
    {
        private readonly FuelcardsContext _db;

        public InvoiceReportRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InvoiceReport source)
        {
            var dbObj = _db.InvoiceReports.FirstOrDefault(s => s.InvoiceDate == source.InvoiceDate && s.AccountNo == source.AccountNo);
            if (dbObj is null) _db.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task UpdateAsync(InvoiceReport source)
        {
            var dbObj = _db.InvoiceReports.FirstOrDefault(s => s.InvoiceDate == source.InvoiceDate && s.AccountNo == source.AccountNo);
            if (dbObj is null) await _db.InvoiceReports.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        private void UpdateDbObject(InvoiceReport dbObj, InvoiceReport source)
        {
            dbObj.InvoiceDate = source.InvoiceDate;
            dbObj.AccountNo = source.AccountNo;
            dbObj.DieselVol = source.DieselVol;
            dbObj.TescoVol = source.TescoVol;
            dbObj.PetrolVol = source.PetrolVol;
            dbObj.LubesVol = source.LubesVol;
            dbObj.GasoilVol = source.GasoilVol;
            dbObj.AdblueVol = source.AdblueVol;
            dbObj.PremDieselVol = source.PremDieselVol;
            dbObj.SuperUnleadedVol = source.SuperUnleadedVol;
            dbObj.SainsburysVol = source.SainsburysVol;
            dbObj.OtherVol = source.OtherVol;
            dbObj.DieselPrice = source.DieselPrice;
            dbObj.TescoPrice = source.TescoPrice;
            dbObj.PetrolPrice = source.PetrolPrice;
            dbObj.LubesPrice = source.LubesPrice;
            dbObj.GasoilPrice = source.GasoilPrice;
            dbObj.AdbluePrice = source.AdbluePrice;
            dbObj.PremDieselPrice = source.PremDieselPrice;
            dbObj.SuperUnleadedPrice = source.SuperUnleadedPrice;
            dbObj.SainsburysPrice = source.SainsburysPrice;
            dbObj.OthersPrice = source.OthersPrice;
            dbObj.Rolled = source.Rolled;
            dbObj.Current = source.Current;
           /* dbObj.PrevRolled = source.PrevRolled;*/
            dbObj.DieselLifted = source.DieselLifted;
            dbObj.Fixed = source.Fixed;
            dbObj.Floating = source.Floating;
            dbObj.TescoSainsburys = source.TescoSainsburys;
            dbObj.NetTotal = source.NetTotal;
            dbObj.Vat = source.Vat;
            dbObj.Total = source.Total;
            dbObj.InvNo = source.InvNo;
            dbObj.PayDate = source.PayDate;
            dbObj.ComPayable = source.ComPayable;
            dbObj.Commission = source.Commission;
            dbObj.Network = source.Network;
        }
    }
}