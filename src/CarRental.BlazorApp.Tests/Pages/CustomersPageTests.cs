using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.BlazorApp.Tests.Infrastructure;
using System.Threading.Tasks;
using FluentAssertions;

namespace CarRental.BlazorApp.Tests.Pages
{
    [TestClass]
    [TestCategory("Playwright")]
    public class CustomersPageTests : PlaywrightTestBase
    {
        [TestMethod]
        public async Task CustomersPage_ShouldLoadCorrectly()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/customers"));

            // Assert
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            
            var title = await page.TitleAsync();
            title.Should().Be("Customers - Car Rental System");

            var heading = await page.Locator("h1").TextContentAsync();
            heading.Should().Be("Customers");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task CustomersPage_ShouldHaveAddNewCustomerButton()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/customers"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var addButton = page.Locator("a[href='/customers/new']");
            var isAddButtonVisible = await addButton.IsVisibleAsync();
            isAddButtonVisible.Should().BeTrue();
            
            var buttonText = await addButton.TextContentAsync();
            buttonText.Should().Be("Add New Customer");

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task CustomersPage_ShouldDisplayCustomersTable()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/customers"));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Assert
            var table = page.Locator("table.table");
            var isTableVisible = await table.IsVisibleAsync();
            isTableVisible.Should().BeTrue();

            // Check table headers
            var headers = table.Locator("thead th");
            var headerCount = await headers.CountAsync();
            headerCount.Should().Be(6, "Should have 6 columns: ID, Name, Unique ID, Email, Phone, Actions");

            var expectedHeaders = new[] { "ID", "Name", "Unique ID", "Email", "Phone", "Actions" };
            for (int i = 0; i < expectedHeaders.Length; i++)
            {
                var headerText = await headers.Nth(i).TextContentAsync();
                headerText.Should().Be(expectedHeaders[i]);
            }

            await page.CloseAsync();
        }

        [TestMethod]
        public async Task CustomersPage_ShouldBeResponsive()
        {
            // Arrange
            var page = await CreatePageAsync();

            // Act
            await page.GotoAsync(GetUrl("/customers"));
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