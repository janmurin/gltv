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
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class InzeratyService : ServiceBase, IInzeratyService
    {

        public InzeratyService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
            : base(context, signInManager)
        {

        }

        public Task<Inzerat> FetchInzeratAsync(int id)
        {
            var tvItem = Context.Inzerat
                .SingleOrDefault(m => m.ID == id);

            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            return Task.FromResult(tvItem);
        }

        public Task<List<Inzerat>> FetchInzeratyAsync()
        {
            List<Inzerat> tvItems = Context.Inzerat
                .OrderByDescending(x => x.DateInserted)
                .Skip(0)
                .Take(100)
                .ToList();

            return Task.FromResult(tvItems);
        }

        public Task<List<Inzerat>> FetchInzeratyAsync(string inzeratType, string location, int priceMax)
        {
            List<Inzerat> tvItems = Context.Inzerat
                .Where(y => y.Type.Equals(inzeratType) || string.IsNullOrEmpty(inzeratType) || inzeratType.Equals("All"))
                .Where(y => y.Location.Contains(location) || string.IsNullOrEmpty(location) || location.Equals("All"))
                .Where(y => y.PriceValue <= priceMax || priceMax <= 0)
                .OrderByDescending(x => x.DateInserted)
                .Skip(0)
                .Take(100)
                .ToList();

            return Task.FromResult(tvItems);
        }

        public Task<List<string>> FetchInzeratyTypesAsync()
        {
            List<string> types = Context.Inzerat
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(q => q)
                .ToList();

            return Task.FromResult(types);
        }

        public Task<List<string>> FetchInzeratyCategoriesAsync()
        {
            List<string> types = Context.Inzerat
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(q => q)
                .ToList();

            return Task.FromResult(types);
        }

        public Task<List<string>> FetchInzeratyLocationsAsync()
        {
            List<string> types = Context.Inzerat
                            .Select(x => x.Location.Substring(7).Trim())
                            .Distinct()
                            .OrderBy(q => q)
                            .ToList();

            return Task.FromResult(types);
        }

        public Task<PaginatedList<Inzerat>> FetchInzeratyAsync(string inzeratType, string inzeratCategory,
            string location, int priceMax, int pageNumber, int pageSize)
        {
            IOrderedQueryable<Inzerat> query = Context.Inzerat
                .Where(y => y.Type.Equals(inzeratType) || string.IsNullOrEmpty(inzeratType) || inzeratType.Equals("All"))
                .Where(y => y.Category.Equals(inzeratCategory) || string.IsNullOrEmpty(inzeratCategory) || inzeratCategory.Equals("All"))
                .Where(y => y.Location.Contains(location) || string.IsNullOrEmpty(location) || location.Equals("All"))
                .Where(y => y.PriceValue <= priceMax || priceMax <= 0)
                .OrderByDescending(x => x.DateInserted);

            return PaginatedList<Inzerat>.CreateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }
    }

}
