using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    internal class SnakeController
    {
        public Snake snake;
        public Direction direction;
        public Direction want;
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

        public Point[] ShortestPathToFruit(Point[] fruits, Direction lastDirection)
        {
            Point[] sortedFruits = Util.DistanceSort(this.snake.Pos, fruits);
            foreach(Point fruit in sortedFruits)
            {
                List<Point> path = PathSearch(fruit, lastDirection);
                if (path != null) return path.ToArray();
            }
            return null;
        }

        private List<Point> PathSearch(Point goal, Direction lastDirection)
        {
            return this.PathSearch(goal, this.snake.Pos, new List<Point>(), lastDirection);
        }

        private List<Point> PathSearch(Point goal, Point ghostHead, List<Point> visited, Direction lastDirection)
        {
            //Console.WriteLine($"{goal} {snake.Pos}");
            List<Point> points = new List<Point>(visited);
            if (ghostHead != snake.Pos) points.Add(ghostHead);
            if (ghostHead.Equals(goal) && (PathToTail(snake.GetFromTail(points.Count), ghostHead, new List<Point>(), points, lastDirection)?.Count ?? 0) != 0) return points;
            //if (ghostHead == goal) return visited;
            Direction[] order = Util.DirectionSort(ghostHead, goal).Where(_ => _ != Util.OppositeDirection(lastDirection)).ToArray();
            foreach(Direction direction in order)
            {
                Point gHead = Util.GetPointInDirection(ghostHead, direction);
                if (gHead == null) continue;
                if (gHead.Equals(goal)) return PathSearch(goal, gHead, new List<Point>(points), direction);
                if (snake.isOverPoint(gHead) != null) continue;
                if (points.Any(_ => _.Equals(gHead))) continue;
                List<Point> newPath = PathSearch(goal, gHead, new List<Point>(points), direction);
                if (newPath != null) return newPath;
            }
            return null;
        }

        private List<Point> PathToTail(Point goal, Point ghostHead, List<Point> visited, List<Point> barriers, Direction lastDirection)
        {
            // Excludes PathToTail check
            // Contains ghost barrier blocks
            if (goal.Equals(new Point(-1, -1))) return new List<Point> { goal };
            if (ghostHead != snake.Pos) visited.Add(ghostHead);
            if (ghostHead == goal) return visited;
            Direction[] order = Util.DirectionSort(ghostHead, goal).Where(_ => _ != Util.OppositeDirection(lastDirection)).ToArray();
            foreach (Direction direction in order)
            {
                Point gHead = Util.GetPointInDirection(ghostHead, direction);
                if(gHead == null) continue;
                if (gHead.Equals(goal)) return visited;
                if (snake.isOverPoint(gHead) != null) continue;
                if (visited.Any(_ => _.Equals(ghostHead))) continue;
                if (barriers.Any(_ => _.Equals(ghostHead))) continue;
                List<Point> newPath = PathToTail(goal, gHead, new List<Point>(visited), barriers, direction);
                if (newPath != null) return newPath;
            }
            return null;
        }
    }
}
