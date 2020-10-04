namespace WebLinking.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class LinkRelationRegistry
    {
        private readonly ISet<string> _registeredRelations;
        private static readonly ISet<string> DefaultRegisteredRelations =
            new HashSet<string>(GetRegisteredRelationValues());

        public const string About = "about";
        public const string Alternate = "alternate";
        public const string Appendix = "appendix";
        public const string Archives = "archives";
        public const string Author = "author";
        public const string BlockedBy = "blocked-by";
        public const string Bookmark = "bookmark";
        public const string Canonical = "canonical";
        public const string Chapter = "chapter";
        public const string CiteAs = "cite-as";
        public const string Collection = "collection";
        public const string Contents = "contents";
        public const string ConvertedFrom = "convertedFrom";
        public const string Copyright = "copyright";
        public const string CreateForm = "create-form";
        public const string Current = "current";
        public const string Describedby = "describedby";
        public const string Describes = "describes";
        public const string Disclosure = "disclosure";
        public const string DnsPrefetch = "dns-prefetch";
        public const string Duplicate = "duplicate";
        public const string Edit = "edit";
        public const string EditForm = "edit-form";
        public const string EditMedia = "edit-media";
        public const string Enclosure = "enclosure";
        public const string First = "first";
        public const string Glossary = "glossary";
        public const string Help = "help";
        public const string Hosts = "hosts";
        public const string Hub = "hub";
        public const string Icon = "icon";
        public const string Index = "index";
        public const string Item = "item";
        public const string Last = "last";
        public const string LatestVersion = "latest-version";
        public const string License = "license";
        public const string Lrdd = "lrdd";
        public const string Memento = "memento";
        public const string Monitor = "monitor";
        public const string MonitorGroup = "monitor-group";
        public const string Next = "next";
        public const string NextArchive = "next-archive";
        public const string Nofollow = "nofollow";
        public const string Noreferrer = "noreferrer";
        public const string Original = "original";
        public const string Payment = "payment";
        public const string Pingback = "pingback";
        public const string Preconnect = "preconnect";
        public const string PredecessorVersion = "predecessor-version";
        public const string Prefetch = "prefetch";
        public const string Preload = "preload";
        public const string Prerender = "prerender";
        public const string Prev = "prev";
        public const string Preview = "preview";
        public const string Previous = "previous";
        public const string PrevArchive = "prev-archive";
        public const string PrivacyPolicy = "privacy-policy";
        public const string Profile = "profile";
        public const string Related = "related";
        public const string Restconf = "restconf";
        public const string Replies = "replies";
        public const string Search = "search";
        public const string Section = "section";
        public const string Self = "self";
        public const string Service = "service";
        public const string Start = "start";
        public const string Stylesheet = "stylesheet";
        public const string Subsection = "subsection";
        public const string SuccessorVersion = "successor-version";
        public const string Tag = "tag";
        public const string TermsOfService = "terms-of-service";
        public const string Timegate = "timegate";
        public const string Timemap = "timemap";
        public const string Type = "type";
        public const string Up = "up";
        public const string VersionHistory = "version-history";
        public const string Via = "via";
        public const string Webmention = "webmention";
        public const string WorkingCopy = "working-copy";
        public const string WorkingCopyOf = "working-copy-of";

        public LinkRelationRegistry()
            : this(DefaultRegisteredRelations)
        {
        }

        public LinkRelationRegistry(
            ICollection<string> registeredRelations)
        {
            if (registeredRelations == null)
            {
                throw new ArgumentNullException(nameof(registeredRelations));
            }

            _registeredRelations =
                new HashSet<string>(registeredRelations);
        }

        public bool IsRegisteredRelation(
            string linkRelation)
        {
            if (string.IsNullOrWhiteSpace(linkRelation)) { return false; }

            return _registeredRelations.Contains(
                linkRelation,
                StringComparer.Ordinal);
        }

        private static IEnumerable<string> GetRegisteredRelationValues()
        {
            return typeof(LinkRelationRegistry)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.IsLiteral)
                .Select(x => (string) x.GetRawConstantValue())
                .ToList();
        }
    }
}
