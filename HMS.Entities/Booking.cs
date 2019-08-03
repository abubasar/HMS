using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entities
{
   public class Booking
    {
        public string Id { get; set; }
        public string AccomodationId { get; set; }
       public Accomodation Accomodation { get; set; }
        public DateTime FromDate { get; set; }
        public int Duration { get; set; }
    }
}
