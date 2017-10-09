using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    public class TxtType
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string content { get; set; }
        /// <summary>
        /// txt条数
        /// </summary>
        public int txtCount { get; set; }
    }
}
