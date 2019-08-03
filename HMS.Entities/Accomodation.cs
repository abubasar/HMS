using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entities
{
   public class Accomodation
    {
        public string Id { get; set; }
        public string AccomodationPackageId { get; set; }
        public AccomodationPackage AccomodationPackage { get; set; }
        public string Name { get; set; }
    }
}
