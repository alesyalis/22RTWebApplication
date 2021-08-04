using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Model
{
    public class User : BaseModel
    {
        public string Name { get; set; }

        public string Password { get; set; }
        public virtual List<Car> MyCars { get; set; }
        public JobType JobType { get; set; }
        public virtual Client Client { get; set; }
    }
}
