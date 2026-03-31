# Philiprehberger.UrlBuilder

[![CI](https://github.com/philiprehberger/dotnet-url-builder/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-url-builder/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.UrlBuilder.svg)](https://www.nuget.org/packages/Philiprehberger.UrlBuilder)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-url-builder)](https://github.com/philiprehberger/dotnet-url-builder/commits/main)

Fluent, immutable URL construction with path segments, query parameters, and proper encoding.

## Installation

```bash
dotnet add package Philiprehberger.UrlBuilder
```

## Usage

### Building URLs with Path and Query Parameters

```csharp
using Philiprehberger.UrlBuilder;

string url = Url.Base("https://api.example.com")
    .Path("v2", "users")
    .Query("page", 1)
    .Query("sort", "name")
    .Build();
// "https://api.example.com/v2/users?page=1&sort=name"
```

### Multi-Value Query Parameters

```csharp
using Philiprehberger.UrlBuilder;

string url = Url.Base("https://example.com/search")
    .Query("tag", new[] { "csharp", "dotnet" })
    .RemoveQuery("obsolete")
    .Build();
// "https://example.com/search?tag=csharp&tag=dotnet"
```

### Fragments, Scheme, Host, and Port

```csharp
using Philiprehberger.UrlBuilder;

Uri uri = Url.Base("https://example.com")
    .Path("docs")
    .Fragment("getting-started")
    .Port(8443)
    .ToUri();
// https://example.com:8443/docs#getting-started
```

## API

### Url

| Method | Description |
|--------|-------------|
| `Base(string baseUrl)` | Create a new `UrlBuilder` from a base URL. |

### UrlBuilder

| Method | Description |
|--------|-------------|
| `Path(params string[] segments)` | Append path segments. |
| `Query(string key, object value)` | Add a query parameter. |
| `Query(string key, IEnumerable values)` | Add multiple values for a query parameter. |
| `RemoveQuery(string key)` | Remove a query parameter by key. |
| `Fragment(string fragment)` | Set the URL fragment. |
| `Scheme(string scheme)` | Set the URL scheme. |
| `Host(string host)` | Set the URL host. |
| `Port(int port)` | Set the URL port. |
| `Build()` | Build the URL as a string. |
| `ToUri()` | Build the URL as a `Uri` instance. |

All methods return a new `UrlBuilder` instance, preserving immutability.

## Development

```bash
dotnet build src/Philiprehberger.UrlBuilder.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-url-builder)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-url-builder/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-url-builder/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
