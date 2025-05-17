
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepositoy(ICatalogContext context) : IProductRepository, IBrandRepository, ITypeReposiroty
    {
        public ICatalogContext _context = context;


        public async Task<Product> GetProduct(string id)
        {
            return await _context
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find(p => true)  //Aucun filtre
                .ToListAsync();
        }

        public Task<IEnumerable<Product>> GetProductSByBrand(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductSByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> CreatProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DelteProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductType>> GetAllTypes()
        {
            throw new NotImplementedException();
        }


    }
}
