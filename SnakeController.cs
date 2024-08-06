using System;
using System.Linq;

namespace SnakeGame
{
    internal class SnakeController
    {
        Snake snake;
        Direction direction;
        Direction want;
        int player = -1;

        public SnakeController(Snake snake)
        {
            this.snake = snake;
            this.direction = Direction.Right;
            this.want = Direction.Right;
        }

        public SnakeController(Snake snake, Direction direction)
        {
            this.snake = snake;
            this.direction = direction;
            this.want = direction;
        }

        public void move()
        {
            switch(this.want)
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

        public void bindMovement()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (player == 1 && (new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow }).Contains(key.Key)) continue;
                if (player == 2 && (new ConsoleKey[] { ConsoleKey.W, ConsoleKey.A, ConsoleKey.S, ConsoleKey.D }).Contains(key.Key)) continue;
                switch(key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            want = Direction.Up;
                            break;
                        }
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        {
                            want = Direction.Right;
                            break;
                        }
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        {
                            want = Direction.Down;
                            break;
                        }
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        {
                            want = Direction.Left;
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
