using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class OrderRepository : Repository<Order, string>, IOrderRepository
{
    public OrderRepository(DataContext context) : base(context, context.Orders)
    {
    }
}