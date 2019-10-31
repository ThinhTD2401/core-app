using CoreApp.Data.Entities;
using CoreApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApp.Data.IRepositories
{
    public interface IBlogTagRepository : IRepository<BlogTag, int>
    {
    }
}
