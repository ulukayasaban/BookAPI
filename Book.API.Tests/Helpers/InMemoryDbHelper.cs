using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Tests.Helpers
{
    public static class InMemoryDbHelper
{
    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
}