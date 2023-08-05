using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Level_Scroller
{
    public partial class Form1 : Form
    {
        private Bitmap bmpSurface;
        private PictureBox pbSurface;
        private Graphics gfxSurface;
        private Font fontArial;
        private PointF scrollPos = new PointF(0, 0);

        public struct keyStates
        {
            public bool up, down, left, right;
        }
        private PointF oldScrollPos = new PointF(-1, -1);
        private keyStates keyState;
        private Timer timer1;

        private Bitmap bmpScrollBuffer;
        private Graphics gfxScrollBuffer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Sub-Tile Buffer Demo";
            this.Size = new Size(800 + 16, 600 + 38);
            fontArial = new Font("Arial", 18);

            //set up level drawing surface
            bmpSurface = new Bitmap(800, 600);
            pbSurface = new PictureBox();
            pbSurface.Parent = this;
            pbSurface.BackColor = Color.Black;
            pbSurface.Dock = DockStyle.Fill;
            pbSurface.Image = bmpSurface;
            gfxSurface = Graphics.FromImage(bmpSurface);

            //create scroll buffer
            bmpScrollBuffer = new Bitmap(25 * 32 + 64, 19 * 32 + 64);
            gfxScrollBuffer = Graphics.FromImage(bmpScrollBuffer);
            
            //fill buffer border area
            gfxScrollBuffer.FillRectangle(Brushes.Gray,
                new Rectangle(0, 0, bmpScrollBuffer.Width, bmpScrollBuffer.Height));

            //fill buffer screen area
            gfxScrollBuffer.FillRectangle(Brushes.BlueViolet,
                new Rectangle(32, 32, 25 * 32, 19 * 32));

            for (int y = 0; y < 19; y++)
                for (int x = 0; x < 25; x++)
                    gfxScrollBuffer.DrawRectangle(Pens.White, 32 + x * 32, 32 + y * 32, 32, 32);

            gfxScrollBuffer.DrawString("SCROLL BUFFER BORDER", fontArial, Brushes.White, 0, 0);


            //create the timer
            timer1 = new Timer();
            timer1.Interval = 20;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_tick);
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            if (keyState.down)
            {
                scrollPos.Y -= 4;
                if (scrollPos.Y < -64) scrollPos.Y = -64;
            }
            if (keyState.up)
            {
                scrollPos.Y += 4;
                if (scrollPos.Y > 0) scrollPos.Y = 0;
            }
            if (keyState.right)
            {
                scrollPos.X -= 4;
                if (scrollPos.X < -64) scrollPos.X = -64;
            }
            if (keyState.left)
            {
                scrollPos.X += 4;
                if (scrollPos.X > 0) scrollPos.X = 0;
            }

            //draw scroll buffer
            gfxSurface.DrawImage(bmpScrollBuffer, scrollPos);

            //print scroll position
            gfxSurface.DrawString(scrollPos.ToString(), fontArial, Brushes.White, 650, 0);

            //update surface
            pbSurface.Image = bmpSurface;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    keyState.up = true;
                    break;

                case Keys.Down:
                case Keys.S:
                    keyState.down = true;
                    break;

                case Keys.Left:
                case Keys.A:
                    keyState.left = true;
                    break;

                case Keys.Right:
                case Keys.D:
                    keyState.right = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;
            
                case Keys.Up:
                case Keys.W:
                    keyState.up = false;
                    break;

                case Keys.Down:
                case Keys.S:
                    keyState.down = false;
                    break;

                case Keys.Left:
                case Keys.A:
                    keyState.left = false;
                    break;

                case Keys.Right:
                case Keys.D:
                    keyState.right = false;
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            bmpSurface.Dispose();
            pbSurface.Dispose();
            gfxSurface.Dispose();
            bmpScrollBuffer.Dispose();
            gfxScrollBuffer.Dispose();
            fontArial.Dispose();
            timer1.Dispose();
        }

    }
}
