using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GLTV.Extensions;

namespace GLTV.Models
{
    public class TvItem
    {
        public int ID { get; set; }

        [StringLength(150, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Time Inserted")]
        public DateTime TimeInserted { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        public string Author { get; set; }
        public bool Deleted { get; set; }

        [Required]
        [Range(3, 120)]
        [RegularExpression(@"^\d{1,3}$", ErrorMessage = "Required integer with max 3 digits.")]
        [Display(Name = "Duration (s)")]
        public int Duration { get; set; }

        [Required]
        public TvItemType Type { get; set; }

        public List<TvItemLocation> Locations { get; set; }
        public List<TvItemFile> Files { get; set; }

        [NotMapped]
        public string GetAnonymousDetailUrl
        {
            get { return $"{Constants.SERVER_URL}/TvItems/DetailsAnonymous/{ID}"; }
        }

        public string GetDetailHyperlink(bool isAnonymous)
        {
            if (isAnonymous)
            {
                return $"<a target=\"_blank\" href=\"{Constants.SERVER_URL}/TvItems/DetailsAnonymous/{ID}\">{Title}</a>";
            }

            return $"<a target=\"_blank\" href=\"{Constants.SERVER_URL}/TvItems/Details/{ID}\">{Title}</a>";
        }
    }

    public class TvItemLocation
    {
        public int ID { get; set; }
        public int TvItemId { get; set; }
        public Location Location { get; set; }
    }

    public class TvItemFile
    {
        public int ID { get; set; }
        public int TvItemId { get; set; }

        [StringLength(150, MinimumLength = 3)]
        [Required]
        public string FileName { get; set; }
        public long Length { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public string Url { get; set; }
        [NotMapped]
        public string FullUrl { get; set; }
        [NotMapped]
        public string AbsolutePath { get; set; }

        public bool IsVideoFile()
        {
            return FileName.ToLower().EndsWith(".mp4") || FileName.ToLower().EndsWith(".mkv");
        }
    }

    public enum TvItemType
    {
        Image = 0,
        Gallery = 1,
        Video = 2,
    }

    public enum Location
    {
        Kosice = 0,
        Zilina = 1,
        BanskaBystrica = 2
    }
}
