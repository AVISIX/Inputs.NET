using System;

namespace Inputs.Misc
{
    public class InputMethodNotFoundException : Exception
    {
        public InputMethodNotFoundException(string message) : base(message)
        {
        }
    }
}
