namespace WebLinking.Core.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class LinkValueTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddLinkParam_Throws_When_Type_Is_Not_Supported(
            string key)
        {
            var ex = Record.Exception(
                () => new LinkValue().AddLinkParam(
                    new LinkParam { Key = key }));

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal(
                "Link param kind is not supported",
                ex.Message);
        }

        [Fact]
        public void AddLinkParam_Adds_Nothing_When_Param_Is_Null()
        {
            var linkValue = new LinkValue()
                .AddLinkParam(null);

            Assert.Empty(linkValue.TargetAttributes);
        }

        [Fact]
        public void AddLinkParam_Adds_RelationType()
        {
            var expected = new LinkParam
            {
                Key = LinkParam.Rel,
                Value = "something something_else",
            };
            var linkValue = new LinkValue()
                .AddLinkParam(expected);

            Assert.NotNull(linkValue.RelationType);
            Assert.Equal(
                new[] { "something", "something_else" },
                linkValue.RelationType.Relations);
        }

        [Fact]
        public void AddLinkParam_Adds_Anchor()
        {
            const string expected = "#anchor";
            var param = new LinkParam
            {
                Key = LinkParam.Anchor,
                Value = expected,
            };
            var linkValue = new LinkValue()
                .AddLinkParam(param);

            Assert.NotNull(linkValue.Anchor);
            Assert.Equal(
                expected,
                linkValue.Anchor);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_Returns_Null_When_Value_Is_NullEmptyOrWhitespace(
            string value)
        {
            Assert.Null(LinkValue.Parse(value));
        }

        [Theory]
        [InlineData("http://localhost")]
        [InlineData("<http://localhost")]
        [InlineData("http://localhost>")]
        [InlineData("<>")]
        [InlineData("< >")]
        [InlineData("<")]
        [InlineData(">")]
        public void Parse_Returns_Null_When_LinkTarget_Is_Badly_Formed(
            string value)
        {
            var linkValue = LinkValue.Parse(value);

            Assert.Null(linkValue);
        }

        [Theory]
        [MemberData(nameof(GetParseTestData))]
        public void Parse_Returns_LinkValue(
            string value,
            LinkValue expected)
        {
            var actual = LinkValue.Parse(value);

            Assert.NotNull(actual);
            AssertEqual(
                expected,
                actual);
        }

        public static IEnumerable<object[]> GetParseTestData()
        {
            yield return new object[]
            {
                "<https://localhost/?&key=value>; rel=\"previous\"; title*=UTF-8'de'Letztes%20Kapitel; anchor=\"#there\"",
                new LinkValue
                    {
                        TargetUri = new Uri("https://localhost/?&key=value"),
                        Anchor = "#there",
                        RelationType =
                            new LinkRelationType(new[] { "previous" }),
                    }
                    .AddLinkParam(
                        new LinkParam
                        {
                            Key = LinkParam.Title,
                            Value = "UTF-8'de'Letztes Kapitel",
                            IsExtendedParameter = true
                        })
                    .AddLinkParam(
                        new LinkParam
                        {
                            Key = LinkParam.Anchor, Value = "should_be_skipped"
                        }),
            };

            yield return new object[]
            {
                "<https://localhost/?&key=value>; rel=\"start http://localhost/value\"; title=\"A title\"; title*=UTF-8'de'Letztes%20Kapitel",
                new LinkValue
                    {
                        TargetUri = new Uri("https://localhost/?&key=value"),
                        RelationType = new LinkRelationType(
                            new[] { "start", "http://localhost/value" }),
                    }
                    .AddLinkParam(
                        new LinkParam
                        {
                            Key = LinkParam.Title, Value = "A title",
                            IsExtendedParameter = false
                        })
                    .AddLinkParam(
                        new LinkParam
                        {
                            Key = LinkParam.Title,
                            Value = "UTF-8'de'Letztes Kapitel",
                            IsExtendedParameter = true
                        }),
            };
        }

        private static void AssertEqual(
            LinkValue expected,
            LinkValue actual)
        {
            Assert.Equal(
                expected.TargetUri,
                actual.TargetUri);
            Assert.Equal(
                expected.Anchor,
                actual.Anchor);

            AssertEqual(
                expected.RelationType,
                actual.RelationType);
            AssertEqual(
                expected.TargetAttributes,
                actual.TargetAttributes);
        }

        private static void AssertEqual(
            LinkRelationType expected,
            LinkRelationType actual)
        {
            if (expected == null)
            {
                Assert.Null(actual);
                return;
            }

            Assert.Equal(
                expected.Relations,
                actual.Relations);
        }

        private static void AssertEqual(
            IEnumerable<LinkParam> expected,
            IEnumerable<LinkParam> actual)
        {
            var expectedParams = expected as LinkParam[] ?? expected.ToArray();
            var actualParams = actual as LinkParam[] ?? actual.ToArray();

            Assert.Equal(
                expectedParams.Length,
                actualParams.Length);
            for (var i = 0;
                i < expectedParams.Length;
                i++)
            {
                AssertEqual(
                    expectedParams[i],
                    actualParams[i]);
            }
        }

        private static void AssertEqual(
            LinkParam expected,
            LinkParam actual)
        {
            Assert.Equal(
                expected.Key,
                actual.Key);
            Assert.Equal(
                expected.Value,
                actual.Value);
            Assert.Equal(
                expected.Kind,
                actual.Kind);
            Assert.Equal(
                expected.IsExtendedParameter,
                actual.IsExtendedParameter);
        }
    }
}
