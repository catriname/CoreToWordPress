namespace CoreApi.Data
{
    public class ProductRepository : EFRepository, IProductRepository
    {
        public ProductRepository(ProductContext dbContext, IMemoryCache memoryCache) : base(dbContext, memoryCache)
        {
        }

    }
}
