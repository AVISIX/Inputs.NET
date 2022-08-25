using System;

namespace Inputs
{
    internal static class KeyMapper
    {
        /// <summary>
        /// Returns the VK key with a matching <paramref name="code"/>
        /// </summary>
        /// <param name="code">The code of the VK-Key.</param>
        /// <returns></returns>
        public static VK MapToVK(this int code)
        {
            if (code < 0)
                return VK.NULL;

            foreach (var key in Enum.GetValues(typeof(VK)))
            {
                if (key == null || ((short)key) == 0)
                    continue;

                if ((short)key == (short)code)
                {
                    return (VK)Enum.Parse(typeof(VK), Enum.GetName(typeof(VK), key));
                }
            }

            return VK.NULL;
        }

        /// <summary>
        /// Returns the VK with a matching <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the VK-Key.</param>
        /// <returns></returns>
        public static VK MapToVK(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return VK.NULL;

            foreach (var key in Enum.GetNames(typeof(VK)))
            {
                if (key.ToLower() == name.ToLower().Trim())
                {
                    return (VK)Enum.Parse(typeof(VK), key);
                }
            }

            return VK.NULL;
        }
    }


    public enum VK : short
    {
        /// <summary>
        /// Null, just null.
        /// </summary>
        NULL = 0x0,

        ///<summary>
        ///Left mouse button
        ///</summary>
        KEY_LBUTTON = 0x01,

        ///<summary>
        ///Right mouse button
        ///</summary>
        KEY_RBUTTON = 0x02,

        ///<summary>
        ///Control-break processing
        ///</summary>
        KEY_CANCEL = 0x03,

        ///<summary>
        ///Middle mouse button (three-button mouse)
        ///</summary>
        KEY_MBUTTON = 0x04,

        ///<summary>
        ///Windows 2000/XP: X1 mouse button
        ///</summary>
        KEY_XBUTTON1 = 0x05,

        ///<summary>
        ///Windows 2000/XP: X2 mouse button
        ///</summary>
        KEY_XBUTTON2 = 0x06,

        ///<summary>
        ///BACKSPACE key
        ///</summary>
        KEY_BACK = 0x08,

        ///<summary>
        ///TAB key
        ///</summary>
        KEY_TAB = 0x09,

        ///<summary>
        ///CLEAR key
        ///</summary>
        KEY_CLEAR = 0x0C,

        ///<summary>
        ///ENTER key
        ///</summary>
        KEY_RETURN = 0x0D,

        ///<summary>
        ///SHIFT key
        ///</summary>
        KEY_SHIFT = 0x10,

        ///<summary>
        ///CTRL key
        ///</summary>
        KEY_CONTROL = 0x11,

        ///<summary>
        ///ALT key
        ///</summary>
        KEY_MENU = 0x12,

        ///<summary>
        ///PAUSE key
        ///</summary>
        KEY_PAUSE = 0x13,

        ///<summary>
        ///CAPS LOCK key
        ///</summary>
        KEY_CAPITAL = 0x14,

        ///<summary>
        ///Input Method Editor (IME) Kana mode
        ///</summary>
        KEY_KANA = 0x15,

        ///<summary>
        ///IME Hangul mode
        ///</summary>
        KEY_HANGUL = 0x15,

        ///<summary>
        ///IME Junja mode
        ///</summary>
        KEY_JUNJA = 0x17,

        ///<summary>
        ///IME final mode
        ///</summary>
        KEY_FINAL = 0x18,

        ///<summary>
        ///IME Hanja mode
        ///</summary>
        KEY_HANJA = 0x19,

        ///<summary>
        ///IME Kanji mode
        ///</summary>
        KEY_KANJI = 0x19,

        ///<summary>
        ///ESC key
        ///</summary>
        KEY_ESCAPE = 0x1B,

        ///<summary>
        ///IME convert
        ///</summary>
        KEY_CONVERT = 0x1C,

        ///<summary>
        ///IME nonconvert
        ///</summary>
        KEY_NONCONVERT = 0x1D,

        ///<summary>
        ///IME accept
        ///</summary>
        KEY_ACCEPT = 0x1E,

        ///<summary>
        ///IME mode change request
        ///</summary>
        KEY_MODECHANGE = 0x1F,

        ///<summary>
        ///SPACEBAR
        ///</summary>
        KEY_SPACE = 0x20,

