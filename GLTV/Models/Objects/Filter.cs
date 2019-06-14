using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class Filter
    {
        public int ID { get; set; }
        [Required]
        public FilterType FilterType { get; set; }
        [Required]
        public FilterCategory FilterCategory { get; set; }
        [Required]
        public string Value { get; set; }
    }

    public enum FilterType
    {
        Type = 1,
        Category = 2,
        Location = 3
    }
    public enum FilterCategory
    {
        All = 0,
        Reality = 1
    }
}
