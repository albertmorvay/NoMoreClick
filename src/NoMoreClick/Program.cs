using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace NoMoreClick
{
    static class Program
    {
        [DllImport("user32.dll")]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (SystemParameters.SnapToDefaultButton)
            {
                System.Windows.Forms.MessageBox.Show("Mouse cursor setting snap-to-default is enabled; please turn this off to use this version of the software!\n\nCurrently the mouse cursor on this computer will automatically move to the default button of a dialog box, such as OK or Apply.\n\nYou can turn this off under:\n\n'Mouse Properties' -> 'Pointer Options'.\n\nFurther guidance here https://superuser.com/a/1177277\n\nThis application will stop running now.", "NoMoreClick - ACTION REQUIRED - PLEASE READ CAREFULLY", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else
            {
                bool createdNew = true;
                using (Mutex mutex = new Mutex(true, "NoMoreClick-6b71c0b6-972a-4385-b574-fcda2169d93e", out createdNew))
                {
                    if (createdNew)
                    {

                        System.Windows.Forms.Application.EnableVisualStyles();
                        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                        System.Windows.Forms.Application.Run(new FormMain());
                    }
                    else
                    {
                        Process current = Process.GetCurrentProcess();
                        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                        {
                            if (process.Id != current.Id)
                            {
                                SetForegroundWindow(process.MainWindowHandle);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
