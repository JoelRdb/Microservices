
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

        public async Task<IEnumerable<Product>> GetProductSByBrand(string brandName)
        {
            return await _context
                .Products
                .Find(b => b.Brands.Name.ToLower() == brandName.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductSByName(string name)
        {
            return await _context
                .Products
                .Find(p => p.Name.ToLower() == name.ToLower())
                .ToListAsync();
        }

        public async Task<Product> CreatProduct(Product product)
        {
            await _context
                .Products
                .InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DelteProduct(string id)
        {
            var deletedProduct = await _context
                .Products
                .DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0; //True : Mongo reconnu la requete et au moins un un élément a été supprimé
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _context
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0; //True : Mongo reconnu la requete et au moins un un élément a été modifié
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context
                .Brands
                .Find(b => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context
                .Types
                .Find(t => true)
                .ToListAsync();
        }


    }
}
