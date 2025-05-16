using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductSByName(string name);
        Task<IEnumerable<Product>> GetProductSByBrand(string name);
        Task<Product> CreatProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DelteProduct(string id);
    }
}
