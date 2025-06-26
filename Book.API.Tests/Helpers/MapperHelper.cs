using Book.API.Mappings;

namespace Book.API.Tests.Helpers
{
    public static class MapperHelper
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }
    }
}