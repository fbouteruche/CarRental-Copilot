using Microsoft.Playwright;
using FluentAssertions;
using System.Threading.Tasks;

namespace CarRental.BlazorApp.Tests.Infrastructure
{
    public static class PlaywrightAssertionExtensions
    {
        public static PlaywrightElementAssertions Should(this ILocator locator)
        {
            return new PlaywrightElementAssertions(locator);
        }
    }

    public class PlaywrightElementAssertions
    {
        private readonly ILocator _locator;

        public PlaywrightElementAssertions(ILocator locator)
        {
            _locator = locator;
        }

        public async Task BeVisibleAsync(string because = "")
        {
            var isVisible = await _locator.IsVisibleAsync();
            isVisible.Should().BeTrue(because);
        }

        public async Task NotBeVisibleAsync(string because = "")
        {
            var isVisible = await _locator.IsVisibleAsync();
            isVisible.Should().BeFalse(because);
        }

        public async Task BeEnabledAsync(string because = "")
        {
            var isEnabled = await _locator.IsEnabledAsync();
            isEnabled.Should().BeTrue(because);
        }

        public async Task BeDisabledAsync(string because = "")
        {
            var isEnabled = await _locator.IsEnabledAsync();
            isEnabled.Should().BeFalse(because);
        }

        public async Task ContainTextAsync(string expectedText, string because = "")
        {
            var text = await _locator.TextContentAsync();
            text.Should().Contain(expectedText, because);
        }

        public async Task HaveTextAsync(string expectedText, string because = "")
        {
            var text = await _locator.TextContentAsync();
            text.Should().Be(expectedText, because);
        }
    }
}