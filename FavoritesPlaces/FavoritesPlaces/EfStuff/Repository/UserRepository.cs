using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.EfStuff.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SpaceDbContext spaceDbContext) : base(spaceDbContext)
        {
        }
        public User Get(string name)
        {
            return _dbSet.SingleOrDefault(x =>
                x.Name.ToLower() == name.ToLower());
        }
    }
}
