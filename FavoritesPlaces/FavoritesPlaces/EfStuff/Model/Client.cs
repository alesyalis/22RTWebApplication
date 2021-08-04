using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Model
{
    public class Client : BaseModel
    {
        public virtual long ForeignKeyUser { get; set; }
        public virtual User User { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
