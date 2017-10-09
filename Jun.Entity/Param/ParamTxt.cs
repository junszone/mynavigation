using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    public class ParamTxt : ParamPagedBase
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string keyword { get; set; }
        public string logic { get; set; }
        public string logiccontent { get; set; }
        public string tag { get; set; }
        public string txttype { get; set; }
    }
}
