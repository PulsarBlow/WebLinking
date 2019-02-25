namespace WebLinking.DemoApi.Application.Data
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using WebLinking.DemoApi.Models;

    public class MemoryValueStore : IValueStore
    {
        private static readonly ConcurrentDictionary<int, ValueModel> Data = new ConcurrentDictionary<int, ValueModel>(GenerateData());

        public ValueModel GetById(int id)
        {
            if (!Data.ContainsKey(id))
            {
                return null;
            }

            return Data[id];
        }

        public PagedCollection<ValueModel> GetPagedCollection(int offset, int limit)
        {
            var items = Data.Values.Skip(offset).Take(limit);
            return new PagedCollection<ValueModel>(items)
            {
                HasNext = (offset + limit) < Data.Count,
                HasPrevious = offset > 0,
                Limit = limit,
                Offset = offset,
                TotalSize = Data.Count,
            };
        }

        private static Dictionary<int, ValueModel> GenerateData(int number = 1000)
        {
            var result = new Dictionary<int, ValueModel>(number);
            for (int i = 0; i < number; i++)
            {
                result.Add(
                    i,
                    new ValueModel { Id = i, Value = $"value {i}", });
            }

            return result;
        }
    }
}
