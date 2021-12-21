using CoreApi.Models;

namespace CoreApi.Interfaces
{
    public interface IRepository
    {
        Task<List<T>> ReadOnlyList<T>(string cacheKey) where T : BaseEntity;
        //
        //Task<T> ReadOnlyItem<T>(string cacheKey, int id) where T : BaseEntity;
    }
}
