using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Models
{
    public class WebServerLog
    {
        public int ID { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }

        public int? TvItemId { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Time Inserted")]
        [Required]
        public DateTime TimeInserted { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Author { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public WebServerLogType Type { get; set; }

        public virtual TvItem TvItem { get; set; }
    }

    public enum WebServerLogType
    {
        ItemInsert = 0,
        ItemUpdate = 1,
        ItemDelete = 2,
        UserLoggedIn = 3,
        AnonymousDetails = 4,
        Exception = 5,
        ServerStartUp = 6,
        ServerShutdown = 7,
        ItemDeleteFiles = 8,
        ItemDeleteSingleFile = 9,
        ItemDeleteZombieFile = 10
    }
}
