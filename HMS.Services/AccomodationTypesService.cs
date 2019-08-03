using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
    public class AccomodationTypesService
    {
        private readonly HMSContext context;
        public AccomodationTypesService()
        {
            this.context = new HMSContext();
        }
       public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            return context.AccomodationTypes.ToList();
        }


        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
             context.AccomodationTypes.Add(accomodationType);
            return context.SaveChanges() > 0;
             
        }
    }

}
