using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models;
using BackendApi.Repositories;
using BackendApi.Tests.Helpers;

namespace BackendApi.Tests.Repositories
{
    [TestFixture]
    public class PropertyRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ShouldAddPropertyToDatabase()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("CreatePropertyTest");
            TestDbContextFactory.SeedPropertyTypes(context);
            
            var ownerRepo = new OwnerRepository(context);
            var propertyRepo = new PropertyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "John", 
                LastName = "Doe", 
                Email = "john@example.com", 
                Phone = "+1234567890" 
            });

            var property = new Property
            {
                OwnerId = owner.Id,
                PropertyTypeId = 1, // residential
                PropertyLength = 100.50m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Description = "Beautiful house",
                Country = "USA",
                City = "New York",
                Street = "5th Avenue",
                ZipCode = "10001"
            };

            // Act
            var result = await propertyRepo.CreateAsync(property);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.PropertyCost.Should().Be(250000m);
            result.Country.Should().Be("USA");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnPropertyWithNavigationProperties_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetPropertyByIdTest");
            TestDbContextFactory.SeedPropertyTypes(context);
            
            var ownerRepo = new OwnerRepository(context);
            var propertyRepo = new PropertyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Jane", 
                LastName = "Smith", 
                Email = "jane@example.com", 
                Phone = "+0987654321" 
            });

            var property = await propertyRepo.CreateAsync(new Property
            {
                OwnerId = owner.Id,
                PropertyTypeId = 2, // commercial
                PropertyLength = 500m,
                PropertyCost = 1000000m,
                DateOfBuilding = new DateTime(2015, 6, 15),
                Description = "Office building",
                Country = "UK",
                City = "London",
                Street = "Oxford Street",
                ZipCode = "W1D 1BS"
            });

            // Act
            var result = await propertyRepo.GetByIdAsync(property.Id);

            // Assert
            result.Should().NotBeNull();
            result!.PropertyCost.Should().Be(1000000m);
            result.Owner.Should().NotBeNull();
            result.Owner.FirstName.Should().Be("Jane");
            result.PropertyType.Should().NotBeNull();
            result.PropertyType.Type.Should().Be("commercial");
        }

        [Test]
        public async Task GetByOwnerIdAsync_ShouldReturnProperties()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetPropertiesByOwnerTest");
            TestDbContextFactory.SeedPropertyTypes(context);
            
            var ownerRepo = new OwnerRepository(context);
            var propertyRepo = new PropertyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Bob", 
                LastName = "Johnson", 
                Email = "bob@example.com", 
                Phone = "+1111111111" 
            });

            await propertyRepo.CreateAsync(new Property 
            { 
                OwnerId = owner.Id, 
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 150000m,
                DateOfBuilding = DateTime.Now.AddYears(-5),
                Country = "USA",
                City = "Chicago"
            });
            
            await propertyRepo.CreateAsync(new Property 
            { 
                OwnerId = owner.Id, 
                PropertyTypeId = 3,
                PropertyLength = 1000m,
                PropertyCost = 500000m,
                DateOfBuilding = DateTime.Now.AddYears(-10),
                Country = "USA",
                City = "Detroit"
            });

            // Act
            var result = await propertyRepo.GetByOwnerIdAsync(owner.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.City == "Chicago");
            result.Should().Contain(p => p.City == "Detroit");
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyProperty()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("UpdatePropertyTest");
            TestDbContextFactory.SeedPropertyTypes(context);
            
            var ownerRepo = new OwnerRepository(context);
            var propertyRepo = new PropertyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Alice", 
                LastName = "Brown", 
                Email = "alice@example.com", 
                Phone = "+2222222222" 
            });

            var property = await propertyRepo.CreateAsync(new Property
            {
                OwnerId = owner.Id,
                PropertyTypeId = 1,
                PropertyLength = 80m,
                PropertyCost = 100000m,
                DateOfBuilding = new DateTime(2010, 1, 1),
                Description = "Original",
                Country = "USA",
                City = "Original City",
                Street = "Original St",
                ZipCode = "00000"
            });

            // Act
            property.PropertyCost = 120000m;
            property.City = "Updated City";
            property.Description = "Updated";
            var result = await propertyRepo.UpdateAsync(property);

            // Assert
            result.PropertyCost.Should().Be(120000m);
            result.City.Should().Be("Updated City");
            result.Description.Should().Be("Updated");
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveProperty()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("DeletePropertyTest");
            TestDbContextFactory.SeedPropertyTypes(context);
            
            var ownerRepo = new OwnerRepository(context);
            var propertyRepo = new PropertyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Delete", 
                LastName = "Test", 
                Email = "delete@example.com", 
                Phone = "+3333333333" 
            });

            var property = await propertyRepo.CreateAsync(new Property
            {
                OwnerId = owner.Id,
                PropertyTypeId = 1,
                PropertyLength = 50m,
                PropertyCost = 75000m,
                DateOfBuilding = DateTime.Now.AddYears(-3),
                Country = "USA",
                City = "ToDelete"
            });

            // Act
            var result = await propertyRepo.DeleteAsync(property.Id);

            // Assert
            result.Should().BeTrue();
            var retrieved = await propertyRepo.GetByIdAsync(property.Id);
            retrieved.Should().BeNull();
        }
    }
}
