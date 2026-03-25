using Xunit;
using Philiprehberger.UrlBuilder;

namespace Philiprehberger.UrlBuilder.Tests;

public class UrlBuilderTests
{
    [Fact]
    public void Build_BaseUrlOnly_ReturnsBaseUrl()
    {
        var url = Url.Base("https://example.com").Build();

        Assert.Equal("https://example.com", url);
    }

    [Fact]
    public void Path_SingleSegment_AppendsToUrl()
    {
        var url = Url.Base("https://example.com")
            .Path("api")
            .Build();

        Assert.Equal("https://example.com/api", url);
    }

    [Fact]
    public void Path_MultipleSegments_AppendsAll()
    {
        var url = Url.Base("https://example.com")
            .Path("api", "v1", "users")
            .Build();

        Assert.Equal("https://example.com/api/v1/users", url);
    }

    [Fact]
    public void Query_SingleParam_AddsQueryString()
    {
        var url = Url.Base("https://example.com")
            .Query("page", 1)
            .Build();

        Assert.Equal("https://example.com?page=1", url);
    }

    [Fact]
    public void Query_MultipleParams_AddsAll()
    {
        var url = Url.Base("https://example.com")
            .Query("page", 1)
            .Query("size", 10)
            .Build();

        Assert.Equal("https://example.com?page=1&size=10", url);
    }

    [Fact]
    public void RemoveQuery_RemovesParameter()
    {
        var url = Url.Base("https://example.com")
            .Query("page", 1)
            .Query("size", 10)
            .RemoveQuery("page")
            .Build();

        Assert.Equal("https://example.com?size=10", url);
    }

    [Fact]
    public void Fragment_SetsFragment()
    {
        var url = Url.Base("https://example.com")
            .Fragment("section")
            .Build();

        Assert.Equal("https://example.com#section", url);
    }

    [Fact]
    public void Scheme_ChangesScheme()
    {
        var url = Url.Base("https://example.com")
            .Scheme("http")
            .Build();

        Assert.Equal("http://example.com", url);
    }

    [Fact]
    public void Host_ChangesHost()
    {
        var url = Url.Base("https://example.com")
            .Host("other.com")
            .Build();

        Assert.Equal("https://other.com", url);
    }

    [Fact]
    public void Port_SetsPort()
    {
        var url = Url.Base("https://example.com")
            .Port(8080)
            .Build();

        Assert.Equal("https://example.com:8080", url);
    }

    [Fact]
    public void ToUri_ReturnsUriInstance()
    {
        var uri = Url.Base("https://example.com")
            .Path("api")
            .ToUri();

        Assert.IsType<Uri>(uri);
        Assert.Equal("https://example.com/api", uri.ToString().TrimEnd('/'));
    }

    [Fact]
    public void Immutability_OriginalUnchanged()
    {
        var original = Url.Base("https://example.com");
        var withPath = original.Path("api");

        Assert.Equal("https://example.com", original.Build());
        Assert.Equal("https://example.com/api", withPath.Build());
    }
}