        ///<summary>
        ///PAGE UP key
        ///</summary>
        KEY_PRIOR = 0x21,

        ///<summary>
        ///PAGE DOWN key
        ///</summary>
        KEY_NEXT = 0x22,

        ///<summary>
        ///END key
        ///</summary>
        KEY_END = 0x23,

        ///<summary>
        ///HOME key
        ///</summary>
        KEY_HOME = 0x24,

        ///<summary>
        ///LEFT ARROW key
        ///</summary>
        KEY_LEFT = 0x25,

        ///<summary>
        ///UP ARROW key
        ///</summary>
        KEY_UP = 0x26,

        ///<summary>
        ///RIGHT ARROW key
        ///</summary>
        KEY_RIGHT = 0x27,

        ///<summary>
        ///DOWN ARROW key
        ///</summary>
        KEY_DOWN = 0x28,

        ///<summary>
        ///SELECT key
        ///</summary>
        KEY_SELECT = 0x29,

        ///<summary>
        ///PRINT key
        ///</summary>
        KEY_PRINT = 0x2A,

        ///<summary>
        ///EXECUTE key
        ///</summary>
        KEY_EXECUTE = 0x2B,

        ///<summary>
        ///PRINT SCREEN key
        ///</summary>
        KEY_SNAPSHOT = 0x2C,

        ///<summary>
        ///INS key
        ///</summary>
        KEY_INSERT = 0x2D,

        ///<summary>
        ///DEL key
        ///</summary>
        KEY_DELETE = 0x2E,

        ///<summary>
        ///HELP key
        ///</summary>
        KEY_HELP = 0x2F,

        ///<summary>
        ///0 key
        ///</summary>
        KEY_0 = 0x30,

        ///<summary>
        ///1 key
        ///</summary>
        KEY_1 = 0x31,

        ///<summary>
        ///2 key
        ///</summary>
        KEY_2 = 0x32,

        ///<summary>
        ///3 key
        ///</summary>
        KEY_3 = 0x33,

        ///<summary>
        ///4 key
        ///</summary>
        KEY_4 = 0x34,

        ///<summary>
        ///5 key
        ///</summary>
        KEY_5 = 0x35,

        ///<summary>
        ///6 key
        ///</summary>
        KEY_6 = 0x36,

        ///<summary>
        ///7 key
        ///</summary>
        KEY_7 = 0x37,

        ///<summary>
        ///8 key
        ///</summary>
        KEY_8 = 0x38,

        ///<summary>
        ///9 key
        ///</summary>
        KEY_9 = 0x39,

        ///<summary>
        ///A key
        ///</summary>
        KEY_A = 0x41,

        ///<summary>
        ///B key
        ///</summary>
        KEY_B = 0x42,

        ///<summary>
        ///C key
        ///</summary>
        KEY_C = 0x43,

        ///<summary>
        ///D key
        ///</summary>
        KEY_D = 0x44,

        ///<summary>
        ///E key
        ///</summary>
        KEY_E = 0x45,

        ///<summary>
        ///F key
        ///</summary>
        KEY_F = 0x46,

        ///<summary>
        ///G key
        ///</summary>
        KEY_G = 0x47,

        ///<summary>
        ///H key
        ///</summary>
        KEY_H = 0x48,

        ///<summary>
        ///I key
        ///</summary>
        KEY_I = 0x49,

        ///<summary>
        ///J key
        ///</summary>
        KEY_J = 0x4A,

        ///<summary>
        ///K key
        ///</summary>
        KEY_K = 0x4B,

        ///<summary>
        ///L key
        ///</summary>
        KEY_L = 0x4C,

        ///<summary>
        ///M key
        ///</summary>
        KEY_M = 0x4D,

        ///<summary>
        ///N key
        ///</summary>
        KEY_N = 0x4E,

        ///<summary>
        ///O key
        ///</summary>
        KEY_O = 0x4F,

        ///<summary>
        ///P key
        ///</summary>
        KEY_P = 0x50,

        ///<summary>
        ///Q key
        ///</summary>
        KEY_Q = 0x51,

        ///<summary>
        ///R key
        ///</summary>
        KEY_R = 0x52,

        ///<summary>
        ///S key
        ///</summary>
        KEY_S = 0x53,

        ///<summary>
        ///T key
        ///</summary>
        KEY_T = 0x54,

