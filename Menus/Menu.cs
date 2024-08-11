using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    abstract class Menu: Interactable
    {
        internal int selectedIndex = 0;
        internal MenuItem[] items;

        public Menu(MenuItem[] items) {
            this.items = items;
        }

        private void NextItem()
        {
            this.selectedIndex = Math.Min(this.items.Length - 1, selectedIndex + 1);
        }

        private void PreviousItem()
        {
            this.selectedIndex = Math.Max(0, selectedIndex - 1);
        }

        private void Select()
        {
            items[selectedIndex].onSelect();
        }

        public override List<KeyAction> KeyActions => new List<KeyAction> {
            new KeyAction(ConsoleKey.W, () => { PreviousItem(); return 0; }),
            new KeyAction(ConsoleKey.UpArrow, () => { PreviousItem(); return 0; }),
            new KeyAction(ConsoleKey.S, () => { NextItem(); return 0; }),
            new KeyAction(ConsoleKey.DownArrow, () => { NextItem(); return 0; }),
            new KeyAction(ConsoleKey.Enter, () => { Console.Clear(); Select(); return 0; }),
            new KeyAction(ConsoleKey.Spacebar, () => { Console.Clear(); Select(); return 0; }),
        };

        public override void Display()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(this.items[i].ToString());
                Console.ResetColor();
            }
        }
    }

    public class MenuItem
    {
        public String name;
        public Func<int> onSelect;
        public MenuItem(String name, Func<int> onSelect) {
            this.name = name;
            this.onSelect = onSelect;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
