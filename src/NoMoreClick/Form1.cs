using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace NoMoreClick
{
    public partial class FormMain : Form
    {
        public static Point pointLastMousePosition;
        public static Point pointLastMouseClickPosition;
        private static System.Timers.Timer aTimer;

        public FormMain()
        {
            InitializeComponent();
            SetLastMouseClickPosition();
            SetTimer();
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
                if (UserIdleTimeUtility.GetTotalMillisecondsSinceLastMouseKeyboardInteraction() > 400)
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
            if (!MouseStillNearToWhereMouseWasBefore(pointToClick,pointLastMouseClickPosition,30))
            {
                MouseClickUtility.LeftClick(pointToClick);
                SetLastMouseClickPosition();
            }
        }

        private void linkLabelAttributionIconPart1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.com/authors/pixel-perfect");
        }

        private void linkLabelAttributionIconPart2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.com/");
        }
    }
}
