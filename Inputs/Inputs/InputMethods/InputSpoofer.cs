using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inputs.InputMethods
{
    /// <summary>
    /// A unique class to Spoof Inputs.
    /// </summary>
    internal static class InputSpoofer
    {
        private static Native.User32.HookProcedure mouseHookProc = MouseSpoofCallback;

        // This will deal with removing the injected-flag.
        // This seems to not travel down the hook chain as this is probably current-process-only.
        private static IntPtr MouseSpoofCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return Native.User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            if (lParam != IntPtr.Zero)
            {
                var s = Marshal.PtrToStructure<Native.User32.MSLLHOOKSTRUCT>(lParam);

                // remove LLMHF_INJECTED-flag                
                const int LLMHF_INJECTED = 0x00000001;

                if ((s.flags & LLMHF_INJECTED) != 0)
                    s.flags &= ~LLMHF_INJECTED;

                // remove LLMHF_LOWER_IL_INJECTED-flag
                const int LLMHF_LOWER_IL_INJECTED = 0x00000002;

                if ((s.flags & LLMHF_LOWER_IL_INJECTED) != 0)
                    s.flags &= ~LLMHF_LOWER_IL_INJECTED;

                // save changes 
                Marshal.StructureToPtr(s, lParam, true);
            }

            return Native.User32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr mouseHook = IntPtr.Zero;
        private static GCHandle? mouseHookHandle = null;

        public static void StartSpoofing()
        {
            // https://stackoverflow.com/questions/69102624/executionengineexception-on-lowlevel-keyboard-hook

            var t = new Thread(() =>
            {
                if (mouseHook != IntPtr.Zero)
                    StopSpoofing();

                if (mouseHookHandle != null)
                    mouseHookHandle.Value.Free();

                mouseHookHandle = GCHandle.Alloc(mouseHook);

                const int WH_MOUSE_LL = 14;

                if (mouseHook == IntPtr.Zero)
                    mouseHook = Native.User32.SetWindowsHookEx(WH_MOUSE_LL, mouseHookProc, Native.Kernel32.GetModuleHandle(null), 0);
            })
            {
                IsBackground = true
            };

            t.Start();

            while (t.ThreadState == ThreadState.Running)
            {
                if (t.ThreadState == ThreadState.Unstarted)
                    break;

                if (t.ThreadState == ThreadState.Aborted)
                    break;

                if (t.ThreadState == ThreadState.Suspended)
                    break;

                Thread.Sleep(60);
            }
        }

        public static void StopSpoofing()
        {
            if (mouseHook != IntPtr.Zero)
                Native.User32.UnhookWindowsHookEx(mouseHook);

            if (mouseHookHandle != null)
                mouseHookHandle.Value.Free();

            mouseHook = IntPtr.Zero;
            mouseHookHandle = null;
        }

        public static void ResetSpoofing()
        {
            StopSpoofing();
            StartSpoofing();
        }
    }
}
