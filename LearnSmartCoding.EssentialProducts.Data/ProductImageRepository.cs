
using LearnSmartCoding.EssentialProducts.Core;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public interface IProductImageRepository : IGenericRepository<ProductImage>
    {
        Task<bool> CommitAsync();
    }

    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(EssentialProductsDbContext context) : base(context)
        {
            Context = context;
        }
        public EssentialProductsDbContext Context { get; }
        public async Task<bool> CommitAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }

}
