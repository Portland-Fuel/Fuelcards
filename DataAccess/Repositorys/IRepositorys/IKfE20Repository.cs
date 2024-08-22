
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;

using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

public interface IKfE20Repository : IRepository<KfE20StoppedCard>
{
    void Update(KfE20StoppedCard source);
    System.Threading.Tasks.Task UpdateAsync(KfE20StoppedCard source);
    IQueryable<KfE20StoppedCard> Where(Func<KfE20StoppedCard, bool> predicate);
    KfE20StoppedCard First(Func<KfE20StoppedCard, bool> predicate);
}
