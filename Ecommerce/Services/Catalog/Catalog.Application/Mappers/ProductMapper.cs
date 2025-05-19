using AutoMapper;

namespace Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly; //règle les propriétés à mapper (publiques ou internal).
                cfg.AddProfile <ProductMappingProfile>(); //ajoute la config de mapping définie dans ma classe ProductMappingProfile.
            });
            var mapper = config.CreateMapper(); //Crée le mapper réel basé sur cette configuration.
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value; //expose l'objet IMapper

    }
}
