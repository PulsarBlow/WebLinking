namespace WebLinking.Integration.AspNetCore.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Internals;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public static class ActionContextExtensions
    {
        public static IEnumerable<LinkValue> GetLinkValues<TItem>(
            this ActionContext context,
            IPagedCollection<TItem> pagedCollection)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (pagedCollection == null)
            {
                return Enumerable.Empty<LinkValue>();
            }

            return LinkValueHelpers.CreateLinkValueCollection(
                new Uri(context.HttpContext.Request.GetDisplayUrl()),
                pagedCollection);
        }
    }
}
