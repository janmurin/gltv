using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;
using Microsoft.AspNetCore.Http;

namespace GLTV.Services
{
    public interface ITvItemService
    {
        TvItem FetchTvItem(int id);
        bool DeleteTvItem(int id);
        List<TvItem> FetchTvItems();
        bool AddTvItem(TvItem item, List<TvItemLocation> tvItemLocations);
        bool UpdateTvItem(TvItem item, List<TvItemLocation> tvItemLocations);
    }
}
