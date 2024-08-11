using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    abstract class Interactable
    {
        abstract public List<KeyAction> KeyActions { get; }
        virtual public void Stop() { }
        abstract public void Display();
    }
}
