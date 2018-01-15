using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GLTV.Services
{
    public class TvItemService : ServiceBase, ITvItemService
    {
        public TvItemService(ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager)
            : base(context, env, signInManager)
        {
        }

        public TvItem FetchTvItem(int id)
        {
            var tvItem = _context.TvItem.SingleOrDefault(m => m.ID == id);
            if (tvItem == null)
            {
                throw new Exception($"Item not found with id {id}.");
            }

            tvItem.Locations = _context.TvItemLocation.ToList().Where(x => x.TvItemId == tvItem.ID).ToList();
            tvItem.Files = _context.TvItemFile.ToList().Where(x => x.TvItemId == tvItem.ID).ToList();
            tvItem.Files.ForEach(i => i.Url = MakeWebPath(i.FileName));

            return tvItem;
        }
    }
}
