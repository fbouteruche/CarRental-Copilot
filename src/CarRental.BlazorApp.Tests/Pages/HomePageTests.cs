using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.BlazorApp.Tests.Infrastructure;
using System.Threading.Tasks;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Playwright")]
    public class HomePageTests : PlaywrightTestBase
    {
        [TestMethod]
        public async Task HomePage_ShouldLoadCorrectly()
        {
            // Skip test if browser is not available due to firewall restrictions
            if (!IsBrowserAvailable)
            {
                Assert.Inconclusive("Playwright browser not available - likely due to firewall restrictions");
                return;
            }

            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/"));

            // Assert
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var title = await page.TitleAsync();
            title.Should().Be("Car Rental System");

            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Welcome to Car Rental System");

            var leadText = await page.Locator("p.lead").TextContentAsync();
            leadText.Should().Contain("A modern web-based car rental management application");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task HomePage_ShouldHaveNavigationCards()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var vehiclesCard = page.Locator(".card").Filter(new() { HasText = "Vehicles" });
            var isVehiclesVisible = await vehiclesCard.IsVisibleAsync();
            isVehiclesVisible.Should().BeTrue();
            
            var customersCard = page.Locator(".card").Filter(new() { HasText = "Customers" });
            var isCustomersVisible = await customersCard.IsVisibleAsync();
            isCustomersVisible.Should().BeTrue();
            
            var dashboardCard = page.Locator(".card").Filter(new() { HasText = "Dashboard" });
            var isDashboardVisible = await dashboardCard.IsVisibleAsync();
            isDashboardVisible.Should().BeTrue();

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task HomePage_VehiclesButton_ShouldNavigateToVehiclesPage()
        {
            // Arrange
            var page = await CreatePageAsync();
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Act
            await page.ClickAsync("a[href='/vehicles']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var url = page.Url;
            url.Should().EndWith("/vehicles");
            
            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Vehicles");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task HomePage_CustomersButton_ShouldNavigateToCustomersPage()
        {
            // Arrange
            var page = await CreatePageAsync();
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Act
            await page.ClickAsync("a[href='/customers']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var url = page.Url;
            url.Should().EndWith("/customers");
            
            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Customers");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task HomePage_DashboardButton_ShouldNavigateToDashboardPage()
        {
            // Arrange
            var page = await CreatePageAsync();
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Act
            await page.ClickAsync("a[href='/dashboard']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var url = page.Url;
            url.Should().EndWith("/dashboard");
            
            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Dashboard");

            await page.CloseAsync();
        }
    }
}