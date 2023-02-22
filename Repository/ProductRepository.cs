using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class ProductRepository : Repository<Product, string>, IProductRepository
{
    public ProductRepository(DataContext context) : base (context, context.Products)
    {
    }
}