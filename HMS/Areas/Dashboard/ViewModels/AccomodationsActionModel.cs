using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationsActionModel
    {
        public string Id { get; set; }
        public string AccomodationPackageId { get; set; }
        public AccomodationPackage AccomodationPackage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
    }
}