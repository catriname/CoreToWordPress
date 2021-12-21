using CoreApi.Interfaces;
using CoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CoreApi.Data
{
    public class EFRepository : IRepository
    {
        protected readonly ProductContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        private MemoryCacheEntryOptions cacheOptions;

        public EFRepository(ProductContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<List<T>> QueryAsync<T>(string sql, string parameters) where T : BaseEntity
        {
            var query = _dbContext.Set<T>().FromSqlRaw(sql, parameters);
            return await query.ToListAsync();
        }

        //accepts column name as string then queries for all unique values in that column
        //great for drop down lists in forms
        public List<string> DistinctColumnValues<T>(string columnName) where T : BaseEntity
        {
            List<string> distinctValues = new List<string>();

            var temp = _dbContext.Set<T>();

            foreach (var t in temp)
            {
                var value = t.GetType().GetProperty(columnName).GetValue(t, null);
                if (value != null && value.ToString() != string.Empty)
                {
                    distinctValues.Add(value.ToString());
                }
            }

            return distinctValues.Distinct().ToList();
        }

        //optional caching was used in larger database instance, can be removed
        public async Task<List<T>> ReadOnlyList<T>(string cacheKey) where T : BaseEntity
        {
            cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(relative: TimeSpan.FromDays(1));
            //cacheOptions = new MemoryCacheEntryOptions()
            //.SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(1));

            // Check cache, return cache if found
            var value = _memoryCache.Get<List<T>>(cacheKey);

            // Not found, get from DB
            if (value == null)
            {

                value = await _dbContext.Set<T>().ToListAsync();
                
                // write it to the cache
                _memoryCache.Set(cacheKey, value, cacheOptions);
            }

            return value;
        }

        //used to return a singular item.  not needed in this example and 
        //this particular db does not have Id as key
        //public async Task<T> ReadOnlyItem<T>(string cacheKey, int id) where T : BaseEntity
        //{
        //    cacheOptions = new MemoryCacheEntryOptions()
        //      .SetAbsoluteExpiration(relative: TimeSpan.FromDays(1));
        //    //cacheOptions = new MemoryCacheEntryOptions()
        //    // .SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(1));

        //    // Check cache, return cache if found
        //    var value = _memoryCache.Get<T>(cacheKey);

        //    if (value == null)
        //    {

        //        value = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

        //        // write it to the cache
        //        _memoryCache.Set(cacheKey, value, cacheOptions);
        //    }

        //    return value;
        //}
    }
}
