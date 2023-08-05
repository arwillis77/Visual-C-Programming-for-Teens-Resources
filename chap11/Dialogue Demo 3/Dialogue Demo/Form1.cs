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

        Game game;
        Level level;
        keyStates keyState;
        bool gameover = false;
        Character hero;
        Character vendor;
        bool talkFlag = false;
        bool talking = false;
        int drawLast = 0;
        Dialogue dialogue;
        string purchase = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "NPC Dialogue Demo 3";

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

            //load vendor
            vendor = new Character(ref game);
            vendor.Load("vendor.char");
            vendor.Position = new Point(600, 300);

            //create dialogue
            dialogue = new Dialogue(ref game);

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
                case Keys.W : keyState.up = true; break;
                case Keys.Down:
                case Keys.S : keyState.down = true; break;
                case Keys.Left:
                case Keys.A : keyState.left = true; break;
                case Keys.Right:
                case Keys.D : keyState.right = true; break; 
                case Keys.Space : talkFlag = true; break;
            }
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
                case Keys.Space : talkFlag = false; break;
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
                doVendor();
                doDialogue();
                if (purchase != "")
                {
                    game.Print((int)hero.Position.X, (int)hero.Position.Y, purchase, Brushes.White);
                }

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

        private void doVendor()
        {
            float relativeX=0, relativeY=0;
            int talkRadius = 70;
            Pen color;

            //draw the vendor sprite
            if (vendor.X > level.ScrollPos.X &&
                vendor.X < level.ScrollPos.X + 23 * 32 &&
                vendor.Y > level.ScrollPos.Y &&
                vendor.Y < level.ScrollPos.Y + 17 * 32)
            {
                relativeX = Math.Abs(level.ScrollPos.X - vendor.X);
                relativeY = Math.Abs(level.ScrollPos.Y - vendor.Y);
                vendor.GetSprite.Draw((int)relativeX, (int)relativeY);
            }

            //get center of hero sprite
            PointF heroCenter = hero.FootPos;
            heroCenter.X += 16;
            heroCenter.Y += 16;
            game.Device.DrawRectangle(Pens.Red, heroCenter.X - 2, 
                heroCenter.Y - 2, 4, 4);

            //get center of NPC
            PointF vendorCenter = new Point((int)relativeX, (int)relativeY);
            vendorCenter.X += vendor.GetSprite.Width / 2;
            vendorCenter.Y += vendor.GetSprite.Height / 2;
            game.Device.DrawRectangle(Pens.Red, vendorCenter.X - 2, 
                vendorCenter.Y - 2, 4, 4);

            double dist = game.Distance(heroCenter, vendorCenter);

            //get distance to the NPC and draw a line
            if (dist < talkRadius)
                color = new Pen(Brushes.Blue, 2.0f);
            else
                color = new Pen(Brushes.Red, 2.0f);
            game.Device.DrawLine(color, heroCenter, vendorCenter);

            //print distance
            game.Print((int)relativeX, (int)relativeY,
                "D = " + dist.ToString("N0"), Brushes.White);

            //draw circle around vendor to show talk radius
            float spriteSize = vendor.GetSprite.Width / 2;
            float centerx = relativeX + spriteSize;
            float centery = relativeY + spriteSize;
            RectangleF circleRect = new RectangleF(centerx - talkRadius,
                centery - talkRadius, talkRadius * 2, talkRadius * 2);
            game.Device.DrawEllipse(color, circleRect);

            //is playing trying to talk to this vendor?
            if (dist < talkRadius)
            {
                if (talkFlag) talking = true;
            }
            else talking = false;
        }

        private void doDialogue()
        {
            if (!talking) return;

            //prepare the dialogue
            dialogue.Title = "Bartholomu The Cheapskate";
            dialogue.Message = "Greetings visitor. Oh my goodness, you look " +
                "like you've seen a bit of action. I ain't got much, y'know, " +
                "but ye'll be get'n a fair price! Better'n you'll ever get " +
                "over at Nathan's, I tell ye that much!";
            dialogue.setButtonText(1, "2s Rusty Dagger (1-4 dmg)");
            dialogue.setButtonText(2, "3s Wool Clothes (1 AC)");
            dialogue.setButtonText(3, "10s Short Sword (2-8 dmg)");
            dialogue.setButtonText(4, "15s Leather Armor (4 AC)");
            dialogue.setButtonText(5, "25s Long Sword (6-12 dmg)");
            dialogue.setButtonText(6, "50s Chain Armor (10 AC)");
            dialogue.setButtonText(7, "30s Long Bow (4-10 dmg)");
            dialogue.setButtonText(8, "90s Plate Armor (18 AC)");
            dialogue.setButtonText(9, "80s Doom Ring (+2 STR)");
            dialogue.setButtonText(10, "45s Bone Necklace (+2 STA)");

            //reposition dialogue window 
            if (hero.CenterPos.X < 400)
            {
                if (hero.CenterPos.Y < 300)
                    dialogue.setCorner(Dialogue.Positions.LowerRight);
                else
                    dialogue.setCorner(Dialogue.Positions.UpperRight);
            } else {
                if (hero.CenterPos.Y < 300)
                    dialogue.setCorner(Dialogue.Positions.LowerLeft);
                else
                    dialogue.setCorner(Dialogue.Positions.UpperLeft);
            }

            //draw dialogue and look for selection
            dialogue.updateMouse(game.MousePos, game.MouseButton);
            dialogue.Draw();
            if (dialogue.Selection > 0)
            {
                talking = false;
                purchase = "You bought " + dialogue.getButtonText(dialogue.Selection);
                dialogue.Selection = 0;
            }
            else purchase = "";
       }

    }
}
