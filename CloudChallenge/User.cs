using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudChallenge
{
    class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string profile_pic_url { get; set; }
        public bool is_private { get; set; }
        public bool is_verified { get; set; }
        public bool followed_by_viewer { get; set; }
        public bool requested_by_viewer { get; set; }
        public Reel reel { get; set; }
    }
}
