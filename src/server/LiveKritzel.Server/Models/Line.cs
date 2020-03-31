using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Models
{
    public class Line
    {

        public Point From { get; set; }
        public Point To { get; set; }

        public string Color { get; set; }
    }
}
