using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Menus
{
    internal class MainMenu: Menu
    {
        public MainMenu(): base(new[] {
                new MenuItem("0 player", () => { Program.AI();  return 0; }),
                new MenuItem("1 player", () => { Program.SinglePlayer(); return 0; }),
                new MenuItem("2 player", () => { Program.MultiPlayer(); return 0; }),
                new MenuItem("Settings", () => { Program.Settings(); return 0; }),
            })
        { }
    }
}
