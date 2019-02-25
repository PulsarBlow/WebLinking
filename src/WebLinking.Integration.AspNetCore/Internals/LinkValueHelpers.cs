namespace WebLinking.Integration.AspNetCore.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Primitives;
    using WebLinking.Core;

    internal static class LinkValueHelpers
    {
        public static LinkValue CreateLinkValue(Uri linkTargetUri, string relationType, int offset, int limit)
        {
            if (linkTargetUri == null)
            {
                throw new ArgumentNullException(nameof(linkTargetUri));
            }

            if (string.IsNullOrWhiteSpace(relationType))
            {
                throw new ArgumentException("relationType is not valid", nameof(relationType));
            }

            UriBuilder builder = new UriBuilder(linkTargetUri);

            var query = QueryHelpers.ParseQuery(builder.Query);
            query["offset"] = new StringValues(offset.ToString());
            query["limit"] = new StringValues(limit.ToString());

            var queryBuilder = new QueryBuilder(query.SelectMany(x => x.Value, (col, v) => new KeyValuePair<string, string>(col.Key, v)));
            builder.Query = queryBuilder.ToQueryString().ToString();

            return new LinkValue
            {
                RelationType = new LinkRelationType(relationType),
                TargetUri = builder.Uri,
            };
        }

        public static IEnumerable<LinkValue> CreateLinkValueCollection<TItem>(Uri linkTargetUri, IPagedCollection<TItem> pagedCollection)
        {
            if (linkTargetUri == null)
            {
                throw new ArgumentNullException(nameof(linkTargetUri));
            }

            if (pagedCollection == null)
            {
                throw new ArgumentNullException(nameof(pagedCollection));
            }

            List<LinkValue> linkValues = new List<LinkValue>
            {
                CreateLinkValue(
                    linkTargetUri,
                    LinkRelationRegistry.Start,
                    0,
                    pagedCollection.Limit)
            };

            if (pagedCollection.HasPrevious)
            {
                linkValues.Add(CreateLinkValue(
                    linkTargetUri,
                    LinkRelationRegistry.Previous,
                    pagedCollection.Offset - pagedCollection.Limit,
                    pagedCollection.Limit));
            }

            if (pagedCollection.HasNext)
            {
                linkValues.Add(CreateLinkValue(
                    linkTargetUri,
                    LinkRelationRegistry.Next,
                    pagedCollection.Offset + pagedCollection.Limit,
                    pagedCollection.Limit));
            }

            return linkValues;
        }
    }
}
