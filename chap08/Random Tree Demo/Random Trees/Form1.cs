using System;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace RPG
{
    public partial class Form1 : Form
    {
        public struct keyStates
        {
            public bool up, down, left, right;
        }

        Game game;
        Level level;
        keyStates keyState;
        bool gameover;
        Bitmap treeImage;
        List<Sprite> trees;
        int treesVisible;
        int drawLast;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            gameover = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameover = false;
            treesVisible = 0;
            drawLast = 0;

            this.Text = "Random Tree Demo";

            //create game object
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);

            //create tilemap
            level = new Level(ref game, 25, 19, 32);
            level.loadTilemap("sample.level");
            level.loadPalette("palette.bmp", 5);

            //load trees
            treeImage = game.LoadBitmap("trees64.png");
            trees = new List<Sprite>();
            for (int n = 0; n< 100;n++)
            {
                Sprite tree = new Sprite(ref game);
                tree.Image = treeImage;
                tree.Columns = 4;
                tree.TotalFrames = 32;
                tree.CurrentFrame = game.Random(31);
                tree.Size = new Size(64, 64);
                tree.Position = new PointF(game.Random(1000), 
                    game.Random(1000));
                trees.Add(tree);
            }

            while (!gameover)
            {
                doUpdate();
            }

            Application.Exit();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: gameover = true; break; 
                case Keys.Up:
                case Keys.W: keyState.up = true; break;
                case Keys.Down:
                case Keys.S: keyState.down = true; break;
                case Keys.Left:
                case Keys.A: keyState.left = true; break;
                case Keys.Right:
                case Keys.D: keyState.right = true; break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W: keyState.up = false; break;
                case Keys.Down:
                case Keys.S: keyState.down = false; break;
                case Keys.Left:
                case Keys.A: keyState.left = false; break;
                case Keys.Right:
                case Keys.D: keyState.right = false; break;
            }
        }

        private void drawTrees()
        {
            int sx,sy;
            treesVisible = 0;
            foreach (Sprite tree in trees)
            {
                sx = (int)level.ScrollPos.X;
                sy = (int)level.ScrollPos.Y;
                if (tree.X > sx && tree.X < sx + 24 * 32 && tree.Y > sy && 
                    tree.Y < sy + 18 * 32)
                {
                    int rx = Math.Abs(sx - (int)tree.X);
                    int ry = Math.Abs(sy - (int)tree.Y);
                    tree.Draw(rx, ry);
                    treesVisible += 1;
                }
            }
        }

        private void doUpdate()
        {
            //respond to user input
            int steps = 4;
            PointF pos = level.ScrollPos;
            if (keyState.up) pos.Y -= steps;
            if (keyState.down) pos.Y += steps;
            if (keyState.left) pos.X -= steps;
            if (keyState.right) pos.X += steps;
            level.ScrollPos = pos;

            //refresh level renderer
            level.Update();

            //get the untimed core frame rate 
            int frameRate = game.FrameRate();

            //drawing code should be limited to 60 fps
            int ticks = Environment.TickCount;
            if (ticks > drawLast + 16)
            {
                drawLast = ticks;

                //draw the tilemap
                level.Draw(0, 0, 800, 600);

                //draw the trees in view 
                drawTrees();

                //print da stats
                game.Print(0, 0, "Scroll " + level.ScrollPos.ToString());
                game.Print(250, 0, "Frame rate " + frameRate.ToString());
                game.Print(500, 0, "Visible trees " + treesVisible.
                    ToString() + "/100");

                //refresh window
                game.Update();
                Application.DoEvents();
            }
            else
            {
                //throttle the cpu
                Thread.Sleep(1);
            }
        }

    }
}
