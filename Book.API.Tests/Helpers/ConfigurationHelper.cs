using Microsoft.Extensions.Configuration;

namespace Book.API.Tests.Helpers
{
    public static class ConfigurationHelper
    {
        public static Microsoft.Extensions.Configuration.IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}