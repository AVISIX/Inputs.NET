using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
    public delegate void MouseHookEventHandler(Point<int> xy, MouseKey key, ClickType type, bool isInjected);

    /// <summary>
    /// A simple class to hook Mouse inputs.
    /// </summary>
    public class MouseHook
    {
        /// <summary>
        /// This event gets raised whenever a mouse key is pressed.
        /// </summary>
        public event MouseHookEventHandler OnKeyPressed;

        /// <summary>
        /// This event gets raised whenever a mouse key gets released, after being pressed down.
        /// </summary>
        public event MouseHookEventHandler OnKeyReleased;

        #region Private
        private const int WH_MOUSE_LL = 14;

        const int LLMHF_INJECTED = 0x00000001;
        const int LLMHF_LOWER_IL_INJECTED = 0x00000002;

        private Native.User32.HookProcedure hookProc;
        private IntPtr hook = IntPtr.Zero;
        #endregion

        public MouseHook()
        {
            hookProc = Callback;
        }

        ~MouseHook()
        {
            Unhook();
        }

        /// <summary>
        /// Start hooking the mouse.
        /// </summary>
        public void Hook()
        {
            hook = SetHook(hookProc, WH_MOUSE_LL);
        }

        /// <summary>
        /// Stop hooking the mouse.
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
            if (nCode < 0)
                return Native.User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            try
            {
                MouseKey key;
                ClickType type;

                switch ((WM)wParam)
                {
                    default:
                        return Native.User32.CallNextHookEx(hook, nCode, wParam, lParam);

                    #region Leftclick
                    case WM.WM_LBUTTONDOWN:
                        type = ClickType.Down;
                        key = MouseKey.Left;
                        break;

                    case WM.WM_LBUTTONUP:
                        type = ClickType.Up;
                        key = MouseKey.Left;
                        break;
                    #endregion

                    #region Rightclick
                    case WM.WM_RBUTTONDOWN:
                        type = ClickType.Down;
                        key = MouseKey.Right;
                        break;

                    case WM.WM_RBUTTONUP:
                        type = ClickType.Up;
                        key = MouseKey.Right;
                        break;
                    #endregion

                    #region WheelClick
                    case WM.WM_MBUTTONDOWN:
                        type = ClickType.Down;
                        key = MouseKey.Middle;
                        break;

                    case WM.WM_MBUTTONUP:
                        type = ClickType.Up;
                        key = MouseKey.Middle;
                        break;
                        #endregion
                }

                var s = Marshal.PtrToStructure<Native.User32.MSLLHOOKSTRUCT>(lParam);

                // if these flags are set, its a fake input 
                bool simulated = ((s.flags & LLMHF_INJECTED) != 0) || ((s.flags & LLMHF_LOWER_IL_INJECTED) != 0);

                if (nCode >= 0)
                {
                    switch ((WM)wParam)
                    {
                        case WM.WM_LBUTTONDOWN:
                        case WM.WM_RBUTTONDOWN:
                        case WM.WM_MBUTTONDOWN:
                            Help.DispatchInThread(() => OnKeyPressed?.Invoke(new Point<int>(s.pt.X, s.pt.Y), key, type, simulated));
                            break;

                        case WM.WM_LBUTTONUP:
                        case WM.WM_RBUTTONUP:
                        case WM.WM_MBUTTONUP:
                            Help.DispatchInThread(() => OnKeyReleased?.Invoke(new Point<int>(s.pt.X, s.pt.Y), key, type, simulated));
                            break;
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
