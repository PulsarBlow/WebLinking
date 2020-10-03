namespace WebLinking.Core.Tests.UnitTests
{
    using System.Collections.Generic;
    using Xunit;

    public class LinkParamTest
    {
        [Theory]
        [InlineData(null, LinkParamKind.None)]
        [InlineData("", LinkParamKind.None)]
        [InlineData(" ", LinkParamKind.None)]
        [InlineData("something", LinkParamKind.Extension)]
        [InlineData(LinkParam.Rel, LinkParamKind.Rel)]
        [InlineData(LinkParam.Anchor, LinkParamKind.Anchor)]
        [InlineData(LinkParam.HrefLang, LinkParamKind.HrefLang)]
        [InlineData(LinkParam.Media, LinkParamKind.Media)]
        [InlineData(LinkParam.Title, LinkParamKind.Title)]
        [InlineData(LinkParam.Type, LinkParamKind.Type)]
        public void ParamType_Returns_Expected_LinkParamType(string key, LinkParamKind expectedType)
        {
            Assert.Equal(expectedType, new LinkParam { Key = key }.Kind);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("key")]
        [InlineData("keyvalue")]
        [InlineData("key:value")]
        [InlineData("key=")]
        [InlineData("key=  ")]
        [InlineData("=value")]
        [InlineData("  =value")]
        public void Parse_Returns_Null_When_Parameter_Is_Badly_Formated(string parameter)
        {
            Assert.Null(LinkParam.Parse(parameter));
        }

        [Theory]
        [InlineData("key=value")]
        [InlineData("KEY=value")]
        [InlineData("key = value")]
        [InlineData(" key = value ")]
        [InlineData("key=\"value\"")]
        public void Parse_Returns_LinkParam(string parameter)
        {
            var expected = new LinkParam { Key = "key", Value = "value" };
            var actual = LinkParam.Parse(parameter);

            Assert.Equal(expected.Key, actual.Key);
            Assert.Equal(expected.Value, actual.Value);
            Assert.Equal(expected.Kind, actual.Kind);
        }

        [Theory]
        [MemberData(nameof(GetLinkParamEdgeCases))]
        public void Parse_Returns_LinkParam_EdgeCases(string parameter, LinkParam expected)
        {
            var actual = LinkParam.Parse(parameter);

            Assert.NotNull(actual);
            Assert.Equal(expected.Key, actual.Key);
            Assert.Equal(expected.Value, actual.Value);
            Assert.Equal(expected.Kind, actual.Kind);
            Assert.Equal(expected.IsExtendedParameter, actual.IsExtendedParameter);
        }

        public static IEnumerable<object[]> GetLinkParamEdgeCases()
        {
            yield return new object[]
            {
                "title*=UTF-8'de'n%c3%a4chstes%20Kapitel",
                new LinkParam { Key = "title", Value = "UTF-8'de'nächstes Kapitel", IsExtendedParameter = true },
            };
        }
    }
}
