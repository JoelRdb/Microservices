using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Entities
{
    public class BaseEntity
    {
        [BsonId] //Indique que c'est une clé primaire
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] //Stock/lit l'Id comme un ObjectId BSON dans MongoDB
        public string Id { get; set; }
    }
}