        ///<summary>
        ///U key
        ///</summary>
        KEY_U = 0x55,

        ///<summary>
        ///V key
        ///</summary>
        KEY_V = 0x56,

        ///<summary>
        ///W key
        ///</summary>
        KEY_W = 0x57,

        ///<summary>
        ///X key
        ///</summary>
        KEY_X = 0x58,

        ///<summary>
        ///Y key
        ///</summary>
        KEY_Y = 0x59,

        ///<summary>
        ///Z key
        ///</summary>
        KEY_Z = 0x5A,

        ///<summary>
        ///Left Windows key (Microsoft Natural keyboard)
        ///</summary>
        KEY_LWIN = 0x5B,

        ///<summary>
        ///Right Windows key (Natural keyboard)
        ///</summary>
        KEY_RWIN = 0x5C,

        ///<summary>
        ///Applications key (Natural keyboard)
        ///</summary>
        KEY_APPS = 0x5D,

        ///<summary>
        ///Computer Sleep key
        ///</summary>
        KEY_SLEEP = 0x5F,

        ///<summary>
        ///Numeric keypad 0 key
        ///</summary>
        KEY_NUMPAD0 = 0x60,

        ///<summary>
        ///Numeric keypad 1 key
        ///</summary>
        KEY_NUMPAD1 = 0x61,

        ///<summary>
        ///Numeric keypad 2 key
        ///</summary>
        KEY_NUMPAD2 = 0x62,

        ///<summary>
        ///Numeric keypad 3 key
        ///</summary>
        KEY_NUMPAD3 = 0x63,

        ///<summary>
        ///Numeric keypad 4 key
        ///</summary>
        KEY_NUMPAD4 = 0x64,

        ///<summary>
        ///Numeric keypad 5 key
        ///</summary>
        KEY_NUMPAD5 = 0x65,

        ///<summary>
        ///Numeric keypad 6 key
        ///</summary>
        KEY_NUMPAD6 = 0x66,

        ///<summary>
        ///Numeric keypad 7 key
        ///</summary>
        KEY_NUMPAD7 = 0x67,

        ///<summary>
        ///Numeric keypad 8 key
        ///</summary>
        KEY_NUMPAD8 = 0x68,

        ///<summary>
        ///Numeric keypad 9 key
        ///</summary>
        KEY_NUMPAD9 = 0x69,

        ///<summary>
        ///Multiply key
        ///</summary>
        KEY_MULTIPLY = 0x6A,

        ///<summary>
        ///Add key
        ///</summary>
        KEY_ADD = 0x6B,

        ///<summary>
        ///Separator key
        ///</summary>
        KEY_SEPARATOR = 0x6C,

        ///<summary>
        ///Subtract key
        ///</summary>
        KEY_SUBTRACT = 0x6D,

        ///<summary>
        ///Decimal key
        ///</summary>
        KEY_DECIMAL = 0x6E,

        ///<summary>
        ///Divide key
        ///</summary>
        KEY_DIVIDE = 0x6F,

        ///<summary>
        ///F1 key
        ///</summary>
        KEY_F1 = 0x70,

        ///<summary>
        ///F2 key
        ///</summary>
        KEY_F2 = 0x71,

        ///<summary>
        ///F3 key
        ///</summary>
        KEY_F3 = 0x72,

        ///<summary>
        ///F4 key
        ///</summary>
        KEY_F4 = 0x73,

        ///<summary>
        ///F5 key
        ///</summary>
        KEY_F5 = 0x74,

        ///<summary>
        ///F6 key
        ///</summary>
        KEY_F6 = 0x75,

        ///<summary>
        ///F7 key
        ///</summary>
        KEY_F7 = 0x76,

        ///<summary>
        ///F8 key
        ///</summary>
        KEY_F8 = 0x77,

        ///<summary>
        ///F9 key
        ///</summary>
        KEY_F9 = 0x78,

        ///<summary>
        ///F10 key
        ///</summary>
        KEY_F10 = 0x79,

        ///<summary>
        ///F11 key
        ///</summary>
        KEY_F11 = 0x7A,

        ///<summary>
        ///F12 key
        ///</summary>
        KEY_F12 = 0x7B,

        ///<summary>
        ///F13 key
        ///</summary>
        KEY_F13 = 0x7C,

        ///<summary>
        ///F14 key
        ///</summary>
        KEY_F14 = 0x7D,

