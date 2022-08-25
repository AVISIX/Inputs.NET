using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static Inputs.Misc.Native.Kernel32;


namespace Inputs.InputMethods.Mouse
{
    /// <summary>
    /// A mouse input method utilizing the 'NtSendUserInput' NT function.
    /// </summary>
    public sealed class NtUserSendInput : IMouseInput
    {
        public string Name => nameof(NtUserSendInput);

        public Dictionary<MouseKey, bool> heldKeys = new Dictionary<MouseKey, bool>();

        private static byte[] NtUserSendInputBytes = new byte[0];
        private delegate void _NtUserSendInput(uint cInputs, IntPtr input, int cbSize);
        private static XoR crypto = new XoR();

        public NtUserSendInput()
        {
            if (NtUserSendInputBytes.Length == 0)
            {
                var temp = Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserSendInput");
                if (temp.Length > 0)
                    NtUserSendInputBytes = crypto.Encrypt(temp);
            }
        }

        private bool Call(Native.User32.MOUSEEVENTF_FLAGS flags, int dx, int dy, uint dwData, int dwExtraInfo)
        {
            try
            {
                if (NtUserSendInputBytes.Length == 0)
                {
                    NtUserSendInputBytes = Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserSendInput");

                    if (NtUserSendInputBytes.Length == 0)
                    {
                        Debug.WriteLine("Failed to get NtUserSendInputBytes bytes");
                        return false;
                    }

                    NtUserSendInputBytes = crypto.Encrypt(NtUserSendInputBytes);
                }

                // Alloc the bytes
                IntPtr addy = VirtualAlloc(IntPtr.Zero, (uint)NtUserSendInputBytes.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

                // Copy the bytes to the address
                Marshal.Copy(crypto.Decrypt(NtUserSendInputBytes), 0, addy, NtUserSendInputBytes.Length);

                // Create input struct
                Native.User32.INPUT input = new Native.User32.INPUT();
                input.type = 0; // 0 = INPUT_MOUSE 
                input.U.mi.dy = dy;
                input.U.mi.dx = dx;
                input.U.mi.mouseData = dwData;
                input.U.mi.dwExtraInfo = (UIntPtr)dwExtraInfo;
                input.U.mi.dwFlags = flags;

                IntPtr inputPtr = Marshal.AllocHGlobal(Marshal.SizeOf(input));
                Marshal.StructureToPtr(input, inputPtr, true);

                // Create a delegate for the memory chunk & execute it 
                ((_NtUserSendInput)Marshal.GetDelegateForFunctionPointer(addy, typeof(_NtUserSendInput)))(1u, inputPtr, Marshal.SizeOf(input));

                // Free the memory
                VirtualFree(addy, NtUserSendInputBytes.Length, FreeType.Release);

                return true;
            }
            catch (Exception ex )
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public bool MoveBy(int x = 0, int y = 0)
        {
            var absolute = Misc.Help.CalculateAbsolutePosition(x, y);
            x = absolute.X;
            y = absolute.Y;

            return Call(Native.User32.MOUSEEVENTF_FLAGS.MOUSEEVENTF_MOVE | Native.User32.MOUSEEVENTF_FLAGS.MOUSEEVENTF_ABSOLUTE, x, y, 0, 0);
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        ~NtUserSendInput() => Dispose();
    }
}
