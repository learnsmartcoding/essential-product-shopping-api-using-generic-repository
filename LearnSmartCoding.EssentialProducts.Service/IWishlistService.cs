using LearnSmartCoding.EssentialProducts.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public interface IWishlistService
    {
        Task<List<WishlistItem>> GetWishlistAsync(string ownerId);        
        Task<bool> IsWishlistExistAsync(string ownerId, long wishlistId);
        Task<bool> IsWishlistExistAsync(long id);
        Task<WishlistItem> GetWishListAsync(string ownerId, long wishlistId);
        Task<bool> CreateWishlistAsync(WishlistItem  wishlistItem);        
        Task<bool> DeleteWishlistAsync(long id);        
        
    }
}
