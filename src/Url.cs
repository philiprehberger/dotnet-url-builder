namespace Philiprehberger.UrlBuilder;

/// <summary>
/// Entry point for fluent URL construction.
/// </summary>
public static class Url
{
    /// <summary>
    /// Creates a new <see cref="UrlBuilder"/> from the specified base URL.
    /// </summary>
    /// <param name="baseUrl">The base URL to start building from.</param>
    /// <returns>A new <see cref="UrlBuilder"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="baseUrl"/> is null or empty.</exception>
    public static UrlBuilder Base(string baseUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        return new UrlBuilder(baseUrl);
    }
}
