using CoreApp.Data.Entities;
using CoreApp.Data.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace CoreApp.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        public AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(x => x.SeoAlias == alias).ToList();
        }
    }
}