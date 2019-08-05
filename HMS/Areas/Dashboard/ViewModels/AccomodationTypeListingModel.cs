using HMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationTypeListingModel
    {
     public   List<AccomodationType> AccomodationTypes { get; set; }
     public string SearchTerm { get; set; }
    }
}