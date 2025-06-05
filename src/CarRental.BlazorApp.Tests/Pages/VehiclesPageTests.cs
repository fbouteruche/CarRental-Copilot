using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.BlazorApp.Tests.Infrastructure;
using System.Threading.Tasks;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Playwright")]
    public class VehiclesPageTests : PlaywrightTestBase
    {
        [TestMethod]
        public async Task VehiclesPage_ShouldLoadCorrectly()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/vehicles"));

            // Assert
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var title = await page.TitleAsync();
            title.Should().Be("Vehicles - Car Rental System");

            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Vehicles");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task VehiclesPage_ShouldHaveAddNewVehicleButton()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/vehicles"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var addButton = page.Locator("a[href='/vehicles/new']");
            var isAddButtonVisible = await addButton.IsVisibleAsync();
            isAddButtonVisible.Should().BeTrue();
            
            var buttonText = await addButton.TextContentAsync();
            buttonText.Should().Be("Add New Vehicle");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task VehiclesPage_ShouldDisplayVehiclesTable()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/vehicles"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var table = page.Locator("table.table");
            var isTableVisible = await table.IsVisibleAsync();
            isTableVisible.Should().BeTrue();

            // Check table headers
            var headers = table.Locator("thead th");
            var headerCount = await headers.CountAsync();
            headerCount.Should().Be(8, "Should have 8 columns: ID, Model, Brand, License Plate, Color, Year, Status, Actions");

            var expectedHeaders = new[] { "ID", "Model", "Brand", "License Plate", "Color", "Year", "Status", "Actions" };
            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                var headerText = await headers.Nth(i).TextContentAsync();
                headerText.Should().Be(expectedHeaders[i]);
            }

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task VehiclesPage_ShouldBeResponsive()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/vehicles"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var tableContainer = page.Locator(".table-responsive");
            var isTableContainerVisible = await tableContainer.IsVisibleAsync();
            isTableContainerVisible.Should().BeTrue();

            var table = tableContainer.Locator("table");
            var isTableVisible = await table.IsVisibleAsync();
            isTableVisible.Should().BeTrue();

            var tableClasses = await table.GetAttributeAsync("class");
            tableClasses.Should().Contain("table-striped");
            tableClasses.Should().Contain("table-hover");

            await page.CloseAsync();
        }
    }
}