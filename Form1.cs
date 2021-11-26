using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using SSD1331;

using System.Runtime.InteropServices;
using System.Windows; // Or use whatever point class you like for the implicit cast operator



namespace ScreenCapt
{
    public partial class Form1 : Form
    {
        ScreenCaptureStream stream;
        SSD1331.SSD1331 oled = new SSD1331.SSD1331();
        long t = 0;
        private AutoResetEvent are = new AutoResetEvent(false);
        Rectangle captureArea = Rectangle.Empty;

        //------------------------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------------------------
        private void btn_getReg_Click(object sender, EventArgs e)
        {
            StartWaitingForClickFromOutside();
        }

        //------------------------------------------------------------------------------------
        private void StartWaitingForClickFromOutside()
        {
            are.Reset();
            var ctx = new SynchronizationContext();

            var task = Task.Run(() =>
            {
                while (true)
                {
                    if (are.WaitOne(1)) break;
                    if (MouseButtons == MouseButtons.Left)
                    {
                        ctx.Send(firstCorner, null);
                        break;
                    }
                }
            });
        }

        //------------------------------------------------------------------------------------
        private void StartWaitingForReleaseFromOutside()
        {
            are.Reset();
            var ctx = new SynchronizationContext();

            var task = Task.Run(() =>
            {
                while (true)
                {
                    if (are.WaitOne(1)) break;
                    if (MouseButtons != MouseButtons.Left)
                    {
                        ctx.Send(secondCorner, null);
                        break;
                    }
                }
            });
        }

        //------------------------------------------------------------------------------------
        private void firstCorner(Object State)
        {
            var mPos = CursorPosition.GetCursorPosition();
            captureArea.X = mPos.X;
            captureArea.Y = mPos.Y;
            StartWaitingForReleaseFromOutside();
        }

        //------------------------------------------------------------------------------------
        private void secondCorner(Object State)
        {
            oled.FTnotCH = rbtn_ft.Checked;
            String initResult = oled.initDispl();

            if (lbl_status.InvokeRequired)
            {
                lbl_status.Invoke(new MethodInvoker(delegate { lbl_status.Text = initResult; }));
            }
            
            var mPos = CursorPosition.GetCursorPosition();
            captureArea.Width = Math.Abs(mPos.X - captureArea.X);
            captureArea.Height = Math.Abs(mPos.Y - captureArea.Y);
            if ( mPos.X < captureArea.X)
            {
                captureArea.X = mPos.X;
            }
            if (mPos.Y < captureArea.Y)
            {
                captureArea.Y = mPos.Y;
            }

            //stop stream if exist
            if (stream != null) stream.SignalToStop();
            // create screen capture video source
            stream = new ScreenCaptureStream(captureArea);
            // set NewFrame event handler
            stream.NewFrame += new NewFrameEventHandler(video_NewFrame);
            stream.FrameInterval = 0; //so fast as possible
            // start the video source
            stream.Start();
        }

        //------------------------------------------------------------------------------------
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // get new frame
            pictureBox1.Image = new Bitmap(eventArgs.Frame, 96, 64);
            oled.sendBitmap(new Bitmap(eventArgs.Frame, 96, 64));
            long ct = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long dt = ct - t;
            t = ct;
            float fps = 1000.0f / (float)dt;

            if (lbl_fps.InvokeRequired)
            {
                lbl_fps.Invoke(new MethodInvoker(delegate { lbl_fps.Text = fps.ToString("0.00"); }));
            }
        }
        //------------------------------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null) stream.SignalToStop();
        }
    }

    internal static class CursorPosition
    {
        [StructLayout(LayoutKind.Sequential)]

        //------------------------------------------------------------------------------------
        public struct PointInter
        {
            public int X;
            public int Y;
            public static explicit operator Point(PointInter point) => new Point(point.X, point.Y);
        }


        //------------------------------------------------------------------------------------
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out PointInter lpPoint);


        //------------------------------------------------------------------------------------
        public static Point GetCursorPosition()
        {
            PointInter lpPoint;
            GetCursorPos(out lpPoint);
            return (Point)lpPoint;
        }
    }
}
