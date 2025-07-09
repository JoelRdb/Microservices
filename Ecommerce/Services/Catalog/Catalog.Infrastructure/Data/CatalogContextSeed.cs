using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        //Ajouter les donées du fichier products.json dans une collection pour les données initiales 
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            //Any(): Vérifie s'il y a au moins un élément dans le résultat de Find
            //Le prédicat b => true: on ne filtre rien (commme SELECT *)
            bool checkProduct = productCollection.Find(p => true).Any();

            //string path = Path.Combine("Data", "SeedData", "products.json");
            if (!checkProduct)
            {
                var productData = File.ReadAllText("Data/SeedData/products.json");
                var data = JsonSerializer.Deserialize<List<Product>>(productData);
                if(data != null)
                {
                    foreach (var item in data)
                    {
                        productCollection.InsertOneAsync(item);
                    }
                }
            }
            else
            {
                var allProducts = productCollection.Find(p => true).ToList();
                foreach (var item in allProducts)
                {
                    Console.WriteLine($"{item.Name} - {item.Price}");
                }              
            }
        }
    }
}
