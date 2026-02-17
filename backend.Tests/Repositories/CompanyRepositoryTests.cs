using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models;
using BackendApi.Repositories;
using BackendApi.Tests.Helpers;

namespace BackendApi.Tests.Repositories
{
    [TestFixture]
    public class CompanyRepositoryTests
    {
        [Test]
        public async Task CreateAsync_ShouldAddCompanyToDatabase()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("CreateCompanyTest");
            var ownerRepo = new OwnerRepository(context);
            var companyRepo = new CompanyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "John", 
                LastName = "Doe", 
                Email = "john@example.com", 
                Phone = "+1234567890" 
            });

            var company = new Company
            {
                OwnerId = owner.Id,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com"
            };

            // Act
            var result = await companyRepo.CreateAsync(company);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.CompanyName.Should().Be("Acme Corp");
            result.OwnerId.Should().Be(owner.Id);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCompanyWithOwner_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetCompanyByIdTest");
            var ownerRepo = new OwnerRepository(context);
            var companyRepo = new CompanyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Jane", 
                LastName = "Smith", 
                Email = "jane@example.com", 
                Phone = "+0987654321" 
            });

            var company = await companyRepo.CreateAsync(new Company
            {
                OwnerId = owner.Id,
                CompanyName = "Tech Inc",
                CompanySite = "https://tech.com"
            });

            // Act
            var result = await companyRepo.GetByIdAsync(company.Id);

            // Assert
            result.Should().NotBeNull();
            result!.CompanyName.Should().Be("Tech Inc");
            result.Owner.Should().NotBeNull();
            result.Owner.FirstName.Should().Be("Jane");
        }

        [Test]
        public async Task GetByOwnerIdAsync_ShouldReturnCompanies()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("GetCompaniesByOwnerTest");
            var ownerRepo = new OwnerRepository(context);
            var companyRepo = new CompanyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Bob", 
                LastName = "Johnson", 
                Email = "bob@example.com", 
                Phone = "+1111111111" 
            });

            await companyRepo.CreateAsync(new Company { OwnerId = owner.Id, CompanyName = "Company1", CompanySite = "https://c1.com" });
            await companyRepo.CreateAsync(new Company { OwnerId = owner.Id, CompanyName = "Company2", CompanySite = "https://c2.com" });

            // Act
            var result = await companyRepo.GetByOwnerIdAsync(owner.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(c => c.CompanyName == "Company1");
            result.Should().Contain(c => c.CompanyName == "Company2");
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyCompany()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("UpdateCompanyTest");
            var ownerRepo = new OwnerRepository(context);
            var companyRepo = new CompanyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Alice", 
                LastName = "Brown", 
                Email = "alice@example.com", 
                Phone = "+2222222222" 
            });

            var company = await companyRepo.CreateAsync(new Company
            {
                OwnerId = owner.Id,
                CompanyName = "Original Name",
                CompanySite = "https://original.com"
            });

            // Act
            company.CompanyName = "Updated Name";
            company.CompanySite = "https://updated.com";
            var result = await companyRepo.UpdateAsync(company);

            // Assert
            result.CompanyName.Should().Be("Updated Name");
            result.CompanySite.Should().Be("https://updated.com");
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveCompany()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext("DeleteCompanyTest");
            var ownerRepo = new OwnerRepository(context);
            var companyRepo = new CompanyRepository(context);
            
            var owner = await ownerRepo.CreateAsync(new Owner 
            { 
                FirstName = "Delete", 
                LastName = "Test", 
                Email = "delete@example.com", 
                Phone = "+3333333333" 
            });

            var company = await companyRepo.CreateAsync(new Company
            {
                OwnerId = owner.Id,
                CompanyName = "ToDelete",
                CompanySite = "https://delete.com"
            });

            // Act
            var result = await companyRepo.DeleteAsync(company.Id);

            // Assert
            result.Should().BeTrue();
            var retrieved = await companyRepo.GetByIdAsync(company.Id);
            retrieved.Should().BeNull();
        }
    }
}
