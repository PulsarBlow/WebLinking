namespace WebLinking.DemoApi.Application.Data
{
    using WebLinking.DemoApi.Models;

    public interface IValueStore
    {
        ValueModel GetById(int id);

        PagedCollection<ValueModel> GetPagedCollection(int offset, int limit);
    }
}
