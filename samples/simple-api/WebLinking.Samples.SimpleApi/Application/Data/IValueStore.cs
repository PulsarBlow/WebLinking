namespace WebLinking.Samples.SimpleApi.Application.Data
{
    using Models;

    public interface IValueStore
    {
        ValueModel GetById(
            int id);

        PagedCollection<ValueModel> GetPagedCollection(
            int offset,
            int limit);
    }
}
