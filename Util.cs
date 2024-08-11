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
    }
}