        ///<summary>
        ///F15 key
        ///</summary>
        KEY_F15 = 0x7E,

        ///<summary>
        ///F16 key
        ///</summary>
        KEY_F16 = 0x7F,

        ///<summary>
        ///F17 key  
        ///</summary>
        KEY_F17 = 0x80,

        ///<summary>
        ///F18 key  
        ///</summary>
        KEY_F18 = 0x81,

        ///<summary>
        ///F19 key  
        ///</summary>
        KEY_F19 = 0x82,

        ///<summary>
        ///F20 key  
        ///</summary>
        KEY_F20 = 0x83,

        ///<summary>
        ///F21 key  
        ///</summary>
        KEY_F21 = 0x84,

        ///<summary>
        ///F22 key, (PPC only) Key used to lock device.
        ///</summary>
        KEY_F22 = 0x85,

        ///<summary>
        ///F23 key  
        ///</summary>
        KEY_F23 = 0x86,

        ///<summary>
        ///F24 key  
        ///</summary>
        KEY_F24 = 0x87,

        ///<summary>
        ///NUM LOCK key
        ///</summary>
        KEY_NUMLOCK = 0x90,

        ///<summary>
        ///SCROLL LOCK key
        ///</summary>
        KEY_SCROLL = 0x91,

        ///<summary>
        ///Left SHIFT key
        ///</summary>
        KEY_LSHIFT = 0xA0,

        ///<summary>
        ///Right SHIFT key
        ///</summary>
        KEY_RSHIFT = 0xA1,

        ///<summary>
        ///Left CONTROL key
        ///</summary>
        KEY_LCONTROL = 0xA2,

        ///<summary>
        ///Right CONTROL key
        ///</summary>
        KEY_RCONTROL = 0xA3,

        ///<summary>
        ///Left MENU key
        ///</summary>
        KEY_LMENU = 0xA4,

        ///<summary>
        ///Right MENU key
        ///</summary>
        KEY_RMENU = 0xA5,

        ///<summary>
        ///Windows 2000/XP: Browser Back key
        ///</summary>
        KEY_BROWSER_BACK = 0xA6,

        ///<summary>
        ///Windows 2000/XP: Browser Forward key
        ///</summary>
        KEY_BROWSER_FORWARD = 0xA7,

        ///<summary>
        ///Windows 2000/XP: Browser Refresh key
        ///</summary>
        KEY_BROWSER_REFRESH = 0xA8,

        ///<summary>
        ///Windows 2000/XP: Browser Stop key
        ///</summary>
        KEY_BROWSER_STOP = 0xA9,

        ///<summary>
        ///Windows 2000/XP: Browser Search key
        ///</summary>
        KEY_BROWSER_SEARCH = 0xAA,

        ///<summary>
        ///Windows 2000/XP: Browser Favorites key
        ///</summary>
        KEY_BROWSER_FAVORITES = 0xAB,

        ///<summary>
        ///Windows 2000/XP: Browser Start and Home key
        ///</summary>
        KEY_BROWSER_HOME = 0xAC,

        ///<summary>
        ///Windows 2000/XP: Volume Mute key
        ///</summary>
        KEY_VOLUME_MUTE = 0xAD,

        ///<summary>
        ///Windows 2000/XP: Volume Down key
        ///</summary>
        KEY_VOLUME_DOWN = 0xAE,

        ///<summary>
        ///Windows 2000/XP: Volume Up key
        ///</summary>
        KEY_VOLUME_UP = 0xAF,

        ///<summary>
        ///Windows 2000/XP: Next Track key
        ///</summary>
        KEY_MEDIA_NEXT_TRACK = 0xB0,

        ///<summary>
        ///Windows 2000/XP: Previous Track key
        ///</summary>
        KEY_EDIA_PREV_TRACK = 0xB1,

        ///<summary>
        ///Windows 2000/XP: Stop Media key
        ///</summary>
        KEY_MEDIA_STOP = 0xB2,

        ///<summary>
        ///Windows 2000/XP: Play/Pause Media key
        ///</summary>
        KEY_MEDIA_PLAY_PAUSE = 0xB3,

        ///<summary>
        ///Windows 2000/XP: Start Mail key
        ///</summary>
        KEY_LAUNCH_MAIL = 0xB4,

