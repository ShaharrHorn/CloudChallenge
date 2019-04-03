using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudChallenge
{
    class Reel
    {
        public string id { get; set; }
        public int expiring_at { get; set; }
        public int? latest_reel_media { get; set; }
        public object seen { get; set; }
        public Owner owner { get; set; }
    }
}
