using System;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RPG;

namespace Portal_Project
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
        bool gameover = false;
        Sprite hero;
        int heroDir = 0;
        bool portalFlag = false;
        Point portalTarget;
        int drawLast = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Portal Demo";

            //create game object
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);

            //create tilemap
            level = new Level(ref game, 25, 19, 32);
            level.loadTilemap("portals.level");
            level.loadPalette("palette.bmp", 5);

            //load hero
            hero = new Sprite(ref game);
            hero.Image = game.LoadBitmap("hero_sword_walk.png");
            hero.Columns = 9;
            hero.TotalFrames = 9 * 8;
            hero.Size = new Size(96, 96);
            hero.Position = new Point(400 - 48, 300 - 48);
            hero.AnimateWrapMode = Sprite.AnimateWrap.WRAP;
            hero.AnimationRate = 20;

            while (!gameover)
            {
                doUpdate();
            }
            Application.Exit();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
           switch (e.KeyCode)
           {
               case Keys.Escape : gameover = true; break; 
               case Keys.Up:
               case Keys.W : keyState.up = false; break; 
               case Keys.Down:
               case Keys.S : keyState.down = false; break; 
               case Keys.Left:
               case Keys.A : keyState.left = false; break;
               case Keys.Right:
               case Keys.D : keyState.right = false; break;
               case Keys.Space:
                   if (portalFlag) level.GridPos = portalTarget;
                   break;
           }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           switch (e.KeyCode)
           {
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

        private void doUpdate()
        {
            //move the tilemap scroll position
            int steps = 8;
            PointF pos = level.ScrollPos;

            //up key movement
            if (keyState.up)
            {
                if (hero.Y > 300 - 48) hero.Y -= steps;
                else
                {
                    pos.Y -= steps;
                    if (pos.Y <= 0) hero.Y -= steps;
                }

            }
            //down key movement
            else if (keyState.down)
            {
                if (hero.Y < 300 - 48)
                    hero.Y += steps;
                else
                {
                    pos.Y += steps;
                    if (pos.Y >= (127 - 19) * 32) hero.Y += steps;
                }
            }

            //left key movement
            if (keyState.left)
            {
                if (hero.X > 400 - 48) hero.X -= steps;
                else
                {
                    pos.X -= steps;
                    if (pos.X <= 0) hero.X -= steps;
                }
            } 
            //right key movement
            else if (keyState.right)
            {
                if (hero.X < 400 - 48) hero.X += steps;
                else
                {
                    pos.X += steps;
                    if (pos.X >= (127 - 25) * 32) hero.X += steps;
                }
            }

            //update scroller position
            level.ScrollPos = pos;
            level.Update();

            //limit player sprite to the screen boundary
            if (hero.X < -32) hero.X = -32;
            else if (hero.X > 800 - 65) hero.X = 800 - 65;
            if (hero.Y < -48) hero.Y = -48;
            else if (hero.Y > 600 - 81) hero.Y = 600 - 81;

            //orient the player in the right direction
            if (keyState.up && keyState.right) heroDir = 1;
            else if (keyState.right && keyState.down) heroDir = 3;
            else if (keyState.down && keyState.left) heroDir = 5;
            else if (keyState.left && keyState.up) heroDir = 7;
            else if (keyState.up) heroDir = 0;
            else if (keyState.right) heroDir = 2;
            else if (keyState.down) heroDir = 4;
            else if (keyState.left) heroDir = 6;
            else heroDir = -1;

            //get the untimed core frame rate 
            int frameRate = game.FrameRate();

            //drawing code should be limited to 60 fps
            int ticks = Environment.TickCount;
            if (ticks > drawLast + 16)
            {
                drawLast = ticks;

                //draw the tilemap
                level.Draw(0, 0, 800, 600);

                //draw the hero
                int startFrame = heroDir * 9;
                int endFrame = startFrame + 8;
                if (heroDir > -1) hero.Animate(startFrame, endFrame);
                hero.Draw();

                //print da stats
                game.Print(700, 0, frameRate.ToString());
                int y = 0;
                game.Print(0, y, "Scroll " + level.ScrollPos.ToString());
                y += 20;
                game.Print(0, y, "Player " + hero.Position.ToString());
                y += 20;

                Point feet = HeroFeet();

                int tilex = (int)(level.ScrollPos.X + feet.X) / 32;
                int tiley = (int)(level.ScrollPos.Y + feet.Y) / 32;
                Level.tilemapStruct ts = level.getTile(tilex, tiley);
                game.Print(0, y, "Tile " + tilex.ToString() + "," + 
                    tiley.ToString() + " = " + ts.tilenum.ToString());
                y += 20;
                if (ts.collidable)
                {
                    game.Print(0, y, "Collidable");
                    y += 20;
                }
                if (ts.portal)
                {
                    game.Print(0, y, "Portal to " + ts.portalx.ToString() + 
                        "," + ts.portaly.ToString());
                    portalFlag = true;
                    portalTarget = new Point(ts.portalx - feet.X / 32, 
                        ts.portaly - feet.Y / 32);
                    y += 20;
                }
                else
                    portalFlag = false;

                //highlight collision areas around player
                game.Device.DrawRectangle(Pens.Blue, hero.Bounds);
                game.Device.DrawRectangle(Pens.Red, feet.X + 16 - 1, 
                    feet.Y + 16 - 1, 2, 2);
                game.Device.DrawRectangle(Pens.Red, feet.X, feet.Y, 32, 32);

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

        //return bottom center position of hero sprite 
        //where feet are touching ground
        private Point HeroFeet()
        {
            return new Point((int)(hero.X + 32), (int)(hero.Y + 32 + 16));
        }
    }
}
