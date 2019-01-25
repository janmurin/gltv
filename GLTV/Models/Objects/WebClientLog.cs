using System;
using System.ComponentModel.DataAnnotations;

namespace GLTV.Models.Objects
{
    public class WebClientLog
    {
        public int ID { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Message { get; set; }

        public int? TvItemFileId { get; set; }
        public int? TvScreenId { get; set; }

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
        public WebClientLogType Type { get; set; }

        public virtual TvItemFile TvItemFile { get; set; }
        public virtual TvScreen TvScreen { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public string GetFormattedMessage()
        {
            switch (Type)
            {
                case WebClientLogType.ProgramRequest:
                    return "Program Request";
                case WebClientLogType.ChatRequest:
                    return "Chat Request";
                case WebClientLogType.Exception:
                    return Message;
                case WebClientLogType.VideoRequest:
                case WebClientLogType.ImageRequest:
                    return $"Request for file {(TvItemFile != null ? TvItemFile.GetDetailHyperlink() : TvItemFileId?.ToString())}<br> from";
                default:
                    return "Unknown type";
            }
        }
    }

    public enum WebClientLogType
    {
        ChatRequest = 0,
        ProgramRequest = 1,
        ImageRequest = 2,
        VideoRequest = 3,
        Exception = 4
    }
}
