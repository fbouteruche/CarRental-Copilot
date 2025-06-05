using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.BlazorApp.Tests.Infrastructure;
using System.Threading.Tasks;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Playwright")]
    public class NavigationTests : PlaywrightTestBase
    {
        [TestMethod]
        public async Task Navigation_ShouldWorkBetweenAllPages()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act & Assert - Start from home page
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Welcome to Car Rental System");

            // Navigate to Vehicles page
            await page.ClickAsync("a[href='/vehicles']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/vehicles");
            
            heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Vehicles");

            // Navigate to Customers page  
            await page.ClickAsync("a[href='/customers']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/customers");
            
            heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Customers");

            // Navigate to Dashboard page
            await page.ClickAsync("a[href='/dashboard']");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/dashboard");
            
            heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Dashboard");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task Navigation_DirectUrlAccess_ShouldWork()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act & Assert - Test direct access to each page
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            var title = await page.TitleAsync();
            title.Should().Be("Car Rental System");

            await page.GotoAsync(GetUrl("/vehicles"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            title = await page.TitleAsync();
            title.Should().Be("Vehicles - Car Rental System");

            await page.GotoAsync(GetUrl("/customers"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            title = await page.TitleAsync();
            title.Should().Be("Customers - Car Rental System");

            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            title = await page.TitleAsync();
            title.Should().Be("Dashboard - Car Rental System");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task Navigation_HomePageCards_ShouldNavigateCorrectly()
        {
            // Arrange
            var page = await CreatePageAsync();
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Act & Assert - Test navigation from home page cards
            // Test Vehicles card navigation
            var vehiclesCard = page.Locator(".card").Filter(new() { HasText = "Vehicles" });
            var isVehiclesCardVisible = await vehiclesCard.IsVisibleAsync();
            isVehiclesCardVisible.Should().BeTrue();
            
            var vehiclesButton = vehiclesCard.Locator("a[href='/vehicles']");
            await vehiclesButton.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/vehicles");

            // Go back to home
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Test Customers card navigation
            var customersCard = page.Locator(".card").Filter(new() { HasText = "Customers" });
            var isCustomersCardVisible = await customersCard.IsVisibleAsync();
            isCustomersCardVisible.Should().BeTrue();
            
            var customersButton = customersCard.Locator("a[href='/customers']");
            await customersButton.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/customers");

            // Go back to home
            await page.GotoAsync(GetUrl("/"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Test Dashboard card navigation
            var dashboardCard = page.Locator(".card").Filter(new() { HasText = "Dashboard" });
            var isDashboardCardVisible = await dashboardCard.IsVisibleAsync();
            isDashboardCardVisible.Should().BeTrue();
            
            var dashboardButton = dashboardCard.Locator("a[href='/dashboard']");
            await dashboardButton.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            page.Url.Should().EndWith("/dashboard");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task Navigation_PageRefresh_ShouldMaintainState()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act & Assert - Test page refresh on different pages
            await page.GotoAsync(GetUrl("/vehicles"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var originalHeading = await page.Locator("h1").TextContentAsync();
            originalHeading.Should().Be("Vehicles");

            // Refresh the page
            await page.ReloadAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var refreshedHeading = await page.Locator("h1").TextContentAsync();
            refreshedHeading.Should().Be("Vehicles");
            page.Url.Should().EndWith("/vehicles");

            // Test refresh on dashboard
            await page.GotoAsync(GetUrl("/dashboard"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            originalHeading = await page.Locator("h1").TextContentAsync();
            originalHeading.Should().Be("Dashboard");

            await page.ReloadAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            refreshedHeading = await page.Locator("h1").TextContentAsync();
            refreshedHeading.Should().Be("Dashboard");
            page.Url.Should().EndWith("/dashboard");

            await page.CloseAsync();
        }
    }
}