using ICSharpCode.SharpZipLib.Zip;

using Inputs;
using Inputs.Misc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Inputs.InputMethods.Drivers
{
    internal class DD 
    {
        public static DD Instance { get; } = new DD();

        public string DDXPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Chrome", "IO", "HID");

        private const string ResourcePath = "Inputs.Resources.ddx.zip";
        private IntPtr handle = IntPtr.Zero;

        #region Private Delegates
        private delegate int DD_MouseButton(int button);
        private delegate int DD_MouseWheel(int rotation);
        private delegate int DD_SetCursorPos(int x, int y);
        private delegate int DD_MoveMouseBy(int dx, int dy);
        private delegate int DD_KeybordButton(int ddcode, int flag);
        private delegate int DD_MapVKtoDD(int vkcode);
        private delegate int pDD_str(string str);

        private DD_MouseButton mouseButton;      //Mouse button 
        private DD_MouseWheel mouseWheel;        //Mouse wheel
        private DD_SetCursorPos setCursorPos;    //Mouse move abs. 
        private DD_MoveMouseBy moveMouseBy;      //Mouse move rel. 
        private DD_KeybordButton keyboardButton; //Keyboard 
        private DD_MapVKtoDD mapVkToDD;          //VK to ddcode
        private pDD_str str;                     //Input visible char
        #endregion

        #region Interface
        public bool IsInstalled
        {
            get
            {
                if (Directory.Exists(DDXPath) == false)
                    return false;

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourcePath))
                {
                    using (var zf = new ZipFile(stream))
                    {
                        DirectoryInfo info = new DirectoryInfo(DDXPath);
                        var files = info.GetFiles();

                        foreach (ZipEntry ze in zf)
                        {
                            if (ze.IsDirectory == true)
                                continue;

                            if (files.Any((f) => f.Name == ze.Name) == false)
                                return false;
                        }
                    }
                }

                return true;
            }
        }

        public bool Install()
        {
            try
            {
                if (IsInstalled == false)
                {
                    #region Unzip
                    if (Directory.Exists(DDXPath) == false)
                        Directory.CreateDirectory(DDXPath);

                    string zip = Path.Combine(DDXPath, "t.zip");

                    if (Help.ExtractResourceTo(ResourcePath, zip) == false)
                        return false;

                    FastZip zipper = new FastZip();

                    zipper.ExtractZip(zip, DDXPath, null);

                    if (File.Exists(zip))
                        File.Delete(zip);
                    #endregion
                }

                if (handle == IntPtr.Zero)
                {
                    string dd_dll = Path.Combine(DDXPath, Environment.Is64BitOperatingSystem ? "DD94687.64.dll" : "DD94687.32.dll");

                    handle = Native.Kernel32.LoadLibrary(dd_dll);

                    #region Mouse Functions
                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_btn");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_btn");
                            return false;
                        }

                        mouseButton = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_MouseButton)) as DD_MouseButton;
                    }

                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_whl");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_whl");
                            return false;
                        }

                        mouseWheel = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_MouseWheel)) as DD_MouseWheel;
                    }

                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_mov");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_mov");
                            return false;
                        }

                        setCursorPos = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_SetCursorPos)) as DD_SetCursorPos;
                    }


                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_movR");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_movR");
                            return false;
                        }

                        moveMouseBy = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_MoveMouseBy)) as DD_MoveMouseBy;
                    }
                    #endregion

                    #region Keyboard Functions
                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_key");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_key");
                            return false;
                        }

                        keyboardButton = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_KeybordButton)) as DD_KeybordButton;
                    }

                    {
                        var func = Native.Kernel32.GetProcAddress(handle, "DD_todc");

                        if (func == IntPtr.Zero)
                        {
                            Debug.WriteLine("Failed to get DD_todc");
                            return false;
                        }

                        mapVkToDD = Marshal.GetDelegateForFunctionPointer(func, typeof(DD_MapVKtoDD)) as DD_MapVKtoDD;
                    }
                    #endregion

                    mouseButton(0); // initialize ?? 
                }

                return true;
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public bool Uninstall()
        {
            if (IsInstalled == false)
                return true;

            try
            {
                if(handle != IntPtr.Zero)
                {
                    Native.Kernel32.FreeLibrary(handle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            try
            {
                if(Directory.Exists(DDXPath) == true)
                    Directory.Delete(DDXPath, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return true;
        }
        #endregion

        public static class Mouse
        {
            public static bool Press(MouseKey key)
            {
                Instance.Install();

                try
                {
                    int result = 0;

                    switch (key)
                    {
                        case MouseKey.Left:
                            result = Instance.mouseButton(1);
                            break;

                        case MouseKey.Middle:
                            result = Instance.mouseButton(16);
                            break;

                        case MouseKey.Right:
                            result = Instance.mouseButton(4);
                            break;
                    }

                    return result == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }

            public static bool Release(MouseKey key)
            {
                Instance.Install();

                try
                {
                    int result = 0;

                    switch (key)
                    {
                        case MouseKey.Left:
                            result = Instance.mouseButton(2);
                            break;

                        case MouseKey.Middle:
                            result = Instance.mouseButton(32);
                            break;

                        case MouseKey.Right:
                            result = Instance.mouseButton(8);
                            break;
                    }

                    return result == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }

            public static bool SetCursorPos(int x, int y)
            {
                Instance.Install();

                try
                {
                    return Instance.setCursorPos(x, y) == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }

            public static bool MoveBy(int dx, int dy)
            {
                Instance.Install();

                try
                {
                    return Instance.moveMouseBy(dx, dy) == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }
        }

        public static class Keyboard
        {
            public static bool Press(VK vk)
            {
                if (vk == VK.NULL)
                    return false;

                Instance.Install();

                try
                {
                    int mapped = Instance.mapVkToDD((int)vk);
                    int result = Instance.keyboardButton(mapped, 1);
                    return result == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }

            public static bool Release(VK vk)
            {
                if (vk == VK.NULL)
                    return false;

                Instance.Install();

                try
                {
                    int mapped = Instance.mapVkToDD((int)vk);
                    int result = Instance.keyboardButton(mapped, 2);
                    return result == 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return false;
            }
        }
    }
}