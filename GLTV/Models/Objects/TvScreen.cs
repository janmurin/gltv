using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Extensions;
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

        [NotMapped]
        public List<TvScreenHandshake> ScreenHandshakes { get; set; }

        [NotMapped]
        public List<WebClientLog> LastActivity { get; set; }

        [NotMapped]
        public int TotalMinutesActive { get; set; }
        [NotMapped]
        public int TotalMinutesActiveLast7days { get; set; }
        [NotMapped]
        public long TotalNetworkUsage7Days { get; set; }

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

        public string GetLast7DaysUptimeFormatted()
        {
            int percent = 100 * TotalMinutesActiveLast7days / (7 * 24 * 60);
            return $"{percent} %";
        }

        public string GetTotalNetworkUsage7DaysFormatted()
        {
            long dailyUsage = 0;
            if (TotalNetworkUsage7Days == 0 || TotalMinutesActiveLast7days == 0)
            {
                dailyUsage = 0;
            }
            else
            {
                dailyUsage = (long)(TotalNetworkUsage7Days / (TotalMinutesActiveLast7days / (60.0 * 24)));
            }
            if (dailyUsage > 1024 * 1024 * 1024)
            {
                return $"<span style=\"color: red;\">{Utils.GetFileSize(dailyUsage)} a day</span>";
            }

            return $"<span style=\"color: green;\">{Utils.GetFileSize(dailyUsage)} a day</span>";
        }
    }
}
