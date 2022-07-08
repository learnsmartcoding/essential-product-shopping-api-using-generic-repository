using LearnSmartCoding.EssentialProducts.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(int noOfProducts = 5);
        ValueTask<Product> GetProductAsync(int id);
        Task<bool> IsProductExistAsync(string name);
        Task<bool> CreateProductAsync(Product Product);
        Task<bool> UpdateProductAsync(Product Product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> GetProductsAsync(string adObjName);

        Task<bool> CreateProductImageAsync(byte[] fileBytes, int productId, string imageName, string mimeType);
        Task<Product> GetProductAndImagesAsync(int id);
        Task<bool> IsProductExistAsync(string name, int productId);

        void DetachEntities(Product product);
    }
}
