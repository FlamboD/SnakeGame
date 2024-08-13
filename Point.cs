using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Point
    {
        int x, y;
        public int X { get => x; }
        public int Y { get => y; }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point Left() => new Point(x - 1, y);
        public Point Up() => new Point(x, y - 1);
        public Point Right() => new Point(x + 1, y);
        public Point Down() => new Point(x, y + 1);

        public override string ToString()
        {
            return $"{{X={x}, Y={y}}}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;
            return ((Point)obj).x == this.x && ((Point)obj).y == this.y;
        }

        public bool Equals(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public Point NormalDirection(Point to)
        {
            int _x = 0, _y = 0;

            if (this.X > to.X) _x = -1;
            if (this.X < to.X) _x = 1;
            if (this.Y > to.Y) _y = -1;
            if (this.Y < to.Y) _y = 1;

            return Math.Abs(_x) > Math.Abs(_y) ? new Point(_x, _y * 2) : new Point(_x * 2, _y);
            //return new Point(_x, _y);
        }
    }
}
