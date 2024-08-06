using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace SnakeGame
{
    internal class Snake
    {
        //Snake head;
        SnakeData data;
        public Point Pos { get => this.data.HeadPos; }
        public bool IsHead { get => this == this.data.Head; }
        public bool IsDead { get => this.isDead(); }

        // public Snake Head { get => this.head; }
        // public Snake Tail { get => getTail(); }

        Snake toHead;
        Snake toTail;
        Point pos;
        int index;

        public Snake() : this(new Point(Settings.gridWidth / 2, Settings.gridHeight / 2)) { }

        public Snake(Point pos)
        {
            this.pos = pos;
            this.data = new SnakeData(this);
            this.index = 0;
        }

        private Snake(Snake snake)
        {
            snake.data.Tail.toTail = this;
            snake.index = snake.data.Tail.index + 1;
            this.toHead = snake.data.Tail;
            this.pos = snake.data.TailPos;
            this.data = snake.data;
            snake.data.increase(this);
        }

        public Snake addNode()
        {
            return new Snake(this.data.Tail);
        }

        public Point GetNextPos(Direction direction) {
            switch (direction)
            {
                case Direction.Up: return new Point(this.data.HeadPos.X, this.data.HeadPos.Y - 1);
                case Direction.Right: return new Point(this.data.HeadPos.X + 1, this.data.HeadPos.Y);
                case Direction.Down: return new Point(this.data.HeadPos.X, this.data.HeadPos.Y + 1);
                case Direction.Left: return new Point(this.data.HeadPos.X - 1, this.data.HeadPos.Y);
            }
            return this.data.HeadPos;
        }

        public HashSet<Point> getPoints()
        {
            HashSet<Point> points = new HashSet<Point>();
            Snake curr = this.data.Head;
            while(curr != null)
            {
                points.Add(curr.pos);
                curr = curr.toTail;
            }
            return points;
        }

        public void move(Point newHeadPos)
        {
            Snake curr = this.data.Tail;
            while (curr.toHead != null)
            {
                curr.pos = curr.toHead.pos;
                curr = curr.toHead;
            }
            curr.pos = newHeadPos;
        }

        public void move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: { move(this.data.HeadPos.Left()); break; }
                case Direction.Up: { move(this.data.HeadPos.Up()); break; }
                case Direction.Right: { move(this.data.HeadPos.Right()); break; }
                case Direction.Down: { move(this.data.HeadPos.Down()); break; }
            }
        }

        public Snake isOverPoint(Point pos)
        {
            Snake curr = this.data.Head;
            do
            {
                if (curr.pos.Equals(pos)) return curr;
                curr = curr.toTail;
            }
            while (curr != null);
            return null;
        }

        private bool isDead()
        {
            Snake curr = this.data.Head;

            if (
                this.data.HeadPos.X < 0 ||
                this.data.HeadPos.X >= Settings.gridWidth ||
                this.data.HeadPos.Y < 0 ||
                this.data.HeadPos.Y >= Settings.gridHeight 
            ) return true;

            while (curr.toTail != null)
            {
                curr = curr.toTail;
                if (this.data.HeadPos.Equals(curr.pos)) { 
                    Console.WriteLine(curr.pos);
                    return true;
                }
            }
            return false;
        }


        Direction getFromHeadDirection()
        {
            if (this.toHead == null) return Direction.Right;
            if (this.pos.X > toHead.pos.X) return Direction.Right;
            if (this.pos.X < toHead.pos.X) return Direction.Left;
            if (this.pos.Y > toHead.pos.Y) return Direction.Down;
            if (this.pos.Y < toHead.pos.Y) return Direction.Up;
            if (this.toHead == null) return Direction.Right;
            return this.toHead.getFromHeadDirection();
        }


        Direction getFromTailDirection()
        {
            if (this.toTail == null) return Direction.Right;
            if (this.pos.X > toTail.pos.X) return Direction.Right;
            if (this.pos.X < toTail.pos.X) return Direction.Left;
            if (this.pos.Y > toTail.pos.Y) return Direction.Down;
            if (this.pos.Y < toTail.pos.Y) return Direction.Up;
            if (this.toTail == null) return Direction.Right;
            return this.toTail.getFromTailDirection();
        }

        public override string ToString()
        {
            if (this.data.Head == this) return "☺";
            if (this.data.Tail == this)
            {
                switch (this.getFromHeadDirection())
                {
                    case Direction.Up: return "↑";
                    case Direction.Right: return "→";
                    case Direction.Down: return "↓";
                    case Direction.Left: return "←";
                }
            }

            Direction dFromHead = getFromHeadDirection();
            Direction dFromTail = getFromTailDirection();

            switch (dFromHead)
            {
                case Direction.Up:
                    {
                        switch (dFromTail)
                        {
                            case Direction.Up: return "";
                            case Direction.Right: return "┐";
                            case Direction.Down: return "│";
                            case Direction.Left: return "┌";
                        }
                        return "";
                    }
                case Direction.Right:
                    {
                        switch (dFromTail)
                        {
                            case Direction.Up: return "┐";
                            case Direction.Right: return "";
                            case Direction.Down: return "┘";
                            case Direction.Left: return "─";
                        }
                        return "";
                    }
                case Direction.Down:
                    {
                        switch (dFromTail)
                        {
                            case Direction.Up: return "│";
                            case Direction.Right: return "┘";
                            case Direction.Down: return "";
                            case Direction.Left: return "└";
                        }
                        return "";
                    }
                case Direction.Left:
                    {
                        switch (dFromTail)
                        {
                            case Direction.Up: return "┌";
                            case Direction.Right: return "─";
                            case Direction.Down: return "└";
                            case Direction.Left: return "";
                        }
                        return "";
                    }
            }
            return "";
        }

        public String whole()
        {
            String s = "";
            Snake curr = this.data.Tail;
            while (curr != null)
            {
                s += curr.pos.ToString();
                curr = curr.toHead;
            }
            return s;
        }

        internal class SnakeData
        {
            internal Snake Head { get; private set; }
            internal Snake Tail { get; private set; }
            internal int Length { get; private set; }
            internal Point HeadPos { get => Head.pos; }
            internal Point TailPos { get => Tail.pos; }

            internal SnakeData(Snake head)
            {
                this.Head = head;
                this.Tail = head;
                this.Length = 0;
            }

            internal int increase(Snake tail)
            {
                this.Length += 1;
                this.Tail = tail;
                return this.Length;
            }
        }
    }
}
