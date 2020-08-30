using System;
using System.Runtime.InteropServices;

namespace NoMoreClick
{
    class UserIdleTimeUtility
    {
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }

        private static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }

        public static double GetTotalMillisecondsSinceLastMouseKeyboardInteraction()
        {
            return TimeSpan.FromMilliseconds(UserIdleTimeUtility.GetIdleTime()).TotalMilliseconds;
        }
    }
}
