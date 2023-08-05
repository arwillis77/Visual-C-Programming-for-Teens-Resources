using System;
using System.Drawing;

namespace RPG
{
    public class Sprite
    {
        public enum AnimateDir
        {
            NONE = 0,
            FORWARD = 1,
            BACKWARD = -1
        }
        public enum AnimateWrap
        {
            WRAP = 0,
            BOUNCE = 1
        }

        private Game p_game;
        private PointF p_position;
        private PointF p_velocity;
        private Size p_size;
        private Bitmap p_bitmap;
        private bool p_alive;
        private int p_columns;
        private int p_totalFrames;
        private int p_currentFrame;
        private AnimateDir p_animationDir;
        private AnimateWrap p_animationWrap;
        private int p_lastTime;
        private int p_animationRate;


        public Sprite(ref Game game)
        {
            p_game = game;
            p_position = new PointF(0, 0);
            p_velocity = new PointF(0, 0);
            p_size = new Size(0, 0);
            p_bitmap = null;
            p_alive = true;
            p_columns = 1;
            p_totalFrames = 1;
            p_currentFrame = 0;
            p_animationDir = AnimateDir.FORWARD;
            p_animationWrap = AnimateWrap.WRAP;
            p_lastTime = 0;
            p_animationRate = 30;
        }

        public bool Alive
        {
            get { return p_alive; }
            set { p_alive = value; }
        }

        public Bitmap Image
        {
            get { return p_bitmap; }
            set { p_bitmap = value; }
        }

        public PointF Position
        {
            get { return p_position; }
            set { p_position = value; }
        }

        public PointF Velocity
        {
            get { return p_velocity; }
            set { p_velocity = value; }
        }

        public float X
        {
            get { return p_position.X; }
            set { p_position.X = value; }
        }

        public float Y
        {
            get { return p_position.Y; }
            set { p_position.Y = value; }
        }

        public Size Size
        {
            get { return p_size; }
            set { p_size = value; }
        }

        public int Width
        {
            get { return p_size.Width; }
            set { p_size.Width = value; }
        }

        public int Height
        {
            get { return p_size.Height; }
            set { p_size.Height = value; }
        }

        public int Columns
        {
            get { return p_columns; }
            set { p_columns = value; }
        }

        public int TotalFrames
        {
            get { return p_totalFrames; }
            set { p_totalFrames = value; }
        }

        public int CurrentFrame
        {
            get { return p_currentFrame; }
            set { p_currentFrame = value; }
        }

        public AnimateDir AnimateDirection
        {
            get { return p_animationDir; }
            set { p_animationDir = value; }
        }

        public AnimateWrap AnimateWrapMode
        {
            get { return p_animationWrap; }
            set { p_animationWrap = value; }
        }

        public int AnimationRate
        {
            get { return 1000 / p_animationRate; }
            set
            {
                if (value == 0) value = 1;
                p_animationRate = 1000 / value;
            }
        }

        public void Animate()
        {
            Animate(0, p_totalFrames - 1);
        }

        public void Animate(int startFrame, int endFrame)
        {
            //do we even need to animate?
            if (p_totalFrames <= 0) return;

            //check animation timing
            int time = Environment.TickCount;
            if (time > p_lastTime + p_animationRate)
            {
                p_lastTime = time;

                //go to next frame
                p_currentFrame += (int)p_animationDir;
                switch (p_animationWrap)
                {
                    case AnimateWrap.WRAP:
                        if (p_currentFrame < startFrame)
                            p_currentFrame = endFrame;
                        else if (p_currentFrame > endFrame)
                            p_currentFrame = startFrame;
                        break;

                    case AnimateWrap.BOUNCE:
                        if (p_currentFrame < startFrame)
                        {
                            p_currentFrame = startFrame;
                            p_animationDir = AnimateDir.FORWARD;
                        }
                        else if (p_currentFrame > endFrame)
                        {
                            p_currentFrame = endFrame;
                            p_animationDir = AnimateDir.BACKWARD;
                        }
                        break;
                }
            }
        }

        public void Draw()
        {
            Rectangle frame = new Rectangle();
            frame.X = (p_currentFrame % p_columns) * p_size.Width;
            frame.Y = (p_currentFrame / p_columns) * p_size.Height;
            frame.Width = p_size.Width;
            frame.Height = p_size.Height;
            p_game.Device.DrawImage(p_bitmap, Bounds, frame, GraphicsUnit.Pixel);
        }

        //returns bounding rectangle around sprite 
        public Rectangle Bounds
        {
            get {
                Rectangle rect = new Rectangle(
                    (int)p_position.X, (int)p_position.Y, 
                    p_size.Width, p_size.Height);
                return rect;
            }
        }

        public bool IsColliding(ref Sprite other)
        {
            //test for bounding rectangle collision
            bool collision = Bounds.IntersectsWith(other.Bounds);
            return collision;
        }

    }
}