        ///<summary>
        ///Windows 2000/XP: Select Media key
        ///</summary>
        KEY_LAUNCH_MEDIA_SELECT = 0xB5,

        ///<summary>
        ///Windows 2000/XP: Start Application 1 key
        ///</summary>
        KEY_LAUNCH_APP1 = 0xB6,

        ///<summary>
        ///Windows 2000/XP: Start Application 2 key
        ///</summary>
        KEY_LAUNCH_APP2 = 0xB7,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_1 = 0xBA,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '+' key
        ///</summary>
        KEY_OEM_PLUS = 0xBB,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the ',' key
        ///</summary>
        KEY_OEM_COMMA = 0xBC,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '-' key
        ///</summary>
        KEY_OEM_MINUS = 0xBD,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '.' key
        ///</summary>
        KEY_OEM_PERIOD = 0xBE,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_2 = 0xBF,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_3 = 0xC0,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_4 = 0xDB,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_5 = 0xDC,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_6 = 0xDD,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_7 = 0xDE,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        KEY_OEM_8 = 0xDF,

        ///<summary>
        ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
        ///</summary>
        KEY_OEM_102 = 0xE2,

        ///<summary>
        ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
        ///</summary>
        KEY_PROCESSKEY = 0xE5,

        ///<summary>
        ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
        ///The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
        ///see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        ///</summary>
        KEY_PACKET = 0xE7,

        ///<summary>
        ///Attn key
        ///</summary>
        KEY_ATTN = 0xF6,

        ///<summary>
        ///CrSel key
        ///</summary>
        KEY_CRSEL = 0xF7,

        ///<summary>
        ///ExSel key
        ///</summary>
        KEY_EXSEL = 0xF8,

        ///<summary>
        ///Erase EOF key
        ///</summary>
        KEY_EREOF = 0xF9,

        ///<summary>
        ///Play key
        ///</summary>
        KEY_PLAY = 0xFA,

        ///<summary>
        ///Zoom key
        ///</summary>
        KEY_ZOOM = 0xFB,

        ///<summary>
        ///Reserved
        ///</summary>
        KEY_NONAME = 0xFC,

        ///<summary>
        ///PA1 key
        ///</summary>
        KEY_PA1 = 0xFD,

        ///<summary>
        ///Clear key
        ///</summary>
        KEY_OEM_CLEAR = 0xFE
    }

    public enum WM
    {
        WM_NULL = 0x00,
        WM_CREATE = 0x01,
        WM_DESTROY = 0x02,
        WM_MOVE = 0x03,
        WM_SIZE = 0x05,
        WM_ACTIVATE = 0x06,
        WM_SETFOCUS = 0x07,
        WM_KILLFOCUS = 0x08,
        WM_ENABLE = 0x0A,
        WM_SETREDRAW = 0x0B,
        WM_SETTEXT = 0x0C,
        WM_GETTEXT = 0x0D,
        WM_GETTEXTLENGTH = 0x0E,
        WM_PAINT = 0x0F,
        WM_CLOSE = 0x10,
        WM_QUERYENDSESSION = 0x11,
        WM_QUIT = 0x12,
        WM_QUERYOPEN = 0x13,
        WM_ERASEBKGND = 0x14,
        WM_SYSCOLORCHANGE = 0x15,
        WM_ENDSESSION = 0x16,
        WM_SYSTEMERROR = 0x17,
        WM_SHOWWINDOW = 0x18,
        WM_CTLCOLOR = 0x19,
        WM_WININICHANGE = 0x1A,
        WM_SETTINGCHANGE = 0x1A,
        WM_DEVMODECHANGE = 0x1B,
        WM_ACTIVATEAPP = 0x1C,
        WM_FONTCHANGE = 0x1D,
        WM_TIMECHANGE = 0x1E,
        WM_CANCELMODE = 0x1F,
        WM_SETCURSOR = 0x20,
        WM_MOUSEACTIVATE = 0x21,
        WM_CHILDACTIVATE = 0x22,
        WM_QUEUESYNC = 0x23,
        WM_GETMINMAXINFO = 0x24,
        WM_PAINTICON = 0x26,
        WM_ICONERASEBKGND = 0x27,
        WM_NEXTDLGCTL = 0x28,
        WM_SPOOLERSTATUS = 0x2A,
        WM_DRAWITEM = 0x2B,
        WM_MEASUREITEM = 0x2C,
        WM_DELETEITEM = 0x2D,
        WM_VKEYTOITEM = 0x2E,
        WM_CHARTOITEM = 0x2F,

