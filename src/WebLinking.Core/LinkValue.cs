namespace WebLinking.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LinkValue
    {
        private readonly List<LinkParam> _targetAttributes = new List<LinkParam>();

        public Uri TargetUri { get; set; }

        public string Anchor { get; set; }

        public LinkRelationType RelationType { get; set; }

        public IReadOnlyCollection<LinkParam> TargetAttributes => this._targetAttributes;

        public LinkValue AddLinkParam(LinkParam param)
        {
            if (param == null)
            {
                return this;
            }

            switch (param.Kind)
            {
                case LinkParamKind.Rel:
                    this.RelationType = this.RelationType ?? LinkRelationType.Parse(param.Value);
                    break;
                case LinkParamKind.Anchor:
                    this.Anchor = this.Anchor ?? param.Value;
                    break;
                case LinkParamKind.HrefLang:
                case LinkParamKind.Media:
                case LinkParamKind.Title:
                case LinkParamKind.Type:
                case LinkParamKind.Extension:
                    this._targetAttributes.Add(param);
                    break;
                default:
                    throw new InvalidOperationException("Link param kind is not supported");
            }

            return this;
        }

        public static LinkValue Parse(string linkValue)
        {
            if (string.IsNullOrWhiteSpace(linkValue))
            {
                return null;
            }

            string[] parts = linkValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Uri targetUri = ParseTargetUri(parts[0].Trim());
            if (targetUri == null)
            {
                return null;
            }

            LinkValue result = new LinkValue
            {
                TargetUri = targetUri,
            };

            for (int i = 1; i < parts.Length; i++)
            {
                LinkParam param = LinkParam.Parse(parts[i].Trim());
                result.AddLinkParam(param);
            }

            return result;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"<{this.TargetUri}>; rel=\"{this.RelationType}\";{(this.Anchor != null ? $" anchor=\"{this.Anchor}\";" : null)}");
            foreach (var item in this._targetAttributes)
            {
                builder.Append($" {item.Key}{(item.IsExtendedParameter ? "*" : null)}={item.Value};");
            }

            return builder.ToString().TrimEnd(';');
        }

        private static Uri ParseTargetUri(string value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !value.StartsWith("<") ||
                !value.EndsWith(">"))
            {
                return null;
            }

            string uri = value.Substring(1, value.Length - 2).Trim();
            if (
                string.IsNullOrEmpty(uri) ||
                !Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            {
                return null;
            }

            return new Uri(uri);
        }
    }
}
