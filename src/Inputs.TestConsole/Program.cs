using Inputs.InputMethods.Mouse;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mouse.SetMethodFrom<MouseDD>();
            Mouse.Move(50, 50);
            Console.Read();
        }
    }
}
