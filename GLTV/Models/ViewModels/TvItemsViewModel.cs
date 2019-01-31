using System.Collections.Generic;
using GLTV.Models.Objects;

namespace GLTV.Models
{
    public class TvItemsViewModel
    {
        public List<TvItem> ActiveTvItems { get; set; }
        public List<TvItem> ExpiredTvItems { get; set; }
    }
}
