using System;
using System.Collections.Generic;
using System.Linq;
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
                        items.Add("BB");
                        break;
                    case Location.Kosice:
                        items.Add("KE");
                        break;
                    case Location.Zilina:
                        items.Add("ZA");
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
    }
}
