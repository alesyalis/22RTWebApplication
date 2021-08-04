using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Model
{
    public class Order: BaseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDateTime { get; set; }
        public virtual Client Client { get; set; }
        public virtual List<Certificate> Certificates { get; set; }
    }
}
