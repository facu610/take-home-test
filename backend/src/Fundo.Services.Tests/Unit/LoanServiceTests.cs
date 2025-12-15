using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Domain;
using Fundo.Applications.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fundo.Services.Tests.Unit
{
    public class LoanServiceTests
    {
        private static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldCreateLoanWithExpectedDefaults()
        {
            // Arrange
            using var context = CreateDbContext();
            LoanService loanService = new LoanService(context);

            // Act
            Loan loan = await loanService.CreateLoanAsync(1000m,"Facu Martinez");

            // Assert
            Assert.NotNull(loan);
            Assert.Equal(1000m, loan.Amount);
            Assert.Equal(1000m, loan.CurrentBalance);
            Assert.Equal("Facu Martinez", loan.ApplicantName);
            Assert.Equal(LoanStatus.Active, loan.Status);
            Assert.True(loan.CreatedAt >= DateTime.UtcNow.AddSeconds(-5));
        }

        [Fact]
        public async Task RegisterPaymentAsync_WhenPaymentEqualsBalance_ShouldMarkAsPaid()
        {
            // Arrange
            using var context = CreateDbContext();
            LoanService loanService = new LoanService(context);

            // Act
            var created = await loanService.CreateLoanAsync(600m, "Test User");
            var updated = await loanService.RegisterPaymentAsync(created.Id, 600m);

            // Assert
            Assert.Equal(0m, updated.CurrentBalance);
            Assert.Equal(LoanStatus.Paid, updated.Status);  
        }

        [Fact]
        public async Task RegisterPaymentAsync_WhenPaymentExceedsBalance_ShouldThrowException()
        {
            // Arrange
            using var context = CreateDbContext();
            LoanService loanService = new LoanService(context);

            // Act
            var created = await loanService.CreateLoanAsync(400m, "Test User");

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await loanService.RegisterPaymentAsync(created.Id, 500m);
            });
        }

        [Fact]
        public async Task RegisterPaymentAsync_WhenLoanNotFound_ShouldThrowException()
        {
            // Arrange
            using var context = CreateDbContext();
            LoanService loanService = new LoanService(context);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await loanService.RegisterPaymentAsync(999, 100m);
            });
        }
    }
}