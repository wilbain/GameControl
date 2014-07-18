using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameControlFramework
{
    public class InputSender : IDisposable
    {
        private IntPtr _windowHandle;


        public InputSender(string windowName)
        {
            if (string.IsNullOrEmpty(windowName))
            {
                _windowHandle = IntPtr.Zero;
            }
            else
            {
                _windowHandle = Win32.FindWindow(null, windowName);
            }

            IntPtr targetThread = Win32.GetWindowThreadProcessId(_windowHandle, IntPtr.Zero);
            IntPtr sourceThread = Win32.GetCurrentThread();
            Win32.AttachThreadInput(sourceThread, targetThread, true);
        }

        public void SendKeyPress(Key key, Key modifier)
        {
            int size = (modifier != Key.None) ? 4 : 2;
            Win32.INPUT[] inputs = new Win32.INPUT[size];

            if (size == 4)
            {
                inputs[0] = CreateInput(modifier, KeyDirection.Down);
                inputs[1] = CreateInput(key, KeyDirection.Down);
                inputs[2] = CreateInput(key, KeyDirection.Up);
                inputs[3] = CreateInput(modifier, KeyDirection.Up);
            }
            else
            {
                inputs[0] = CreateInput(key, KeyDirection.Down);
                inputs[1] = CreateInput(key, KeyDirection.Up);
            }

            Win32.SendInput((uint) inputs.Length, inputs, Marshal.SizeOf(typeof(Win32.INPUT)));
        }

        public void BringWindowToFocus()
        {
            Win32.SetForegroundWindow(_windowHandle);
        }

        public void MoveMouse(int x, int y)
        {
            var point = new Win32.Win32Point { X = x, Y = y };

            Win32.ClientToScreen(_windowHandle, ref point);
            Win32.SetCursorPos(point.X, point.Y);
        }

        public void Dispose()
        {
            IntPtr targetThread = Win32.GetWindowThreadProcessId(_windowHandle, IntPtr.Zero);
            IntPtr sourceThread = Win32.GetCurrentThread();
            Win32.AttachThreadInput(sourceThread, targetThread, false);
        }

        private Win32.INPUT CreateInput(Key key, KeyDirection keyDirection)
        {
            uint eventFlags = (keyDirection == KeyDirection.Down) ? Win32.KEYEVENTF_EXTENDEDKEY : Win32.KEYEVENTF_EXTENDEDKEY | Win32.KEYEVENTF_KEYUP;

            return new Win32.INPUT
                {
                    type = Win32.INPUT_KEYBOARD,
                    u = new Win32.InputUnion
                    {
                        ki = new Win32.KEYBDINPUT
                        {
                            wVk = (ushort)KeyInterop.VirtualKeyFromKey(key),
                            wScan = (ushort) KeyInterop.VirtualKeyFromKey(key),
                            dwFlags = eventFlags,
                            dwExtraInfo = Win32.GetMessageExtraInfo(),
                        }
                    }
                };
        }

        private enum KeyDirection
        {
            Up,
            Down
        }
    }
}
