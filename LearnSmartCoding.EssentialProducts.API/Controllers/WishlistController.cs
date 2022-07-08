using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Update;
using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistService wishlistService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ILogger<WishlistController> Logger { get; }

        public WishlistController(ILogger<WishlistController> logger,
            IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            this.wishlistService = wishlistService;
            this.httpContextAccessor = httpContextAccessor;
        }




        [HttpGet("all", Name = "GetWishlists")]
        [ProducesResponseType(typeof(List<WishlistItemViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWishlistItemsAsync()
        {
            var adObjectId = httpContextAccessor.HttpContext.User.GetObjectId();
            var userName = httpContextAccessor.HttpContext.User.GetNameIdentifierId();
            var adObjName = adObjectId??"Admin";
            Logger.LogInformation($"Executing {nameof(GetWishlistItemsAsync)}");

            var wishlists = await wishlistService.GetWishlistAsync(adObjName);

            var wishlistsViewModel = wishlists.Select(s => new WishlistItemViewModel()
            {
                OwnerADObjectId = s.OwnerADObjectId,
                ProductId = Convert.ToInt32(s.ProductId),
                Id = s.Id
            }).ToList();

            return Ok(wishlistsViewModel);
        }




        [HttpPost("", Name = "CreateWishlist")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostWishlistAsync([FromBody] CreateWishlistItem createWishlistItem)
        {
            Logger.LogInformation($"Executing {nameof(PostWishlistAsync)}");
            var adObjectId = httpContextAccessor.HttpContext.User.GetObjectId();
            var userName = httpContextAccessor.HttpContext.User.GetNameIdentifierId();

            
            var wishListInDB = await wishlistService.GetWishListAsync(adObjectId??"Admin", createWishlistItem.ProductId);

            if (wishListInDB==null)
            {
                var entity = new WishlistItem()
                {
                    OwnerADObjectId = adObjectId ?? "Admin",
                    ProductId = createWishlistItem.ProductId
                };

                var isSuccess = await wishlistService.CreateWishlistAsync(entity);
                return new CreatedAtRouteResult("GetWishlist",
                  new { id = entity.Id });
            }
            return new CreatedAtRouteResult("GetWishlist",
                   new { id = wishListInDB.Id });
        }




        [HttpDelete("{id}", Name = "DeleteWishlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        //[Authorize]
        //[AuthorizeForScopes(Scopes = new string[] {
        //    "https://learnsmartcoding.onmicrosoft.com/api/product.write"
        //})]
        public async Task<IActionResult> DeleteWishlistAsync([FromRoute] long id)
        {

            Logger.LogInformation($"Executing {nameof(DeleteWishlistAsync)}");

            var exist = await wishlistService.IsWishlistExistAsync(id);

            if (!exist)
                return NotFound();

            await wishlistService.DeleteWishlistAsync(id);

            return Ok();
        }


    }
}
