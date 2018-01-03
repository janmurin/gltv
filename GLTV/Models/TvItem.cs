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
        public string Title { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd }", ApplyFormatInEditMode = true)]
        public DateTime TimeInserted { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Author { get; set; }
        public int Duration { get; set; }
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
