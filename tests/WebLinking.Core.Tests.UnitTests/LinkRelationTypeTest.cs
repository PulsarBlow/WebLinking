namespace WebLinking.Core.Tests.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait(
    TestTraits.UnitTest.NAME,
    TestTraits.UnitTest.VALUE)]
    public class LinkRelationTypeTest
    {
        [Fact]
        public void Constructor_Throws_When_Relations_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new LinkRelationType(null));
        }

        [Theory]
        [MemberData(nameof(GenerateTestData_For_Constructor_Adds_Relations))]
        public void Constructor_Adds_Relations(string[] expected)
        {
            var linkRelationType = new LinkRelationType(expected);
            var actual = linkRelationType.Relations;

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GenerateTestData_For_Constructor_Adds_Relations()
        {
            yield return new[] { new[] { string.Empty } };
            yield return new[] { new[] { "relation1" } };
            yield return new[] { new[] { "relation1", "relation2" } };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_Returns_Null_When_Value_Is_NullEmptyOrWhitespace(string value)
        {
            Assert.Null(LinkRelationType.Parse(value));
        }

        [Theory]
        [InlineData("something", new[] { "something" })]
        [InlineData("something something_else", new[] { "something", "something_else" })]
        [InlineData("  something   something_else  ", new[] { "something", "something_else" })]
        [InlineData("something something", new[] { "something", "something" })]
        [InlineData("something1something", new[] { "something1something" })]
        [InlineData("somethingAsomething", new[] { "somethingAsomething" })]
        [InlineData("something-something", new[] { "something-something" })]
        public void Parse_Returns_LinkRelationType(string value, string[] expectedRelations)
        {
            var actual = LinkRelationType.Parse(value);

            Assert.NotNull(actual);
            Assert.Equal(expectedRelations, actual.Relations);
        }
    }
}
