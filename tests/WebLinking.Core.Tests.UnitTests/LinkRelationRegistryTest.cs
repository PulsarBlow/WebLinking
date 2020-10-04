namespace WebLinking.Core.Tests.UnitTests
{
    using System;
    using Xunit;

    public class LinkRelationRegistryTest
    {
        [Fact]
        public void Constructor_Throws_When_Relations_Is_Null()
        {
            var ex = Record.Exception(() => new LinkRelationRegistry(null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_Sets_RegisteredRelations()
        {
            var relations = new[] { "relation1", "relation2", "relation2" };
            var registry = new LinkRelationRegistry(relations);

            Assert.True(registry.IsRegisteredRelation(relations[0]));
            Assert.True(registry.IsRegisteredRelation(relations[1]));
            Assert.True(registry.IsRegisteredRelation(relations[2]));
        }

        [Fact]
        public void Constructor_Sets_DefaultRegisteredRelations()
        {
            var registry = new LinkRelationRegistry();

            Assert.True(
                registry.IsRegisteredRelation(LinkRelationRegistry.Appendix));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void
            IsRegisteredRelation_Returns_False_When_LinkRelation_Is_NullEmptyOrWhiteSpace(
                string linkRelation)
        {
            var registry = new LinkRelationRegistry();

            Assert.False(registry.IsRegisteredRelation(linkRelation));
        }

        [Fact]
        public void GetRegisteredRelationValues_DoesNot_Register_Readonly()
        {
            var registry = new RegistryTest();

            Assert.False(registry.IsRegisteredRelation(registry.Something));
        }

        private class RegistryTest : LinkRelationRegistry
        {
#pragma warning disable SA1401
            public readonly string Something = "something";
#pragma warning restore SA1401
        }
    }
}
