/*
 * Visual C# Game Programming for Teens
 * Copyright 2011 by Jonathan S. Harbour
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using RPG;

namespace Sprite_Demo
{
    public partial class Form1 : Form
    {
        Game game;
        bool p_gameOver = false;
        int p_startTime = 0;
        int p_currentTime = 0;
        int frameCount = 0;
        int frameTimer = 0;
        float frameRate = 0;
        int score = 0;
        Sprite dragon;
        Sprite zombie;
        Sprite spider;
        Sprite skeleton;
        Bitmap grass;
        Sprite archer;
        Sprite arrow;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Main();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Game_KeyPressed(e.KeyCode);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Shutdown();
        }

        public bool Game_Init()
        {
            this.Text = "Archery Shooting Game";

            grass = game.LoadBitmap("grass.bmp");

            //load the archer
            archer = new Sprite(ref game);
            archer.Image = game.LoadBitmap("archer_attack.png");
            archer.Size = new Size(96, 96);
            archer.Columns = 10;
            archer.TotalFrames = 80;
            archer.AnimationRate = 20;
            archer.Position = new PointF(360, 500);
            archer.AnimateDirection = Sprite.AnimateDir.NONE;

            //load the arrow
            arrow = new Sprite(ref game);
            arrow.Image = game.LoadBitmap("arrow.png");
            arrow.Size = new Size(32, 32);
            arrow.TotalFrames = 1;
            arrow.Velocity = new PointF(0, -12.0f);
            arrow.Alive = false;

            //load the zombie
            zombie = new Sprite(ref game);
            zombie.Image = game.LoadBitmap("zombie walk.png");
            zombie.Size = new Size(96, 96);
            zombie.Columns = 8;
            zombie.TotalFrames = 64;
            zombie.Position = new PointF(100, 10);
            zombie.Velocity = new PointF(-2.0f, 0);
            zombie.AnimationRate = 10;

            //load the spider
            spider = new Sprite(ref game);
            spider.Image = game.LoadBitmap("redspiderwalking.png");
            spider.Size = new Size(96, 96);
            spider.Columns = 8;
            spider.TotalFrames = 64;
            spider.Position = new PointF(500, 80);
            spider.Velocity = new PointF(3.0f, 0);
            spider.AnimationRate = 20;

            //load the dragon
            dragon = new Sprite(ref game);
            dragon.Image = game.LoadBitmap("dragonflying.png");
            dragon.Size = new Size(128, 128);
            dragon.Columns = 8;
            dragon.TotalFrames = 64;
            dragon.AnimationRate = 20;
            dragon.Position = new PointF(300, 130);
            dragon.Velocity = new PointF(-4.0f, 0);

            //load the skeleton
            skeleton = new Sprite(ref game);
            skeleton.Image = game.LoadBitmap("skeleton_walk.png");
            skeleton.Size = new Size(96, 96);
            skeleton.Columns = 9;
            skeleton.TotalFrames = 72;
            skeleton.Position = new PointF(400, 190);
            skeleton.Velocity = new PointF(5.0f, 0);
            skeleton.AnimationRate = 30;

            return true;
        }

        public void Game_Update(int time)
        {
            if (arrow.Alive)
            {
                //see if arrow hit spider
                if (arrow.IsColliding(ref spider))
                {
                    arrow.Alive = false;
                    score++;
                    spider.X = 800;
                }

                //see if arrow hit dragon
                if (arrow.IsColliding(ref dragon))
                {
                    arrow.Alive = false;
                    score++;
                    dragon.X = 800;
                }

                //see if arrow hit zombie
                if (arrow.IsColliding(ref zombie))
                {
                    arrow.Alive = false;
                    score++;
                    zombie.X = 800;
                }

                //see if arrow hit skeleton
                if (arrow.IsColliding(ref skeleton))
                {
                    arrow.Alive = false;
                    score++;
                    skeleton.X = 800;
                }
            }
        }

        public void Game_Draw()
        {
            int row = 0;

            //draw background
            game.DrawBitmap(ref grass, 0, 0, 800, 600);

            //draw the arrow
            if (arrow.Alive)
            {
                arrow.Y += arrow.Velocity.Y;
                if (arrow.Y < -32)
                    arrow.Alive = false;
                arrow.Draw();
            }

            //draw the archer
            archer.Animate(10, 19);
            if (archer.CurrentFrame == 19)
            {
                archer.AnimateDirection = Sprite.AnimateDir.NONE;
                archer.CurrentFrame = 10;
                arrow.Alive = true;
                arrow.Position = new PointF(
                    archer.X + 32, archer.Y);
            }
            archer.Draw();

            //draw the zombie
            zombie.X += zombie.Velocity.X;
            if (zombie.X < -96) zombie.X = 800;
            row = 6;
            zombie.Animate(row * 8 + 1, row * 8 + 7);
            zombie.Draw();

            //draw the spider
            spider.X += spider.Velocity.X;
            if (spider.X > 800) spider.X = -96;
            row = 2;
            spider.Animate(row * 8 + 1, row * 8 + 7);
            spider.Draw();

            //draw the skeleton
            skeleton.X += skeleton.Velocity.X;
            if (skeleton.X > 800) skeleton.X = -96;
            row = 2;
            skeleton.Animate(row * 9 + 1, row * 9 + 8);
            skeleton.Draw();

            //draw the dragon
            dragon.X += dragon.Velocity.X;
            if (dragon.X < -128) dragon.X = 800;
            row = 6;
            dragon.Animate(row * 8 + 1, row * 8 + 7);
            dragon.Draw();

            game.Print(0, 0, "SCORE " + score.ToString());
        }

        public void Game_End()
        {
            dragon.Image.Dispose();
            dragon = null;
            archer.Image.Dispose();
            archer = null;
            spider.Image.Dispose();
            spider = null;
            zombie.Image.Dispose();
            zombie = null;
            grass = null;
        }

        public void Game_KeyPressed(System.Windows.Forms.Keys key)
        {
            switch (key)
            {
                case Keys.Escape: Shutdown(); break;
                case Keys.Space:
                    if (!arrow.Alive)
                    {
                        archer.AnimateDirection = Sprite.AnimateDir.FORWARD;
                        archer.CurrentFrame = 10;
                    }
                    break;
                case Keys.Right: break;
                case Keys.Down:  break;
                case Keys.Left:  break;
            }
        }

        public void Shutdown()
        {
            p_gameOver = true;
        }

        //*******************************************
        //real time game loop
        //*******************************************
        public void Main()
        {
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);

            //load and initialize game assets
            Game_Init();

            while (!p_gameOver)
            {
                //update timer
                p_currentTime = Environment.TickCount;

                //let gameplay code update 
                Game_Update(p_currentTime - p_startTime);

                //refresh at 60 FPS
                if (p_currentTime > p_startTime + 16)
                {
                    //update timing 
                    p_startTime = p_currentTime;

                    //let gameplay code draw
                    Game_Draw();

                    //give the form some cycles 
                    Application.DoEvents();

                    //let the game object update
                    game.Update();
                }

                frameCount += 1;
                if (p_currentTime > frameTimer + 1000)
                {
                    frameTimer = p_currentTime;
                    frameRate = frameCount;
                    frameCount = 0;
                }
            }

            //free memory and shut down
            Game_End();
            Application.Exit();
        }

    }

}
