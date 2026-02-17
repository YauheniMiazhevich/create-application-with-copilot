using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models;
using BackendApi.Repositories;
using BackendApi.Tests.Helpers;

namespace BackendApi.Tests.Repositories
{
    [TestFixture]
    public class OwnerRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ShouldAddOwnerToDatabase()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("CreateOwnerTest");
            var repository = new OwnerRepository(context);
            var owner = new Owner
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "123 Main St",
                Description = "Test owner"
            };

            // Act
            var result = await repository.CreateAsync(owner);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.FirstName.Should().Be("John");
            result.IsCompanyContact.Should().BeFalse();
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnOwner_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetOwnerByIdTest");
            var repository = new OwnerRepository(context);
            var owner = new Owner
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Phone = "+0987654321",
                Address = "456 Oak Ave"
            };
            await repository.CreateAsync(owner);

            // Act
            var result = await repository.GetByIdAsync(owner.Id);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Smith");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetOwnerByIdNotFoundTest");
            var repository = new OwnerRepository(context);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyOwner()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("UpdateOwnerTest");
            var repository = new OwnerRepository(context);
            var owner = new Owner
            {
                FirstName = "Original",
                LastName = "Name",
                Email = "original@example.com",
                Phone = "+1111111111"
            };
            await repository.CreateAsync(owner);

            // Act
            owner.FirstName = "Updated";
            owner.Email = "updated@example.com";
            var result = await repository.UpdateAsync(owner);

            // Assert
            result.FirstName.Should().Be("Updated");
            result.Email.Should().Be("updated@example.com");
            
            var retrieved = await repository.GetByIdAsync(owner.Id);
            retrieved!.FirstName.Should().Be("Updated");
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveOwner()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("DeleteOwnerTest");
            var repository = new OwnerRepository(context);
            var owner = new Owner
            {
                FirstName = "ToDelete",
                LastName = "Owner",
                Email = "delete@example.com",
                Phone = "+2222222222"
            };
            await repository.CreateAsync(owner);

            // Act
            var result = await repository.DeleteAsync(owner.Id);

            // Assert
            result.Should().BeTrue();
            var retrieved = await repository.GetByIdAsync(owner.Id);
            retrieved.Should().BeNull();
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenOwnerNotExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("DeleteOwnerNotFoundTest");
            var repository = new OwnerRepository(context);

            // Act
            var result = await repository.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("OwnerExistsTest");
            var repository = new OwnerRepository(context);
            var owner = new Owner
            {
                FirstName = "Exists",
                LastName = "Test",
                Email = "exists@example.com",
                Phone = "+3333333333"
            };
            await repository.CreateAsync(owner);

            // Act
            var result = await repository.ExistsAsync(owner.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("OwnerNotExistsTest");
            var repository = new OwnerRepository(context);

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllOwners()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetAllOwnersTest");
            var repository = new OwnerRepository(context);
            
            await repository.CreateAsync(new Owner { FirstName = "Owner1", LastName = "Test", Email = "owner1@test.com", Phone = "+1111111111" });
            await repository.CreateAsync(new Owner { FirstName = "Owner2", LastName = "Test", Email = "owner2@test.com", Phone = "+2222222222" });
            await repository.CreateAsync(new Owner { FirstName = "Owner3", LastName = "Test", Email = "owner3@test.com", Phone = "+3333333333" });

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(o => o.FirstName == "Owner1");
            result.Should().Contain(o => o.FirstName == "Owner2");
            result.Should().Contain(o => o.FirstName == "Owner3");
        }
    }
}
