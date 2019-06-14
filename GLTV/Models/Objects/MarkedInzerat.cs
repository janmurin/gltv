using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class MarkedInzerat
    {
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int InzeratId { get; set; }
    }
}
