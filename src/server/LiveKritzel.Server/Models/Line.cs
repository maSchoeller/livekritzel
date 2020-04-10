using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Models
{
    public class Line
    {

        public Point From { get; set; } = new Point();
        public Point To { get; set; } = new Point();

        public string Color { get; set; } = string.Empty;
        public int Width { get; set; }
    }
}
