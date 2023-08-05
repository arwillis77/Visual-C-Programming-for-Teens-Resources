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
        private bool p_gameOver = false;
        private int p_startTime = 0;
        private int p_currentTime = 0;
        public Game game;
        public Bitmap dragonImage;
        public Sprite dragonSprite;
        public Bitmap grass;
        public int frameCount = 0;
        public int frameTimer = 0;
        public float frameRate = 0;
        public PointF velocity;
        public int direction = 2;

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
            this.Text = "Sprite Drawing Demo";
            grass = game.LoadBitmap("grass.bmp");
            dragonImage = game.LoadBitmap("dragon.png");
            dragonSprite = new Sprite(ref game);
            dragonSprite.Image = dragonImage;
            dragonSprite.Width = 256;
            dragonSprite.Height = 256;
            dragonSprite.Columns = 8;
            dragonSprite.TotalFrames = 64;
            dragonSprite.AnimationRate = 20;
            dragonSprite.X = 250;
            dragonSprite.Y = 150;
            return true;
        }

        public void Game_Update(int time)
        {
        }

        public void Game_Draw()
        {
            //draw background
            game.DrawBitmap(ref grass, 0, 0, 800, 600);

            //move the dragon sprite
            switch (direction)
            {
                case 0: velocity = new Point(0, -1); break;
                case 2: velocity = new Point(1, 0); break;
                case 4: velocity = new Point(0, 1); break;
                case 6: velocity = new Point(-1, 0); break; 
        }
            dragonSprite.X += velocity.X;
            dragonSprite.Y += velocity.Y;

            //animate and draw dragon sprite
            dragonSprite.Animate(direction * 8 + 1, direction * 8 + 7);
            dragonSprite.Draw();

            game.Print(0, 0, "Press Arrow Keys to change direction");
        }

        public void Game_End()
        {
            dragonImage = null;
            dragonSprite = null;
            grass = null;
        }

        public void Game_KeyPressed(System.Windows.Forms.Keys key)
        {
            switch (key)
            {
                case Keys.Escape: Shutdown(); break;
                case Keys.Up: direction = 0; break;
                case Keys.Right: direction = 2; break;
                case Keys.Down: direction = 4; break;
                case Keys.Left: direction = 6; break;
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
