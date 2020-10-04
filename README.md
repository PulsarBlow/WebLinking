# Web Linking for .NET

> A .NET implementation of the Web Linking proposed standard [RFC8288](https://tools.ietf.org/html/rfc8288)

[![NuGet](https://img.shields.io/nuget/v/WebLinking.Core.svg)](https://www.nuget.org/packages/WebLinking.Core/) [![netstandard 2.0](https://img.shields.io/badge/netstandard-2.0-brightgreen.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

The RFC8288 is a [IETF](https://www.ietf.org/) specification that defines a model for the relationships between resources on the Web ("links") and the type of those relationships ("link relation types"). It also defines the serialisation of such links in HTTP headers with the `Link` header field.

This .NET Standard library is a strict implementation of this specification.

---

## Getting Started

This implementation provides two .NET Standard 2.0 NuGet packages and a sample API.

### WebLinking.Core

The core implementation of the specification.\
Provides Link header format definition and abstractions.

This library helps you add Web Linking RFC support into your .NET Application (.NET Framework or .NET Core).

`dotnet add package WebLinking.Core`

### WebLinking.Integration.AspNetCore

AspNet Core MVC integration layer and abstractions.

This library helps you integrate Web Linking support into your ASP.NET Core MVC application.

`dotnet add package WebLinking.Integration.AspNetCore`

### WebLinking.Samples.SimpleApi

A sample project to demo how to integrate WebLinking into an AspNetCore Mvc application.\
Found in the [samples/simple-api](./samples/simple-api/) directory.

Once your run this demo api, you can fetch a paginated collection of values.\
Responses contains a Link headers containing a set a Web Links to navigate throught the collection.

__Example__

_Request_

We fetch 10 values, starting from the 10th one.

`GET https://localhost:5001/api/values?offset=10&limit=10`

_Response_

The API returns the 10 requested values and a set of Links to navigate within the set:

-  `start`: the begining of the collection
-  `previous`: the previous set of 10 values
-  `next`: the next set of 10 values

`Link: <https://localhost:5001/api/values?offset=0&limit=10>; rel="start",<https://localhost:5001/api/values?offset=0&limit=10>; rel="previous",<https://localhost:5001/api/values?offset=20&limit=10>; rel="next"`

## What is Web Linking ?

Without being limited to that, Web Link is often seen as an API pagination mean.\
It is a lightweight alternative to HATOAS, where pagination cursors are embedded in the resource representation.\
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

This difference is subtle but it helps producing a lightweight Resource representation.\
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

### Implementation notes

The current proposed standard leave some place for some interpretations. Implementation interpretations are disambiguated in the following points :

-   Multiple identical link relation types

> The rel parameter can, however, contain multiple link relation types.
> When this occurs, it establishes multiple links that share the same
> context, target, and target attributes.

Nothing is specified concerning multiple identical link relation types.\
Thus, this implementation does not deduplicate multiple identical link relation types.
