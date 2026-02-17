using Microsoft.EntityFrameworkCore;
using BackendApi.Data;
using BackendApi.Models;

namespace BackendApi.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            var context = new ApplicationDbContext(options);
            return context;
        }

        public static void SeedPropertyTypes(ApplicationDbContext context)
        {
            context.PropertyTypes.AddRange(
                new PropertyType { Id = 1, Type = "residential" },
                new PropertyType { Id = 2, Type = "commercial" },
                new PropertyType { Id = 3, Type = "industrial" },
                new PropertyType { Id = 4, Type = "raw land" },
                new PropertyType { Id = 5, Type = "special purpose" }
            );
            context.SaveChanges();
        }
    }
}
