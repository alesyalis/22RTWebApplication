using TwenyTwoRT.EfStuff.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Repository.IRepository
{
    public interface IBaseRepository<ModelType> where ModelType : BaseModel
    {
        public ModelType Get(long id);
        public List<ModelType> GetAll();
        public void Remove(long id);
        public void Remove(IEnumerable<long> id);
        public void Remove(ModelType model);
        public void Save(ModelType model);
    }
}
