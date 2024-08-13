using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class InputController
    {
        Interactable obj;
        public bool ignoreInput = false;
        private void reset()
        {
            this.obj = null;
        }

        public void control(Interactable obj, bool updateOnKey)
        {
            this.reset();
            this.obj = obj;
            List<KeyAction> actions = obj.KeyActions;

            if (updateOnKey && obj != null) obj.Display();
            while(this.obj != null)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                // Console.WriteLine(ignoreInput);
                if (ignoreInput) continue;

                switch(key.Key)
                {
                    case ConsoleKey.Escape:
                        {
                            obj.Stop();
                            Console.Clear();
                            Program.MainMenu();
                            return;
                        }
                }

                foreach(KeyAction keyAction in actions.Where((ka) => ka.key == key.Key))
                {
                    keyAction.action.Invoke();
                }
                if(updateOnKey)
                {
                    Console.Clear();
                    if (obj != null) obj.Display();
                }
            }
        }

    }
    public class KeyAction
    {
        public ConsoleKey key { get; }
        public Func<int> action { get; }

        public KeyAction(ConsoleKey key, Func<int> action)
        {
            this.key = key;
            this.action = action;
        }
    }
}
