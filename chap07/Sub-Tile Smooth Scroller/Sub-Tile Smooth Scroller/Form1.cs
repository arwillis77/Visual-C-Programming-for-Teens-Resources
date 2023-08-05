using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Level_Scroller
{
    public struct tilemapStruct
    {
        public int tilenum;
        public string data1;
        public bool collidable;
    }

    struct keyStates
    {
        public bool up, down, left, right;
    }

    public partial class Form1 : Form
    {
        const int COLUMNS = 5;
        Bitmap bmpSurface;
        PictureBox pbSurface;
        Graphics gfxSurface;
        Font fontArial;
        keyStates keyState;
        Timer timer1;
        tilemapStruct[] tilemap;
        PointF subtile = new PointF(0, 0);
        Bitmap bmpTiles;
        Bitmap bmpScrollBuffer;
        Graphics gfxScrollBuffer;
        PointF scrollPos = new PointF(0, 0);
        PointF oldScrollPos = new PointF(-1, -1);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Sub-Tile Smooth Scroller";
            this.Size = new Size(900, 700);
            fontArial = new Font("Arial", 18);

            //set up level drawing surface
            bmpSurface = new Bitmap(1024, 768);
            pbSurface = new PictureBox();
            pbSurface.Parent = this;
            pbSurface.BackColor = Color.Black;
            pbSurface.Dock = DockStyle.Fill;
            pbSurface.Image = bmpSurface;
            gfxSurface = Graphics.FromImage(bmpSurface);

            //create tilemap
            tilemap = new tilemapStruct[128 * 128];
            bmpTiles = new Bitmap("palette.bmp");
            loadTilemapFile("level1.level");

            //create scroll buffer
            bmpScrollBuffer = new Bitmap(25 * 32 + 64, 19 * 32 + 64);
            gfxScrollBuffer = Graphics.FromImage(bmpScrollBuffer);
            
            //create the timer
            timer1 = new Timer();
            timer1.Interval = 20;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_tick);
        }

        private void timer1_tick(object sender, EventArgs e)
        {
            int steps = 4;
            if (keyState.up)
            {
                scrollPos.Y -= steps;
                if (scrollPos.Y < 0) scrollPos.Y = 0;
            }
            if (keyState.down)
            {
                scrollPos.Y += steps;
                if (scrollPos.Y > (127 - 19) * 32) scrollPos.Y = 
                    (127 - 19) * 32;
            }
            if (keyState.left)
            {
                scrollPos.X -= steps;
                if (scrollPos.X < 0) scrollPos.X = 0;
            }
            if (keyState.right)
            {
                scrollPos.X += steps;
                if (scrollPos.X > (127 - 25) * 32) scrollPos.X = 
                    (127 - 25) * 32;
            }

            //clear the ground
            //note that this is usually not needed when drawing
            //the game level but this example draws the whole buffer
            gfxSurface.Clear(Color.Black);

            //update and draw the tiles
            drawScrollBuffer();

            //print scroll position
            gfxSurface.DrawString("Scroll " + scrollPos.ToString(), 
                fontArial, Brushes.White, 0, 0);
            gfxSurface.DrawString("Sub-tile " + subtile.ToString(), 
                fontArial, Brushes.White, 300, 0);
            
            //draw a rect representing the actual scroll area
            gfxSurface.DrawRectangle(Pens.Blue, 0, 0, 801, 601);
            gfxSurface.DrawRectangle(Pens.Blue, 1, 1, 801, 601);

            //update surface
            pbSurface.Image = bmpSurface;
        }

        public void updateScrollBuffer()
        {
            //fill scroll buffer with tiles
            int tilenum, sx, sy;
            for (int x = 0; x<26; x++)
                for (int y = 0; y < 20; y++)
                {
                    sx = (int)(scrollPos.X / 32) + x;
                    sy = (int)(scrollPos.Y / 32) + y;
                    tilenum = tilemap[sy * 128 + sx].tilenum;
                    drawTileNumber(x, y, tilenum, COLUMNS);
                }
        }

        public void drawTileNumber(int x, int y, int tile, int columns)
        {
            int sx = (tile % columns) * 33;
            int sy = (tile / columns) * 33;
            Rectangle src = new Rectangle(sx, sy, 32, 32);
            int dx = x * 32;
            int dy = y * 32;
            gfxScrollBuffer.DrawImage(bmpTiles, dx, dy, src, 
                GraphicsUnit.Pixel);
        }

        public void drawScrollBuffer()
        {
            //fill scroll buffer only when moving
            if (scrollPos != oldScrollPos)
            {
                updateScrollBuffer();
                oldScrollPos = scrollPos;
            }

            //calculate sub-tile size
            subtile.X = scrollPos.X % 32;
            subtile.Y = scrollPos.Y % 32;

            //create the source rect
            Rectangle source = new Rectangle((int)subtile.X, (int)subtile.Y,
                bmpScrollBuffer.Width, bmpScrollBuffer.Height);

            //draw the scroll viewport
            gfxSurface.DrawImage(bmpScrollBuffer, 1, 1, source, 
                GraphicsUnit.Pixel);
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

        private void loadTilemapFile(string filename)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList nodelist = doc.GetElementsByTagName("tiles");
                foreach (XmlNode node in nodelist)
                {
                    XmlElement element = (XmlElement)node;
                    int index = 0;
                    int value = 0;
                    string data1 = "";
                    bool collidable = false;

                    //read tile index #
                    index = Convert.ToInt32(element.GetElementsByTagName(
                        "tile")[0].InnerText);

                    //read tilenum
                    value = Convert.ToInt32(element.GetElementsByTagName(
                        "value")[0].InnerText);

                    //read data1
                    data1 = Convert.ToString(element.GetElementsByTagName(
                        "data1")[0].InnerText);

                    //read collidable
                    collidable = Convert.ToBoolean(element.
                        GetElementsByTagName("collidable")[0].InnerText);

                    tilemap[index].tilenum = value;
                    tilemap[index].data1 = data1;
                    tilemap[index].collidable = collidable;
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
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
