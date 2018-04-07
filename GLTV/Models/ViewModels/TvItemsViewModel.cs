﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Models
{
    public class TvItemsViewModel
    {
        public List<TvItem> ActiveTvItems { get; set; }
        public List<TvItem> ExpiredTvItems { get; set; }
    }
}
