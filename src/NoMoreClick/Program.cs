using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NoMoreClick;

internal static class Program
{
    private const uint GetSnapToDefaultButton = 0x005F;

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var singleInstance = new Mutex(true, "NoMoreClick-6b71c0b6-972a-4385-b574-fcda2169d93e", out bool firstInstance);
        if (!firstInstance)
            return;

        if (SystemParametersInfo(GetSnapToDefaultButton, 0, out int snapToDefault, 0) && snapToDefault != 0)
        {
            MessageBox.Show(
                "Turn off Snap To under Mouse Properties → Pointer Options before starting NoMoreClick. "
                + "Windows can move the pointer to a dialog's default button, where dwell clicking could activate it.",
                "NoMoreClick", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var tray = new TrayApplicationContext();
            Application.Run(tray);
        }
        catch (Win32Exception error)
        {
            MessageBox.Show($"NoMoreClick could not monitor mouse and keyboard input.\n\n{error.Message}",
                "NoMoreClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    [DllImport("user32.dll", EntryPoint = "SystemParametersInfoW")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SystemParametersInfo(uint action, uint parameter, out int value, uint flags);
}
