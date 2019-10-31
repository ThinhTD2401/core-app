using CoreApp.Data.EF;
using CoreApp.Data.Entities;
using CoreApp.Data.IRepositories;

namespace CoreApp.Application.Implementation
{
    public class ContactRespository : EFRepository<Contact, string>, IContactRepository
    {
        public ContactRespository(AppDbContext context) : base(context)
        {
        }
    }
}