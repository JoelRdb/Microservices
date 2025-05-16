using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    interface ICatalogContext
    {
        IMongoCollection<ProductType> Types { get; }
        IMongoCollection<ProductBrand> Brands { get; }
        IMongoCollection<Product> Products { get; }
    }
}
