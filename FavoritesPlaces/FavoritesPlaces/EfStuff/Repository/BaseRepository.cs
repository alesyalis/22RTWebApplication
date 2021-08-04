using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.EfStuff.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Repository
{
    public abstract class BaseRepository<ModelType> :
        IBaseRepository<ModelType> where ModelType : BaseModel
    {
        protected SpaceDbContext _spaceDbContext;
        protected DbSet<ModelType> _dbSet;

        public BaseRepository(SpaceDbContext spaceDbContext)
        {
            _spaceDbContext = spaceDbContext;
            _dbSet = _spaceDbContext.Set<ModelType>();
        }
        public virtual List<ModelType> GetAll()
        {
            return _dbSet.ToList();
        }
        public virtual ModelType Get(long id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == id);
        }
        public virtual void Save(ModelType model)
        {
            if (model.Id > 0)
            {
                _dbSet.Update(model);
            }
            else
            {
                _dbSet.Add(model);
            }
            _spaceDbContext.SaveChanges();
        }
        public virtual void Remove(long id)
        {
            var model = Get(id);
            _dbSet.Remove(model);
        }
        public virtual void Remove(ModelType model)
        {
            _spaceDbContext.Remove(model);
            _spaceDbContext.SaveChanges();
        }
        public virtual void Remove(IEnumerable<long> ids)
        {
            foreach (var userid in ids)
            {
                Remove(userid);
            }
        }

    }
}
