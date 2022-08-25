using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.Misc
{
    public class InputMethodNotFoundException : Exception
    {
        public InputMethodNotFoundException(string message) : base(message)
        {

        }
    }
}
