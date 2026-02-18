using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackendApi.Models;

namespace BackendApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedMockDataAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed Owners
            if (!await context.Owners.AnyAsync())
            {
                var owners = new List<Owner>
                {
                    new Owner
                    {
                        FirstName = "James",
                        LastName = "Anderson",
                        Email = "james.anderson@email.com",
                        Phone = "+1-555-0101",
                        Address = "12 Oak Street, New York, NY 10001",
                        Description = "Primary property investor with portfolio in New York.",
                        IsCompanyContact = true
                    },
                    new Owner
                    {
                        FirstName = "Sarah",
                        LastName = "Mitchell",
                        Email = "sarah.mitchell@email.com",
                        Phone = "+1-555-0102",
                        Address = "45 Maple Avenue, Los Angeles, CA 90001",
                        Description = "Private owner managing residential properties.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Robert",
                        LastName = "Chen",
                        Email = "robert.chen@email.com",
                        Phone = "+1-555-0103",
                        Address = "78 Pine Road, Chicago, IL 60601",
                        Description = "Commercial real estate owner based in Chicago.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Emily",
                        LastName = "Thompson",
                        Email = "emily.thompson@email.com",
                        Phone = "+1-555-0104",
                        Address = "23 Cedar Lane, Houston, TX 77001",
                        Description = "Owner specializing in vacation rental properties.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Michael",
                        LastName = "Davis",
                        Email = "michael.davis@email.com",
                        Phone = "+1-555-0105",
                        Address = "91 Birch Boulevard, Phoenix, AZ 85001",
                        Description = "Long-term rental property owner in Phoenix.",
                        IsCompanyContact = false
                    }
                };

                await context.Owners.AddRangeAsync(owners);
                await context.SaveChangesAsync();

                // Seed Company - linked to first owner
                var firstOwner = owners[0];
                var company = new Company
                {
                    OwnerId = firstOwner.Id,
                    CompanyName = "Anderson Real Estate Group",
                    CompanySite = "https://andersonrealestategroup.com"
                };

                await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();

                // Seed Properties - each owner linked to one property (1-to-1 by index)
                var propertyTypes = await context.PropertyTypes.OrderBy(pt => pt.Id).ToListAsync();
                int typeCount = propertyTypes.Count;

                var properties = new List<Property>
                {
                    new Property
                    {
                        OwnerId = owners[0].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 185.50m,
                        PropertyCost = 850000.00m,
                        DateOfBuilding = new DateTime(2010, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Modern apartment in the heart of Manhattan with stunning city views.",
                        Country = "United States",
                        City = "New York",
                        Street = "12 Oak Street",
                        ZipCode = "10001"
                    },
                    new Property
                    {
                        OwnerId = owners[1].Id,
                        PropertyTypeId = propertyTypes[1 % typeCount].Id,
                        PropertyLength = 220.00m,
                        PropertyCost = 1200000.00m,
                        DateOfBuilding = new DateTime(2005, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Spacious family home in Beverly Hills with private pool.",
                        Country = "United States",
                        City = "Los Angeles",
                        Street = "45 Maple Avenue",
                        ZipCode = "90210"
                    },
                    new Property
                    {
                        OwnerId = owners[2].Id,
                        PropertyTypeId = propertyTypes[2 % typeCount].Id,
                        PropertyLength = 310.00m,
                        PropertyCost = 2500000.00m,
                        DateOfBuilding = new DateTime(1998, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Prime commercial office space in downtown Chicago.",
                        Country = "United States",
                        City = "Chicago",
                        Street = "78 Pine Road",
                        ZipCode = "60601"
                    },
                    new Property
                    {
                        OwnerId = owners[3].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 95.00m,
                        PropertyCost = 420000.00m,
                        DateOfBuilding = new DateTime(2018, 8, 30, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Cozy vacation cottage near Lake Houston with scenic views.",
                        Country = "United States",
                        City = "Houston",
                        Street = "23 Cedar Lane",
                        ZipCode = "77001"
                    },
                    new Property
                    {
                        OwnerId = owners[4].Id,
                        PropertyTypeId = propertyTypes[1 % typeCount].Id,
                        PropertyLength = 140.00m,
                        PropertyCost = 375000.00m,
                        DateOfBuilding = new DateTime(2015, 1, 14, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Modern single-family home in a quiet Phoenix suburb.",
                        Country = "United States",
                        City = "Phoenix",
                        Street = "91 Birch Boulevard",
                        ZipCode = "85001"
                    }
                };

                await context.Properties.AddRangeAsync(properties);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@backendapi.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
