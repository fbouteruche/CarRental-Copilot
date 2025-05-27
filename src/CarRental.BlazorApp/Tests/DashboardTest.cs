using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.BlazorApp.Tests
{
    public class DashboardTest
    {
        [Fact]
        public async Task Dashboard_ShouldLoad_WithoutErrors()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => 
                {
                    // We could configure test services here if needed
                });

            // Act
            var client = application.CreateClient();
            var response = await client.GetAsync("/dashboard");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            
            // Optionally read and assert on the content
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Dashboard", content); // Page title should be in the content
            Assert.Contains("Vehicles", content);
            Assert.Contains("Customers", content);
            Assert.Contains("Rentals", content);
        }
    }
}