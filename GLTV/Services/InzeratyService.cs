using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class InzeratyService : ServiceBase, IInzeratyService
    {
        private List<Filter> _filters;


        public InzeratyService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {

        }

        public Task<List<string>> FetchInzeratyTypesAsync()
        {
            List<string> types = Context.Filter
                .Where(x => x.FilterCategory == FilterCategory.Reality && x.FilterType == FilterType.Type)
                .Select(x => x.Value)
                .OrderBy(q => q)
                .ToList();

            return Task.FromResult(types);
        }

        public Task<List<string>> FetchInzeratyCategoriesAsync()
        {
            List<string> categories = Context.Filter
                .Where(x => x.FilterCategory == FilterCategory.Reality && x.FilterType == FilterType.Category)
                .Select(x => x.Value)
                .OrderBy(q => q)
                .ToList();

            return Task.FromResult(categories);
        }

        public Task<List<string>> FetchInzeratyLocationsAsync()
        {
            List<string> locations = GetFilters()
                .Where(x => x.FilterCategory == FilterCategory.All && x.FilterType == FilterType.Location)
                .Select(x => x.Value)
                .OrderBy(q => q)
                .ToList();

            return Task.FromResult(locations);
        }

        public Task<PaginatedList<Inzerat>> FetchInzeratyAsync(string inzeratType, string inzeratCategory,
            string location, int priceMax, int pageNumber, int pageSize)
        {
            int locationID = GetLocationId(location);

            // first filter by location
            List<Inzerat> locationInzeraty = Context.Inzerat
                .Where(y => locationID == 0 || y.LocationID == locationID)
                //.Where(y => string.IsNullOrEmpty(location) || y.Location.Contains(location) || location.Equals("All"))
                .OrderBy(x => x.ID)
                .ToList();
            // get ignored Inzeraty
            HashSet<int> ignoredInzeratyIds = Context.IgnoredInzerat
                .Where(x => x.UserName.Equals(CurrentUser.Identity.Name))
                .Select(x => x.InzeratId)
                .ToHashSet();
            // get marked Inzeraty
            HashSet<int> markedInzeratyIds = Context.MarkedInzerat
                .Where(x => x.UserName.Equals(CurrentUser.Identity.Name))
                .Select(x => x.InzeratId)
                .ToHashSet();
            // filter out ignored Inzeraty 
            locationInzeraty = locationInzeraty
                .Where(x => !ignoredInzeratyIds.Contains(x.ID) && !markedInzeratyIds.Contains(x.ID))
                .ToList();

            List<Inzerat> filteredInzeraty = locationInzeraty
                .Where(y => string.IsNullOrEmpty(inzeratType) || y.Type.Equals(inzeratType) || inzeratType.Equals("All"))
                .Where(y => string.IsNullOrEmpty(inzeratCategory) || y.Category.Equals(inzeratCategory) || inzeratCategory.Equals("All"))
                .Where(y => y.PriceValue <= priceMax || priceMax <= 0)
                .OrderByDescending(x => x.DateInserted)
                .ToList();

            return Task.FromResult(PaginatedList<Inzerat>.Create(filteredInzeraty, pageNumber, pageSize));
        }

        public Task<PaginatedList<Inzerat>> FetchMarkedInzeratyAsync(string inzeratType, string inzeratCategory, string location, int priceMax, int pageNumber,
    int pageSize)
        {
            List<Inzerat> markedInzeraty = (from inzerat in Context.Inzerat
                                            join m in Context.MarkedInzerat
                                                on inzerat.ID equals m.InzeratId
                                            where m.UserName.Equals(CurrentUser.Identity.Name)
                                            orderby m.ID descending
                                            select inzerat)
                .ToList();

            int locationID = GetLocationId(location);

            List<Inzerat> filteredInzeraty = markedInzeraty
                .Where(y => locationID == 0 || y.LocationID == locationID)
                .Where(y => string.IsNullOrEmpty(inzeratType) || y.Type.Equals(inzeratType) || inzeratType.Equals("All"))
                .Where(y => string.IsNullOrEmpty(inzeratCategory) || y.Category.Equals(inzeratCategory) || inzeratCategory.Equals("All"))
                .Where(y => y.PriceValue <= priceMax || priceMax <= 0)
                .ToList();

            return Task.FromResult(PaginatedList<Inzerat>.Create(filteredInzeraty, pageNumber, pageSize));
        }

        public Task<PaginatedList<Inzerat>> FetchIgnoredInzeratyAsync(string inzeratType, string inzeratCategory, string location, int priceMax,
            int pageNumber, int pageSize)
        {
            List<Inzerat> ignoredInzeraty = (from inzerat in Context.Inzerat
                                             join m in Context.IgnoredInzerat
                                                 on inzerat.ID equals m.InzeratId
                                             where m.UserName.Equals(CurrentUser.Identity.Name)
                                             orderby m.ID descending
                                             select inzerat)
                .ToList();

            int locationID = GetLocationId(location);
            
            List<Inzerat> filteredInzeraty = ignoredInzeraty
                .Where(y => locationID == 0 || y.LocationID == locationID)
                //.Where(y => string.IsNullOrEmpty(location) || y.Location.Contains(location) || location.Equals("All"))
                .Where(y => string.IsNullOrEmpty(inzeratType) || y.Type.Equals(inzeratType) || inzeratType.Equals("All"))
                .Where(y => string.IsNullOrEmpty(inzeratCategory) || y.Category.Equals(inzeratCategory) || inzeratCategory.Equals("All"))
                .Where(y => y.PriceValue <= priceMax || priceMax <= 0)
                .ToList();

            return Task.FromResult(PaginatedList<Inzerat>.Create(filteredInzeraty, pageNumber, pageSize));
        }

        public Task IgnoreInzeratForUser(int id)
        {
            Context.Update(new IgnoredInzerat()
            {
                InzeratId = id,
                UserName = CurrentUser.Identity.Name
            });
            Context.SaveChanges();

            CancelMarkedInzeratForUser(id);

            return Task.CompletedTask;
        }

        public Task MarkInzeratForUser(int id)
        {
            Context.Update(new MarkedInzerat()
            {
                InzeratId = id,
                UserName = CurrentUser.Identity.Name
            });
            Context.SaveChanges();

            CancelIgnoredInzeratForUser(id);

            return Task.CompletedTask;
        }

        public Task CancelMarkedInzeratForUser(int id)
        {
            List<MarkedInzerat> markedInzerats = Context.MarkedInzerat
                .Where(x => x.UserName.Equals(CurrentUser.Identity.Name) && x.InzeratId == id)
                .ToList();
            Context.RemoveRange(markedInzerats);

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task CancelIgnoredInzeratForUser(int id)
        {
            List<IgnoredInzerat> ignoredInzerats = Context.IgnoredInzerat
                .Where(x => x.UserName.Equals(CurrentUser.Identity.Name) && x.InzeratId == id)
                .ToList();
            Context.RemoveRange(ignoredInzerats);

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        private List<Filter> GetFilters()
        {
            if (_filters == null)
            {
                _filters = Context.Filter.ToList();
            }

            return _filters;
        }

        private int GetLocationId(string location)
        {
            return GetFilters()
                       .Where(x => x.FilterCategory == FilterCategory.All && x.FilterType == FilterType.Location)
                       .FirstOrDefault(x => x.Value.Equals(location))?.ID ?? 0;
        }
    }

}
