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
        Task<List<string>> FetchInzeratyTypesAsync();
        Task<List<string>> FetchInzeratyCategoriesAsync();
        Task<List<string>> FetchInzeratyLocationsAsync();
        Task<PaginatedList<Inzerat>> FetchInzeratyAsync(string inzeratType, string inzeratCategory, string location,
            int priceMax, int pageNumber, int pageSize);

        Task IgnoreInzeratForUser(int id);
        Task MarkInzeratForUser(int id);
        Task CancelMarkedInzeratForUser(int id);
        Task CancelIgnoredInzeratForUser(int id);
        Task<PaginatedList<Inzerat>> FetchMarkedInzeratyAsync(string inzeratType, string inzeratCategory, string location, int priceMax, int pageNumber, int pageSize);
        Task<PaginatedList<Inzerat>> FetchIgnoredInzeratyAsync(string inzeratType, string inzeratCategory, string location, int priceMax, int pageNumber, int pageSize);
        List<Inzerat> FetchInzeratyForNotifications(FilterData filterData);
    }
}
