using Fundo.Applications.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration
{
    public class LoanManagementControllerTests : IClassFixture<WebApplicationFactory<Fundo.Applications.WebApi.Startup>>
    {
        private readonly HttpClient _client;

        public LoanManagementControllerTests(WebApplicationFactory<Fundo.Applications.WebApi.Startup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetLoans_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/loans");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostLoans_ShouldCreateLoanAndReturn201()
        {
            // Arrange
            CreateLoanRequest request = new CreateLoanRequest
            {
                Amount = 1500m,
                ApplicantName = "Test User"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/loans", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.Contains("/loans/", response.Headers.Location!.ToString());
        }

        [Fact]
        public async Task GetLoanById_WhenNotExists_ShouldReturn404()
        {
            var response = await _client.GetAsync("/loans/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
