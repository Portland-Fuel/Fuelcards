using System.Threading.Tasks;
using Portland.Data.Repository.IRepository;
using System.Collections.Generic;
using System;
using System.Linq;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;


namespace Portland.Data.Repository.IRepository
{
    public interface IFgCardRepository : IRepository<FgCard>
    {
        void Update(FgCard fgCard);
        Task UpdateAsync(FgCard fgCard);
        IQueryable<FgCard> Where(Func<FgCard, bool> predicate);
        List<FgCard> RawSQL(int? where1, string where2);
    }
}
