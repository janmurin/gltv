using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models.Objects;

namespace GLTV.Services.Interfaces
{
    public interface IInzeratyService
    {
        Task<Inzerat> FetchInzeratAsync(int id);
        Task<List<Inzerat>> FetchInzeratyAsync();
        Task<List<Inzerat>> FetchInzeratyAsync(string inzeratType, string location, int priceMax);
        Task<List<string>> FetchInzeratyTypesAsync();
        Task<List<string>> FetchInzeratyLocationsAsync();
        Task<PaginatedList<Inzerat>> FetchInzeratyAsync(string inzeratType, string location, int priceMax, int pageNumber, int pageSize);
    }
}
