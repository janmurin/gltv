using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GLTV.Models.Objects
{
    public class Inzerat
    {
        public int ID { get; set; }
        public string Portal { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public string Email { get; set; }
        public DateTime DateInserted { get; set; }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
