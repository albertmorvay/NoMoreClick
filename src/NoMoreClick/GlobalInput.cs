using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NoMoreClick;

internal readonly record struct GlobalKeyEvent(Keys Key, bool IsDown, bool IsInjected);

internal readonly record struct GlobalMouseEvent(
    GlobalMouseEventKind Kind, Point Position, MouseButtons Button, bool IsInjected);

internal enum GlobalMouseEventKind
{
    Move,
    ButtonDown,
    ButtonUp,
    Wheel
}

// Create and dispose on the UI thread. Event handlers run inside the native hooks
// and must return promptly to keep Windows from removing the hooks.
internal sealed class GlobalInput : IDisposable
{
    private const int KeyboardHookId = 13;
    private const int MouseHookId = 14;
    private const int HookAction = 0;
    private const int InvalidHookHandle = 1404;
    private const uint KeyboardInjectedFlag = 0x10;
    private const uint MouseInjectedFlag = 0x01;

    private const uint KeyDown = 0x0100;
    private const uint KeyUp = 0x0101;
    private const uint SystemKeyDown = 0x0104;
    private const uint SystemKeyUp = 0x0105;
    private const uint MouseMove = 0x0200;
    private const uint LeftButtonDown = 0x0201;
    private const uint LeftButtonUp = 0x0202;
    private const uint RightButtonDown = 0x0204;
    private const uint RightButtonUp = 0x0205;
    private const uint MiddleButtonDown = 0x0207;
    private const uint MiddleButtonUp = 0x0208;
    private const uint VerticalWheel = 0x020A;
    private const uint XButtonDown = 0x020B;
    private const uint XButtonUp = 0x020C;
    private const uint HorizontalWheel = 0x020E;

    private readonly HookProcedure keyboardProcedure;
    private readonly HookProcedure mouseProcedure;
    private GCHandle registrationRoot;
    private nint keyboardHook;
    private nint mouseHook;
    private bool disposed;

    public GlobalInput()
    {
        keyboardProcedure = HandleKeyboard;
        mouseProcedure = HandleMouse;

        nint module = GetModuleHandleW(null);
        if (module == 0)
            throw new Win32Exception(Marshal.GetLastPInvokeError(), "Could not locate the application module.");

        // Windows does not keep managed delegates alive while hooks are registered.
        registrationRoot = GCHandle.Alloc(this);
        try
        {
            keyboardHook = InstallHook(KeyboardHookId, keyboardProcedure, module, "keyboard");
            mouseHook = InstallHook(MouseHookId, mouseProcedure, module, "mouse");
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public event Action<GlobalKeyEvent>? KeyChanged;
    public event Action<GlobalMouseEvent>? MouseChanged;
    public Exception? CallbackError { get; private set; }

    // Use a virtual key such as Keys.ControlKey or Keys.LButton, and poll outside a hook callback.
    public static bool IsKeyDown(Keys key) => GetAsyncKeyState((int)key) < 0;

    public void Dispose()
    {
        disposed = true;
        RemoveHook(ref mouseHook);
        RemoveHook(ref keyboardHook);

        if (mouseHook == 0 && keyboardHook == 0 && registrationRoot.IsAllocated)
            registrationRoot.Free();
    }

    private static nint InstallHook(int id, HookProcedure procedure, nint module, string name)
    {
        nint hook = SetWindowsHookExW(id, procedure, module, 0);
        if (hook == 0)
            throw new Win32Exception(Marshal.GetLastPInvokeError(), $"Could not monitor global {name} input.");

        return hook;
    }

    private void RemoveHook(ref nint hook)
    {
        if (hook == 0)
            return;

        if (!UnhookWindowsHookEx(hook))
        {
            int error = Marshal.GetLastPInvokeError();
            if (error != InvalidHookHandle)
            {
                // Keep the callback rooted if Windows might still have a reference to it.
                CallbackError ??= new Win32Exception(error, "Could not remove a global input hook.");
                return;
            }
        }

        hook = 0;
    }

    private nint HandleKeyboard(int code, nuint message, nint data)
    {
        if (code == HookAction && !disposed && CallbackError is null && KeyChanged is { } callback)
        {
            try
            {
                uint keyMessage = (uint)message;
                if (keyMessage is KeyDown or KeyUp or SystemKeyDown or SystemKeyUp)
                {
                    var input = Marshal.PtrToStructure<KeyboardHookData>(data);
                    callback(new((Keys)input.VirtualKey, keyMessage is KeyDown or SystemKeyDown,
                        (input.Flags & KeyboardInjectedFlag) != 0));
                }
            }
            catch (Exception error)
            {
                CallbackError = error;
            }
        }

        return CallNextHookEx(0, code, message, data);
    }

    private nint HandleMouse(int code, nuint message, nint data)
    {
        if (code == HookAction && !disposed && CallbackError is null && MouseChanged is { } callback)
        {
            try
            {
                var input = Marshal.PtrToStructure<MouseHookData>(data);
                bool injected = (input.Flags & MouseInjectedFlag) != 0;
                if ((!injected || input.ExtraInfo != MouseClickUtility.InjectedClickTag) &&
                    DecodeMouseMessage((uint)message, input.MouseData) is { } mouse)
                {
                    callback(new(mouse.Kind, new Point(input.X, input.Y), mouse.Button, injected));
                }
            }
            catch (Exception error)
            {
                CallbackError = error;
            }
        }

        return CallNextHookEx(0, code, message, data);
    }

    private static (GlobalMouseEventKind Kind, MouseButtons Button)? DecodeMouseMessage(uint message, uint data) =>
        message switch
        {
            MouseMove => (GlobalMouseEventKind.Move, MouseButtons.None),
            VerticalWheel or HorizontalWheel => (GlobalMouseEventKind.Wheel, MouseButtons.None),
            LeftButtonDown => (GlobalMouseEventKind.ButtonDown, MouseButtons.Left),
            LeftButtonUp => (GlobalMouseEventKind.ButtonUp, MouseButtons.Left),
            RightButtonDown => (GlobalMouseEventKind.ButtonDown, MouseButtons.Right),
            RightButtonUp => (GlobalMouseEventKind.ButtonUp, MouseButtons.Right),
            MiddleButtonDown => (GlobalMouseEventKind.ButtonDown, MouseButtons.Middle),
            MiddleButtonUp => (GlobalMouseEventKind.ButtonUp, MouseButtons.Middle),
            XButtonDown => (GlobalMouseEventKind.ButtonDown, DecodeXButton(data)),
            XButtonUp => (GlobalMouseEventKind.ButtonUp, DecodeXButton(data)),
            _ => null
        };

    private static MouseButtons DecodeXButton(uint data) => (data >> 16) switch
    {
        1 => MouseButtons.XButton1,
        2 => MouseButtons.XButton2,
        _ => MouseButtons.None
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardHookData
    {
        public uint VirtualKey;
        public uint ScanCode;
        public uint Flags;
        public uint Time;
        public nuint ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseHookData
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public nuint ExtraInfo;
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate nint HookProcedure(int code, nuint message, nint data);

    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint GetModuleHandleW(string? moduleName);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern nint SetWindowsHookExW(int id, HookProcedure procedure, nint module, uint threadId);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(nint hook);

    [DllImport("user32.dll", ExactSpelling = true)]
    private static extern nint CallNextHookEx(nint hook, int code, nuint message, nint data);

    [DllImport("user32.dll", ExactSpelling = true)]
    private static extern short GetAsyncKeyState(int virtualKey);
}
