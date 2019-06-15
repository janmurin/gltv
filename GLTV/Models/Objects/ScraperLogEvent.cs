using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class ScraperLogEvent
    {
        public int ID { get; set; }
        [Required]
        public int EventID { get; set; }
        [Required]
        public DateTime TimeInserted { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
