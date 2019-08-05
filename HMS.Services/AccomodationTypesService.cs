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
            return context.AccomodationTypes.AsEnumerable();
        }

        public IEnumerable<AccomodationType> SearchAccomodationTypes(string searchTerm)
        {
            var queryable= context.AccomodationTypes.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
               queryable= queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())); 
            }

            return queryable.AsEnumerable();
        }

        public AccomodationType GetAccomodationTypeBYId(string id)
        {
            return context.AccomodationTypes.Find(id);
        }

        public bool SaveAccomodationType(AccomodationType accomodationType)
        {
             context.AccomodationTypes.Add(accomodationType);
            return context.SaveChanges() > 0;
             
        }

        public bool UpdateAccomodationType(AccomodationType accomodationType)
        {
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0; 
        }

        public bool DeleteAccomodationType(AccomodationType accomodationType)
        {
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Deleted;

            return context.SaveChanges() > 0; 
        }
    } 

}
