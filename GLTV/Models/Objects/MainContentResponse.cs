using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace GLTV.Models
{
    public class MainContentResponse
    {
        public MainContentResponse(TvItem item)
        {
            id = item.ID;
            groupID = (int)Location.Kosice;
            timeInserted = item.TimeInserted.ToString("yyyy-MM-dd hh:mm:ss.FFF", CultureInfo.InvariantCulture);
            expired = DateTime.Compare(DateTime.Now, item.EndTime) > 0;
            published = true;
            editEnabled = false;
            author = item.Author;
            headline = item.Title;
            duration = item.Duration;
            startDate = item.StartTime.ToString("yyyy-MM-dd hh:mm:ss.FFF", CultureInfo.InvariantCulture);
            endDate = item.EndTime.ToString("yyyy-MM-dd hh:mm:ss.FFF", CultureInfo.InvariantCulture);
            important = true;
            formattedStart = item.StartTime.Ticks;
            formattedEnd = item.EndTime.Ticks;

            locations = new List<McLocation>();
            foreach (TvItemLocation itemLocation in item.Locations)
            {
                locations.Add(new McLocation(itemLocation));
            }

            fileInfoList = new List<McFileInfo>();
            foreach (TvItemFile itemFile in item.Files)
            {
                fileInfoList.Add(new McFileInfo(itemFile, item.Duration));
            }
        }

        // db fields
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("groupID")]
        public int groupID { get; set; }
        [JsonProperty("timeInserted")]
        public String timeInserted { get; set; }
        [JsonProperty("expired")]
        public bool expired { get; set; }
        [JsonProperty("published")]
        public bool published { get; set; }
        [JsonProperty("editEnabled")]
        public bool editEnabled { get; set; }
        // insert fields
        [JsonProperty("author")]
        public String author { get; set; }
        [JsonProperty("headline")]
        public String headline { get; set; }
        [JsonProperty("duration")]
        public int duration { get; set; }
        [JsonProperty("startDate")]
        public String startDate { get; set; }
        [JsonProperty("endDate")]
        public String endDate { get; set; }
        [JsonProperty("important")]
        public bool important { get; set; }
        [JsonProperty("fileInfoList")]
        public List<McFileInfo> fileInfoList { get; set; }
        [JsonProperty("locations")]
        public List<McLocation> locations { get; set; }
        [JsonProperty("formattedStart")]
        public long formattedStart { get; set; }
        [JsonProperty("formattedEnd")]
        public long formattedEnd { get; set; }

    }

    public class McLocation
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public String name { get; set; }
        [JsonProperty("name_SK")]
        public String name_SK { get; set; }
        [JsonProperty("name_EN")]
        public String name_EN { get; set; }

        public McLocation(TvItemLocation location)
        {
            id = location.ID;
            name = location.Location.ToString();
            name_SK = location.Location.ToString();
            name_EN = location.Location.ToString();
        }
    }

    public class McFileInfo
    {
        [JsonProperty("filename")]
        private String filename { get; set; }
        [JsonProperty("length")]
        private long length { get; set; }
        [JsonProperty("mainContentID")]
        private int mainContentID { get; set; }
        [JsonProperty("duration")]
        private int duration { get; set; }
        [JsonProperty("fullPath")]
        private String fullPath { get; set; }
        [JsonProperty("thumb300Path")]
        private String thumb300Path { get; set; }

        public McFileInfo(TvItemFile itemFile, int dur)
        {
            filename = itemFile.FileName;
            length = itemFile.Length;
            mainContentID = itemFile.TvItemId;
            duration = dur;
            fullPath = itemFile.FullUrl(true);
            thumb300Path = fullPath;
        }
    }
}
