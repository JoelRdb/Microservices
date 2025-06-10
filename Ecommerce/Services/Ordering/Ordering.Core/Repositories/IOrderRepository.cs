using Ordering.Core.Entities;

namespace Ordering.Core.Repositories
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersByUserName(string userName);
    }
}
