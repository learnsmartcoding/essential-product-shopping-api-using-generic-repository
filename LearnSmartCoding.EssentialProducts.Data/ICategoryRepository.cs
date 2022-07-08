using LearnSmartCoding.EssentialProducts.Core;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> CommitAsync();
    }
}
