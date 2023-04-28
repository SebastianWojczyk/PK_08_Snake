using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK_07_Snake
{
    public partial class BoardSnake
    {
        private class Snake
        {
            public List<Point> Segments { get; }
            public Direction direction { get; set; }
            public Snake(List<Point> initSegments)
            {
                Segments = initSegments;
                direction = Direction.Up;
            }
        }
    }
}