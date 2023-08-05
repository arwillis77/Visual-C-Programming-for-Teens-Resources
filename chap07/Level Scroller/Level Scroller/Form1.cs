using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Level_Scroller
{
    public partial class Form1 : Form
    {
       public struct tilemapStruct
        {
            public int tilenum;
            public string data1;
            public bool collidable;
        }
        const int COLUMNS = 5;
        private Bitmap bmpTiles;
        private Bitmap bmpSurface;
        private PictureBox pbSurface;
        private Graphics gfxSurface;
        private Font fontArial;
        private tilemapStruct[] tilemap;
        private PointF scrollPos = new PointF(0, 0);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Level Scroller";
            this.Size = new Size(800 + 16, 600 + 38);

            //create tilemap
            tilemap = new tilemapStruct[128 * 128];

            //set up level drawing surface
            bmpSurface = new Bitmap(800, 600);
            pbSurface = new PictureBox();
            pbSurface.Parent = this;
            pbSurface.BackColor = Color.Black;
            pbSurface.Dock = DockStyle.Fill;
            pbSurface.Image = bmpSurface;
            gfxSurface = Graphics.FromImage(bmpSurface);

            //create font
            fontArial = new Font("Arial", 18);

            //load tiles bitmap
            bmpTiles = new Bitmap("palette.bmp");

            //load the tilemap
            loadTilemapFile("level1.level");

            drawTilemap();
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
                    scrollPos.Y -= 1;
                    if (scrollPos.Y < 0) scrollPos.Y = 0;
                    drawTilemap();
                    break;

                case Keys.Down:
                case Keys.S:
                    scrollPos.Y += 1;
                    if (scrollPos.Y > 127 - 19) scrollPos.Y = 127 - 19;
                    drawTilemap();
                    break;

                case Keys.Left:
                case Keys.A:
                    scrollPos.X -= 1;
                    if (scrollPos.X < 0) scrollPos.X = 0;
                    drawTilemap();
                    break;

                case Keys.Right:
                case Keys.D:
                    scrollPos.X += 1;
                    if (scrollPos.X > 127 - 25) scrollPos.X = 127 - 25;
                    drawTilemap();
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            bmpSurface.Dispose();
            pbSurface.Dispose();
            gfxSurface.Dispose();
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
                collidable = Convert.ToBoolean(element.GetElementsByTagName(
                    "collidable")[0].InnerText);

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

        private void drawTilemap()
        {
            int tilenum, sx, sy;

            for (int x = 0; x < 25; x++)
                for (int y = 0; y < 19; y++)
                {
                    sx = (int)scrollPos.X + x;
                    sy = (int)scrollPos.Y + y;
                    tilenum = tilemap[sy * 128 + sx].tilenum;
                    drawTileNumber(x, y, tilenum);
                }

            string text = "Scroll " + scrollPos.ToString();
            gfxSurface.DrawString(text, fontArial, Brushes.White, 10, 10);
        }

        public void drawTileNumber(int x, int y, int tile)
        {
            //draw tile
            int sx = (tile % COLUMNS) * 33;
            int sy = (tile / COLUMNS) * 33;
            Rectangle src = new Rectangle(sx, sy, 32, 32);
            int dx = x * 32;
            int dy = y * 32;
            gfxSurface.DrawImage(bmpTiles, dx, dy, src, GraphicsUnit.Pixel);

            //save changes
            pbSurface.Image = bmpSurface;
        }
    }
}
