namespace WebLinking.Samples.SimpleApi.Models
{
    using System.Collections.Generic;
    using Integration.AspNetCore;

    public class PagedCollection<TModel> : IPagedCollection<TModel>
    {
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public int TotalSize { get; set; }

        public ICollection<TModel> Items { get; private set; }

        public PagedCollection(
            IEnumerable<TModel> items)
        {
            Items = new List<TModel>(items ?? new TModel[] { });
        }
    }
}
