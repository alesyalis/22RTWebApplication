using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.EfStuff.Model
{
    public class Certificate: BaseModel
    {
        public string Name { get; set; }
        public CertificateType CertificateType { get; set; }
        public virtual List<Order> OrderedBy { get; set; }
    }
}
