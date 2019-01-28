using System.Collections.Generic;
using GLTV.Models.Objects;

namespace GLTV.Models.ViewModels
{
    public class DeletedViewModel
    {
        public List<TvItem> TvItems { get; set; }
        public List<TvItemFile> ZombieFiles { get; set; }
        public long TotalUndeletedFileSize { get; set; }
    }
}
