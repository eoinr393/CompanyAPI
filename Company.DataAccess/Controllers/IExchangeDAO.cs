using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public interface IExchangeDAO
    {
        Task<Models.Exchange> CreateExchange(Models.Exchange exchange);
        Task<Models.Exchange> GetExchangeById(int id);
        Task<Models.Exchange> GetExchangeByName(string name);
    }
}
