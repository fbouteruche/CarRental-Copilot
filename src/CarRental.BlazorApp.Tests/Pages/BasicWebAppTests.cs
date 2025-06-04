using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using CarRental.BlazorApp;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Integration")]
    public class BasicWebAppTests
    {
        [TestMethod]
        public async Task WebApp_HomePage_ShouldLoadSuccessfully()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Welcome to Car Rental System");
            content.Should().Contain("A modern web-based car rental management application");
        }

        [TestMethod]
        public async Task WebApp_DashboardPage_ShouldLoadSuccessfully()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/dashboard");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Dashboard");
            content.Should().Contain("Vehicles");
            content.Should().Contain("Customers");
            content.Should().Contain("Rentals");
        }

        [TestMethod]
        public async Task WebApp_VehiclesPage_ShouldLoadSuccessfully()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/vehicles");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Vehicles");
            content.Should().Contain("Add New Vehicle");
        }

        [TestMethod]
        public async Task WebApp_CustomersPage_ShouldLoadSuccessfully()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/customers");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Customers");
            content.Should().Contain("Add New Customer");
        }

        [TestMethod]
        public async Task WebApp_AllMainPages_ShouldHaveCorrectTitles()
        {
            // Arrange
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act & Assert
            var homeResponse = await client.GetAsync("/");
            homeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var homeContent = await homeResponse.Content.ReadAsStringAsync();
            homeContent.Should().Contain("<title>Car Rental System</title>");

            var dashboardResponse = await client.GetAsync("/dashboard");
            dashboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var dashboardContent = await dashboardResponse.Content.ReadAsStringAsync();
            dashboardContent.Should().Contain("<title>Dashboard - Car Rental System</title>");

            var vehiclesResponse = await client.GetAsync("/vehicles");
            vehiclesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var vehiclesContent = await vehiclesResponse.Content.ReadAsStringAsync();
            vehiclesContent.Should().Contain("<title>Vehicles - Car Rental System</title>");

            var customersResponse = await client.GetAsync("/customers");
            customersResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var customersContent = await customersResponse.Content.ReadAsStringAsync();
            customersContent.Should().Contain("<title>Customers - Car Rental System</title>");
        }
    }
}