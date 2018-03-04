using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models
{
    public class ClientEvent
    {
        public int ID { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }

        public int? TvItemFileId { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Time Inserted")]
        [Required]
        public DateTime TimeInserted { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Source { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public ClientEventType Type { get; set; }

        public virtual TvItemFile TvItemFile { get; set; }
    }

    public enum ClientEventType
    {
        ChatRequest = 0,
        ProgramRequest = 1,
        ImageRequest = 2,
        VideoRequest = 3,
        Exception = 4
    }
}
