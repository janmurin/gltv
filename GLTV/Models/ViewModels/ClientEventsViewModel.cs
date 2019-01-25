using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.ViewModels
{
    public class ClientEventsViewModel
    {
        public List<ClientEvent> ClientEvents { get; set; }
        public List<string> Sources { get; set; }
        public List<ClientEvent> LastProgramClientEvents { get; set; }
    }
}
