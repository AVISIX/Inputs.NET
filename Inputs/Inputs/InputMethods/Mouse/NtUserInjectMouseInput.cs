using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static Inputs.Misc.Native.Kernel32;

namespace Inputs.InputMethods.Mouse
{
    /// <summary>
    /// A mouse input method utilizing the undocumented 'NtUserInjectMouseInput' NT function.
    /// </summary>
    internal class NtUserInjectMouseInput : IMouseInput
    {
        public string Name => nameof(NtUserInjectMouseInput);

        private IntPtr handle = IntPtr.Zero;

        public NtUserInjectMouseInput()
        {
            handle = LoadLibrary("win32u.dll");
        }

        public Dictionary<MouseKey, bool> heldKeys = new Dictionary<MouseKey, bool>();
    
        private delegate void _NtUserInjectMouseInput(IntPtr input, int n);

        private bool Call(Native.User32.MOUSEEVENTF_FLAGS flags, int dx, int dy, uint dwData, int dwExtraInfo)
        {
            try
            {
                if (handle == IntPtr.Zero)
                {
                    handle = LoadLibrary("win32u.dll");
                }

                if (handle == IntPtr.Zero)
                {
                    Debug.WriteLine("Failed to load win32u.dll");
                    return false;
                }

                try
                {
                    IntPtr address = GetProcAddress(handle, "NtUserInjectMouseInput");

                    if (address == IntPtr.Zero)
                    {
                        Debug.WriteLine("Failed to get Address for the 'NtUserInjectMouseInput'-function.");
                        return false;
                    }

                    unsafe
                    {
                        *(void**)&address = (void*)address; // https://github.com/Zpes/mouse-input-injection/blob/master/direct_mouse_input/mouse_interface.hpp#L35
                    }

                    Native.User32.MOUSEINPUT input = new Native.User32.MOUSEINPUT();
                    input.dx = dx;
                    input.dy = dy;
                    input.mouseData = dwData;
                    input.dwExtraInfo = (UIntPtr)dwExtraInfo;
                    input.dwFlags = flags;

                    IntPtr inputPtr = Marshal.AllocHGlobal(Marshal.SizeOf(input));
                    Marshal.StructureToPtr(input, inputPtr, true);

                    ((_NtUserInjectMouseInput)Marshal.GetDelegateForFunctionPointer(address, typeof(_NtUserInjectMouseInput)))(inputPtr, 1);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public bool MoveBy(int x = 0, int y = 0)
        {
            return Call(~Native.User32.MOUSEEVENTF_FLAGS.MOUSEEVENTF_MOVE | ~Native.User32.MOUSEEVENTF_FLAGS.MOUSEEVENTF_ABSOLUTE, x, y, 0, 0);
        }

        public bool Press(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.ContainsKey(key) == true)
                return true;

            var result = Call(key.MapMouseKey(true), 0, 0, 0, 0);

            heldKeys.Add(key, true);

            return result;
        }

        public bool Release(MouseKey key = MouseKey.Left)
        {
            if (heldKeys.ContainsKey(key) == false)
                return true;

            var result = Call(key.MapMouseKey(false), 0, 0, 0, 0);

            heldKeys.Remove(key);

            return result;
        }

        public void Dispose()
        {
            try
            {
                foreach (var key in heldKeys)
                {
                    Call(key.Key.MapMouseKey(false), 0, 0, 0, 0); // release all held keys 
                }

                FreeLibrary(handle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        ~NtUserInjectMouseInput() => Dispose();
    }
}
