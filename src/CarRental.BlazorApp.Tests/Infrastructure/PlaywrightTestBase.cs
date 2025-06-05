using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using CarRental.BlazorApp;
using System.Threading.Tasks;
using System;

namespace CarRental.BlazorApp.Tests.Infrastructure
{
    [TestClass]
    public class PlaywrightTestBase
    {
        protected static IPlaywright _playwright;
        protected static IBrowser _browser;
        protected static WebApplicationFactory<Program> _factory;
        protected static string _baseUrl;

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext context)
        {
            try
            {
                // Try to install Playwright browsers (may fail due to firewall restrictions)
                Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });

                // Initialize Playwright
                _playwright = await Playwright.CreateAsync();
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-dev-shm-usage" }
                });
            }
            catch (Exception ex)
            {
                // If Playwright fails to initialize (e.g., due to firewall restrictions),
                // we'll skip browser tests but still set up the web application factory
                Console.WriteLine($"Warning: Playwright initialization failed: {ex.Message}");
            }

            // Create WebApplicationFactory for the Blazor app
            _factory = new WebApplicationFactory<Program>();

            // Use ConfigurationBuilder to read Playwright.BaseUrl from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .Build();
            var configBaseUrl = config["Playwright:BaseUrl"];

            if (!string.IsNullOrWhiteSpace(configBaseUrl))
            {
                _baseUrl = configBaseUrl.TrimEnd('/');
            }
            else
            {
                // Fallback: Get the base URL from the test server
                var client = _factory.CreateClient();
                _baseUrl = client.BaseAddress?.ToString()?.TrimEnd('/');
            }
        }

        [AssemblyCleanup]
        public static async Task AssemblyCleanup()
        {
            if (_browser != null)
                await _browser.CloseAsync();
            _playwright?.Dispose();
            _factory?.Dispose();
        }

        protected async Task<IPage> CreatePageAsync()
        {
            if (_browser == null)
                throw new InvalidOperationException("Browser not initialized - Playwright tests cannot run due to missing browser binaries");

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                BaseURL = _baseUrl
            });
            return await context.NewPageAsync();
        }

        protected string GetUrl(string path) => $"{_baseUrl}{path}";

        protected bool IsBrowserAvailable => _browser != null;
    }
}