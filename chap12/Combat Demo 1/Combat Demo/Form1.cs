using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RPG
{
    public partial class Form1 : Form
    {
        public struct keyStates
        {
            public bool up, down, left, right;
        }

        const int ATTACK_RADIUS = 70;
        const int NUM_MONSTERS = 25;

        Game game;
        Level level;
        keyStates keyState;
        bool gameover = false;
        int drawLast = 0; 
        Character hero;
        bool attackFlag = false;
        bool attacking = false;
        int monstersInRange = 0;
        Character[] monsters;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Combat Demo 1";

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

            //create monster sprites
            monsters = new Character[NUM_MONSTERS];
            for (int n = 1; n < NUM_MONSTERS; n++)
            {
                monsters[n] = new Character(ref game);
                monsters[n].Load("zombie.char");
                monsters[n].Position = new Point(game.Random(800, 2000), 
                    game.Random(0, 1200));
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
                case Keys.Space: attackFlag = true; break;
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
                case Keys.Space: attackFlag = false; break;
            }
        }

        private void doUpdate()
        {
            int frameRate = game.FrameRate();
            int ticks = Environment.TickCount;
            if (ticks > drawLast + 16)
            {
                drawLast = ticks;
                doScrolling();
                doHero();
                doMonsters();

                game.Print(700, 0, frameRate.ToString());
                int y = 0;
                game.Print(0, 0, "Scroll " + level.ScrollPos.ToString());
                game.Print(0, 20, "Player " + hero.Position.ToString());

                //get position under player's feet
                PointF feet = hero.FootPos;
                int tilex = (int)(level.ScrollPos.X + feet.X) / 32;
                int tiley = (int)(level.ScrollPos.Y + feet.Y) / 32;
                Level.tilemapStruct ts = level.getTile(tilex, tiley);
                game.Print(0, 40, "Tile " + tilex.ToString() + "," + 
                    tiley.ToString() + " = " + ts.tilenum.ToString());

                game.Print(0, 60, "Attacking: " + attacking.ToString());

                game.Print(0, 80, "Monsters in range: " + 
                    monstersInRange.ToString());

                game.Update();
                Application.DoEvents();
            }
            else Thread.Sleep(1);
        }

        private void doScrolling()
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
                if (hero.Y < 300 - 48) hero.Y += steps;
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

            //draw the tilemap
            level.Draw(0, 0, 800, 600);
        }

        private void doHero()
        {
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

            //draw the hero
            hero.Draw();
        }

        private void doMonsters()
        {
            PointF relativePos;
            Pen color;
            PointF heroCenter;
            PointF monsterCenter;
            double dist=0;
            float spriteSize;
            Point center;
            RectangleF circleRect;

            //get center of hero sprite
            heroCenter = hero.CenterPos;
            game.Device.DrawRectangle(Pens.Red, heroCenter.X - 2, 
                heroCenter.Y - 2, 4, 4);

            monstersInRange = 0;

            for (int n = 1; n < NUM_MONSTERS; n++)
            {
                //is monster in view?
                if (monsters[n].X > level.ScrollPos.X &&
                    monsters[n].X < level.ScrollPos.X + 23 * 32 &&
                    monsters[n].Y > level.ScrollPos.Y &&
                    monsters[n].Y < level.ScrollPos.Y + 17 * 32)
                {

                    monstersInRange += 1;

                    relativePos = new PointF(
                        Math.Abs(level.ScrollPos.X - monsters[n].X),
                        Math.Abs(level.ScrollPos.Y - monsters[n].Y));

                    //draw the monster sprite
                    monsters[n].GetSprite.Draw((int)relativePos.X, 
                        (int)relativePos.Y);

                    //get center of NPC
                    monsterCenter = relativePos;
                    monsterCenter.X += monsters[n].GetSprite.Width / 2;
                    monsterCenter.Y += monsters[n].GetSprite.Height / 2;
                    game.Device.DrawRectangle(Pens.Red, monsterCenter.X - 2,
                        monsterCenter.Y - 2, 4, 4);

                    //get distance to the NPC
                    dist = hero.CenterDistance(monsterCenter);

                    //draw line to NPCs in view
                    if (dist < ATTACK_RADIUS)
                        color = new Pen(Brushes.Blue, 2.0f);
                    else
                        color = new Pen(Brushes.Red, 2.0f);
                    
                    game.Device.DrawLine(color, heroCenter, monsterCenter);

                    //print distance
                    game.Print((int)relativePos.X, (int)relativePos.Y, 
                        "D = " + dist.ToString("N0"), Brushes.White);

                    //draw circle around monster to show attack radius
                    spriteSize = monsters[n].GetSprite.Width / 2;
                    center = new Point(
                        (int)(relativePos.X + spriteSize),
                        (int)(relativePos.Y + spriteSize));
                    circleRect = new RectangleF(
                        center.X - ATTACK_RADIUS, center.Y - ATTACK_RADIUS,
                        ATTACK_RADIUS * 2, ATTACK_RADIUS * 2);
                    game.Device.DrawEllipse(color, circleRect);
                }

                //is player trying to attack to this monster?
                if (dist < ATTACK_RADIUS)
                {
                    if (attackFlag) attacking = true;
                }
                else attacking = false;
            }
        }

    }
}
