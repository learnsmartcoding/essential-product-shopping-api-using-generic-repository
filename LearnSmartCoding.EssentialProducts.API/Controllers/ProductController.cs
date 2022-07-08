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
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ILogger<ProductController> Logger { get; }

        public ProductController(ILogger<ProductController> logger,
            IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            this.productService = productService;
            this.httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAsync([FromRoute] int id)
        {

            Logger.LogInformation($"Executing {nameof(GetProductAsync)}");

            var product = await productService.GetProductAndImagesAsync(id);

            if (product == null)
                return NotFound();

            var productViewModel = new ProductViewModel()
            {
                AvailableSince = product.AvailableSince,
                CategoryId = Convert.ToInt16(product.CategoryId),
                Descriptions = product.Descriptions,
                Id = product.Id,
                IsActive = product.IsActive,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages.Any() ? product.ProductImages.Select(s => new ProductImagesViewModel()
                {
                    Id = s.Id,
                    Mime = s.Mime,
                    ImageName = s.ImageName,
                    ProductId = Convert.ToInt32(s.ProductId),
                    Base64Image = Convert.ToBase64String(s.Image),
                    IsActive = true
                }).ToList() : new List<ProductImagesViewModel>()
            };

            return Ok(productViewModel);
        }

        [HttpGet("all", Name = "GetProducts")]
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsAsync(int noOfProducts = 5)
        {

            Logger.LogInformation($"Executing {nameof(GetProductsAsync)}");

            var products = await productService.GetProductsAsync(noOfProducts);

            var productsViewModel = products.Select(product =>
            new ProductViewModel()
            {
                AvailableSince = product.AvailableSince,
                CategoryId = Convert.ToInt16(product.CategoryId),
                Descriptions = product.Descriptions,
                Id = product.Id,
                IsActive = product.IsActive,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages.Any() ? product.ProductImages.Select(s => new ProductImagesViewModel()
                {
                    Id = s.Id,
                    Mime = s.Mime,
                    ImageName = s.ImageName,
                    ProductId = Convert.ToInt32(s.ProductId),
                    Base64Image = Convert.ToBase64String(s.Image),
                    IsActive = true
                }).ToList() : new List<ProductImagesViewModel>()
            }).ToList();

            return Ok(productsViewModel);
        }

        [HttpGet("productsByOwner/all", Name = "GetProductsByOwner")]
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByOwnerAsync()
        {
            var adObjectId = httpContextAccessor.HttpContext.User.GetObjectId() ?? "Admin";
            var userName = httpContextAccessor.HttpContext.User.GetNameIdentifierId();

            Logger.LogInformation($"Executing {nameof(GetProductsByOwnerAsync)}");
            

            var products = await productService.GetProductsAsync(adObjectId.ToLower());

            var productsViewModel = products.Select(product =>
             new ProductViewModel()
             {
                 AvailableSince = product.AvailableSince,
                 CategoryId = Convert.ToInt16(product.CategoryId),
                 Descriptions = product.Descriptions,
                 Id = product.Id,
                 IsActive = product.IsActive,
                 Name = product.Name,
                 Price = product.Price,
                 ProductImages = product.ProductImages.Any() ? product.ProductImages.Select(s => new ProductImagesViewModel()
                 {
                     Id = s.Id,
                     Mime = s.Mime,
                     ImageName = s.ImageName,
                     ProductId = Convert.ToInt32(s.ProductId),
                     Base64Image = Convert.ToBase64String(s.Image),
                     IsActive = true
                 }).ToList() : new List<ProductImagesViewModel>()
             }).ToList();

            return Ok(productsViewModel);
        }


        [HttpPost("", Name = "CreateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[Authorize]
        //[AuthorizeForScopes(Scopes = new string[] {
        //    "https://learnsmartcoding.onmicrosoft.com/api/product.write"
        //})]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostProductAsync([FromBody] CreateProduct createProduct)
        {
            var adObjectId = httpContextAccessor.HttpContext.User.GetObjectId() ?? "Admin";
            var userName = httpContextAccessor.HttpContext.User.GetNameIdentifierId();

            Logger.LogInformation($"Executing {nameof(PostProductAsync)}");

            var entity = new Product()
            {
                AvailableSince = createProduct.AvailableSince,
                CategoryId = createProduct.CategoryId,
                Descriptions = createProduct.Descriptions,
                IsActive = createProduct.IsActive,
                Name = createProduct.Name,
                Price = createProduct.Price,
                CreatedBy = "TODO after authentication enabled",
                CreatedDate = DateTime.Now,
                ProductOwnerId = 1 //for inmemory                
                //,ProductOwner = new ProductOwner() //commented for inmemory
                //{
                   
                //    OwnerADObjectId = adObjectId??"admin", //todo, take from claims
                //    OwnerName = userName??"admin",//todo, take from claims
                    
                //}
            };

            var isSuccess = await productService.CreateProductAsync(entity);

            return new CreatedAtRouteResult("GetProduct",
                   new { id = entity.Id });
        }


        [HttpPut("", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        //[Authorize]
        //[AuthorizeForScopes(Scopes = new string[] {
        //    "https://learnsmartcoding.onmicrosoft.com/api/product.write"
        //})]
        public async Task<IActionResult> PutProductAsync([FromBody] UpdateProduct updateProduct)
        {
            Logger.LogInformation($"Executing PutProductAsync");
            //retrive from db and then update the entity
            var entity = await productService.GetProductAsync(updateProduct.Id);

            entity.Id = updateProduct.Id;
               entity.AvailableSince = updateProduct.AvailableSince;
               entity.CategoryId = updateProduct.CategoryId;
               entity.Descriptions = updateProduct.Descriptions;
               entity.IsActive = updateProduct.IsActive;
               entity.Name = updateProduct.Name;
               entity.Price = updateProduct.Price;
               entity.ModifiedBy = "TODO after authentication enabled";
               entity.ModifiedDate = DateTime.Now;

            await productService.UpdateProductAsync(entity);

            return Ok();
        }


        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        //[Authorize]
        //[AuthorizeForScopes(Scopes = new string[] {
        //    "https://learnsmartcoding.onmicrosoft.com/api/product.write"
        //})]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {

            Logger.LogInformation($"Executing {nameof(DeleteProductAsync)}");

            var product = await productService.GetProductAsync(id);

            if (product == null)
                return NotFound();

            await productService.DeleteProductAsync(id);

            return Ok();
        }

        [HttpPost("upload/{id}", Name = "UploadProductImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]       
        public async Task<IActionResult> UploadProductImageAsync(IFormFile file, [FromRoute] int id)
        {
            if (!IsValidFile(file))
            {
                return BadRequest(new { message = "Invalid file extension" });
            }

            byte[] fileBytes = null;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileBytes = stream.ToArray();
            }

            await productService.CreateProductImageAsync(fileBytes, id, file.FileName, file.ContentType);

            return Ok();
        }


        private bool IsValidFile(IFormFile file)
        {
            List<string> validFormats = new List<string>() { ".jpg", ".png", ".svg",".jpeg" };
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return validFormats.Contains(extension);
        }

    }
}
