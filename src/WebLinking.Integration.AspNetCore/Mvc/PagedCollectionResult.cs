namespace WebLinking.Integration.AspNetCore.Mvc
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class PagedCollectionResult<T> : ObjectResult
    {
        private readonly IPagedCollection<T> _pagedCollection;

        public PagedCollectionResult(IPagedCollection<T> pagedCollection)
            : base(pagedCollection)
        {
            _pagedCollection = pagedCollection ?? throw new ArgumentNullException(nameof(pagedCollection));
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var links = context.GetLinkValues(_pagedCollection);
            var response = context.HttpContext.Response;
            if (!response.HasStarted)
            {
                response.Headers.AddWebLink(links);
            }

            await base.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
}
