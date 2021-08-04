using TwenyTwoRT.EfStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Repository.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public User Get(string name);
    }
}
