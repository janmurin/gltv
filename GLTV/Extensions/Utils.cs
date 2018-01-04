using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;

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
    }
}
