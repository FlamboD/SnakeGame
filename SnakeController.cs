using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    internal class SnakeController
    {
        public Snake snake;
        Direction direction;
        Direction want;
        public int player = -1;

        public SnakeController(Snake snake)
        {
            this.snake = snake;
            this.direction = Direction.Right;
            this.want = Direction.Right;
        }

        public SnakeController(Snake snake, Direction direction, int player)
        {
            this.snake = snake;
            this.direction = direction;
            this.want = direction;
            this.player = player;
        }

        public void move()
        {
            switch (this.want)
            {
                case Direction.Left:
                    {
                        if (this.direction == Direction.Right) { this.want = this.direction; }
                        break;
                    }
                case Direction.Up:
                    {
                        if (this.direction == Direction.Down) { this.want = this.direction; }
                        break;
                    }
                case Direction.Right:
                    {
                        if (this.direction == Direction.Left) { this.want = this.direction; }
                        break;
                    }
                case Direction.Down:
                    {
                        if (this.direction == Direction.Up) { this.want = this.direction; }
                        break;
                    }
            }

            this.direction = this.want;
            snake.move(this.want);
        }

        public List<KeyAction> KeyActions => new List<KeyAction> {
            new KeyAction(ConsoleKey.W, () => { if(player != 2) this.want = Direction.Up; return 0; }),
            new KeyAction(ConsoleKey.UpArrow, () => { if(player != 1) this.want = Direction.Up; return 0; }),
            new KeyAction(ConsoleKey.A, () => { if(player != 2) this.want = Direction.Left; return 0; }),
            new KeyAction(ConsoleKey.LeftArrow, () => { if(player != 1) this.want = Direction.Left; return 0; }),
            new KeyAction(ConsoleKey.S, () => { if(player != 2) this.want = Direction.Down; return 0; }),
            new KeyAction(ConsoleKey.DownArrow, () => { if(player != 1) this.want = Direction.Down; return 0; }),
            new KeyAction(ConsoleKey.D, () => { if(player != 2) this.want = Direction.Right; return 0; }),
            new KeyAction(ConsoleKey.RightArrow, () => { if(player != 1) this.want = Direction.Right; return 0; }),
        };

        public static void bindMovement(List<SnakeController> controllers)
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                SnakeController controller = controllers[0];
                if(controller.player != -1)
                {
                    //if ((new ConsoleKey[] { ConsoleKey.W, ConsoleKey.A, ConsoleKey.S, ConsoleKey.D }).Contains(key.Key)) continue;
                    if ((new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow }).Contains(key.Key)) controller = controllers[1];
                }

                switch(key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            controller.want = Direction.Up;
                            break;
                        }
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        {
                            controller.want = Direction.Right;
                            break;
                        }
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        {
                            controller.want = Direction.Down;
                            break;
                        }
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        {
                            controller.want = Direction.Left;
                            break;
                        }
                    case ConsoleKey.Spacebar:
                        {
                            Program.restart();
                            break;
                        }
                }
            }
        }
    }
}
