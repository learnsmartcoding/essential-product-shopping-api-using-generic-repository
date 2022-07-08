using LearnSmartCoding.EssentialProducts.Core;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EssentialProductsDbContext context) : base(context)
        {
            Context = context;
        }

        public EssentialProductsDbContext Context { get; }

        public async Task<bool> CommitAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
