using LearnSmartCoding.EssentialProducts.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Service
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        ValueTask<Category> GetCategoryAsync(short id);
        Task<bool> IsCategoryExistAsync(string name);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(short id);
        void DetachEntity(Category entityToDetach);
    }
}
