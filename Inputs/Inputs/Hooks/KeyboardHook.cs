using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.Hooks
{
    /// <summary>
    /// A delegate for the Mouse Hook.
    /// </summary>
    /// <param name="xy">The coordinates of the mouse.</param>
    /// <param name="key">The key which was pressed.</param>
    /// <param name="type">The type of the click that was made. Either "up" or "down".</param>
    /// <param name="isInjected">Is the key artificial/injected?</param>
    public delegate void KeyboardHookEventHandler(VK vk, bool isInjected);

    /// <summary>
    /// A simple class to Hook keyboard inputs.
    /// </summary>
    public class KeyboardHook
    {
        /// <summary>
        /// This event gets raised whenever a key is pressed down.
        /// </summary>
        public event KeyboardHookEventHandler OnKeyPressed;

        /// <summary>
        /// The event gets raised whenever a key is released, after being pressed down.
        /// </summary>
        public event KeyboardHookEventHandler OnKeyReleased;

        #region Private
        private const int WH_KEYBOARD_LL = 13;

        const int LLKHF_INJECTED = 0x00000010;
        const int LLKHF_LOWER_IL_INJECTED = 0x00000002;

        private Native.User32.HookProcedure hookProc;
        private IntPtr hook = IntPtr.Zero;
        #endregion

        public KeyboardHook()
        {
            hookProc = Callback;
        }

        ~KeyboardHook()
        {
            Unhook();
        }

        /// <summary>
        /// Hook the Keyboard.
        /// </summary>
        public void Hook()
        {
            hook = SetHook(hookProc, WH_KEYBOARD_LL);
        }

        /// <summary>
        /// Unhook the Keyboard.
        /// </summary>
        public void Unhook()
        {
            if (hook != IntPtr.Zero)
                Native.User32.UnhookWindowsHookEx(hook);
        }

        private IntPtr SetHook(Native.User32.HookProcedure proc, int hkType)
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule main = process.MainModule)
                return Native.User32.SetWindowsHookEx(hkType, proc, Native.Kernel32.GetModuleHandle(main.ModuleName), 0);
        }

        private IntPtr Callback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || lParam == IntPtr.Zero)
                return Native.User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            try
            {
                var hkStruct = Marshal.PtrToStructure<Native.User32.MSLLHOOKSTRUCT>(lParam);

                VK vk = KeyMapper.MapToVK(Marshal.ReadInt32(lParam));

                if (vk != VK.NULL)
                {
                    // if these flags are set, its a fake input 
                    bool simulated = ((hkStruct.flags & LLKHF_INJECTED) != 0) || ((hkStruct.flags & LLKHF_LOWER_IL_INJECTED) != 0);

                    if (nCode >= 0 && ((WM)wParam == WM.WM_KEYDOWN || (WM)wParam == WM.WM_SYSKEYDOWN))
                    {
                        Help.DispatchInThread(() => OnKeyPressed?.Invoke(vk, true));
                    }
                    else
                    if (nCode >= 0 && ((WM)wParam == WM.WM_KEYUP || (WM)wParam == WM.WM_SYSKEYUP))
                    {
                        Help.DispatchInThread(() => OnKeyReleased?.Invoke(vk, true));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return Native.User32.CallNextHookEx(hook, nCode, wParam, lParam);
        }
    }
}
