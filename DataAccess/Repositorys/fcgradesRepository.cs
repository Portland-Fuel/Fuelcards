using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class fcgradesRepository : Repository<FcGrade>, IfcgradesRepository
    {
		private readonly FuelcardsContext _db;

		public fcgradesRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(FcGrade source)
		{
			var dbObj = _db.FcGrades.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task UpdateAsync(FcGrade source)
		{
			var dbObj = _db.FcGrades.FirstOrDefault(s => s.Id == source.Id);
			if (dbObj is null) await _db.FcGrades.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
       
        private void UpdateDbObject(FcGrade dbObj, FcGrade source)
		{
            dbObj.Id = dbObj.Id;
            dbObj.GradeId = source.GradeId;
            dbObj.Grade = source.Grade;
        }
    }
}