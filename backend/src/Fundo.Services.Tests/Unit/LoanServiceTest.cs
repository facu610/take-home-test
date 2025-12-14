using System;
using System.Threading.Tasks;
using FluentAssertions;
using Fundo.Applications.WebApi.Data;
using Fundo.Applications.WebApi.Domain;
using Fundo.Applications.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fundo.Services.Tests.Unit
{
    public class LoanServiceTest
    {
        private LoanService CreateService(out AppDbContext context)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new AppDbContext(options);
            return new LoanService(context);
        }

        [Fact]
        public async Task CreateLoanAsync_SetsInitialValues()
        {
            var service = CreateService(out _);
            var amount = 1000m;
            var applicantName = "John Doe";
            var beforeCreation = DateTime.UtcNow;

            var loan = await service.CreateLoanAsync(amount, applicantName);

            loan.Amount.Should().Be(amount);
            loan.CurrentBalance.Should().Be(amount);
            loan.ApplicantName.Should().Be(applicantName);
            loan.Status.Should().Be(LoanStatus.Active);
            var afterCreation = DateTime.UtcNow;
            loan.CreatedAt.Should().BeOnOrAfter(beforeCreation).And.BeOnOrBefore(afterCreation);
        }
    }
}
