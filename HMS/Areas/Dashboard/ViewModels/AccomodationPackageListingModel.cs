using HMS.Entities;
using HMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationPackageListingModel
    {
        public List<AccomodationPackage> AccomodationPackages { get; set; }
        public List<AccomodationType> AccomodationTypes { get; set; }
        public string SearchTerm { get; set; }
        public string AccomodationTypeId { get; set; }

        public Pager Pager { get; set; }
    }
}