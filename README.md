# Web Linking for .NET

> A .NET implementation of the Web Linking proposed standard [RFC8288](https://tools.ietf.org/html/rfc8288)

[![NuGet](https://img.shields.io/nuget/v/WebLinking.Core.svg)](https://www.nuget.org/packages/WebLinking.Core/) [![netstandard 2.0](https://img.shields.io/badge/netstandard-2.0-brightgreen.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

The RFC8288 is a specification that defines a model for the relationships between resources on the Web ("links") and the type of those relationships ("link relation types"). It also defines the serialisation of such links in HTTP headers with the Link header field.

This .NET Standard library is a strict implementation of this specification.

---

## Getting Started

This implementation provides two .NET Standard 2.0 NuGet packages and a demo MVC application.

### WebLinking.Core

The core implementation of the specification.  
Provides Link header format definition and abstractions.

The bare minimum to support Link header in your .NET project.

#### package manager

`Install-Package WebLinking.Core`

#### dotnet cli

`dotnet add package WebLinking.Core`

### WebLinking.Integration.AspNetCore

AspNet Core MVC integration layer and abstractions.

#### package manager

`Install-Package WebLinking.Integration.AspNetCore`

#### dotnet cli

`dotnet add package WebLinking.Integration.AspNetCore`

### WebLinking.DemoApi

A sample project to demo how to integrate WebLinking into an AspNetCore Mvc application

## What is Web Linking ?

Without being limited to that, Web Link is often seen as an API pagination mean.  
It is a lightweight alternative to HATOAS, where pagination cursors are embedded in the resource representation.
With Web Linking, pagination cursors are sent in the header of the HTTP response.

For example, [Github API v3](https://developer.github.com/v3/#pagination) uses Web Linking to express pagination cursors :

```
Link: <https://api.github.com/user/repos?page=3&per_page=100>; rel="next",
<https://api.github.com/user/repos?page=50&per_page=100>; rel="last"
```

API Clients don't need to implement link formatting, they just use links returned by the API. It makes them resilient to link format changes over time. The `rel` attribute defines the type of relation this link provides.

### How Web Linking is different from HATOAS ?

Beside specifying the Link format, Web Linking also defines how links should be convey in the HTTP response.

In opposite to a more traditional HATOAS approach, where Links are conveyed inside the Resource representation, with Web Linking links are conveyed in a Link HTTP header.

This difference is subtle but it helps producing a lightweight Resource representation.  
When you need to convey the Link in the response body, you need to wrap your Resource definition into a more verbose structure.

With Web Linking you can just return a `pure` Resource representation.

However, keep in mind that it is a the price of the developer experience. HTTP Headers are less discoverable than HTTP Response body (eg. Browser).

### Web Linking in the industry

Who is using Web Linking ?

-   [Github](https://developer.github.com/v3/#pagination)
-   [GitLab](https://docs.gitlab.com/ee/api/#pagination-link-header)
-   [Sentry.io](https://docs.sentry.io/api/pagination/)
-   [Dribble](http://developer.dribbble.com/v1/#pagination)
-   [PandaScore](https://developers.pandascore.co/doc/#section/Introduction/Pagination)

just to name of few...
