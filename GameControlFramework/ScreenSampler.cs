using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GameControlFramework
{
    public class ScreenSampler
    {
        public Color GetPixelColour(int x, int y)
        {
            IntPtr hdc = Win32.GetDC(IntPtr.Zero);
            uint pixel = Win32.GetPixel(hdc, x, y);
            Win32.ReleaseDC(IntPtr.Zero, hdc);

            byte[] colourBytes = BitConverter.GetBytes(pixel);
            return Color.FromArgb(colourBytes[0], colourBytes[1], colourBytes[2], colourBytes[3]);
        }

        public Color GetPixelColourAtMousePointer()
        {
            var w32Mouse = new Win32.Win32Point();
            Win32.GetCursorPos(ref w32Mouse);
            return GetPixelColour(w32Mouse.X, w32Mouse.Y);
        }

        public Color GetPixelColourAtMousePointAfterMovingMouseAway()
        {
            Color colour;

            var w32Mouse = new Win32.Win32Point();
            Win32.GetCursorPos(ref w32Mouse);
            using (var sender = new InputSender(null))
            {
                sender.MoveMouse(0, 0);
                Thread.Sleep(250);
                colour = GetPixelColour(w32Mouse.X, w32Mouse.Y);
                sender.MoveMouse(w32Mouse.X, w32Mouse.Y);
            }

            return colour;
        }
    }
}