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
            Program.restart();
            // Console.Read();
            //Console.WriteLine(words.Split());

            // Console.ReadKey();
        }

        public static void restart() {
            Program.game = new GameController(true);
            Program.game.start();
        }
    }
}
