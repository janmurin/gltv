using System.Collections.Generic;
using GLTV.Models.Objects;

namespace GLTV.Models.ViewModels
{
    public class ClientEventsViewModel
    {
        public List<WebClientLog> ClientEvents { get; set; }
        public List<TvScreen> ActiveTvScreens { get; set; }
    }
}
