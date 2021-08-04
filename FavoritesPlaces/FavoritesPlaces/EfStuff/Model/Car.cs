using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Model
{
    public class Car: BaseModel
    {
        public string Model { get; set; }
        public virtual User Owner { get; set; }
        
    }
}
