using System.Collections.Generic;
using Domains.Abstract;
using Domains.Entities;

namespace Domains.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEnumerable<User> Users
        {
            get { return _applicationContext.Users; }
        }
    }
}