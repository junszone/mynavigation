using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    public class Txt
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string content { get; set; }
        public string html { get; set; }
        public string txttype { get; set; }
        public string tag { get; set; }
        public string fileName{get;set;}
    }
}
