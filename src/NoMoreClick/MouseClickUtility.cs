using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace NoMoreClick;

internal static class MouseClickUtility
{
    private const uint InputMouse = 0;
    private const uint LeftDown = 0x0002;
    private const uint LeftUp = 0x0004;
    private const uint RightDown = 0x0008;
    private const uint RightUp = 0x0010;

    // Scope the marker to this process so other accessibility tools' events can still be observed.
    internal static readonly nuint InjectedClickTag = (nuint)RandomNumberGenerator.GetInt32(1, int.MaxValue);
    private static readonly int InputSize = Marshal.SizeOf<Input>();

    public static bool TryClick(MouseButtons button, bool doubleClick = false)
    {
        (uint downFlag, uint upFlag) = button switch
        {
            MouseButtons.Left => (LeftDown, LeftUp),
            MouseButtons.Right => (RightDown, RightUp),
            _ => throw new ArgumentOutOfRangeException(nameof(button))
        };

        Input down = CreateMouseInput(downFlag);
        Input up = CreateMouseInput(upFlag);
        Input[] inputs = doubleClick ? [down, up, down, up] : [down, up];

        // No motion or keyboard events are sent. Windows uses the current cursor and modifiers.
        uint inserted = SendInput((uint)inputs.Length, inputs, InputSize);
        if (inserted == inputs.Length)
            return true;

        // A short write may leave the button pressed. Try to release it without repeating the click.
        if ((inserted & 1) != 0)
            SendInput(1, [up], InputSize);

        return false;
    }

    public static bool TrySetRightButton(bool down) =>
        SendInput(1, [CreateMouseInput(down ? RightDown : RightUp)], InputSize) == 1;

    private static Input CreateMouseInput(uint flags) => new()
    {
        Type = InputMouse,
        Mouse = new MouseInput { Flags = flags, ExtraInfo = InjectedClickTag }
    };

    // MOUSEINPUT is the largest member of the native INPUT union and supplies its alignment.
    [StructLayout(LayoutKind.Sequential)]
    private struct Input
    {
        public uint Type;
        public MouseInput Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput
    {
        public int X;
        public int Y;
        public uint Data;
        public uint Flags;
        public uint Time;
        public nuint ExtraInfo;
    }

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern uint SendInput(uint count, [In] Input[] inputs, int size);
}
