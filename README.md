# Philiprehberger.UrlBuilder

[![CI](https://github.com/philiprehberger/dotnet-url-builder/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-url-builder/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.UrlBuilder.svg)](https://www.nuget.org/packages/Philiprehberger.UrlBuilder)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-url-builder)](LICENSE)

Fluent, immutable URL construction with path segments, query parameters, and proper encoding.

## Installation

```bash
dotnet add package Philiprehberger.UrlBuilder
```

## Usage

```csharp
using Philiprehberger.UrlBuilder;

// Basic URL construction
string url = Url.Base("https://api.example.com")
    .Path("v2", "users")
    .Query("page", 1)
    .Query("sort", "name")
    .Build();
// "https://api.example.com/v2/users?page=1&sort=name"

// Multi-value query parameters
string url2 = Url.Base("https://example.com/search")
    .Query("tag", new[] { "csharp", "dotnet" })
    .Build();
// "https://example.com/search?tag=csharp&tag=dotnet"

// Modify parts of a URL
string url3 = Url.Base("https://example.com")
    .Path("docs")
    .Fragment("getting-started")
    .Build();
// "https://example.com/docs#getting-started"

// Convert to Uri
Uri uri = Url.Base("https://example.com")
    .Path("api")
    .ToUri();
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

## License

MIT
