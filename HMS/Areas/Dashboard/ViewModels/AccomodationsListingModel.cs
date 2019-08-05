using HMS.Entities;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationsListingModel
    {
        public List<AccomodationPackage> AccomodationPackages { get; set; }
        public List<Accomodation> Accomodations { get; set; }
        public string SearchTerm { get; set; }
        public string AccomodationPackageId { get; set; }

        public Pager Pager { get; set; }
    }
}