using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Util
    {
        public static int Clamp(int min, int max, int value)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public static double Clamp(double min, double max, double value)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public static Direction[] DirectionSort(Point from, Point to)
        {
            if (from == null || to == null || from.Equals(to)) return new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

            Point n = from.NormalDirection(to);

            //double angle = (((Math.Atan2(to.Y, to.X) - Math.Atan2(from.Y, from.X)) * (180 / Math.PI)) + 360) % 360;
            //Console.WriteLine(from);
            //Console.WriteLine(to);
            //Console.WriteLine(angle);
            //Console.WriteLine((Math.Atan2(to.Y - from.Y, to.X - from.X)) * (180 / Math.PI));
            //if (angle <= 45) return new[] { Direction.Right, Direction.Down, Direction.Up, Direction.Left };
            //else if (angle <= 90) return new[] { Direction.Down, Direction.Right, Direction.Left, Direction.Up };
            //else if (angle <= 135) return new[] { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
            //else if (angle <= 180) return new[] { Direction.Left, Direction.Down, Direction.Up, Direction.Right };
            //else if (angle <= 225) return new[] { Direction.Left, Direction.Up, Direction.Down, Direction.Right };
            //else if (angle <= 270) return new[] { Direction.Up, Direction.Left, Direction.Right, Direction.Down };
            //else if (angle <= 315) return new[] {Direction.Up, Direction.Right, Direction.Left, Direction.Down };
            //else return new[] { Direction.Right, Direction.Up, Direction.Down, Direction.Left };

            if (n.X >= 0 && n.Y >= 0) // +=x +=y
            {
                if ((Math.Abs(n.X) > Math.Abs(n.Y) && n.Y != 0) || n.X == 0) return new[] { Direction.Down, Direction.Right, Direction.Left, Direction.Up };
                return new[] { Direction.Right, Direction.Down, Direction.Up, Direction.Left };
            }
            if (n.X >= 0 && n.Y <= 0)
            {
                if ((Math.Abs(n.X) > Math.Abs(n.Y) && n.Y != 0) || n.X == 0) return new[] { Direction.Up, Direction.Right, Direction.Left, Direction.Down };
                return new[] { Direction.Right, Direction.Up, Direction.Down, Direction.Left };
            }
            if (n.X <= 0 && n.Y >= 0)
            {
                if ((Math.Abs(n.X) > Math.Abs(n.Y) && n.Y != 0) || n.X == 0) return new[] { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
                return new[] { Direction.Left, Direction.Down, Direction.Up, Direction.Right };
            }
            if (n.X <= 0 && n.Y <= 0)
            {
                if ((Math.Abs(n.X) > Math.Abs(n.Y) && n.Y != 0) || n.X == 0) return new[] { Direction.Up, Direction.Left, Direction.Right, Direction.Down };
                return new[] { Direction.Left, Direction.Up, Direction.Down, Direction.Right };
            }


            return new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        }

        public static Point[] DistanceSort(Point main, Point[] goals)
        {
            bool swapped;
            do
            {
                swapped = false;
                for(int i = 0; i < goals.Length-1; i++)
                {
                    if(CalcDistance(main, goals[i]) > CalcDistance(main, goals[i+1]))
                    {
                        swapped = true;
                        Point temp = goals[i];
                        goals[i] = goals[i+1];
                        goals[i+1] = temp;
                    }
                }
                if (!swapped) return goals;
            } while(swapped);
            return goals;
        }

        private static double CalcDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public static Point GetPointInDirection(Point p, Direction d)
        {
            if(p == null) return null;

            if(d == Direction.Up && p.Y <= 0) return null;
            if(d == Direction.Right && p.X >= Settings.gridWidth - 1) return null;
            if(d == Direction.Down && p.Y >= Settings.gridHeight - 1) return null;
            if(d == Direction.Left && p.X <= 0) return null;

            switch(d)
            {
                case Direction.Up:
                    {
                        return new Point(p.X, p.Y - 1);
                    }
                case Direction.Right:
                    {
                        return new Point(p.X + 1, p.Y);
                    }
                case Direction.Down:
                    {
                        return new Point(p.X, p.Y + 1);
                    }
                case Direction.Left:
                    {
                        return new Point(p.X - 1, p.Y);
                    }
            }
            return p;
        }

        public static Direction PointToDirection(Point from, Point to)
        {
            if(from == null || to == null) return Direction.Right;

            if (from.X < to.X) return Direction.Right;
            else if (from.X > to.X) return Direction.Left;
            else if (from.Y < to.Y) return Direction.Down;
            else if (from.Y > to.Y) return Direction.Up;

            return Direction.Right;
        }

        public static Direction OppositeDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
            }
            return Direction.Left;
        }
    }
}
