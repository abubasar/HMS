using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entities
{
  public  class AccomodationPackage
    {
       // [Key]
        public string Id { get; set; }
        
        public string AccomodationTypeId { get; set; }
       // [ForeignKey("AccomodationTypeId")]
        public  AccomodationType AccomodationType { get; set; }
        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
    }
}
