using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    /// <summary>
    /// 导航
    /// </summary>
    public class Navigation
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Decimal order { get; set; }
        public string createid { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public string keyword { get; set; }
        public string logic { get; set; }
    }
}
