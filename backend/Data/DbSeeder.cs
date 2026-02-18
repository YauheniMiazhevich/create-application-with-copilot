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
                    },
                    new Owner
                    {
                        FirstName = "Laura",
                        LastName = "Garcia",
                        Email = "laura.garcia@email.com",
                        Phone = "+1-555-0106",
                        Address = "14 Sunset Drive, Miami, FL 33101",
                        Description = "Luxury condo investor focused on Miami Beach properties.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "David",
                        LastName = "Wilson",
                        Email = "david.wilson@email.com",
                        Phone = "+1-555-0107",
                        Address = "55 Harbor View, Seattle, WA 98101",
                        Description = "Pacific Northwest property developer with waterfront focus.",
                        IsCompanyContact = true
                    },
                    new Owner
                    {
                        FirstName = "Olivia",
                        LastName = "Martinez",
                        Email = "olivia.martinez@email.com",
                        Phone = "+1-555-0108",
                        Address = "33 Peachtree Lane, Atlanta, GA 30301",
                        Description = "Southern residential property manager and investor.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Ethan",
                        LastName = "Brown",
                        Email = "ethan.brown@email.com",
                        Phone = "+1-555-0109",
                        Address = "7 Beacon Hill Road, Boston, MA 02101",
                        Description = "Historic property specialist in the Boston area.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Sophia",
                        LastName = "Lee",
                        Email = "sophia.lee@email.com",
                        Phone = "+1-555-0110",
                        Address = "22 Market Street, San Francisco, CA 94101",
                        Description = "Tech corridor real estate investor in San Francisco.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Noah",
                        LastName = "Taylor",
                        Email = "noah.taylor@email.com",
                        Phone = "+1-555-0111",
                        Address = "88 Magnolia Street, Nashville, TN 37201",
                        Description = "Mixed-use property developer in Nashville.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Isabella",
                        LastName = "White",
                        Email = "isabella.white@email.com",
                        Phone = "+1-555-0112",
                        Address = "61 Lakeshore Drive, Denver, CO 80201",
                        Description = "Mountain region property owner with ski resort holdings.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Liam",
                        LastName = "Jackson",
                        Email = "liam.jackson@email.com",
                        Phone = "+1-555-0113",
                        Address = "19 River Road, Portland, OR 97201",
                        Description = "Eco-friendly property developer in the Pacific Northwest.",
                        IsCompanyContact = true
                    },
                    new Owner
                    {
                        FirstName = "Mia",
                        LastName = "Harris",
                        Email = "mia.harris@email.com",
                        Phone = "+1-555-0114",
                        Address = "47 Grand Avenue, Minneapolis, MN 55401",
                        Description = "Midwestern apartment complex owner and manager.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Lucas",
                        LastName = "Clark",
                        Email = "lucas.clark@email.com",
                        Phone = "+1-555-0115",
                        Address = "5 Riverwalk Plaza, San Antonio, TX 78201",
                        Description = "Commercial and retail property investor in Texas.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Ava",
                        LastName = "Robinson",
                        Email = "ava.robinson@email.com",
                        Phone = "+1-555-0116",
                        Address = "30 Bayview Terrace, San Diego, CA 92101",
                        Description = "Coastal property specialist with vacation rental portfolio.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Mason",
                        LastName = "Lewis",
                        Email = "mason.lewis@email.com",
                        Phone = "+1-555-0117",
                        Address = "12 Capitol Hill Ave, Washington, DC 20001",
                        Description = "Urban real estate developer near government district.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Charlotte",
                        LastName = "Walker",
                        Email = "charlotte.walker@email.com",
                        Phone = "+1-555-0118",
                        Address = "66 Elm Street, Detroit, MI 48201",
                        Description = "Urban revitalization investor in downtown Detroit.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Oliver",
                        LastName = "Hall",
                        Email = "oliver.hall@email.com",
                        Phone = "+1-555-0119",
                        Address = "3 Creekside Way, Charlotte, NC 28201",
                        Description = "Suburban development specialist in the Carolinas.",
                        IsCompanyContact = false
                    },
                    new Owner
                    {
                        FirstName = "Emma",
                        LastName = "Young",
                        Email = "emma.young@email.com",
                        Phone = "+1-555-0120",
                        Address = "99 Desert Palm Road, Las Vegas, NV 89101",
                        Description = "Commercial real estate owner with casino district holdings.",
                        IsCompanyContact = true
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
                    },
                    new Property
                    {
                        OwnerId = owners[5].Id,
                        PropertyTypeId = propertyTypes[2 % typeCount].Id,
                        PropertyLength = 160.00m,
                        PropertyCost = 980000.00m,
                        DateOfBuilding = new DateTime(2019, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Luxury oceanview condo on Miami Beach with rooftop terrace.",
                        Country = "United States",
                        City = "Miami",
                        Street = "14 Sunset Drive",
                        ZipCode = "33101"
                    },
                    new Property
                    {
                        OwnerId = owners[6].Id,
                        PropertyTypeId = propertyTypes[3 % typeCount].Id,
                        PropertyLength = 5200.00m,
                        PropertyCost = 3800000.00m,
                        DateOfBuilding = new DateTime(2000, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Waterfront development land overlooking Puget Sound.",
                        Country = "United States",
                        City = "Seattle",
                        Street = "55 Harbor View",
                        ZipCode = "98101"
                    },
                    new Property
                    {
                        OwnerId = owners[7].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 175.00m,
                        PropertyCost = 520000.00m,
                        DateOfBuilding = new DateTime(2012, 7, 20, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Charming townhouse in Midtown Atlanta with modern finishes.",
                        Country = "United States",
                        City = "Atlanta",
                        Street = "33 Peachtree Lane",
                        ZipCode = "30301"
                    },
                    new Property
                    {
                        OwnerId = owners[8].Id,
                        PropertyTypeId = propertyTypes[4 % typeCount].Id,
                        PropertyLength = 250.00m,
                        PropertyCost = 1750000.00m,
                        DateOfBuilding = new DateTime(1890, 5, 3, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Restored Victorian mansion on Beacon Hill with original details.",
                        Country = "United States",
                        City = "Boston",
                        Street = "7 Beacon Hill Road",
                        ZipCode = "02101"
                    },
                    new Property
                    {
                        OwnerId = owners[9].Id,
                        PropertyTypeId = propertyTypes[1 % typeCount].Id,
                        PropertyLength = 130.00m,
                        PropertyCost = 1450000.00m,
                        DateOfBuilding = new DateTime(2020, 2, 14, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Contemporary flat in SOMA district close to tech campuses.",
                        Country = "United States",
                        City = "San Francisco",
                        Street = "22 Market Street",
                        ZipCode = "94101"
                    },
                    new Property
                    {
                        OwnerId = owners[10].Id,
                        PropertyTypeId = propertyTypes[2 % typeCount].Id,
                        PropertyLength = 280.00m,
                        PropertyCost = 1100000.00m,
                        DateOfBuilding = new DateTime(2017, 11, 22, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Mixed-use retail and residential building in buzzing East Nashville.",
                        Country = "United States",
                        City = "Nashville",
                        Street = "88 Magnolia Street",
                        ZipCode = "37201"
                    },
                    new Property
                    {
                        OwnerId = owners[11].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 200.00m,
                        PropertyCost = 690000.00m,
                        DateOfBuilding = new DateTime(2008, 3, 5, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Mountain ski chalet near Breckenridge with panoramic views.",
                        Country = "United States",
                        City = "Denver",
                        Street = "61 Lakeshore Drive",
                        ZipCode = "80201"
                    },
                    new Property
                    {
                        OwnerId = owners[12].Id,
                        PropertyTypeId = propertyTypes[3 % typeCount].Id,
                        PropertyLength = 8500.00m,
                        PropertyCost = 2200000.00m,
                        DateOfBuilding = new DateTime(2016, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Sustainably built mixed-use campus on the Willamette River.",
                        Country = "United States",
                        City = "Portland",
                        Street = "19 River Road",
                        ZipCode = "97201"
                    },
                    new Property
                    {
                        OwnerId = owners[13].Id,
                        PropertyTypeId = propertyTypes[1 % typeCount].Id,
                        PropertyLength = 320.00m,
                        PropertyCost = 890000.00m,
                        DateOfBuilding = new DateTime(2003, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Large apartment complex with 24 units near the University of Minnesota.",
                        Country = "United States",
                        City = "Minneapolis",
                        Street = "47 Grand Avenue",
                        ZipCode = "55401"
                    },
                    new Property
                    {
                        OwnerId = owners[14].Id,
                        PropertyTypeId = propertyTypes[2 % typeCount].Id,
                        PropertyLength = 450.00m,
                        PropertyCost = 1600000.00m,
                        DateOfBuilding = new DateTime(2014, 8, 7, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Riverwalk retail strip with high foot traffic in downtown San Antonio.",
                        Country = "United States",
                        City = "San Antonio",
                        Street = "5 Riverwalk Plaza",
                        ZipCode = "78201"
                    },
                    new Property
                    {
                        OwnerId = owners[15].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 118.00m,
                        PropertyCost = 760000.00m,
                        DateOfBuilding = new DateTime(2021, 1, 25, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Brand-new beachfront bungalow steps from Coronado Beach.",
                        Country = "United States",
                        City = "San Diego",
                        Street = "30 Bayview Terrace",
                        ZipCode = "92101"
                    },
                    new Property
                    {
                        OwnerId = owners[16].Id,
                        PropertyTypeId = propertyTypes[2 % typeCount].Id,
                        PropertyLength = 390.00m,
                        PropertyCost = 3200000.00m,
                        DateOfBuilding = new DateTime(2011, 5, 12, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Class-A office building two blocks from Capitol Hill.",
                        Country = "United States",
                        City = "Washington",
                        Street = "12 Capitol Hill Ave",
                        ZipCode = "20001"
                    },
                    new Property
                    {
                        OwnerId = owners[17].Id,
                        PropertyTypeId = propertyTypes[0 % typeCount].Id,
                        PropertyLength = 210.00m,
                        PropertyCost = 310000.00m,
                        DateOfBuilding = new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Newly renovated loft in Detroit's Corktown arts district.",
                        Country = "United States",
                        City = "Detroit",
                        Street = "66 Elm Street",
                        ZipCode = "48201"
                    },
                    new Property
                    {
                        OwnerId = owners[18].Id,
                        PropertyTypeId = propertyTypes[1 % typeCount].Id,
                        PropertyLength = 165.00m,
                        PropertyCost = 445000.00m,
                        DateOfBuilding = new DateTime(2013, 4, 28, 0, 0, 0, DateTimeKind.Utc),
                        Description = "Suburban family home with large yard in South Charlotte.",
                        Country = "United States",
                        City = "Charlotte",
                        Street = "3 Creekside Way",
                        ZipCode = "28201"
                    },
                    new Property
                    {
                        OwnerId = owners[19].Id,
                        PropertyTypeId = propertyTypes[4 % typeCount].Id,
                        PropertyLength = 600.00m,
                        PropertyCost = 4500000.00m,
                        DateOfBuilding = new DateTime(2007, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                        Description = "High-end entertainment venue on the Las Vegas Strip.",
                        Country = "United States",
                        City = "Las Vegas",
                        Street = "99 Desert Palm Road",
                        ZipCode = "89101"
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
                    UserName = "Admin",
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

            // Seed Regular User
            var userEmail = "user@backendapi.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);

            if (regularUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "User",
                    Email = userEmail,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
