using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace GLTV.Models.Objects
{
    /// <summary>
    /// contains basic information about tv screens that connected to the server
    /// - tv screen is recognized by making program request
    /// - "tv screens" which have very few program requests in TvScreenHandshakes table, can be ignored as tv screens and not visible on the frontend
    /// </summary>
    public class TvScreen
    {
        public int ID { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string IpAddress { get; set; }

        [Required]
        public Location Location { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        [Display(Name = "Last Handshake")]
        [Required]
        public DateTime LastHandshake { get; set; }

        public override bool Equals(object obj)
        {
            var screen = obj as TvScreen;
            return screen != null &&
                   IpAddress == screen.IpAddress &&
                   Location == screen.Location;
        }

        public override int GetHashCode()
        {
            var hashCode = 922657394;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IpAddress);
            hashCode = hashCode * -1521134295 + Location.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
