
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
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

        public async Task<Pagination<Product>> GetProducts(CatalogSpecsParams catalogSpecsParams)
        {
            var builder = Builders<Product>.Filter; // Fourni par MongoDriver
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(catalogSpecsParams.Search))
            {
                filter &= builder.Where(p => p.Name.ToLower().Contains(catalogSpecsParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId)){
                filter &= builder.Eq(p => p.Brands.Id, catalogSpecsParams.BrandId);
            }
            if (!string.IsNullOrEmpty(catalogSpecsParams.TypeId)){
                filter &= builder.Eq(p => p.Types.Id, catalogSpecsParams.TypeId);
            }

            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await _context.Products
                .Find(filter)
                .Skip((catalogSpecsParams.PageIndex - 1) * catalogSpecsParams.PageSize)
                .Limit(catalogSpecsParams.PageSize)
                .ToListAsync();

            return new Pagination<Product>(catalogSpecsParams.PageIndex,catalogSpecsParams.PageSize,(int)totalItems,data);

            //return await _context
            //    .Products
            //    .Find(p => true)  //Aucun filtre
            //    .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
        {
            return await _context
                .Products
                .Find(b => b.Brands.Name.ToLower() == brandName.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
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

        public async Task<bool> DeleteProduct(string id)
        {
            var deletedProduct = await _context
                .Products
                .DeleteOneAsync(p => p.Id == id);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0; //True : Mongo reconnu la requete et au moins un un élément a été supprimé
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedProduct = await _context
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0; //True : Mongo reconnu la requete et au moins un un élément a été modifié
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
