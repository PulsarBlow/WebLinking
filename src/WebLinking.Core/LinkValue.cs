namespace WebLinking.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LinkValue
    {
        private readonly List<LinkParam> _targetAttributes =
            new List<LinkParam>();

        public Uri TargetUri { get; set; }

        public string Anchor { get; set; }

        public LinkRelationType RelationType { get; set; }

        public IReadOnlyCollection<LinkParam> TargetAttributes
            => _targetAttributes;

        public LinkValue AddLinkParam(
            LinkParam param)
        {
            if (param == null) { return this; }

            switch (param.Kind)
            {
                case LinkParamKind.Rel:
                    RelationType ??= LinkRelationType.Parse(param.Value);
                    break;
                case LinkParamKind.Anchor:
                    Anchor ??= param.Value;
                    break;
                case LinkParamKind.HrefLang:
                case LinkParamKind.Media:
                case LinkParamKind.Title:
                case LinkParamKind.Type:
                case LinkParamKind.Extension:
                    _targetAttributes.Add(param);
                    break;
                default:
                    throw new InvalidOperationException(
                        "Link param kind is not supported");
            }

            return this;
        }

        public static LinkValue Parse(
            string linkValue)
        {
            if (string.IsNullOrWhiteSpace(linkValue)) { return null; }

            var parts = linkValue.Split(
                new[] { ';' },
                StringSplitOptions.RemoveEmptyEntries);

            var targetUri = ParseTargetUri(
                parts[0]
                    .Trim());
            if (targetUri == null) { return null; }

            var result = new LinkValue
            {
                TargetUri = targetUri,
            };

            for (var i = 1;
                i < parts.Length;
                i++)
            {
                var param = LinkParam.Parse(
                    parts[i]
                        .Trim());
                result.AddLinkParam(param);
            }

            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(
                $"<{TargetUri}>; rel=\"{RelationType}\";{(Anchor != null ? $" anchor=\"{Anchor}\";" : null)}");
            foreach (var item in _targetAttributes)
            {
                builder.Append(
                    $" {item.Key}{(item.IsExtendedParameter ? "*" : null)}={item.Value};");
            }

            return builder.ToString()
                .TrimEnd(';');
        }

        private static Uri ParseTargetUri(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value)
                || !value.StartsWith("<")
                || !value.EndsWith(">")) { return null; }

            var uri = value.Substring(
                    1,
                    value.Length - 2)
                .Trim();
            if (
                string.IsNullOrEmpty(uri)
                || !Uri.IsWellFormedUriString(
                    uri,
                    UriKind.RelativeOrAbsolute)) { return null; }

            return new Uri(uri);
        }
    }
}
