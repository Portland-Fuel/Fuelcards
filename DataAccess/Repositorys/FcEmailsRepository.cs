using DataAccess.Fuelcards;using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;
namespace Portland.Data.Repository
{

    public class FcEmailsRepository : Repository<FcEmail>, IFcEmailsRepository
    {
        private readonly FuelcardsContext _db;

        public FcEmailsRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FcEmail source)
        {
            var dbObj = _db.FcEmails.FirstOrDefault(s => s.Account == source.Account);
            if (dbObj is null) _db.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task UpdateAsync(FcEmail source)
        {
            var dbObj = _db.FcEmails.FirstOrDefault(s => s.Account == source.Account);
            if (dbObj is null) await _db.FcEmails.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        
        private void UpdateDbObject(FcEmail dbObj, FcEmail source)
        {
            dbObj.Account = source.Account;
            dbObj.To = source.To;
            dbObj.Cc = source.Cc;
            dbObj.Bcc = source.Bcc;
        }
    }
}