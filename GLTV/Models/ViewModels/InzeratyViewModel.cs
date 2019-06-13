using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Models.ViewModels
{
    public class InzeratyViewModel
    {
        public List<Inzerat> Inzeraty { get; set; }
        public SelectList InzeratyTypes { get; set;}
        public string InzeratType { get; set; }
        public string Location { get; set; }
        public SelectList Locations { get; set; }
        public String PriceString { get; set; }
    }
}
