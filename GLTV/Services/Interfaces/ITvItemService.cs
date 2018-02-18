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
        List<TvItem> FetchTvItems(bool deleted);
        bool AddTvItem(TvItem item);
        bool UpdateTvItem(TvItem item);
        List<TvItem> FetchActiveTvItems(Location location);
    }
}
