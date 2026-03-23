using System.Collections;
using System.Text;

namespace Philiprehberger.UrlBuilder;

/// <summary>
/// An immutable URL builder that constructs URLs with path segments, query parameters,
/// fragments, and proper encoding. Each method returns a new instance.
/// </summary>
public sealed class UrlBuilder
{
    private readonly string _scheme;
    private readonly string _host;
    private readonly int? _port;
    private readonly IReadOnlyList<string> _pathSegments;
    private readonly IReadOnlyList<KeyValuePair<string, string>> _queryParams;
    private readonly string? _fragment;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlBuilder"/> class from a base URL string.
    /// </summary>
    /// <param name="baseUrl">The base URL to parse.</param>
    internal UrlBuilder(string baseUrl)
    {
        var uri = new Uri(baseUrl, UriKind.Absolute);
        _scheme = uri.Scheme;
        _host = uri.Host;
        _port = uri.IsDefaultPort ? null : uri.Port;

        var path = uri.AbsolutePath.Trim('/');
        _pathSegments = string.IsNullOrEmpty(path)
            ? []
            : path.Split('/').ToList();

        var queryParams = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrEmpty(uri.Query))
        {
            var query = uri.Query.TrimStart('?');
            foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                var key = Uri.UnescapeDataString(parts[0]);
                var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;
                queryParams.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        _queryParams = queryParams;
        _fragment = string.IsNullOrEmpty(uri.Fragment) ? null : uri.Fragment.TrimStart('#');
    }

    private UrlBuilder(
        string scheme,
        string host,
        int? port,
        IReadOnlyList<string> pathSegments,
        IReadOnlyList<KeyValuePair<string, string>> queryParams,
        string? fragment)
    {
        _scheme = scheme;
        _host = host;
        _port = port;
        _pathSegments = pathSegments;
        _queryParams = queryParams;
        _fragment = fragment;
    }

    /// <summary>
    /// Appends one or more path segments to the URL.
    /// Segments are automatically encoded.
    /// </summary>
    /// <param name="segments">The path segments to append.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the appended segments.</returns>
    public UrlBuilder Path(params string[] segments)
    {
        var newSegments = new List<string>(_pathSegments);

        foreach (var segment in segments)
        {
            foreach (var part in segment.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                newSegments.Add(part);
            }
        }

        return new UrlBuilder(_scheme, _host, _port, newSegments, _queryParams, _fragment);
    }

    /// <summary>
    /// Adds a query parameter with a single value.
    /// The key and value are automatically encoded.
    /// </summary>
    /// <param name="key">The query parameter key.</param>
    /// <param name="value">The query parameter value.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the added query parameter.</returns>
    public UrlBuilder Query(string key, object value)
    {
        var newParams = new List<KeyValuePair<string, string>>(_queryParams)
        {
            new(key, value?.ToString() ?? string.Empty)
        };

        return new UrlBuilder(_scheme, _host, _port, _pathSegments, newParams, _fragment);
    }

    /// <summary>
    /// Adds multiple values for a query parameter.
    /// Each value is added as a separate key=value pair.
    /// </summary>
    /// <param name="key">The query parameter key.</param>
    /// <param name="values">The query parameter values.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the added query parameters.</returns>
    public UrlBuilder Query(string key, IEnumerable values)
    {
        var newParams = new List<KeyValuePair<string, string>>(_queryParams);

        foreach (var value in values)
        {
            newParams.Add(new KeyValuePair<string, string>(key, value?.ToString() ?? string.Empty));
        }

        return new UrlBuilder(_scheme, _host, _port, _pathSegments, newParams, _fragment);
    }

    /// <summary>
    /// Removes all query parameters with the specified key.
    /// </summary>
    /// <param name="key">The query parameter key to remove.</param>
    /// <returns>A new <see cref="UrlBuilder"/> without the specified query parameter.</returns>
    public UrlBuilder RemoveQuery(string key)
    {
        var newParams = _queryParams
            .Where(p => !string.Equals(p.Key, key, StringComparison.Ordinal))
            .ToList();

        return new UrlBuilder(_scheme, _host, _port, _pathSegments, newParams, _fragment);
    }

    /// <summary>
    /// Sets the URL fragment (the part after #).
    /// </summary>
    /// <param name="fragment">The fragment value.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the specified fragment.</returns>
    public UrlBuilder Fragment(string fragment)
    {
        return new UrlBuilder(_scheme, _host, _port, _pathSegments, _queryParams, fragment);
    }

    /// <summary>
    /// Sets the URL scheme (e.g., "https", "http").
    /// </summary>
    /// <param name="scheme">The scheme value.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the specified scheme.</returns>
    public UrlBuilder Scheme(string scheme)
    {
        return new UrlBuilder(scheme, _host, _port, _pathSegments, _queryParams, _fragment);
    }

    /// <summary>
    /// Sets the URL host.
    /// </summary>
    /// <param name="host">The host value.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the specified host.</returns>
    public UrlBuilder Host(string host)
    {
        return new UrlBuilder(_scheme, host, _port, _pathSegments, _queryParams, _fragment);
    }

    /// <summary>
    /// Sets the URL port.
    /// </summary>
    /// <param name="port">The port number.</param>
    /// <returns>A new <see cref="UrlBuilder"/> with the specified port.</returns>
    public UrlBuilder Port(int port)
    {
        return new UrlBuilder(_scheme, _host, port, _pathSegments, _queryParams, _fragment);
    }

    /// <summary>
    /// Builds the URL as a string with proper encoding.
    /// </summary>
    /// <returns>The constructed URL string.</returns>
    public string Build()
    {
        var sb = new StringBuilder();

        sb.Append(_scheme);
        sb.Append("://");
        sb.Append(_host);

        if (_port.HasValue)
        {
            sb.Append(':');
            sb.Append(_port.Value);
        }

        if (_pathSegments.Count > 0)
        {
            sb.Append('/');
            sb.Append(string.Join("/", _pathSegments.Select(Uri.EscapeDataString)));
        }

        if (_queryParams.Count > 0)
        {
            sb.Append('?');
            var pairs = _queryParams.Select(p =>
                $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}");
            sb.Append(string.Join("&", pairs));
        }

        if (!string.IsNullOrEmpty(_fragment))
        {
            sb.Append('#');
            sb.Append(Uri.EscapeDataString(_fragment));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Builds the URL as a <see cref="Uri"/> instance.
    /// </summary>
    /// <returns>A <see cref="Uri"/> representing the constructed URL.</returns>
    public Uri ToUri()
    {
        return new Uri(Build(), UriKind.Absolute);
    }
}
