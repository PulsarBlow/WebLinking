namespace WebLinking.Integration.AspNetCore.Tests.UnitTests.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Mvc;
    using Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    public class ActionContextExtensionsTest
    {
        private const string Scheme = "http";
        private const string Host = "localhost";
        private const string PathBase = "/path-base";
        private const string Path = "/path";
        private const string QueryString = "?key=value";

        [Fact]
        public void GetLinkValues_Throws_When_ActionContext_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "context",
                () => ActionContextExtensions.GetLinkValues(
                    null,
                    new ObjectPagedCollection()));
        }

        [Fact]
        public void
            GetLinkValues_Returns_EmptyCollection_When_PagedCollection_Is_Null()
        {
            var result =
                new ActionContext().GetLinkValues(
                    (IPagedCollection<object>) null);
            Assert.Empty(result);
        }

        [Fact]
        public void GetLinkValues_Returns_LinkValues()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = new HostString(
                Host,
                80);
            httpContext.Request.PathBase = new PathString(PathBase);
            httpContext.Request.Path = new PathString(Path);
            httpContext.Request.QueryString = new QueryString(QueryString);
            httpContext.Request.Scheme = Scheme;

            const int limit = 1;
            const int startOffset = 0;
            const int nextOffset = 2;
            const int previousOffset = 0;

            var pagedCollection = new ObjectPagedCollection
            {
                HasNext = true,
                HasPrevious = true,
                Items = new object[] { "item1", "item2" },
                Limit = limit,
                Offset = 1,
                TotalSize = 2,
            };

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor());

            var result = actionContext.GetLinkValues(pagedCollection)
                .ToList();

            Assert.Equal(
                CreateLinkValue(
                    startOffset,
                    limit,
                    LinkRelationRegistry.Start),
                result[0]
                    .ToString());

            Assert.Equal(
                CreateLinkValue(
                    previousOffset,
                    limit,
                    LinkRelationRegistry.Previous),
                result[1]
                    .ToString());

            Assert.Equal(
                CreateLinkValue(
                    nextOffset,
                    limit,
                    LinkRelationRegistry.Next),
                result[2]
                    .ToString());
        }

        private string CreateLinkValue(
            int offset,
            int limit,
            string rel)
            => $"<{Scheme}://{Host}{PathBase}{Path}{QueryString}&offset={offset}&limit={limit}>; rel=\"{rel}\"";

        private class ObjectPagedCollection : IPagedCollection<object>
        {
            public bool HasNext { get; set; }

            public bool HasPrevious { get; set; }

            public int Limit { get; set; }

            public int Offset { get; set; }

            public int TotalSize { get; set; }

            public ICollection<object> Items { get; set; }
        }
    }
}