        WM_SETFONT = 0x30,
        WM_GETFONT = 0x31,
        WM_SETHOTKEY = 0x32,
        WM_GETHOTKEY = 0x33,
        WM_QUERYDRAGICON = 0x37,
        WM_COMPAREITEM = 0x39,
        WM_COMPACTING = 0x41,
        WM_WINDOWPOSCHANGING = 0x46,
        WM_WINDOWPOSCHANGED = 0x47,
        WM_POWER = 0x48,
        WM_COPYDATA = 0x4A,
        WM_CANCELJOURNAL = 0x4B,
        WM_NOTIFY = 0x4E,
        WM_INPUTLANGCHANGEREQUEST = 0x50,
        WM_INPUTLANGCHANGE = 0x51,
        WM_TCARD = 0x52,
        WM_HELP = 0x53,
        WM_USERCHANGED = 0x54,
        WM_NOTIFYFORMAT = 0x55,
        WM_CONTEXTMENU = 0x7B,
        WM_STYLECHANGING = 0x7C,
        WM_STYLECHANGED = 0x7D,
        WM_DISPLAYCHANGE = 0x7E,
        WM_GETICON = 0x7F,
        WM_SETICON = 0x80,

        WM_NCCREATE = 0x81,
        WM_NCDESTROY = 0x82,
        WM_NCCALCSIZE = 0x83,
        WM_NCHITTEST = 0x84,
        WM_NCPAINT = 0x85,
        WM_NCACTIVATE = 0x86,
        WM_GETDLGCODE = 0x87,
        WM_NCMOUSEMOVE = 0xA0,
        WM_NCLBUTTONDOWN = 0xA1,
        WM_NCLBUTTONUP = 0xA2,
        WM_NCLBUTTONDBLCLK = 0xA3,
        WM_NCRBUTTONDOWN = 0xA4,
        WM_NCRBUTTONUP = 0xA5,
        WM_NCRBUTTONDBLCLK = 0xA6,
        WM_NCMBUTTONDOWN = 0xA7,
        WM_NCMBUTTONUP = 0xA8,
        WM_NCMBUTTONDBLCLK = 0xA9,

        WM_INPUT = 0x00FF,

        WM_KEYFIRST = 0x100,
        WM_KEYDOWN = 0x100,
        WM_KEYUP = 0x101,
        WM_CHAR = 0x102,
        WM_DEADCHAR = 0x103,
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105,
        WM_SYSCHAR = 0x106,
        WM_SYSDEADCHAR = 0x107,
        WM_KEYLAST = 0x108,

        WM_IME_STARTCOMPOSITION = 0x10D,
        WM_IME_ENDCOMPOSITION = 0x10E,
        WM_IME_COMPOSITION = 0x10F,
        WM_IME_KEYLAST = 0x10F,

        WM_INITDIALOG = 0x110,
        WM_COMMAND = 0x111,
        WM_SYSCOMMAND = 0x112,
        WM_TIMER = 0x113,
        WM_HSCROLL = 0x114,
        WM_VSCROLL = 0x115,
        WM_INITMENU = 0x116,
        WM_INITMENUPOPUP = 0x117,
        WM_MENUSELECT = 0x11F,
        WM_MENUCHAR = 0x120,
        WM_ENTERIDLE = 0x121,

        WM_CTLCOLORMSGBOX = 0x132,
        WM_CTLCOLOREDIT = 0x133,
        WM_CTLCOLORLISTBOX = 0x134,
        WM_CTLCOLORBTN = 0x135,
        WM_CTLCOLORDLG = 0x136,
        WM_CTLCOLORSCROLLBAR = 0x137,
        WM_CTLCOLORSTATIC = 0x138,

        WM_MOUSEFIRST = 0x200,
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_LBUTTONDBLCLK = 0x203,
        WM_RBUTTONDOWN = 0x204,
        WM_RBUTTONUP = 0x205,
        WM_RBUTTONDBLCLK = 0x206,
        WM_MBUTTONDOWN = 0x207,
        WM_MBUTTONUP = 0x208,
        WM_MBUTTONDBLCLK = 0x209,
        WM_MOUSEWHEEL = 0x20A,
        WM_MOUSEHWHEEL = 0x20E,

