using Inputs.InputMethods.Drivers;

using System.Collections.Generic;

namespace Inputs.InputMethods.Mouse
{
    public sealed class MouseDD : IMouseInput
    {
        public string Name => nameof(MouseDD);

        public List<MouseKey> heldKeys = new List<MouseKey>();

        public void Dispose()
        {
            foreach(var key in heldKeys)
            {
                DD.Mouse.Release(key);
            }
        }

        public bool MoveBy(int x = 0, int y = 0)
        {
            return DD.Mouse.MoveBy(x, y);
        }

        public bool Press(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.Contains(key))
                return true;
            
            heldKeys.Add(key);

            return DD.Mouse.Press(key);
        }

        public bool Release(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.Contains(key) == false)
                return true;

            heldKeys.Remove(key);

            return DD.Mouse.Release(key);
        }
    }
}
