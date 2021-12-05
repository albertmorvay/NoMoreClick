using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Config.Net;
using Gma.System.MouseKeyHook;

namespace NoMoreClick
{
    public partial class FormMain : Form
    {
        private IKeyboardMouseEvents m_Events;
        public static Point pointLastMousePosition;
        public static Point pointLastMouseClickPosition;
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer TimerToggleMouseClickAssistance;
        private bool mouseIsBeingDragged = false;
        private DateTime dateTimeLastKeyboardInput = DateTime.UtcNow;
        private bool lastMouseClickWasARightClick = false;
        private bool mouseClickAssistanceEnabled = true;
        private DateTime dateTimeLeftMouseClickPressedDown = DateTime.UtcNow;
        private DateTime dateTimeRightMouseClickPressedDown = DateTime.UtcNow;
        private DateTime dateTimeLastMouseScrollWheelInput = DateTime.UtcNow;
        private bool mouseMovedOutsideOfClickDeadZoneAroundPreviousMousePositionWithoutStoppingToClick = false;
        private int postClickDeadzoneRadius;
        private int clickDelayMs;
        private int clickDelayAfterRightClickMs;
        private int toggleClickAssistanceMs;
        private int noClickAfterTypingMs;
        private int noClickAfterScrollingMs;
        private int noClickAfterPhysicalMouseClickMs;
        private IMySettings settings;

        public FormMain()
        {
            SetWindowLocationBottomRightAboveIconTray();

            settings = new ConfigurationBuilder<IMySettings>()
               .UseJsonConfig()
               .Build();

            postClickDeadzoneRadius = settings.PostClickDeadzoneRadius;
            clickDelayMs = settings.ClickDelayMs;
            clickDelayAfterRightClickMs = settings.ClickDelayAfterRightClickMs;
            toggleClickAssistanceMs = settings.ToggleClickAssistanceMs;
            noClickAfterTypingMs = settings.NoClickAfterTypingMs;
            noClickAfterScrollingMs = settings.NoClickAfterScrollingMs;
            noClickAfterPhysicalMouseClickMs = settings.NoClickAfterPhysicalMouseClickMs;

            InitializeComponent();
            SetLastMouseClickPosition();
            SetTimer();
            SubscribeGlobal();
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(100);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (!MouseStillNearToWhereMouseWasBefore(Cursor.Position, pointLastMouseClickPosition, postClickDeadzoneRadius))
            {
                mouseMovedOutsideOfClickDeadZoneAroundPreviousMousePositionWithoutStoppingToClick = true;
            }

            var mouseClickDelayInMilliseconds = clickDelayMs;
            if (lastMouseClickWasARightClick)
            {
                mouseClickDelayInMilliseconds = clickDelayAfterRightClickMs;            }
                if (mouseClickAssistanceEnabled
                    && !userJustClicked()
                    && UserIdleTimeUtility.GetTotalMillisecondsSinceLastMouseKeyboardInteraction() > mouseClickDelayInMilliseconds 
                    && !mouseIsBeingDragged
                    && !userWasJustTyping()
                    && !userWasJustScrolling())
                {
                LeftMouseClick(Cursor.Position);
            }
        }

        private void SetLastMouseClickPosition()
        {
            pointLastMouseClickPosition = Cursor.Position;
            mouseMovedOutsideOfClickDeadZoneAroundPreviousMousePositionWithoutStoppingToClick = false;
        }

        private bool MouseStillNearToWhereMouseWasBefore(Point pointCurrentMousePosition, Point pointPreviousMousePosition, int radiusAroundPreviousMousePosition)
        {
            var result = true;

            Rectangle rectangle = new Rectangle(pointPreviousMousePosition.X - radiusAroundPreviousMousePosition, pointPreviousMousePosition.Y - radiusAroundPreviousMousePosition, radiusAroundPreviousMousePosition * 2, radiusAroundPreviousMousePosition * 2);
            if (!rectangle.Contains(pointCurrentMousePosition))
                result = false;

            return result;
        }

        private void LeftMouseClick(Point pointToClick)
        {
            if (mouseMovedOutsideOfClickDeadZoneAroundPreviousMousePositionWithoutStoppingToClick)
            {
                MouseClickUtility.LeftClick(pointToClick);
                SetLastMouseClickPosition();
            }
            else if (!MouseStillNearToWhereMouseWasBefore(pointToClick,pointLastMouseClickPosition, postClickDeadzoneRadius))
            {
                MouseClickUtility.LeftClick(pointToClick);
                SetLastMouseClickPosition();
            }
        }

        private void SubscribeGlobal()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
          
            m_Events.KeyPress += HookManager_KeyPress;
            
            m_Events.MouseClick += OnMouseClick;
            m_Events.MouseDown += OnMouseDown;
            m_Events.MouseUp += OnMouseUp;
            m_Events.MouseDragStarted += OnMouseDragStarted;
            m_Events.MouseDragFinished += OnMouseDragFinished;
            m_Events.MouseWheel += HookManager_MouseWheel;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.KeyPress -= HookManager_KeyPress;
            
