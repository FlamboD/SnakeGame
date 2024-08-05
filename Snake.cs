using System;

namespace SnakeGame
{
    internal class Snake
    {
        Snake head;

        public Snake Head { get => this.head; }
        public Snake Tail { get => getTail(); }

        Snake toHead;
        Snake toTail;
        Point pos;

        public Snake()
        {
            this.head = this;
            this.pos = new Point(Settings.gridWidth / 2, Settings.gridHeight / 2);
        }

        public Snake(Point pos)
        {
            this.head = this;
            this.pos = pos;
        }

        public Snake(Snake prev)
        {
            this.toHead = prev;
            prev.toTail = this;
            this.head = prev.head;
            this.pos = prev.pos;
        }

        public Point GetNextPos(Direction direction) {
            switch(direction)
            {
                case Direction.Up: return new Point(this.head.pos.X, this.head.pos.Y-1);
                case Direction.Right: return new Point(this.head.pos.X+1, this.head.pos.Y);
                case Direction.Down: return new Point(this.head.pos.X, this.head.pos.Y+1);
                case Direction.Left: return new Point(this.head.pos.X-1, this.head.pos.Y);
            }
            return this.head.pos;
        }

        private Snake getTail()
        {
            Snake curr = this;
            while (curr.toTail != null)
            {
                curr = curr.toTail;
            }
            return curr;
        }

        public void move(Point newHeadPos)
        {
            Snake curr = this.getTail();
            while(curr.toHead != null)
            {
                curr.pos = curr.toHead.pos;
                curr = curr.toHead;
            }
            curr.pos = newHeadPos;
        }

        public void move(Direction direction)
        {
            switch(direction)
            {
                case Direction.Left: { move(this.head.pos.Left()); break; }
                case Direction.Up: { move(this.head.pos.Up()); break; }
                case Direction.Right: { move(this.head.pos.Right()); break; }
                case Direction.Down: { move(this.head.pos.Down()); break; }
            }
        }

        public Snake isOverPoint(Point pos)
        {
            Snake curr = this.head;
            do
            {
                if (curr.pos.Equals(pos)) return curr;
                curr = curr.toTail;
            }
            while (curr != null);
            return null;
        }

        public bool isDead()
        {
            Snake curr = this.head;
            while (curr.toTail != null)
            {
                curr = curr.toTail;
                if(curr == this.head) return true;
            }
            return false;
        }

        public Snake addNode()
        {
            return new Snake(this.Tail);
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
            if (this.head == this) return "☺";
            if (this.Tail == this)
            {
                switch(this.getFromHeadDirection())
                {
                    case Direction.Up: return "↑";
                    case Direction.Right: return "→";
                    case Direction.Down: return "↓";
                    case Direction.Left: return "←";
                }
            }

            Direction dFromHead = getFromHeadDirection();
            Direction dFromTail = getFromTailDirection();

            switch(dFromHead)
            {
                case Direction.Up:
                    {
                        switch(dFromTail)
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
            Snake curr = Tail;
            while(curr != null)
            {
                s += curr.pos.ToString();
                curr = curr.toHead;
            }
            return s;
        }
    }
}
