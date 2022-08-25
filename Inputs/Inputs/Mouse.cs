using Inputs.InputMethods;
using Inputs.InputMethods.Drivers;
using Inputs.InputMethods.Mouse;
using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Inputs.Misc.Native.Kernel32;

namespace Inputs
{
    /// <summary>
    /// The types of clicks that are supported.
    /// </summary>
    public enum ClickType
    {
        Down,
        Up
    }

    /// <summary>
    /// The type of mouse-keys that are supported.
    /// </summary>
    public enum MouseKey
    {
        Left,
        Right,
        Middle
    }

    /// <summary>
    /// The global static class to control the mouse.
    /// </summary>
    public static class Mouse
    {
        private static XoR crypto = new XoR();
        private static int spoofCounter = 0;
        private static IMouseInput standardInput = null;

        static Mouse()
        {
            // initialize the method object
            SetMethodFrom<MouseEvent>();
            standardInput = MethodResolver<IMouseInput>.GetMethodObjectFor<NtUserSendInput>();
            ForceInit();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        // cleanup (destructor alternative)
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            standardInput?.Dispose();
            MethodObject?.Dispose();
        }

        #region Public Properties
        /// <summary>
        /// The current object of the input-method that is being used.
        /// </summary>
        public static IMouseInput MethodObject { get; private set; }

        /// <summary>
        /// Defines the amount of mouse inputs that can be made before the spoofer will be reset.
        /// </summary>
        public static int SpoofingResetThreshold { get; set; } = 100;

        /// <summary>
        /// Enable or disable, if win32 calls should get spoofed.
        /// Default is 'true'.
        /// </summary>
        public static bool SpoofCalls { get; set; }
        #endregion

        /// <summary>
        /// Set the current input-method. This will reset all the currently held down keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void SetMethodFrom<T>()
        {
            MethodObject?.Dispose();
            MethodObject = MethodResolver<IMouseInput>.GetMethodObjectFor<T>();
        }

        /// <summary>
        /// This is an optional way to initialize all the "spoofing-byte-arrays" early on,
        /// because otherwise the the arrays will be initialized when calling a function related to it the first time.
        /// I personally suggest using this in your app.xaml, main function or whatever your entrypoint may be.
        /// </summary>
        public static void ForceInit()
        {
            // init each of the byte arrays
            // but make sure to encrypt them!

            if (NtUserSetCursorPosBytes.Length == 0)
            {
                var temp = Inputs.Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserSetCursorPos");
                if (temp.Length > 0)
                    NtUserSetCursorPosBytes = crypto.Encrypt(temp);
            }

            if (NtUserGetCursorPosBytes.Length == 0)
            {
                var temp = Inputs.Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserGetCursorPos");
                if (temp.Length > 0)
                    NtUserGetCursorPosBytes = crypto.Encrypt(temp);
            }

            InputSpoofer.StartSpoofing();
        }

        #region SetCursorPos
        private static byte[] NtUserSetCursorPosBytes = new byte[0];
        private delegate bool _NtUserSetCursorPos(int x, int y);

