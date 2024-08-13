using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace SnakeGame.Menus
{
    internal class SettingsMenu: Menu
    {
        private static SettingsMenu instance;

        internal enum _Settings
        {
            GridWidth,
            GridHeight,
            MovesPerSecond,
            FruitOnBoard,
        }

        SettingsItem[] _items;

        public static SettingsMenu Create()
        {
            SettingsMenu.instance = new SettingsMenu(
                    new[] {
                    new SettingsItem(_Settings.GridWidth, 8, 50, new MenuItem($"Grid width", () => { 
                        SettingsMenu.edit(8, 50, _Settings.GridWidth);
                        return 0; 
                    })),
                    new SettingsItem(_Settings.GridHeight, 8, 50, new MenuItem($"Grid height", () => {
                        SettingsMenu.edit(8, 50, _Settings.GridHeight);
                        return 0;
                    })),
                    new SettingsItem(_Settings.MovesPerSecond, 0.2, 10, new MenuItem($"Moves per second", () => {
                        SettingsMenu.edit(0.2, 10, _Settings.MovesPerSecond);
                        return 0;
                    })),
                    new SettingsItem(_Settings.FruitOnBoard, 1, 10, new MenuItem($"Fruit on board", () => {
                        SettingsMenu.edit(1, 10, _Settings.FruitOnBoard);
                        return 0;
                    })),
                });
            return SettingsMenu.instance;
        }

        private static void edit(double min, double max, _Settings settings)
        {
            Program.controller.ignoreInput = true;
            string resp = Interaction.InputBox("Enter a value", settings.ToString());
            Program.controller.ignoreInput = false;

            if (resp == null || resp == "") return;
            double converted;

            try
            {
                converted = Convert.ToDouble(resp);
            }
            catch { return; }

            switch(settings)
            {
                case _Settings.GridWidth:
                    {
                        Settings.gridWidth = Util.Clamp((int)min, (int)max, (int)converted);
                        break;
                    }
                case _Settings.GridHeight:
                    {
                        Settings.gridHeight = Util.Clamp((int)min, (int)max, (int)converted);
                        break;
                    }
                case _Settings.MovesPerSecond:
                    {
                        Settings.fruitOnBoard = Util.Clamp((int)min, (int)max, (int)converted);
                        break;
                    }
                case _Settings.FruitOnBoard:
                    {
                        Settings.movesPerSecond = Util.Clamp(min, max, converted);
                        break;
                    }
            }
        }

        private SettingsMenu(SettingsItem[] items): base(items.Select(_ => _.item).Concat(new[] { new MenuItem("Back", () => { Program.MainMenu(); return 0; }) }).ToArray())
        {
            this._items = items;
        }


        public override void Display()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if(i < this._items.Length)
                {
                    SettingsItem curr = this._items[i];
                    Console.WriteLine($"{curr.item}: {GetValue(curr._settings)}");
                } else
                {
                    Console.WriteLine(this.items[i]);
                }
                Console.ResetColor();
            }
        }

        private string GetValue(_Settings setting)
        {
            switch(setting)
            {
                case _Settings.GridWidth: return Settings.gridWidth.ToString();
                case _Settings.GridHeight: return Settings.gridHeight.ToString();
                case _Settings.MovesPerSecond: return Settings.movesPerSecond.ToString();
                case _Settings.FruitOnBoard: return Settings.fruitOnBoard.ToString();
            }
            return 0.ToString();
        }

        internal class SettingsItem
        {
            internal _Settings _settings;
            internal MenuItem item;

            internal SettingsItem(_Settings settings, double min, double max, MenuItem item )
            {
                this._settings = settings;
                this.item = item;
            }
        }
    }
}
