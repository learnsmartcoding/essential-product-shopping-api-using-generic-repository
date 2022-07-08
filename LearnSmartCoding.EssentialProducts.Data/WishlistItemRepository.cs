
using LearnSmartCoding.EssentialProducts.Core;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public interface IWishlistItemRepository : IGenericRepository<WishlistItem>
    {
        Task<bool> CommitAsync();
    }

    public class WishlistItemRepository : GenericRepository<WishlistItem>, IWishlistItemRepository
    {
        public WishlistItemRepository(EssentialProductsDbContext context) : base(context)
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
