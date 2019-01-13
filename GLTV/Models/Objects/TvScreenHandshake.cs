using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class TvScreenHandshake
    {
        public int ID { get; set; }

        public int TvScreenId { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        [Display(Name = "First Handshake")]
        [Required]
        public DateTime FirstHandshake { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        [Display(Name = "Last Handshake")]
        [Required]
        public DateTime LastHandshake { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public WebClientLogType Type { get; set; }

        public TvScreen TvScreen { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