        WM_PARENTNOTIFY = 0x210,
        WM_ENTERMENULOOP = 0x211,
        WM_EXITMENULOOP = 0x212,
        WM_NEXTMENU = 0x213,
        WM_SIZING = 0x214,
        WM_CAPTURECHANGED = 0x215,
        WM_MOVING = 0x216,
        WM_POWERBROADCAST = 0x218,
        WM_DEVICECHANGE = 0x219,

        WM_MDICREATE = 0x220,
        WM_MDIDESTROY = 0x221,
        WM_MDIACTIVATE = 0x222,
        WM_MDIRESTORE = 0x223,
        WM_MDINEXT = 0x224,
        WM_MDIMAXIMIZE = 0x225,
        WM_MDITILE = 0x226,
        WM_MDICASCADE = 0x227,
        WM_MDIICONARRANGE = 0x228,
        WM_MDIGETACTIVE = 0x229,
        WM_MDISETMENU = 0x230,
        WM_ENTERSIZEMOVE = 0x231,
        WM_EXITSIZEMOVE = 0x232,
        WM_DROPFILES = 0x233,
        WM_MDIREFRESHMENU = 0x234,

        WM_IME_SETCONTEXT = 0x281,
        WM_IME_NOTIFY = 0x282,
        WM_IME_CONTROL = 0x283,
        WM_IME_COMPOSITIONFULL = 0x284,
        WM_IME_SELECT = 0x285,
        WM_IME_CHAR = 0x286,
        WM_IME_KEYDOWN = 0x290,
        WM_IME_KEYUP = 0x291,

        WM_MOUSEHOVER = 0x2A1,
        WM_NCMOUSELEAVE = 0x2A2,
        WM_MOUSELEAVE = 0x2A3,

        WM_CUT = 0x300,
        WM_COPY = 0x301,
        WM_PASTE = 0x302,
        WM_CLEAR = 0x303,
        WM_UNDO = 0x304,

        WM_RENDERFORMAT = 0x305,
        WM_RENDERALLFORMATS = 0x306,
        WM_DESTROYCLIPBOARD = 0x307,
        WM_DRAWCLIPBOARD = 0x308,
        WM_PAINTCLIPBOARD = 0x309,
        WM_VSCROLLCLIPBOARD = 0x30A,
        WM_SIZECLIPBOARD = 0x30B,
        WM_ASKCBFORMATNAME = 0x30C,
        WM_CHANGECBCHAIN = 0x30D,
        WM_HSCROLLCLIPBOARD = 0x30E,
        WM_QUERYNEWPALETTE = 0x30F,
        WM_PALETTEISCHANGING = 0x310,
        WM_PALETTECHANGED = 0x311,

        WM_HOTKEY = 0x312,
        WM_PRINT = 0x317,
        WM_PRINTCLIENT = 0x318,

        WM_HANDHELDFIRST = 0x358,
        WM_HANDHELDLAST = 0x35F,
        WM_PENWINFIRST = 0x380,
        WM_PENWINLAST = 0x38F,
        WM_COALESCE_FIRST = 0x390,
        WM_COALESCE_LAST = 0x39F,
        WM_DDE_FIRST = 0x3E0,
        WM_DDE_INITIATE = 0x3E0,
        WM_DDE_TERMINATE = 0x3E1,
        WM_DDE_ADVISE = 0x3E2,
        WM_DDE_UNADVISE = 0x3E3,
        WM_DDE_ACK = 0x3E4,
        WM_DDE_DATA = 0x3E5,
        WM_DDE_REQUEST = 0x3E6,
        WM_DDE_POKE = 0x3E7,
        WM_DDE_EXECUTE = 0x3E8,
        WM_DDE_LAST = 0x3E8,

        WM_USER = 0x400,
        WM_APP = 0x8000
    }

