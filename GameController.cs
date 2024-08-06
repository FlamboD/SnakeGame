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
        List<Snake> snakes = new List<Snake>();
        List<SnakeController> playerController = new List<SnakeController>();
        List<Point> fruits = new List<Point>();
        Point midpoint { get => new Point(Settings.gridWidth/2, Settings.gridHeight/2); }
        Point fract(int f) { return new Point(Settings.gridWidth/f, Settings.gridHeight/f); }

        Random random = new Random();
        bool playing = true;

        public GameController() : this(false) { }

        public GameController(bool multiplayer)
        {
            if(multiplayer) {
                Snake s1 = new Snake(new Point(3, 3));
                this.snakes.Add(s1);
                Snake s2 = new Snake(new Point(Settings.gridWidth - 3, Settings.gridHeight - 3));
                this.snakes.Add(s2);
            }
            this.snakes.Add(new Snake(midpoint));
            this.snakes[0].addNode();
            this.snakes[0].addNode();

            this.playerController.Add(new SnakeController(this.snakes[0], Direction.Right));

            for (int i = 0; i < Settings.fruitOnBoard; i++)
            {
                this.spawnFruit();
            }
        }

        public void start()
        {
            new Thread(tick).Start();

            this.playerController.bindMovement();
        }

        public void tick()
        {
            while(playing)
            {
                this.playerController.move();
                this.checkFruitCollision();
                this.playing = !this.snakes[0].IsDead;
                //this.spawnFruit();

                // check collisions
                // reprint board
                Console.Clear();
                print(this.display());


                Thread.Sleep(Convert.ToInt32(1000/Settings.movesPerSecond));
                // return;
            }
        }

        private void print(String input) {
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++) {
                char[] chars = lines[i].ToCharArray();
                for(int j = 0; j < chars.Length; j++) {
                    char c = chars[j];
                    switch(c) {
                        case 'R': {
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        }
                        case 'G': {
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        }
                        case 'B': {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        }
                        case 'Y': {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        }
                        case 'C': {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        }
                        default: {
                            Console.Write(c);
                            Console.ResetColor();
                            break;
                        }
                    }

                    
                }
                Console.WriteLine();
            }
        }

        List<Point> getAvailablePoints()
        {
            HashSet<Point> _snake = new HashSet<Point>();
            this.snakes.ForEach(s => { _snake.UnionWith(s.getPoints()); });

            HashSet<Point> _fruits = fruits.ToHashSet();
            //for(int i = 0; i < )
            return forEachCell(
                new List<Point>(), 
                (prev, row, col) => {
                    Point here = new Point(row, col);
                    if(_snake.Contains(here) || _fruits.Contains(here)) return prev;
                    prev.Add(here);
                    return prev;
                }
            );
        }

        private T forEachCell<T>(T initialValue, Func<T, int, int, T> eachCell, Func<T, int, T> eachRow)
        {
            T ret = initialValue;
            for (int j = 0; j < Settings.gridHeight; j++)
            {
                for (int i = 0; i < Settings.gridWidth; i++)
                {
                    if (eachCell != null) ret = eachCell.Invoke(ret, i, j);
                }
                if (eachRow != null) ret = eachRow.Invoke(ret, j);
            }
            return ret;
        }

        private T forEachCell<T>(T initialValue, Func<T, int, int, T> eachCell)
        {
            return forEachCell<T>(initialValue, eachCell, null);
        }

        private Point spawnFruit()
        {
            List<Point> available = getAvailablePoints();
            if (available.Count == 0) return null;
            Point fruit = available[random.Next(0, available.Count)];
            this.fruits.Add(fruit);
            return fruit;
        }

        public String display()
        {
            String ret = forEachCell(
                "", 
                (String prev, int row, int col) =>
            {
                Point here = new Point(row, col);
                // BEGIN IF SNAKE
                Snake s = snakes[0].isOverPoint(here);
                if (s != null) prev += (s.IsHead ? playing ? "C" : "R" : (row + col) % 2 == 0 ? "B" : "G") + s.ToString();
                // END IF SNAKE
                else if (fruits.Contains(here)) prev += "Yó";
                else
                {
                    if ((row + col) % 2 == 0) prev += "•";
                    else prev += "◦";
                }
                return prev += " ";
            },
            (String prev, int row) =>
            {
                return prev + "\n";
            });
            return ret;
        }

        private bool checkFruitCollision() {
            Point[] hit = fruits.Where((p) => p.Equals(this.snakes[0].Pos)).ToArray();
            if (hit.Length == 0) return false;
            fruits.Remove(hit[0]);
            this.spawnFruit();
            this.snakes[0].addNode();
            return true;
        }
    }
}
