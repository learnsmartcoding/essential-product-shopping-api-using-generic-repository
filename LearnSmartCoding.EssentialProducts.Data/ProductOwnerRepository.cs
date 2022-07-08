
using LearnSmartCoding.EssentialProducts.Core;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public interface IProductOwnerRepository : IGenericRepository<ProductOwner>
    {
        Task<bool> CommitAsync();
    }

    public class ProductOwnerRepository : GenericRepository<ProductOwner>, IProductOwnerRepository
    {
        public ProductOwnerRepository(EssentialProductsDbContext context) : base(context)
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
