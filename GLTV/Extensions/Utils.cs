using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLTV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Extensions
{
    public class Utils
    {
        public static string GetLocationsString(List<TvItemLocation> locs)
        {
            List<string> items = new List<string>();
            foreach (TvItemLocation itemLocation in locs)
            {
                switch (itemLocation.Location)
                {
                    case Location.BanskaBystrica:
                        items.Add(Constants.BB);
                        break;
                    case Location.Kosice:
                        items.Add(Constants.KE);
                        break;
                    case Location.Zilina:
                        items.Add(Constants.ZA);
                        break;
                }
            }
            items = items.OrderBy(q => q).ToList();

            return String.Join(", ", items);
        }

        //public static List<SelectListItem> Locations { get; } = new List<SelectListItem>
        //{
        //    new SelectListItem {Value = "", Text = ""},
        //    new SelectListItem {Value = "" + (int)Location.BanskaBystrica, Text = "Banska Bystrica"},
        //    new SelectListItem {Value = "" + (int)Location.Kosice, Text = "Kosice"},
        //    new SelectListItem {Value = "" + (int)Location.Zilina, Text = "Zilina"},
        //};

        public static List<SelectListItem> Types { get; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "", Text = ""},
            new SelectListItem {Value = "" + (int)TvItemType.Image, Text = "Image"},
            new SelectListItem {Value = "" + (int)TvItemType.Gallery, Text = "Gallery"},
            new SelectListItem {Value = "" + (int)TvItemType.Video, Text = "Video"},
        };

        public static string GetTotalFileSize(TvItem item)
        {
            long size = 0;
            foreach (TvItemFile itemFile in item.Files)
            {
                size += itemFile.Deleted ? 0 : itemFile.Length;
            }

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public static string GetFileSize(TvItemFile item)
        {
            long size = item.Length;

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public static string GetFilesList(TvItem item)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TvItemFile itemFile in item.Files)
            {
                if (itemFile.Deleted)
                {
                    sb.Append(itemFile.FileName + "<br />");
                }
                else
                {
                    sb.Append("<a href=\"" + itemFile.Url + "\" target=\"_blank\">" + itemFile.FileName + "</a>&nbsp;&nbsp;" + GetFileSize(itemFile) + "<br />");
                }
            }

            return sb.ToString();
        }

        public static string GetFormattedDuration(TvItem item)
        {
            long duration = item.Duration*item.Files.Count;
            
            TimeSpan time = TimeSpan.FromSeconds(duration);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            return time.ToString(@"mm\:ss");
        }
    }
}
