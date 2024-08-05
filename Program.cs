using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Program
    {
        static GameController game;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Program.game = new GameController();
            Program.game.start();
            // Console.Read();
        }
    }
}
