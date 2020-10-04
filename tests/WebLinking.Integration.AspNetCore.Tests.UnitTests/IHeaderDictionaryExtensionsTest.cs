namespace WebLinking.Integration.AspNetCore.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Xunit;

    public class IHeaderDictionaryExtensionsTest
    {
        private readonly Mock<IHeaderDictionary> _headerDictionaryMock =
            new Mock<IHeaderDictionary>();

        private readonly LinkValue _start = new LinkValue
        {
            Anchor = "anchor",
            RelationType = new LinkRelationType(LinkRelationRegistry.Start),
            TargetUri = new Uri("https://localhost/"),
        };

        private readonly LinkValue _previous = new LinkValue
        {
            Anchor = "anchor",
            RelationType = new LinkRelationType(LinkRelationRegistry.Previous),
            TargetUri = new Uri("https://localhost/"),
        };

        [Fact]
        public void AddWebLink_Throws_When_Headers_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "headers",
                () => IHeaderDictionaryExtensions.AddWebLink(
                    null,
                    new LinkValue()));

            Assert.Throws<ArgumentNullException>(
                "headers",
                () => IHeaderDictionaryExtensions.AddWebLink(
                    null,
                    new List<LinkValue>()));

            Assert.Throws<ArgumentNullException>(
                "headers",
                () => IHeaderDictionaryExtensions.AddWebLink(
                    null,
                    new LinkValue(),
                    new LinkValue()));
        }

        [Fact]
        public void AddWebLink_Throws_When_LinkValue_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "linkValue",
                () => _headerDictionaryMock.Object.AddWebLink(
                    (LinkValue) null));
        }

        [Fact]
        public void AddWebLink_Adds_Link_Using_LinkValue_To_Headers()
        {
            _headerDictionaryMock.Object.AddWebLink(_start);

            _headerDictionaryMock.Verify(
                x => x.Add(
                    It.Is<string>(s => s == "Link"),
                    It.Is<StringValues>(
                        sv => sv.ToString() == _start.ToString())));
        }

        [Fact]
        public void AddWebLink_Throws_When_LinkValueCollection_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "linkValueCollection",
                () => _headerDictionaryMock.Object.AddWebLink(
                    (IEnumerable<LinkValue>) null));
        }

        [Fact]
        public void AddWebLink_Adds_Link_Using_LinkValueCollection_To_Headers()
        {
            _headerDictionaryMock.Object.AddWebLink(
                _start,
                _previous);

            _headerDictionaryMock.Verify(
                x => x.Add(
                    It.Is<string>(s => s == "Link"),
                    It.Is<StringValues>(
                        sv => sv.ToString() == $"{_start},{_previous}")));
        }

        [Fact]
        public void AddWebLink_Adds_Link_Using_LinkValueParams_To_Headers()
        {
            _headerDictionaryMock.Object.AddWebLink(
                _start,
                _previous);

            _headerDictionaryMock.Verify(
                x => x.Add(
                    It.Is<string>(s => s == "Link"),
                    It.Is<StringValues>(
                        sv => sv.ToString() == $"{_start},{_previous}")));
        }
    }
}
