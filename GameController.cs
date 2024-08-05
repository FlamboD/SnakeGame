using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class GameController
    {
        Snake snake;
        SnakeController playerController;
        Point[] fruit;
        Point midpoint { get => new Point(Settings.gridWidth/2, Settings.gridHeight/2); }


        public GameController()
        {
            this.snake = new Snake(midpoint);
            this.snake.addNode();
            this.snake.addNode();

            this.playerController = new SnakeController(this.snake, Direction.Right);

            for (int i = 0; i < Settings.fruitOnBoard; i++)
            {

            }
        }

        public void start()
        {
            new Thread(tick).Start();

            this.playerController.bindMovement();
        }

        public void tick()
        {
            while(true)
            {
                this.playerController.move();

                // check collisions
                // reprint board
                Console.Clear();
                Console.WriteLine(this.display());


                Thread.Sleep(Convert.ToInt32(1000/Settings.movesPerSecond));
                // return;
            }
        }

        public String display()
        {
            String ret = "";
            for (int j = 0; j < Settings.gridHeight; j++)
            {
                for (int i = 0; i < Settings.gridWidth; i++)
                {
                    Snake s = snake.isOverPoint(new Point(i, j));
                    if (s != null) ret += s.ToString();
                    else
                    {
                        if ((i + j) % 2 == 0)
                            ret += "•";
                        else
                            ret += "◦";
                    }
                    ret += " ";
                }
                ret += "\n";
            }
            return ret;
        }
    }
}
