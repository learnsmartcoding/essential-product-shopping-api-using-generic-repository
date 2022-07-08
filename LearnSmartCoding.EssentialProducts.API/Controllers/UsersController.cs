using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public ILogger<WishlistController> Logger { get; }

        public UsersController(ILogger<WishlistController> logger,
            IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
        }

        [HttpGet("", Name = "GetUserAccess")]
        [ProducesResponseType(typeof(List<UserAccess>), StatusCodes.Status200OK)]
        public IActionResult GetUserAccess()
        {
            var model = new List<UserAccess>() {
                new UserAccess() { Id=1,UserId="specialuser", HasAccess=true },
                new UserAccess() { Id=1,UserId="admin", HasAccess=true },
                new UserAccess() { Id=1,UserId="user", HasAccess=false },
                new UserAccess() { Id=1,UserId="readonly", HasAccess=false },
                new UserAccess() { Id=1,UserId="limited", HasAccess=false }
            };
            return Ok(model);
        }
    }

    public class UserAccess
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool HasAccess { get; set; }
    }
}
