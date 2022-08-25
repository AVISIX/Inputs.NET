using Inputs.InputMethods.Keyboard;
using Inputs.InputMethods;
using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Inputs
{
    public static class Keyboard
    {
        private static XoR crypto = new XoR();
        private static int spoofCounter = 0;
        private static IKeyboardInput standardInput = null;

        static Keyboard()
        {
            // initialize the method object
            SetMethodFrom<keybd_event>();
            standardInput = MethodResolver<IKeyboardInput>.GetMethodObjectFor<NtUserSendInput>();
            ForceInit();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        // cleanup (destructor alternative)
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            standardInput.Dispose();
            MethodObject.Dispose();
        }

        #region Public Properties
        /// <summary>
        /// The current object of the input-method that is being used.
        /// </summary>
        public static IKeyboardInput MethodObject { get; private set; }

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
        public static void SetMethodFrom<T>() where T : class
        {
            MethodObject.Dispose();
            MethodObject = MethodResolver<IKeyboardInput>.GetMethodObjectFor<T>();
        }

        /// <summary>
        /// This is an optional way to initialize all the "spoofing-byte-arrays" early on,
        /// because otherwise the the arrays will be initialized when calling a function related to it the first time.
        /// I personally suggest using this in your app.xaml, main function or whatever your entrypoint may be.
        /// </summary>
        public static void ForceInit()
        {
            InputSpoofer.StartSpoofing();
        }

        #region Clicking
        private static Dictionary<VK, DateTime> _clickTimeout = null;
        private static Dictionary<VK, DateTime> clickTimeout
        {
            get
            {
                if (_clickTimeout == null)
                {
                    _clickTimeout = new Dictionary<VK, DateTime>();

                    foreach (var key in Enum.GetValues(typeof(VK)))
                    {
                        if (_clickTimeout.ContainsKey((VK)key) == true)
                            continue;

                        _clickTimeout.Add((VK)key, DateTime.MinValue);
                    }
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
        public static void Click(VK key, double delay = .273)
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
        /// Press a specific Key down.
        /// </summary>
        /// <param name="key">The key you want to press.</param>
        public static void Press(VK key)
        {
            if (key == VK.NULL)
                return;

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
        /// Release a specific key. (This will only affect keys that are already down)
        /// </summary>
        /// <param name="key">The key you want to release.</param>
        public static void Release(VK key)
        {
            if (key == VK.NULL)
                return;

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
