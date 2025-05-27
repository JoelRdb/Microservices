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
    public static class BrandContextSeed
    {
        //Ajouter les donées du fichier brands.json dans une collection pour les données initiales 
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            //Any(): Vérifie s'il y a au moins un élément dans le résultat de Find
            //Le prédicat b => true: on ne filtre rien (commme SELECT *)
            bool checkBrands = brandCollection.Find(b => true).Any();
            //string path = Path.Combine("Data", "SeedData", "brands.json");
            if (!checkBrands)
            {
                var brandsData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands != null)
                {
                    foreach (var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }
        }
    }
}
