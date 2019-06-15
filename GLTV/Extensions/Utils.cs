using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLTV.Models.Objects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLTV.Extensions
{
    public class Utils
    {
        public static string GetFileSize(long bytes)
        {
            long size = bytes;

            string[] sizes = { "B", "KiB", "MiB", "GiB", "TiB" };
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

        public static string GetFormattedDuration(int minutesActive)
        {
            TimeSpan time = TimeSpan.FromMinutes(minutesActive);

            if (Math.Abs(time.TotalSeconds) > 3600 * 24)
            {
                return time.ToString(@"d' days '");
            }

            return time.ToString(@"h' h 'mm' m 'ss' s'");
        }

        public static string GetElapsedTime(DateTime timeStamp)
        {
            TimeSpan time = DateTime.Now - timeStamp;

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            return time.ToString(@"d' days 'h' h 'mm' m 'ss' s'");
        }

        public static string GetRemainingTime(DateTime now, DateTime destTime)
        {
            TimeSpan time = TimeSpan.FromSeconds((destTime - now).TotalSeconds);

            if (Math.Abs(time.TotalSeconds) > 3600 * 24)
            {
                return time.ToString(@"d' days '");
            }

            return time.ToString(@"h' h 'mm' m 'ss' s'");
        }
    }
}
