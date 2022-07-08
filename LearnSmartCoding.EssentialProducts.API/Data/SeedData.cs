using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LearnSmartCoding.EssentialProducts.API.Data
{
    public static class SeedData
    {
        private readonly static string categoryDataPath = @"StaticFiles\Category.json";
        private readonly static string productDataPath = @"StaticFiles\Products.json";
        public static void Initialize(EssentialProductsDbContext context)
        {           
            var categories = GetStaticCategoryAsync().Result;
            context.Category.AddRange(categories);

            var products = GetStaticProductsAsync().Result;
            context.Product.AddRange(products);

            context.ProductOwner.Add(new ProductOwner() {  OwnerADObjectId="admin", OwnerName="admin"});

            context.SaveChanges();
        }

        public static async Task<string> ReadAllTextFromPathAsync(string path)
        {
            var responseFilePath = AppDomain.CurrentDomain.BaseDirectory + path;
            var responseData = await File.ReadAllTextAsync(responseFilePath);
            return responseData;
        }

        public static async Task<List<Category>> GetStaticCategoryAsync()
        {
            var data = await ReadAllTextFromPathAsync(categoryDataPath);
            var category = JsonConvert.DeserializeObject<List<Category>>(data);
            return category;
        }

        public static async Task<List<Product>> GetStaticProductsAsync()
        {
            var data = await ReadAllTextFromPathAsync(productDataPath);
            var products = JsonConvert.DeserializeObject<List<Product>>(data);
            products.ForEach(p =>
            {
                if (p.CategoryId == 7)
                {
                    p.ProductOwnerId = 1;
                }
            });
            return products;
        }
    }
}
