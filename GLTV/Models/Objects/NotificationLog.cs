using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class NotificationMessage
    { 
        public string User { get; set; }
        public int Count { get; set; }
    }

    public class NotificationLog
    {
        public int ID { get; set; }
        public int InzeratID { get; set; }
        public DateTime? TimeInserted { get; set; }
        public string Message { get; set; }
    }
}
