using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class TypeContextSeed
    {
        //Ajouter les donées du fichier types.json dans une collection pour les données initiales 
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            //Any(): Vérifie s'il y a au moins un élément dans le résultat de Find
            //Le prédicat b => true: on ne filtre rien (commme SELECT *)
            bool checkTypes = typeCollection.Find(t => true).Any();
            //string path = Path.Combine("Data", "SeedData", "types.json");
            if (!checkTypes)
            {
                var seedData = File.ReadAllText("Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(seedData);
                if(types != null)
                {
                    foreach (var item in types)
                    {
                        typeCollection.InsertOneAsync(item); 
                    }
                }
            }

        }
    }
}