            m_Events.MouseClick -= OnMouseClick;
            m_Events.MouseDown -= OnMouseDown;
            m_Events.MouseUp -= OnMouseUp;
            m_Events.MouseDragStarted -= OnMouseDragStarted;
            m_Events.MouseDragFinished -= OnMouseDragFinished;
            m_Events.MouseWheel -= HookManager_MouseWheel;

            m_Events.Dispose();
            m_Events = null;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            lastMouseClickWasARightClick = false;
            if (e.Button == MouseButtons.Right)
              lastMouseClickWasARightClick = true;

            if(e.Button == MouseButtons.Left)
                dateTimeLeftMouseClickPressedDown = DateTime.UtcNow;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dateTimeRightMouseClickPressedDown = DateTime.UtcNow;
                SetTimerToggleMouseClickAssistance();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTimeRightMouseClickPressedDown);
                TimerToggleMouseClickAssistance.Dispose();
            }
        }
        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            dateTimeLastMouseScrollWheelInput = DateTime.UtcNow;
        }

        private void SetTimerToggleMouseClickAssistance()
        {
            // Create a timer with a two second interval.
            TimerToggleMouseClickAssistance = new System.Timers.Timer(toggleClickAssistanceMs);
            // Hook up the Elapsed event for the timer. 
            TimerToggleMouseClickAssistance.Elapsed += OnTimedEventTimerToggleMouseClickAssistance;
            TimerToggleMouseClickAssistance.AutoReset = false;
            TimerToggleMouseClickAssistance.Enabled = true;
        }

        private void OnTimedEventTimerToggleMouseClickAssistance(Object source, ElapsedEventArgs e)
        {
            ToggleMouseClickAssistance();
            TimerToggleMouseClickAssistance.Dispose();
        }

        private void ToggleMouseClickAssistance()
        {
            if(mouseClickAssistanceEnabled)
            {
                mouseClickAssistanceEnabled = false;
                PlaySound(Properties.Resources.mouse_assist_off);
            }
            else
            {
                mouseClickAssistanceEnabled = true;
                PlaySound(Properties.Resources.mouse_assist_on);
            }
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            mouseIsBeingDragged = true;
        }

        private void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            mouseIsBeingDragged = false;
            SetLastMouseClickPosition();
        }

        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            dateTimeLastKeyboardInput = DateTime.UtcNow;
        }

        private bool userWasJustTyping()
        {
            var result = false;

            if (DateTimeUtcIsWithinMillisecondsOfNowUtc(dateTimeLastKeyboardInput, noClickAfterTypingMs)) 
            {
                result = true;
                SetLastMouseClickPosition();
            }

            return result;
        }

        private bool userWasJustScrolling()
        {
            var result = false;

            if (DateTimeUtcIsWithinMillisecondsOfNowUtc(dateTimeLastMouseScrollWheelInput, noClickAfterScrollingMs))
            {
                result = true;
                SetLastMouseClickPosition();
            }

            return result;
        }

        private bool userJustClicked()
        {
            var result = false;

            if (DateTimeUtcIsWithinMillisecondsOfNowUtc(dateTimeLeftMouseClickPressedDown, noClickAfterPhysicalMouseClickMs))
            {
                result = true;
                SetLastMouseClickPosition();
            }

            return result;
        }

        private bool DateTimeUtcIsWithinMillisecondsOfNowUtc(DateTime dateTimeUtc, int milliseconds)
        {
            var result = false;

            TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTimeUtc);
            if (timeSpan.TotalMilliseconds < milliseconds)
                result = true;

            return result;
        }

        private void linkLabelAttributionIconPart1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.com/authors/pixel-perfect");
        }

        private void linkLabelAttributionIconPart2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.com/");
        }

        private async void PlaySound(Stream stream)
        {
            using (SoundPlayer player = new SoundPlayer(stream))
            {
                await Task.Run(() => player.Play());
            }
        }

        private void FormMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Unsubscribe();
        }

        private void SetWindowLocationBottomRightAboveIconTray()
        {
            Rectangle primaryScreenWorkingArea = Screen.PrimaryScreen.Bounds;
            this.Location = new Point(primaryScreenWorkingArea.Right - this.Size.Width -13,
                                      primaryScreenWorkingArea.Bottom - (this.Size.Height - 27));
        }

        protected override void WndProc(ref Message m)
        {
            // Catch WM_SYSCOMMAND, SC_MINIMIZE
            if (m.Msg == 0x112 && m.WParam.ToInt32() == 0xf020)
            {
                MinimizeForm();
                return;
            }
            base.WndProc(ref m);
        }

        private void MinimizeForm()
        {
            Hide();
            WindowState = FormWindowState.Minimized;
        }

        private void RestoreForm()
        {
            Show();
            BringToFront();
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreForm();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreForm();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
