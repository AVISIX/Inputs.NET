using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inputs.InputMethods.Mouse
{
    /// <summary>
    /// A mouse input method utilizing the 'MouseEvent' win32 function.
    /// </summary>
    internal sealed class MouseEvent : IMouseInput
    {
        public string Name => nameof(MouseEvent);

        public Dictionary<MouseKey, bool> heldKeys = new Dictionary<MouseKey, bool>();

        public bool MoveBy(int x = 0, int y = 0)
        {
            Native.User32.mouse_event(Native.User32.MOUSEEVENTF_FLAGS.MOUSEEVENTF_MOVE, x, y, 0, 0);
            return true;
        }

        public bool Press(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.ContainsKey(key) == true)
                return true;

            Native.User32.mouse_event(key.MapMouseKey(true), 0, 0, 0, 0);

            heldKeys.Add(key, true);
            
            return true;
        }

        public bool Release(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.ContainsKey(key) == false)
                return true;

            Native.User32.mouse_event(key.MapMouseKey(false), 0, 0, 0, 0);

            heldKeys.Remove(key);

            return true;
        }

        public void Dispose()
        {
            try
            {
                foreach (var key in heldKeys)
                {
                    Native.User32.mouse_event(key.Key.MapMouseKey(false), 0, 0, 0, 0); // release all held keys 
                }
            }
            catch (Exception ex )
            {
                Debug.WriteLine(ex);
            }
        }

        ~MouseEvent() => Dispose();
    }
}