    public enum ScanCodeShort : short
    {
        LBUTTON = 0,
        RBUTTON = 0,
        CANCEL = 70,
        MBUTTON = 0,
        XBUTTON1 = 0,
        XBUTTON2 = 0,
        BACK = 14,
        TAB = 15,
        CLEAR = 76,
        RETURN = 28,
        SHIFT = 42,
        CONTROL = 29,
        MENU = 56,
        PAUSE = 0,
        CAPITAL = 58,
        KANA = 0,
        HANGUL = 0,
        JUNJA = 0,
        FINAL = 0,
        HANJA = 0,
        KANJI = 0,
        ESCAPE = 1,
        CONVERT = 0,
        NONCONVERT = 0,
        ACCEPT = 0,
        MODECHANGE = 0,
        SPACE = 57,
        PRIOR = 73,
        NEXT = 81,
        END = 79,
        HOME = 71,
        LEFT = 75,
        UP = 72,
        RIGHT = 77,
        DOWN = 80,
        SELECT = 0,
        PRINT = 0,
        EXECUTE = 0,
        SNAPSHOT = 84,
        INSERT = 82,
        DELETE = 83,
        HELP = 99,
        KEY_0 = 11,
        KEY_1 = 2,
        KEY_2 = 3,
        KEY_3 = 4,
        KEY_4 = 5,
        KEY_5 = 6,
        KEY_6 = 7,
        KEY_7 = 8,
        KEY_8 = 9,
        KEY_9 = 10,
        KEY_A = 30,
        KEY_B = 48,
        KEY_C = 46,
        KEY_D = 32,
        KEY_E = 18,
        KEY_F = 33,
        KEY_G = 34,
        KEY_H = 35,
        KEY_I = 23,
        KEY_J = 36,
        KEY_K = 37,
        KEY_L = 38,
        KEY_M = 50,
        KEY_N = 49,
        KEY_O = 24,
        KEY_P = 25,
        KEY_Q = 16,
        KEY_R = 19,
        KEY_S = 31,
        KEY_T = 20,
        KEY_U = 22,
        KEY_V = 47,
        KEY_W = 17,
        KEY_X = 45,
        KEY_Y = 21,
        KEY_Z = 44,
        LWIN = 91,
        RWIN = 92,
        APPS = 93,
        SLEEP = 95,
        NUMPAD0 = 82,
        NUMPAD1 = 79,
        NUMPAD2 = 80,
        NUMPAD3 = 81,
        NUMPAD4 = 75,
        NUMPAD5 = 76,
        NUMPAD6 = 77,
        NUMPAD7 = 71,
        NUMPAD8 = 72,
        NUMPAD9 = 73,
        MULTIPLY = 55,
        ADD = 78,
        SEPARATOR = 0,
        SUBTRACT = 74,
        DECIMAL = 83,
        DIVIDE = 53,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        F13 = 100,
        F14 = 101,
        F15 = 102,
        F16 = 103,
        F17 = 104,
        F18 = 105,
        F19 = 106,
        F20 = 107,
        F21 = 108,
        F22 = 109,
        F23 = 110,
        F24 = 118,
        NUMLOCK = 69,
        SCROLL = 70,
        LSHIFT = 42,
        RSHIFT = 54,
        LCONTROL = 29,
        RCONTROL = 29,
        LMENU = 56,
        RMENU = 56,
        BROWSER_BACK = 106,
        BROWSER_FORWARD = 105,
        BROWSER_REFRESH = 103,
        BROWSER_STOP = 104,
        BROWSER_SEARCH = 101,
        BROWSER_FAVORITES = 102,
        BROWSER_HOME = 50,
        VOLUME_MUTE = 32,
        VOLUME_DOWN = 46,
        VOLUME_UP = 48,
        MEDIA_NEXT_TRACK = 25,
        MEDIA_PREV_TRACK = 16,
        MEDIA_STOP = 36,
        MEDIA_PLAY_PAUSE = 34,
        LAUNCH_MAIL = 108,
        LAUNCH_MEDIA_SELECT = 109,
        LAUNCH_APP1 = 107,
        LAUNCH_APP2 = 33,
        OEM_1 = 39,
        OEM_PLUS = 13,
        OEM_COMMA = 51,
        OEM_MINUS = 12,
        OEM_PERIOD = 52,
        OEM_2 = 53,
        OEM_3 = 41,
        OEM_4 = 26,
        OEM_5 = 43,
        OEM_6 = 27,
        OEM_7 = 40,
        OEM_8 = 0,
        OEM_102 = 86,
        PROCESSKEY = 0,
        PACKET = 0,
        ATTN = 0,
        CRSEL = 0,
        EXSEL = 0,
        EREOF = 93,
        PLAY = 0,
        ZOOM = 98,
        NONAME = 0,
        PA1 = 0,
        OEM_CLEAR = 0,
    }
}
