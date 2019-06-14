using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Models.ViewModels
{
    public class InzeratyViewModel
    {
        public PaginatedList<Inzerat> Inzeraty { get; set; }
        public SelectList InzeratyTypes { get; set;}
        public SelectList InzeratyCategories { get; set; }
        public string InzeratType { get; set; }
        public string InzeratCategory { get; set; }
        public string Location { get; set; }
        public SelectList Locations { get; set; }
        public String PriceString { get; set; }
    }
}
