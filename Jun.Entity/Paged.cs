using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jun.Entity
{
    public class Paged<T>
    {
        public int total { get; set; }
        public List<T> rows { get; set; }
    }
}
