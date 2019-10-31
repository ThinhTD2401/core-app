﻿using CoreApp.Data.Entities;
using CoreApp.Infrastructure.Interfaces;

namespace CoreApp.Data.IRepositories
{
    public interface IContactRepository : IRepository<Contact, string>
    {
    }
}