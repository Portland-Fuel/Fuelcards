
namespace Fuelcards.Repositories
{
    public interface IQueriesRepository
    {
        void GetListOfAddonsForCustomer(int PortlandId);
        int? GetPortlandIdFromXeroId(string xeroId);
    }
}