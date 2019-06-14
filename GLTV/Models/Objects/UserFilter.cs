using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GLTV.Models.Objects
{
    public class FilterData
    {
        public string InzeratType { get; set; }
        public string InzeratCategory { get; set; }
        public string Location { get; set; }
        public string PriceString { get; set; }
    }

    public class UserFilter
    {
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string FilterDataJson { get; set; }

        [NotMapped]
        public FilterData FilterData { get; set; }
    }
}
