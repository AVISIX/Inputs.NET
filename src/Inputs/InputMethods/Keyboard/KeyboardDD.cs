using Inputs.InputMethods.Drivers;

using System.Collections.Generic;

namespace Inputs.InputMethods.Keyboard
{
    public class KeyboardDD : IKeyboardInput
    {
        public string Name => nameof(KeyboardDD);

        public List<VK> heldKeys = new List<VK>();

        public void Dispose()
        {
            foreach(var key in heldKeys)
            {
                DD.Keyboard.Release(key);
            }
        }

        public bool Press(VK key)
        {
            if (key == VK.NULL)
                return true;

            if (heldKeys.Contains(key) == true)
                return true;
            
            heldKeys.Add(key);

            return DD.Keyboard.Press(key);
        }

        public bool Release(VK key)
        {
            if (key == VK.NULL)
                return true;
            
            if (heldKeys.Contains(key) == false)
                return true;

            heldKeys.Remove(key);

            return DD.Keyboard.Release(key);
        }
    }
}
