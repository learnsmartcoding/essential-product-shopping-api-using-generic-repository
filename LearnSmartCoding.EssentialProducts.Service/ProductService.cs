using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductImageRepository productImageRepository;

        public ProductService(ILogger<ProductService> logger,
            IProductRepository ProductRepository, IProductImageRepository productImageRepository)
        {
            Logger = logger;
            this.ProductRepository = ProductRepository;
            this.productImageRepository = productImageRepository;
        }

        public ILogger<ProductService> Logger { get; }
        public IProductRepository ProductRepository { get; }

        public async Task<bool> CreateProductAsync(Product Product)
        {
            ProductRepository.Add(Product);
            return await ProductRepository.CommitAsync();
        }

        public async Task<bool> CreateProductImageAsync(byte[] fileBytes, int productId, string imageName, string mimeType)
        {
            ProductImage productImage = new ProductImage()
            {
                ProductId = productId,
                Image = fileBytes,
                Mime = mimeType,
                ImageName = imageName,
                IsActive = true
            };
            var product = await ProductRepository.GetByIdAsync(productId);
            product.ProductImages = new List<ProductImage>() { productImage };

            return await ProductRepository.CommitAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productToDelete = await ProductRepository.GetProductAndImagesAsync(id);
            productToDelete.ProductImages.ForEach(i =>
            {
                productImageRepository.Delete(i);
            });

            ProductRepository.Delete(id);
            return await ProductRepository.CommitAsync();
        }

        public void DetachEntities(Product product)
        {
            ProductRepository.DetachEntities(product);  
        }

        public Task<Product> GetProductAndImagesAsync(int id)
        {
            return ProductRepository.GetProductAndImagesAsync(id);
        }

        public ValueTask<Product> GetProductAsync(int id)
        {
            return ProductRepository.GetByIdAsync(id);
        }

        public Task<List<Product>> GetProductsAsync(int noOfProducts = 5)
        {            
            return ProductRepository.GetAllProducts(noOfProducts);
        }

        public Task<List<Product>> GetProductsAsync(string adObjName)
        {
            return ProductRepository.GetAllProducts(adObjName);
        }

        public async Task<bool> IsProductExistAsync(string name)
        {
            var Product = await ProductRepository.GetAsync(g => g.Name.ToLower() == name.ToLower());
            return Product.Any();
        }

        public async Task<bool> IsProductExistAsync(string name, int productId)
        {
            var Product = await ProductRepository.GetAsync(g => g.Name.ToLower() == name.ToLower() && g.Id != productId);
            return Product.Any();
        }

        public async Task<bool> UpdateProductAsync(Product Product)
        {
            
            ProductRepository.Update(Product);
            return await ProductRepository.CommitAsync();
        }
    }
}
