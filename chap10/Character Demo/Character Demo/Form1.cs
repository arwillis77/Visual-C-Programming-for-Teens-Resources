using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using RPG;

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
        bool gameover = false;
        Character hero;
        bool portalFlag = false;
        Point portalTarget;
        int drawLast = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Character Demo";

            //create game object
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);

            //create tilemap
            level = new Level(ref game, 25, 19, 32);
            level.loadTilemap("sample.level");
            level.loadPalette("palette.bmp", 5);

            //load hero
            hero = new Character(ref game);
            hero.Load("paladin.char");
            hero.Position = new Point(400 - 48, 300 - 48);

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
                case Keys.Escape: gameover = true; break;
                case Keys.Up:
                case Keys.W: keyState.up = false; break;
                case Keys.Down:
                case Keys.S: keyState.down = false; break;
                case Keys.Left:
                case Keys.A: keyState.left = false; break;
                case Keys.Right:
                case Keys.D: keyState.right = false; break;
                case Keys.Space:
                    if (portalFlag) level.GridPos = portalTarget;
                    break;
                case Keys.D1:
                    hero.AnimationState = Character.AnimationStates.Walking;
                    break;
                case Keys.D2:
                    hero.AnimationState = Character.AnimationStates.Attacking;
                    break;
                case Keys.D3:
                    hero.AnimationState = Character.AnimationStates.Dying;
                    break;

            }
        }

        private void doUpdate()
        {
            //move the tilemap scroll position
            int steps = 4;
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
            if (pos.X < 0) pos.X = 0;
            if (pos.Y < 0) pos.Y = 0;
            level.ScrollPos = pos;
            level.Update();

            //limit player sprite to the screen boundary
            if (hero.X < -32) hero.X = -32;
            else if (hero.X > 800 - 65) hero.X = 800 - 65;
            if (hero.Y < -48) hero.Y = -48;
            else if (hero.Y > 600 - 81) hero.Y = 600 - 81;

            //orient the player in the right direction
            if (keyState.up && keyState.right) hero.Direction = 1;
            else if (keyState.right && keyState.down) hero.Direction = 3;
            else if (keyState.down && keyState.left) hero.Direction = 5;
            else if (keyState.left && keyState.up) hero.Direction = 7;
            else if (keyState.up) hero.Direction = 0;
            else if (keyState.right) hero.Direction = 2;
            else if (keyState.down) hero.Direction = 4;
            else if (keyState.left) hero.Direction = 6;
            else hero.Direction = -1;

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
                hero.Draw();

                //print stats
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
                game.Device.DrawRectangle(Pens.Blue, hero.GetSprite.Bounds);
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
