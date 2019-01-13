using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Models.Objects;

namespace GLTV.Models.ViewModels
{
    public class ClientEventsViewModel
    {
        public List<WebClientLog> ClientEvents { get; set; }
        public List<string> Sources { get; set; }
        public List<WebClientLog> LastProgramClientEvents { get; set; }
    }
}
