using System.Collections.Generic;
using Domains.Entities;

namespace Domains.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }

    }
}