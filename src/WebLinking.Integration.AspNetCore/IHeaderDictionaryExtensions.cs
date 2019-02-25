namespace WebLinking.Integration.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using WebLinking.Core;

    public static class IHeaderDictionaryExtensions
    {
        public static IHeaderDictionary AddWebLink(this IHeaderDictionary headers, LinkValue linkValue)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (linkValue == null)
            {
                throw new ArgumentNullException(nameof(linkValue));
            }

            headers.Add("Link", new StringValues(linkValue.ToString()));
            return headers;
        }

        public static IHeaderDictionary AddWebLink(this IHeaderDictionary headers, IEnumerable<LinkValue> linkValueCollection)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (linkValueCollection == null)
            {
                throw new ArgumentNullException(nameof(linkValueCollection));
            }

            // Join because using 2 stringvalues will create 2 link headers.
            // We want only 1.
            headers.Add("Link", new StringValues(string.Join(",", linkValueCollection.Select(x => x.ToString()))));
            return headers;
        }

        public static IHeaderDictionary AddWebLink(this IHeaderDictionary headers, params LinkValue[] linkValueCollection)
        {
            return AddWebLink(headers, linkValueCollection as IEnumerable<LinkValue>);
        }
    }
}
