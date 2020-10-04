namespace WebLinking.Core
{
    using System;
    using System.Collections.Generic;

    public class LinkRelationType
    {
        private readonly List<string> _relations = new List<string>();

        public IReadOnlyCollection<string> Relations
            => _relations.AsReadOnly();

        public LinkRelationType(
            IEnumerable<string> relations)
        {
            if (relations == null)
            {
                throw new ArgumentNullException(nameof(relations));
            }

            _relations.AddRange(relations);
        }

        public LinkRelationType(
            params string[] relations)
            : this(relations as IEnumerable<string>)
        {
        }

        public static LinkRelationType Parse(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return null; }

            return new LinkRelationType(
                value
                    .Trim()
                    .Split(
                        new[] { " " },
                        StringSplitOptions.RemoveEmptyEntries));
        }

        public override string ToString()
            => string.Join(
                " ",
                _relations);
    }
}
