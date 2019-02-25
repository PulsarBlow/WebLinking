namespace WebLinking.Integration.AspNetCore
{
    using System.Collections.Generic;

    public interface IPagedCollection<TItem>
    {
        bool HasNext { get; }

        bool HasPrevious { get; }

        int Limit { get; }

        int Offset { get; }

        int TotalSize { get; }

        ICollection<TItem> Items { get; }
    }
}
