namespace WebLinking.Integration.AspNetCore.Tests.UnitTests.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Routing;
    using WebLinking.Integration.AspNetCore.Mvc;
    using Xunit;

    public class ActionContextExtensionsTest
    {
        [Fact]
        public void GetLinkValues_Throws_When_ActionContext_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "context",
                () => ActionContextExtensions.GetLinkValues(null, new ObjectPagedCollection()));
        }

        [Fact]
        public void GetLinkValues_Returns_EmptyCollection_When_PagedCollection_Is_Null()
        {
            var result = ActionContextExtensions.GetLinkValues(new ActionContext(), (IPagedCollection<object>)null);

            Assert.Empty(result);
        }

        [Fact]
        public void GetLinkValues_Returns_LinkValues()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = new HostString("localhost", 80);
            httpContext.Request.PathBase = new PathString("/pathbase");
            httpContext.Request.Path = new PathString("/path");
            httpContext.Request.QueryString = new QueryString("?key=value");
            httpContext.Request.Scheme = "http";

            var pagedCollection = new ObjectPagedCollection()
            {
                HasNext = true,
                HasPrevious = true,
                Items = new object[] { "item1", "item2" },
                Limit = 1,
                Offset = 1,
                TotalSize = 2,
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            var result = actionContext.GetLinkValues(pagedCollection).ToList();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("<http://localhost/pathbase/path?key=value&offset=0&limit=1>; rel=\"start\"", result[0].ToString());
            Assert.Equal("<http://localhost/pathbase/path?key=value&offset=0&limit=1>; rel=\"previous\"", result[1].ToString());
            Assert.Equal("<http://localhost/pathbase/path?key=value&offset=2&limit=1>; rel=\"next\"", result[2].ToString());
        }

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
