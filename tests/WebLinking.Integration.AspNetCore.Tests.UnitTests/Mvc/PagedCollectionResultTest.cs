namespace WebLinking.Integration.AspNetCore.Tests.UnitTests.Mvc
{
    using System;
    using System.Threading.Tasks;
    using AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Xunit;

    public class PagedCollectionResultTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock =
            new Mock<IServiceProvider>();
        private readonly Mock<IActionResultExecutor<ObjectResult>>
            _actionResultExecutorMock =
                new Mock<IActionResultExecutor<ObjectResult>>();
        private readonly Mock<IPagedCollection<object>> _pagedCollectionMock =
            new Mock<IPagedCollection<object>>();
        private readonly Mock<IHttpResponseFeature> _httpResponseFeature =
            new Mock<IHttpResponseFeature>();
        private readonly Mock<IHeaderDictionary> _headerDictionaryMock =
            new Mock<IHeaderDictionary>();

        public PagedCollectionResultTest()
        {
            _serviceProviderMock
                .Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(_actionResultExecutorMock.Object);
            _httpResponseFeature
                .SetupGet(x => x.Headers)
                .Returns(_headerDictionaryMock.Object);
        }

        [Fact]
        public void Constructor_Throws_When_PagedCollection_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PagedCollectionResult<IPagedCollection<object>>(
                    null));
        }

        [Fact]
        public void ExecuteResultAsync_Throws_When_Context_Is_Null()
        {
            var pagedCollectionResult =
                new PagedCollectionResult<object>(_pagedCollectionMock.Object);

            Assert.ThrowsAsync<ArgumentNullException>(
                () => pagedCollectionResult.ExecuteResultAsync(null));
        }

        [Theory]
        [InlineData(
            true,
            0)]
        [InlineData(
            false,
            1)]
        public async Task
            ExecuteResultAsync_Adds_WebLinks_Only_If_Response_Has_Not_Started(
                bool responseHasStarted,
                int numLinkAddedToHeaders)
        {
            _httpResponseFeature
                .SetupGet(x => x.HasStarted)
                .Returns(responseHasStarted);

            var pagedCollectionResult =
                new PagedCollectionResult<object>(_pagedCollectionMock.Object);
            var actionContext = new ActionContext(
                CreateDefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor());

            await pagedCollectionResult.ExecuteResultAsync(actionContext)
                .ConfigureAwait(false);

            _headerDictionaryMock.Verify(
                x => x.Add(
                    It.Is<string>(y => y == "Link"),
                    It.IsAny<StringValues>()),
                Times.Exactly(numLinkAddedToHeaders));
            _actionResultExecutorMock.Verify(
                x => x.ExecuteAsync(
                    It.Is<ActionContext>(y => y == actionContext),
                    It.Is<ObjectResult>(z => z == pagedCollectionResult)));
        }

        private DefaultHttpContext CreateDefaultHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Host = new HostString(
                "localhost",
                80);
            httpContext.Request.PathBase = new PathString("/pathbase");
            httpContext.Request.Path = new PathString("/path");
            httpContext.Request.QueryString = new QueryString("?key=value");
            httpContext.Request.Scheme = "http";
            httpContext.RequestServices = _serviceProviderMock.Object;
            httpContext.Features.Set(_httpResponseFeature.Object);
            return httpContext;
        }
    }
}
