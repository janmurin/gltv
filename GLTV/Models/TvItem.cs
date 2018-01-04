using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models
{
    public class TvItem
    {
        public int ID { get; set; }

        [StringLength(150, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        
        public DateTime TimeInserted { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime EndTime { get; set; }

        public string Author { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public TvItemType Type { get; set; }

        public List<TvItemLocation> Locations { get; set; }
        public List<TvItemFile> Files { get; set; }
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
        public string FileName { get; set; }
        public long Length { get; set; }
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
