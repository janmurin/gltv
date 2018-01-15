using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models;

namespace GLTV.Services
{
    public interface ITvItemService
    {
        TvItem FetchTvItem(int id);
    }
}
