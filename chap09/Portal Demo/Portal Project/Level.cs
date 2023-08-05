using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace RPG
{
    public class Level
    {
        public struct tilemapStruct
        {
            public int tilenum;
            public string data1;
            public string data2;
            public string data3;
            public string data4;
            public bool collidable;
            public bool portal;
            public int portalx;
            public int portaly;
            public string portalfile;
        }

        private Game p_game;
        private Size p_mapSize = new Size(0, 0);
        private Size p_windowSize = new Size(0, 0);
        private int p_tileSize;
        private Bitmap p_bmpTiles;
        private int p_columns;
        private Bitmap p_bmpScrollBuffer;
        private Graphics p_gfxScrollBuffer;
        private tilemapStruct[] p_tilemap;
        private PointF p_scrollPos = new PointF(0, 0);
        private PointF p_subtile = new PointF(0, 0);
        private PointF p_oldScrollPos = new PointF(-1, -1);

        public Level(ref Game game, int width, int height, int tileSize)
        {
            p_game = game;
            p_windowSize = new Size(width, height);
            p_mapSize = new Size(width * tileSize, height * tileSize);
            p_tileSize = tileSize;

            //create scroll buffer
            p_bmpScrollBuffer = new Bitmap(p_mapSize.Width + p_tileSize,
                p_mapSize.Height + p_tileSize);
            p_gfxScrollBuffer = Graphics.FromImage(p_bmpScrollBuffer);

            //create tilemap 
            p_tilemap = new tilemapStruct[128 * 128];
        }

        public tilemapStruct getTile(PointF p)
        {
            return getTile((int)(p.Y * 128 + p.X));
        }

        public tilemapStruct getTile(int pixelx, int pixely)
        {
            return getTile(pixely * 128 + pixelx);
        }

        public tilemapStruct getTile(int index)
        {
            return p_tilemap[index];
        }

        //get/set scroll position by whole tile position
        public Point GridPos
        {
            get
            {
                int x = (int)p_scrollPos.X / p_tileSize;
                int y = (int)p_scrollPos.Y / p_tileSize;
                return new Point(x, y);
            }
            set
            {
                float x = value.X * p_tileSize;
                float y = value.Y * p_tileSize;
                p_scrollPos = new PointF(x, y);
            }
        }

        //get/set scroll position by pixel position 
        public PointF ScrollPos
        {
            get { return p_scrollPos; }
            set { p_scrollPos = value; }
        }


        public bool loadTilemap(string filename)
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
                    tilemapStruct ts;
                    string data;

                    //read data fields from xml 
                    data = element.GetElementsByTagName("tile")[0].
                        InnerText;
                    index = Convert.ToInt32(data);
                    data = element.GetElementsByTagName("value")[0].
                        InnerText;
                    ts.tilenum = Convert.ToInt32(data);
                    data = element.GetElementsByTagName("data1")[0].
                        InnerText;
                    ts.data1 = Convert.ToString(data);
                    data = element.GetElementsByTagName("data2")[0].
                        InnerText;
                    ts.data2 = Convert.ToString(data);
                    data = element.GetElementsByTagName("data3")[0].
                        InnerText;
                    ts.data3 = Convert.ToString(data);
                    data = element.GetElementsByTagName("data4")[0].
                        InnerText;
                    ts.data4 = Convert.ToString(data);
                    data = element.GetElementsByTagName("collidable")[0].
                        InnerText;
                    ts.collidable = Convert.ToBoolean(data);
                    data = element.GetElementsByTagName("portal")[0].
                        InnerText;
                    ts.portal = Convert.ToBoolean(data);
                    data = element.GetElementsByTagName("portalx")[0].
                        InnerText;
                    ts.portalx = Convert.ToInt32(data);
                    data = element.GetElementsByTagName("portaly")[0].
                        InnerText;
                    ts.portaly = Convert.ToInt32(data);
                    data = element.GetElementsByTagName("portalfile")[0].
                        InnerText;
                    ts.portalfile = Convert.ToString(data);

                    //store data in tilemap
                    p_tilemap[index] = ts;
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                return false;
            }
            return true;
        }

        public bool loadPalette(string filename, int columns)
        {
            p_columns = columns;
            try
            {
                p_bmpTiles = new Bitmap(filename);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public void Update()
        {
            //fill the scroll buffer only when moving
            if (p_scrollPos != p_oldScrollPos)
            {
                p_oldScrollPos = p_scrollPos;

                //validate X range
                if (p_scrollPos.X < 0) p_scrollPos.X = 0;
                if (p_scrollPos.X > (127 - p_windowSize.Width) * p_tileSize)
                    p_scrollPos.X = (127 - p_windowSize.Width) * p_tileSize;

                //validate Y range
                if (p_scrollPos.Y < 0) p_scrollPos.Y = 0;
                if (p_scrollPos.Y > (127 - p_windowSize.Height) * p_tileSize)
                    p_scrollPos.Y = (127 - p_windowSize.Height) * p_tileSize;

                //calculate sub-tile size
                p_subtile.X = p_scrollPos.X % p_tileSize;
                p_subtile.Y = p_scrollPos.Y % p_tileSize;

                //fill scroll buffer with tiles
                int tilenum, sx, sy;
                for (int x = 0; x < p_windowSize.Width + 1; x++)
                    for (int y = 0; y < p_windowSize.Height + 1; y++)
                    {
                        sx = (int)p_scrollPos.X / p_tileSize + x;
                        sy = (int)p_scrollPos.Y / p_tileSize + y;
                        tilenum = p_tilemap[sy * 128 + sx].tilenum;
                        drawTileNumber(x, y, tilenum);
                    }
            }
        }

        public void drawTileNumber(int x, int y, int tile)
        {
            int sx = (tile % p_columns) * (p_tileSize + 1);
            int sy = (tile / p_columns) * (p_tileSize + 1);
            Rectangle src = new Rectangle(sx, sy, p_tileSize, p_tileSize);
            int dx = x * p_tileSize;
            int dy = y * p_tileSize;
            p_gfxScrollBuffer.DrawImage(p_bmpTiles, dx, dy, src,
                GraphicsUnit.Pixel);
        }

        public void Draw(Rectangle rect)
        {
            Draw(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void Draw(int width, int height)
        {
            Draw(0, 0, width, height);
        }

        public void Draw(int x, int y, int width, int height)
        {
            Rectangle source = new Rectangle((int)p_subtile.X,
                (int)p_subtile.Y, width, height);
            p_game.Device.DrawImage(p_bmpScrollBuffer, x, y, source,
                GraphicsUnit.Pixel);
        }
    }
}
