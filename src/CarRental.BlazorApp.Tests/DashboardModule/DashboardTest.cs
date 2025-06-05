using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using CarRental.BlazorApp;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.DashboardModule
{
    [TestClass]
    [TestCategory("Integration")]
    public class DashboardTest
    {
        [TestMethod]
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            // Optionally read and assert on the content
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Dashboard"); // Page title should be in the content
            content.Should().Contain("Vehicles");
            content.Should().Contain("Customers");
            content.Should().Contain("Rentals");
        }
    }
}