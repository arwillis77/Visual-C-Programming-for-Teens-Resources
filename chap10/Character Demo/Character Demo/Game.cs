using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace RPG
{
    public class Game
    {
        private Graphics p_device;
        private Bitmap p_surface;
        private PictureBox p_pb;
        private Form p_frm;
        private Font p_font;
        private bool p_gameOver;

        private Random p_random;
        private int p_count;
        private int p_lastTime;
        private int p_frames;


        public Game(ref Form form, int width, int height)
        {
            Trace.WriteLine("Game class constructor");

            p_device = null;
            p_surface = null;
            p_pb = null;
            p_frm = null;
            p_font = null;
            p_gameOver = false;
            p_random = new Random();
            p_count = 0;
            p_lastTime = 0;
            p_frames = 0;

            //set form properties
            p_frm = form;
            p_frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            p_frm.MaximizeBox = false;
            //adjust size for window border
            p_frm.Size = new Size(width + 6, height + 28);

            //create a picturebox
            p_pb = new PictureBox();
            p_pb.Parent = p_frm;
            //p_pb.Dock = DockStyle.Fill;
            p_pb.Location = new Point(0, 0);
            p_pb.Size = new Size(width, height);
            p_pb.BackColor = Color.Black;

            //create graphics device
            p_surface = new Bitmap(p_frm.Size.Width, p_frm.Size.Height);
            p_pb.Image = p_surface;
            p_device = Graphics.FromImage(p_surface);

            //set the default font
            SetFont("Arial", 18, FontStyle.Regular);
        }

        ~Game()
        {
            Trace.WriteLine("Game class destructor");
            p_device.Dispose();
            p_surface.Dispose();
            p_pb.Dispose();
            p_font.Dispose();
        }

        public Graphics Device
        {
            get { return p_device; }
        }

        public void Update()
        {
            //refresh the drawing surface
            p_pb.Image = p_surface;
        }

        /*
         * font support with several Print variations
         */
        public void SetFont(string name, int size, FontStyle style)
        {
            p_font = new Font(name, size, style, GraphicsUnit.Pixel);
        }

        public void Print(int x, int y, string text, Brush color)
        {
            Device.DrawString(text, p_font, color, (float)x, (float)y);
        }

        public void Print(Point pos, string text, Brush color)
        {
            Print(pos.X, pos.Y, text, color);
        }

        public void Print(int x, int y, string text)
        {
            Print(x, y, text, Brushes.White);
        }

        public void Print(Point pos, string text)
        {
            Print(pos.X, pos.Y, text);
        }

        /*
         * Bitmap support functions
         */
        public Bitmap LoadBitmap(string filename)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(filename);
            }
            catch (Exception ex) { }
            return bmp;
        }

        public void DrawBitmap(ref Bitmap bmp, float x, float y)
        {
            p_device.DrawImageUnscaled(bmp, (int)x, (int)y);
        }

        public void DrawBitmap(ref Bitmap bmp, float x, float y, int width, int height)
        {
            p_device.DrawImageUnscaled(bmp, (int)x, (int)y, width, height);
        }

        public void DrawBitmap(ref Bitmap bmp, Point pos)
        {
            p_device.DrawImageUnscaled(bmp, pos);
        }

        public void DrawBitmap(ref Bitmap bmp, Point pos, Size size)
        {
            p_device.DrawImageUnscaled(bmp, pos.X, pos.Y, size.Width, size.Height);
        }

        public int FrameRate()
        {
            //calculate core frame rate
            int ticks = Environment.TickCount;
            p_count += 1;
            if (ticks > p_lastTime + 1000)
            {
                p_lastTime = ticks;
                p_frames = p_count;
                p_count = 0;
            }
            return p_frames;
        }

public int Random(int max)
{
    return Random(0, max);
}

public int Random(int min, int max)
{
    return p_random.Next(min, max);
}


    }
}
