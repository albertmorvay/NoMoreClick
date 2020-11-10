using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

using Gma.System.MouseKeyHook;

namespace NoMoreClick
{
    public partial class FormMain : Form
    {

        private IKeyboardMouseEvents m_Events;
        public static Point pointLastMousePosition;
        public static Point pointLastMouseClickPosition;
        private static System.Timers.Timer aTimer;
        private bool mouseIsBeingDragged = false;
        private DateTime dateTimeLastKeyboardInput = DateTime.UtcNow;
        private bool lastMouseClickWasARightClick = false;
        private bool mouseClickAssistanceEnabled = true;
        private DateTime dateTimeRightMouseClickPressedDown = DateTime.UtcNow;

        public FormMain()
        {
            InitializeComponent();
            SetLastMouseClickPosition();
            SetTimer();
            SubscribeGlobal();
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(20);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var mouseClickDelayInMilliseconds = 360;
            if (lastMouseClickWasARightClick)
            {
                mouseClickDelayInMilliseconds = 1080;            }
            if (mouseClickAssistanceEnabled
                && UserIdleTimeUtility.GetTotalMillisecondsSinceLastMouseKeyboardInteraction() > mouseClickDelayInMilliseconds 
                && !mouseIsBeingDragged
                && !userWasJustTyping())
            {
                LeftMouseClick(Cursor.Position);
            }
        }

        private void SetLastMouseClickPosition()
        {
            pointLastMouseClickPosition = Cursor.Position;
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
            if (!MouseStillNearToWhereMouseWasBefore(pointToClick,pointLastMouseClickPosition,20))
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

            m_Events.Dispose();
            m_Events = null;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            lastMouseClickWasARightClick = false;
            if (e.Button == MouseButtons.Right)
                lastMouseClickWasARightClick = true;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dateTimeRightMouseClickPressedDown = DateTime.UtcNow;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTimeRightMouseClickPressedDown);
                if (timeSpan.TotalMilliseconds > 800)
                {
                    ToggleMouseClickAssistance();
                }
            }
        }

        private void ToggleMouseClickAssistance()
        {
            if(mouseClickAssistanceEnabled)
            {
                mouseClickAssistanceEnabled = false;
            }
            else
            {
                mouseClickAssistanceEnabled = true;
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

            TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTimeLastKeyboardInput);
            if (timeSpan.TotalMilliseconds < 1000)
            {
                result = true;
                SetLastMouseClickPosition();
            }

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

        private void FormMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Unsubscribe();
        }
    }
}
