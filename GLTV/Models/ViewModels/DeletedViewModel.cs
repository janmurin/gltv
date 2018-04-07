using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Models.ViewModels
{
    public class DeletedViewModel
    {
        public List<TvItem> TvItems { get; set; }
        public List<TvItemFile> ZombieFiles { get; set; }
    }
}
