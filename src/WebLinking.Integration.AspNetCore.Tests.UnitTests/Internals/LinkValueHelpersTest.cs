namespace WebLinking.Integration.AspNetCore.Tests.UnitTests.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebLinking.Core;
    using WebLinking.Integration.AspNetCore.Internals;
    using Xunit;

    public class LinkValueHelpersTest
    {
        private readonly IEqualityComparer<LinkValue> _comparer = new LinkValueComparer();

        private readonly Uri _linkTargetUri = new Uri("https://localhost:1337/values?param=value");

        private readonly LinkValue _start = new LinkValue
        {
            RelationType = new LinkRelationType(LinkRelationRegistry.Start),
            TargetUri = new Uri("https://localhost:1337/values?param=value&offset=0&limit=5"),
        };

        private readonly LinkValue _previous = new LinkValue
        {
            RelationType = new LinkRelationType(LinkRelationRegistry.Previous),
            TargetUri = new Uri("https://localhost:1337/values?param=value&offset=0&limit=5"),
        };

        private readonly LinkValue _next = new LinkValue
        {
            RelationType = new LinkRelationType(LinkRelationRegistry.Next),
            TargetUri = new Uri("https://localhost:1337/values?param=value&offset=10&limit=5"),
        };

        private readonly ObjectPagedCollection _pagedCollection = new ObjectPagedCollection
        {
            HasPrevious = true,
            HasNext = true,
            Limit = 5,
            Offset = 5,
            TotalSize = 15,
        };

        [Fact]
        public void CreateLinkValue_Throws_When_Uri_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "linkTargetUri",
                () => LinkValueHelpers.CreateLinkValue(null, "relationType", int.MaxValue, int.MaxValue));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateLinkValue_Throws_When_RelationType_Is_NullEmptyOrWhitespace(string value)
        {
            Assert.Throws<ArgumentException>(
                "relationType",
                () => LinkValueHelpers.CreateLinkValue(new Uri("http://localhost/"), value, int.MaxValue, int.MaxValue));
        }

        [Theory]
        [InlineData(
            "https://localhost:1337/values?param1=abc&offset=1&limit=100",
            "https://localhost:1337/values?param1=abc",
            1,
            100)]
        [InlineData(
            "http://localhost/values?offset=0&limit=0",
            "http://localhost/values",
            0,
            0)]
        public void CreateLinkValue_Returns_LinkValue(
            string expected,
            string value,
            int offset,
            int limit)
        {
            string relationType = "relationType";
            var result = LinkValueHelpers.CreateLinkValue(new Uri(value), relationType, offset, limit);

            Assert.NotNull(result);
            Assert.Contains(relationType, result.RelationType.Relations);
            Assert.Equal(expected, result.TargetUri.ToString());
        }

        [Fact]
        public void CreateLinkValueCollection_Throws_When_LinkTargetUri_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "linkTargetUri",
                () => LinkValueHelpers.CreateLinkValueCollection(null, new ObjectPagedCollection()));
        }

        [Fact]
        public void CreateLinkValueCollection_Throws_When_PagedCollection_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(
                "pagedCollection",
                () => LinkValueHelpers.CreateLinkValueCollection(new Uri("http://localhost/"), (IPagedCollection<object>)null));
        }

        [Fact]
        public void CreateLinkValueCollection_Returns_LinkValueCollection()
        {
            var result = LinkValueHelpers.CreateLinkValueCollection(_linkTargetUri, _pagedCollection);

            Assert.Equal(3, result.Count());
            Assert.Contains(_start, result, _comparer);
            Assert.Contains(_previous, result, _comparer);
            Assert.Contains(_next, result, _comparer);
        }

        [Fact]
        public void CreateLinkValueCollection_Returns_LinkValueCollection_Without_Previous_LinkValue()
        {
            _pagedCollection.HasPrevious = false;

            var result = LinkValueHelpers.CreateLinkValueCollection(_linkTargetUri, _pagedCollection);

            Assert.Equal(2, result.Count());
            Assert.Contains(_start, result, _comparer);
            Assert.DoesNotContain(_previous, result, _comparer);
            Assert.Contains(_next, result, _comparer);
        }

        [Fact]
        public void CreateLinkValueCollection_Returns_LinkValueCollection_Without_Next_LinkValue()
        {
            _pagedCollection.HasNext = false;

            var result = LinkValueHelpers.CreateLinkValueCollection(_linkTargetUri, _pagedCollection);

            Assert.Equal(2, result.Count());
            Assert.Contains(_start, result, new LinkValueComparer());
            Assert.Contains(_previous, result, new LinkValueComparer());
            Assert.DoesNotContain(_next, result, new LinkValueComparer());
        }

        private class ObjectPagedCollection : IPagedCollection<object>
        {
            public bool HasNext { get; set; }

            public bool HasPrevious { get; set; }

            public int Limit { get; set; }

            public int Offset { get; set; }

            public int TotalSize { get; set; }

            public ICollection<object> Items { get; set; } = new List<object>();
        }

        private class LinkValueComparer : IEqualityComparer<LinkValue>
        {
            public bool Equals(LinkValue x, LinkValue y)
            {
                return x.RelationType.ToString() == y.RelationType.ToString() &&
                    x.TargetUri.ToString() == y.TargetUri.ToString();
            }

            public int GetHashCode(LinkValue obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
