using System.Linq;
using System.Collections.Generic;
using LearnSmartCoding.EssentialProducts.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> CommitAsync();

        Task<List<Product>> GetAllProducts(int noOfProducts);
        Task<List<Product>> GetAllProducts(string adObjName);
        
        Task<Product> GetProductAndImagesAsync(int id);
    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(EssentialProductsDbContext context) : base(context)
        {
            Context = context;
        }
        public EssentialProductsDbContext Context { get; }
        public async Task<bool> CommitAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public Task<List<Product>> GetAllProducts(int noOfProducts)
        {
            return Context.Product.Include(i => i.ProductImages).OrderByDescending(o => o.CreatedDate).Take(noOfProducts).AsNoTracking().ToListAsync();
        }

        public Task<List<Product>> GetAllProducts(string adObjName)
        {
            return Context.Product.Include(i => i.ProductImages).Include(d => d.ProductOwner).Where(
                w => w.ProductOwner.OwnerName == adObjName).OrderByDescending(o => o.CreatedDate).AsNoTracking().ToListAsync();
        }

        public Task<Product> GetProductAndImagesAsync(int id)
        {
            return Context.Product.Include(i=>i.ProductImages).FirstOrDefaultAsync(f=>f.Id==id);
        }
    }

}