        /// <summary>
        /// Set the Cursor Position to a specific point. 
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="spoofCall">Enable or disable if the call should get spoofed.</param>
        public static bool SetCursorPos(int x, int y)
        {
            if (SpoofCalls == false)
                return Native.User32.SetCursorPos(x, y);

            if (MethodObject is MouseDD)
                return DD.Mouse.SetCursorPos(x, y);

            try
            {
                if (NtUserSetCursorPosBytes.Length == 0)
                {
                    // we want to call the syscall directly 
                    NtUserSetCursorPosBytes = Inputs.Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserSetCursorPos");

                    // if everything fails, just do a normal call 
                    if (NtUserSetCursorPosBytes.Length == 0)
                        return Native.User32.SetCursorPos(x, y);

                    NtUserSetCursorPosBytes = crypto.Encrypt(NtUserSetCursorPosBytes);
                }

                // Alloc the bytes
                IntPtr addy = VirtualAlloc(IntPtr.Zero, (uint)NtUserSetCursorPosBytes.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

                // Copy the bytes to the address
                Marshal.Copy(crypto.Decrypt(NtUserSetCursorPosBytes), 0, addy, NtUserSetCursorPosBytes.Length);

                if (++spoofCounter >= SpoofingResetThreshold)
                {
                    InputSpoofer.ResetSpoofing();
                    spoofCounter = 0;
                }

                // Create a delegate for the memory chunk & execute it 
                bool result = ((_NtUserSetCursorPos)Marshal.GetDelegateForFunctionPointer(addy, typeof(_NtUserSetCursorPos)))(x, y);

                // Free the memory
                VirtualFree(addy, NtUserSetCursorPosBytes.Length, FreeType.Release);

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Native.User32.SetCursorPos(x, y);
            }

        }

        /// <summary>
        /// Set the Cursor Position to a specific point. 
        /// </summary>
        /// <param name="xy">The coordinates as point struct.</param>
        /// <param name="spoofCall">Enable or disable if the call should get spoofed.</param>
        public static void SetCursorPos(Point<int> xy) => SetCursorPos(xy.X, xy.Y);
        #endregion

        #region GetCursorPos
        private static Point<int> GetCursorPosW32()
        {
            if (Native.User32.GetCursorPos(out System.Drawing.Point pt) == false)
                return new Point<int>(-1, -1);

            return new Point<int>(pt.X, pt.Y);
        }

        private static byte[] NtUserGetCursorPosBytes = new byte[0];
        private delegate int _NtUserGetCursorPos(out Native.User32.POINT p, int n);

        /// <summary>
        /// Returns the current Cursor coordinates.
        /// </summary>
        /// <returns>The coordinates, or -1x and -1y if it fails.</returns>
        public static Point<int> GetCursorPos()
        {
            if (SpoofCalls == false)
                return GetCursorPosW32();

            try
            {
                // same story
                if (NtUserGetCursorPosBytes.Length == 0)
                {
                    NtUserGetCursorPosBytes = Inputs.Misc.Help.GetUnmanagedFunctionBytes("win32u", "NtUserGetCursorPos");

                    if (NtUserGetCursorPosBytes.Length == 0)
                        return GetCursorPosW32();

                    NtUserGetCursorPosBytes = crypto.Encrypt(NtUserGetCursorPosBytes);
                }

                // Alloc the bytes
                IntPtr addy = VirtualAlloc(IntPtr.Zero, (uint)NtUserGetCursorPosBytes.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

                // Copy the bytes to the address
                Marshal.Copy(crypto.Decrypt(NtUserGetCursorPosBytes), 0, addy, NtUserGetCursorPosBytes.Length);

                // Create a delegate for the memory chunk & execute it 
                ((_NtUserGetCursorPos)Marshal.GetDelegateForFunctionPointer(addy, typeof(_NtUserGetCursorPos)))(out Native.User32.POINT p, 1);

                // Free the memory
                VirtualFree(addy, NtUserGetCursorPosBytes.Length, FreeType.Release);

                return new Point<int>(p.X, p.Y);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return GetCursorPosW32();
            }
        }
        #endregion

        #region Clicking & Moving 
        /// <summary>
        /// Move the Mouse from its current position by X and Y.
        /// </summary>
        /// <param name="x">The amount of pixels the mouse should move on the X-axis.</param>
        /// <param name="y">The amount of pixels the mouse should move on the Y-axis.</param>
        /// <param name="spoofCall">Enable or disable if the call should get spoofed.</param>
        public static void Move(int x, int y)
        {
            if (x == 0 && y == 0)
                return;

            if (++spoofCounter >= SpoofingResetThreshold)
            {
                InputSpoofer.ResetSpoofing();
                spoofCounter = 0;
            }

            if (SpoofCalls == false)
            {
                standardInput.MoveBy(x, y);
                return;
            }

            try
            {
                if (MethodObject.MoveBy(x, y) == false)
                    standardInput.MoveBy(x, y);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                standardInput.MoveBy(x, y);
            }
        }

        private static Dictionary<MouseKey, DateTime> _clickTimeout = null;
        private static Dictionary<MouseKey, DateTime> clickTimeout
        {
            get
            {
                if (_clickTimeout == null)
                {
                    _clickTimeout = new Dictionary<MouseKey, DateTime>();

                    foreach (var key in Enum.GetValues(typeof(MouseKey)))
                        _clickTimeout.Add((MouseKey)key, DateTime.MinValue);
                }

                return _clickTimeout;
            }
        }

        /// <summary>
        /// Click a specific Mouse button with a delay.
        /// </summary>
        /// <param name="key">The key you want to press.</param>
        /// <param name="delay">The delay (in seconds) between press & release. Default is the average click speed of a human being.</param>
        /// <param name="spoofcall">Should the call get spoofed?</param>
        public static void Click(MouseKey key, double delay = .273)
        {
            // we wait until we can click again so its more human 
            while (DateTime.Now < clickTimeout[key])
                Thread.Sleep(10);

            if (delay < 0) delay = .273;
            if (delay == 0) delay = .01;

            Press(key);
            Thread.Sleep((int)(delay * 1000.0));

            Release(key);

            // we do a little timeout here, because if you call this function multiple times, 
            // it might press again directly after release and we dont want that
            clickTimeout[key] = DateTime.Now + TimeSpan.FromSeconds(delay);
        }

        /// <summary>
        /// Press a specific key down.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="spoofcall"></param>
        /// <exception cref="Exception">May throw an exception if the key is invalid or unsupported.</exception>
        public static void Press(MouseKey key)
        {
            if (++spoofCounter >= SpoofingResetThreshold)
            {
                InputSpoofer.ResetSpoofing();
                spoofCounter = 0;
            }

            if (SpoofCalls == false)
            {
                standardInput.Press(key);
                return;
            }

            try
            {
                if (MethodObject.Press(key) == false)
                    standardInput.Press(key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                standardInput.Press(key);
            }
        }

        /// <summary>
        /// Release a key that is pressed.
        /// </summary>
        /// <param name="key">The key you want to release.</param>
        /// <param name="spoofcall">Should the call get spoofed?</param>
        /// <exception cref="Exception">May throw an exception if the key is invalid or unsupported.</exception>
        public static void Release(MouseKey key)
        {
            if (++spoofCounter >= SpoofingResetThreshold)
            {
                InputSpoofer.ResetSpoofing();
                spoofCounter = 0;
            }

            if (SpoofCalls == false)
            {
                standardInput.Release(key);
                return;
            }

            try
            {
                if (MethodObject.Release(key) == false)
                    standardInput.Release(key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                standardInput.Release(key);
            }
        }
        #endregion
    }
}
