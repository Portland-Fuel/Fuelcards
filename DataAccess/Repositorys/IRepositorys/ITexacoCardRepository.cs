using Portland.Data.Repository.IRepository;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;


namespace Portland.Data.Repository.IRepository
{
	public interface ITexacoCardRepository : IRepository<TexacoCard>
	{
		void Update(TexacoCard source);
		Task UpdateAsync(TexacoCard source);
        IQueryable<TexacoCard> Where(Func<TexacoCard, bool> predicate);
        List<TexacoCard> RawSQL(string where1, string where2);
    }
}