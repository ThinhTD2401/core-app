using CoreApp.Data.Entities;
using CoreApp.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace CoreApp.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}