using System;
using Portland.Data.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IKfE19CardsRepository : IRepository<KfE19Card>
	{
		void Update(KfE19Card source);
		Task UpdateAsync(KfE19Card source);
        IEnumerable<KfE19Card> Where(Func<KfE19Card, bool> predicate);
        IEnumerable<KfE19Card> First(Func<KfE19Card, bool> predicate);
        List<KfE19Card> RawSQL(string where1, string where2);
    }
}