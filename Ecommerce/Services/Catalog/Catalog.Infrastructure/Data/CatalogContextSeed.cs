using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProduct = productCollection.Find(p => true).Any();
            string path = Path.Combine("Data", "SeedData", "products.json");
            if (!checkProduct)
            {
                var productData = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Product>>(productData);
                if(data != null)
                {
                    foreach (var item in data)
                    {
                        productCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
