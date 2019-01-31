using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GLTV.Models
{
    public class TvItemCreateViewModel : TvItemEditViewModel
    {
        [Required(ErrorMessage = "At least one file is required.")]
        public List<IFormFile> Files { get; set; }
    }
}
