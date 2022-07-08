using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistItemRepository wishlistItemRepository;

        public WishlistService(IWishlistItemRepository wishlistItemRepository)
        {
            this.wishlistItemRepository = wishlistItemRepository;
        }
        public Task<bool> CreateWishlistAsync(WishlistItem wishlistItem)
        {
            wishlistItemRepository.Add(wishlistItem);
            return wishlistItemRepository.CommitAsync();
        }

        public Task<bool> DeleteWishlistAsync(long id)
        {
            wishlistItemRepository.Delete(id);
            return wishlistItemRepository.CommitAsync();
        }

        public Task<List<WishlistItem>> GetWishlistAsync(string ownerId)
        {
            return wishlistItemRepository.GetAsync(g => g.OwnerADObjectId == ownerId);
        }

        public async Task<bool> IsWishlistExistAsync(string ownerId, long wishlistId)
        {
            var items = await wishlistItemRepository.GetAsync(g => g.OwnerADObjectId == ownerId && g.ProductId == wishlistId);
            return items.Any();
        }

        public async Task<WishlistItem> GetWishListAsync(string ownerId, long wishlistId)
        {
            var items = await wishlistItemRepository.GetAsync(g => g.OwnerADObjectId == ownerId && g.ProductId == wishlistId);
            return items.FirstOrDefault();
        }

        public async Task<bool> IsWishlistExistAsync(long id)
        {
            var items = await wishlistItemRepository.GetAsync(g => g.Id == id);
            return items.Any();
        }
    }
}
