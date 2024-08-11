using SnakeGame.Menus;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Program
    {
        public static InputController controller;
        static GameController game;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Program.controller = new InputController();
            Program.MainMenu();
        }

        public static void MainMenu() {
            Program.controller.control(new MainMenu(), true);
        }

        public static void Settings() {
            Program.controller.control(SettingsMenu.Create(), true);
        }

        public static void SinglePlayer() {
            Program.game = new GameController(false);
            Program.game.start();
            Program.controller.control(Program.game, false);
        }

        public static void MultiPlayer() {
            Program.game = new GameController(true);
            Program.game.start();
            Program.controller.control(Program.game, false);
        }

        public static void restart() {
            Program.game = new GameController(true);
            Program.controller.control(Program.game, false);
            Program.game.start();
        }
    }
}
