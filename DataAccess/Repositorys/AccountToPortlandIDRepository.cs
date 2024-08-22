using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{
    class AccountToPortlandIDRepository : Repository<FcNetworkAccNoToPortlandId>, IAccountToPortlandIDRepository
    {
        private readonly FuelcardsContext _db;

        public AccountToPortlandIDRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }
        public void Update(FcNetworkAccNoToPortlandId source)
        {
            var dbObj = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(s => s.PortlandId == source.PortlandId);
            if (dbObj is null) _db.FcNetworkAccNoToPortlandIds.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task<List<int>> GetAllPortlandIds()
        {
            return _db.FcNetworkAccNoToPortlandIds
                    .Select(row => row.PortlandId)
                    .ToList();
        }
        public async Task<List<int>> GetAllAccounts(int? PortlandID)
        {
            return _db.FcNetworkAccNoToPortlandIds.Where(e=>e.PortlandId == PortlandID)
                    .Select(row => row.FcAccountNo)
                    .ToList();
        }
        public async Task UpdateAsync(FcNetworkAccNoToPortlandId source)
        {
            var dbObj = _db.FcNetworkAccNoToPortlandIds.FirstOrDefault(s => s.PortlandId == source.PortlandId);
            if (dbObj is null) await _db.FcNetworkAccNoToPortlandIds.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }

        public async Task<int> GetLatestID()
        {
            var id = _db.FcNetworkAccNoToPortlandIds.OrderByDescending(e => e.Id).FirstOrDefault().Id;
            return id;

        }
        public async Task<int> GetLatestPortlandID()
        {
            return _db.FcNetworkAccNoToPortlandIds.OrderByDescending(e => e.PortlandId).FirstOrDefault().PortlandId;
        }

        private void UpdateDbObject(FcNetworkAccNoToPortlandId dbObj, FcNetworkAccNoToPortlandId source)
        {
            dbObj.PortlandId = source.PortlandId;
            dbObj.FcAccountNo = source.FcAccountNo;
            dbObj.Id = source.Id;
            dbObj.Network = source.Network;
        }
    }
}
