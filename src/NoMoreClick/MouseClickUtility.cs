using System.Drawing;
using System.Runtime.InteropServices;

namespace NoMoreClick
{
    class MouseClickUtility
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void LeftClick(Point pointToClick)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, pointToClick.X, pointToClick.Y, 0, 0);
        }
        public static void RightClick(Point pointToClick)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, pointToClick.X, pointToClick.Y, 0, 0);
        }
    }
}
