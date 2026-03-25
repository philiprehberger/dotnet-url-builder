using Xunit;
using Philiprehberger.UrlBuilder;

namespace Philiprehberger.UrlBuilder.Tests;

public class UrlTests
{
    [Fact]
    public void Base_ValidUrl_ReturnsUrlBuilder()
    {
        var builder = Url.Base("https://example.com");

        Assert.NotNull(builder);
    }

    [Fact]
    public void Base_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Url.Base(""));
    }

    [Fact]
    public void Base_WhitespaceString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Url.Base("   "));
    }
}
