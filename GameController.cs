using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class GameController: Interactable
    {
        //List<Snake> snakes = new List<Snake>();
        List<SnakeController> playerControllers = new List<SnakeController>();
        List<Point> fruits = new List<Point>();
        Point midpoint { get => new Point(Settings.gridWidth/2, Settings.gridHeight/2); }

        Random random = new Random();
        bool playing = true;
        bool isAI = false;
        List<Direction> pathAI = new List<Direction>();

        public GameController() : this(false) { }

        public GameController(bool multiplayer)
        {
            if(multiplayer) {
                this.playerControllers.Add(new SnakeController(new Snake(new Point(2, 2)), Direction.Right, 1));
                this.playerControllers.Add(new SnakeController(new Snake(new Point(Settings.gridWidth - 3, Settings.gridHeight - 3)), Direction.Left, 2));
            } else {
                this.playerControllers.Add(new SnakeController(new Snake(midpoint), Direction.Right, -1));
            }

            foreach(SnakeController controller in this.playerControllers) {
                controller.snake.addNode();
                controller.snake.addNode();
            }

            //foreach (SnakeController controller in this.playerControllers)
            //{
            //    controller.move();
            //    controller.move();
            //}

            for (int i = 0; i < Settings.fruitOnBoard; i++)
            {
                this.spawnFruit();
            }
        }

        public static GameController AIGame()
        {
            GameController controller = new GameController();
            controller.isAI = true;
            return controller;
        }

        public void start()
        {
            new Thread(tick).Start();

            //SnakeController.bindMovement(this.playerControllers);
        }

        public override void Stop()
        {
            this.playing = false;
        }

        public void tick()
        {
            while(playing)
            {
                foreach (SnakeController controller in this.playerControllers) { 
                    if(isAI)
                    {
                        try
                        {

                            if (pathAI.Count == 0)
                            {
                                Point[] shortestPath = controller.ShortestPathToFruit(fruits.ToArray(), controller.direction);
                                Direction[] directions = shortestPath.Select(_ => Util.PointToDirection(controller.snake.Pos, _)).ToArray();
                                pathAI.AddRange(directions);
                            }
                            if (pathAI.Count != 0)
                            {
                                controller.want = pathAI[0];
                                pathAI.RemoveAt(0);
                            }
                        }
                        catch { }
                    }
                    
                    controller.move(); 
                }
                this.checkFruitCollision();
                this.playing = !this.playerControllers.Any(sc => sc.snake.IsDead);


                Console.Clear();
                Display();


                Thread.Sleep(Convert.ToInt32(1000/Settings.movesPerSecond));
            }
        }

        public override List<KeyAction> KeyActions => isAI ? new List<KeyAction>() { new KeyAction(ConsoleKey.Spacebar, () => { Program.AI(); return 0; }) } : playerControllers.ConvertAll((_) => _.KeyActions).SelectMany(_ => _).Concat(new[] { new KeyAction(ConsoleKey.Spacebar, () => { if (this.playerControllers.Count > 1) Program.MultiPlayer(); else Program.SinglePlayer(); return 0; }) }).ToList();

        public override void Display() {
            string[] lines = this.ToString().Split('\n');
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
            this.playerControllers.ForEach(s => { _snake.UnionWith(s.snake.getPoints()); });

            HashSet<Point> _fruits = fruits.ToHashSet();
            //for(int i = 0; i < )
            List<Point> availablePoints = forEachCell(
                new List<Point>(), 
                (prev, row, col) => {
                    Point here = new Point(row, col);
                    if(_snake.Any(_ => _.Equals(here)) || _fruits.Any(_ => _.Equals(here))) return prev;
                    prev.Add(here);
                    return prev;
                }
            );
            return availablePoints;
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
            return forEachCell(initialValue, eachCell, null);
        }

        private Point spawnFruit()
        {
            List<Point> available = getAvailablePoints();
            if (available.Count == 0) return null;
            Point fruit = available[random.Next(0, available.Count)];
            this.fruits.Add(fruit);
            return fruit;
        }

        public override String ToString()
        {
            String ret = forEachCell(
                "", 
                (String prev, int row, int col) =>
            {
                Point here = new Point(row, col);
                // BEGIN IF SNAKE
                //Snake s = snakes[0].isOverPoint(here);
                Snake s = null;
                foreach(SnakeController controller in this.playerControllers)
                    s = controller.snake.isOverPoint(here) ?? s;
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
            bool hitFruit = false;
            foreach(SnakeController controller in this.playerControllers)
            {
                Point[] hit = fruits.Where((p) => p.Equals(controller.snake.Pos)).ToArray();
                if (hit.Length == 0) {
                    hitFruit = true;
                    continue;
                }
                fruits.Remove(hit.First());
                this.spawnFruit();
                controller.snake.addNode();
            }
            return hitFruit;
        }
    }
}
