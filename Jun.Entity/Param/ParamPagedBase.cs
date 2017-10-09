using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    public class ParamPagedBase
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public string search { get; set; }
        public string key { get; set; }
    }
}
