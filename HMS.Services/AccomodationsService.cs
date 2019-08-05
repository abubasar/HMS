using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class AccomodationsService
    {
        private readonly HMSContext context;
        public AccomodationsService()
        {
            this.context = new HMSContext();
        }
        public IEnumerable<Accomodation> GetAllAccomodations()
        {
            return context.Accomodations.AsEnumerable();
        }

        public IEnumerable<Accomodation> SearchAccomodations(string searchTerm, string accomodationPackageId, int page, int recordSize)
        {
            var queryable = context.Accomodations.Include("AccomodationPackage").AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())

                || x.Description.ToLower().ToString().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(accomodationPackageId))
            {
                queryable = queryable.Where(x => x.AccomodationPackageId == accomodationPackageId);
            }

            return queryable.OrderBy(x => x.AccomodationPackageId).Skip((page - 1) * recordSize).Take(recordSize).AsEnumerable();
        }


        public int SearchAccomodationsCount(string searchTerm, string accomodationPackageId)
        {
            var queryable = context.Accomodations.Include("AccomodationPackage").AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())

                || x.Description.ToString().ToLower().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrEmpty(accomodationPackageId))
            {
                queryable = queryable.Where(x => x.AccomodationPackageId == accomodationPackageId);
            }

            return queryable.Count();
        }

        public Accomodation GetAccomodationById(string id)
        {
            return context.Accomodations.Find(id);
        }

        public bool SaveAccomodation(Accomodation accomodation)
        {
            context.Accomodations.Add(accomodation);
            return context.SaveChanges() > 0;

        }

        public bool UpdateAccomodation(Accomodation accomodation)
        {
            context.Entry(accomodation).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public bool DeleteAccomodation(Accomodation accomodation)
        {
            context.Entry(accomodation).State = System.Data.Entity.EntityState.Deleted;

            return context.SaveChanges() > 0;
        }
    }

}
