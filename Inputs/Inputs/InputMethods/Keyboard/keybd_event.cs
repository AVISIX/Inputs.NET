using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inputs.InputMethods.Keyboard
{
    /// <summary>
    /// A keyboard input method utilizing the deprecated 'keybd_event' win32 function.
    /// </summary>
    internal sealed class keybd_event : IKeyboardInput
    {
        public string Name => nameof(keybd_event);

        private List<VK> heldKeys = new List<VK>();

        public bool Press(VK key)
        {
            if (heldKeys.Contains(key) == true)
                return true;

            try
            {
                Native.User32.keybd_event((byte)key, 0, Native.User32.KEYEVENTF_KEYDOWN, UIntPtr.Zero);

                heldKeys.Add(key);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public bool Release(VK key)
        {
            if (heldKeys.Contains(key) == false)
                return true;

            try
            {
                Native.User32.keybd_event((byte)key, 0, Native.User32.KEYEVENTF_KEYUP, UIntPtr.Zero);

                heldKeys.Remove(key);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public void Dispose()
        {
            foreach (var key in heldKeys)
            {
                Native.User32.keybd_event((byte)key, 0, Native.User32.KEYEVENTF_KEYUP, UIntPtr.Zero);
            }
        }

        ~keybd_event() => Dispose();
    }
}
