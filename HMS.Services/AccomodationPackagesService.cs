using HMS.Data;
using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Services
{
   public class AccomodationPackagesService
    { 
   private readonly HMSContext context;
    public AccomodationPackagesService()
    {
        this.context = new HMSContext();
    }
    public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
    {
        return context.AccomodationPackages.AsEnumerable();
    }

    public IEnumerable<AccomodationPackage> SearchAccomodationPackages(string searchTerm,string accomodationTypeId,int page,int recordSize)
    {
        var queryable = context.AccomodationPackages.Include("AccomodationType").AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) 
            
            ||x.NoOfRoom.ToString().Contains(searchTerm) || x.FeePerNight.ToString().Contains(searchTerm));
        }
            if (!string.IsNullOrEmpty(accomodationTypeId))
            {
                queryable = queryable.Where(x => x.AccomodationTypeId==accomodationTypeId);
            }
         
        return queryable.OrderBy(x=>x.AccomodationTypeId).Skip((page - 1) * recordSize).Take(recordSize).AsEnumerable();
    }


        public int SearchAccomodationPackagesCount(string searchTerm, string accomodationTypeId)
        {
            var queryable = context.AccomodationPackages.Include("AccomodationType").AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())

                || x.NoOfRoom.ToString().Contains(searchTerm) || x.FeePerNight.ToString().Contains(searchTerm));
            }
            if (!string.IsNullOrEmpty(accomodationTypeId))
            {
                queryable = queryable.Where(x => x.AccomodationTypeId == accomodationTypeId);
            }

            return queryable.Count();
        }

        public AccomodationPackage GetAccomodationPackageBYId(string id)
    {
        return context.AccomodationPackages.Find(id);
    }

    public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
    {
        context.AccomodationPackages.Add(accomodationPackage);
        return context.SaveChanges() > 0;

    }

    public bool UpdateAccomodationPackage(AccomodationPackage accomodationPackage)
    {
        context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Modified;

        return context.SaveChanges() > 0;
    }

    public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
    {
        context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;

        return context.SaveChanges() > 0;
    }
} 

}
