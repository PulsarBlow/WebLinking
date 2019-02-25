namespace WebLinking.DemoApi.Models
{
    using System.Collections.Generic;
    using WebLinking.Integration.AspNetCore;

    public class PagedCollection<TModel> : IPagedCollection<TModel>
    {
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public int TotalSize { get; set; }

        public ICollection<TModel> Items { get; private set; } = new List<TModel>();

        public PagedCollection(IEnumerable<TModel> items)
        {
            Items = new List<TModel>(items ?? new TModel[] { });
        }
    }
}
