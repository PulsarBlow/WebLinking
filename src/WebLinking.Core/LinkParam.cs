namespace WebLinking.Core
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    public class LinkParam
    {
        public const string Rel = "rel";
        public const string Anchor = "anchor";
        public const string HrefLang = "hreflang";
        public const string Media = "media";
        public const string Title = "title";
        public const string Type = "type";

        private const string GroupNameKey = "key";
        private const string GroupNameValue = "value";
        private const string GroupNameValueUrlEncoded = "value_urlencoded";

        private static readonly string PARAMPATTERN = $"\\s*(?<{GroupNameKey}>[a-zA-Z_\\*0-9]+)\\s*=\\s*(?:[\"'](?<{GroupNameValue}>[^\"']*)[\"']|(?<{GroupNameValueUrlEncoded}>\\S+))";
        private static readonly Regex ParamRegex = new Regex(PARAMPATTERN, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
        private static readonly Dictionary<string, LinkParamKind> KnownParamTypes = new Dictionary<string, LinkParamKind>
        {
            // rev is not supported because of deprecation
            // https://tools.ietf.org/html/rfc8288#section-3.3
            [Rel] = LinkParamKind.Rel,
            [Anchor] = LinkParamKind.Anchor,
            [HrefLang] = LinkParamKind.HrefLang,
            [Media] = LinkParamKind.Media,
            [Title] = LinkParamKind.Title,
            [Type] = LinkParamKind.Type,
        };

        public LinkParamKind Kind
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Key))
                {
                    return LinkParamKind.None;
                }

                if (!KnownParamTypes.TryGetValue(this.Key, out var paramType))
                {
                    return LinkParamKind.Extension;
                }

                return paramType;
            }
        }

        public string Key { get; set; }

        public string Value { get; set; }

        // Extended parameter ends with an asterisk '*' character.
        // They should be treated as a value containing character Encoding and Language Information
        // https://tools.ietf.org/html/rfc8187#section-3.2.1
        public bool IsExtendedParameter { get; set; }

        public static LinkParam Parse(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return null;
            }

            var match = ParamRegex.Match(parameter);
            if (!match.Success || !match.Groups[GroupNameKey].Success)
            {
                return null;
            }

            string key = match.Groups[GroupNameKey].Value
                .Trim()
                .ToLowerInvariant();

            LinkParam result = new LinkParam
            {
                Key = key.Replace("*", string.Empty),
                IsExtendedParameter = key.EndsWith("*"),
            };

            if (match.Groups[GroupNameValue].Success)
            {
                result.Value = match.Groups[GroupNameValue].Value.Trim();
            }
            else if (match.Groups[GroupNameValueUrlEncoded].Success)
            {
                result.Value = WebUtility.UrlDecode(match.Groups[GroupNameValueUrlEncoded].Value.Trim());
            }

            return result;
        }
    }
}
