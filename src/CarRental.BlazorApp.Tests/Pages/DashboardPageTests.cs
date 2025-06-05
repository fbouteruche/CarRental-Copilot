using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.BlazorApp.Tests.Infrastructure;
using System.Threading.Tasks;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Playwright")]
    public class DashboardPageTests : PlaywrightTestBase
    {
        [TestMethod]
        public async Task DashboardPage_ShouldLoadCorrectly()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/dashboard"));

            // Assert
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var title = await page.TitleAsync();
            title.Should().Be("Dashboard - Car Rental System");

            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Dashboard");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task DashboardPage_ShouldDisplayVehicleMetrics()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var vehiclesSection = page.Locator(".card").Filter(new() { HasText = "Vehicles" });
            var isVehiclesSectionVisible = await vehiclesSection.IsVisibleAsync();
            isVehiclesSectionVisible.Should().BeTrue();

            var totalVehiclesCard = vehiclesSection.Locator(".card").Filter(new() { HasText = "Total Vehicles" });
            var isTotalVehiclesVisible = await totalVehiclesCard.IsVisibleAsync();
            isTotalVehiclesVisible.Should().BeTrue();

            var availableVehiclesCard = vehiclesSection.Locator(".card").Filter(new() { HasText = "Available Vehicles" });
            var isAvailableVehiclesVisible = await availableVehiclesCard.IsVisibleAsync();
            isAvailableVehiclesVisible.Should().BeTrue();

            // Check that metrics are displayed as numbers
            var totalVehiclesText = await totalVehiclesCard.Locator("h3").TextContentAsync();
            totalVehiclesText.Should().NotBeNullOrEmpty();
            int.TryParse(totalVehiclesText, out _).Should().BeTrue("Total vehicles should be a number");

            var availableVehiclesText = await availableVehiclesCard.Locator("h3").TextContentAsync();
            availableVehiclesText.Should().NotBeNullOrEmpty();
            int.TryParse(availableVehiclesText, out _).Should().BeTrue("Available vehicles should be a number");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task DashboardPage_ShouldDisplayCustomerMetrics()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var customersSection = page.Locator(".card").Filter(new() { HasText = "Customers" });
            var isCustomersSectionVisible = await customersSection.IsVisibleAsync();
            isCustomersSectionVisible.Should().BeTrue();

            var registeredCustomersCard = customersSection.Locator(".card").Filter(new() { HasText = "Registered Customers" });
            var isRegisteredCustomersVisible = await registeredCustomersCard.IsVisibleAsync();
            isRegisteredCustomersVisible.Should().BeTrue();

            // Check that metric is displayed as a number
            var customersText = await registeredCustomersCard.Locator("h3").TextContentAsync();
            customersText.Should().NotBeNullOrEmpty();
            int.TryParse(customersText, out _).Should().BeTrue("Customer count should be a number");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task DashboardPage_ShouldDisplayRentalMetrics()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var rentalsSection = page.Locator(".card").Filter(new() { HasText = "Rentals" });
            var isRentalsSectionVisible = await rentalsSection.IsVisibleAsync();
            isRentalsSectionVisible.Should().BeTrue();

            var activeRentalsCard = rentalsSection.Locator(".card").Filter(new() { HasText = "Active Rentals" });
            var isActiveRentalsVisible = await activeRentalsCard.IsVisibleAsync();
            isActiveRentalsVisible.Should().BeTrue();

            var returnsTodayCard = rentalsSection.Locator(".card").Filter(new() { HasText = "Returns Today" });
            var isReturnsTodayVisible = await returnsTodayCard.IsVisibleAsync();
            isReturnsTodayVisible.Should().BeTrue();

            var returnsIn7DaysCard = rentalsSection.Locator(".card").Filter(new() { HasText = "Returns in 7 Days" });
            var isReturnsIn7DaysVisible = await returnsIn7DaysCard.IsVisibleAsync();
            isReturnsIn7DaysVisible.Should().BeTrue();

            // Check that all metrics are displayed as numbers
            var activeRentalsText = await activeRentalsCard.Locator("h3").TextContentAsync();
            activeRentalsText.Should().NotBeNullOrEmpty();
            int.TryParse(activeRentalsText, out _).Should().BeTrue("Active rentals should be a number");

            var returnsTodayText = await returnsTodayCard.Locator("h3").TextContentAsync();
            returnsTodayText.Should().NotBeNullOrEmpty();
            int.TryParse(returnsTodayText, out _).Should().BeTrue("Returns today should be a number");

            var returnsIn7DaysText = await returnsIn7DaysCard.Locator("h3").TextContentAsync();
            returnsIn7DaysText.Should().NotBeNullOrEmpty();
            int.TryParse(returnsIn7DaysText, out _).Should().BeTrue("Returns in 7 days should be a number");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task DashboardPage_ShouldHaveResponsiveLayout()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert - Check that Bootstrap responsive classes are present
            var vehiclesRow = page.Locator(".row").Filter(new() { HasText = "Vehicles" });
            var isVehiclesRowVisible = await vehiclesRow.IsVisibleAsync();
            isVehiclesRowVisible.Should().BeTrue();

            var customersRow = page.Locator(".row").Filter(new() { HasText = "Customers" });
            var isCustomersRowVisible = await customersRow.IsVisibleAsync();
            isCustomersRowVisible.Should().BeTrue();

            var rentalsRow = page.Locator(".row").Filter(new() { HasText = "Rentals" });
            var isRentalsRowVisible = await rentalsRow.IsVisibleAsync();
            isRentalsRowVisible.Should().BeTrue();

            // Check for responsive column classes
            var columns = page.Locator("[class*='col-md']");
            var columnCount = await columns.CountAsync();
            columnCount.Should().BeGreaterThan(0, "Should have responsive columns");

            await page.CloseAsync();
        }
    }
}