using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace RPG
{
    class Dialogue
    {
        private Game p_game;
        private Font p_font;
        private PointF p_position;
        private Positions p_corner;
        private PointF p_size;

        public enum Positions
        {
            UpperLeft,
            LowerLeft,
            UpperRight,
            LowerRight
        }

        public Dialogue(ref Game game)
        {
            p_game = game;
            p_corner = Positions.UpperRight;
            p_size = new PointF(360, 260);
            p_font = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        //get/set position in pixels 
        public PointF Position
        {
            get { return p_position; }
            set { p_position = value; }
        }

        public void setCorner(Positions corner)
        {
            p_corner = corner;
        }

        private void Print(int x, int y, string text, Brush color)
        {
            p_game.Device.DrawString(text, p_font, color, x, y);
        }

        public void Draw()
        {
            switch (p_corner)
            {
                case Positions.UpperLeft:
                    p_position = new PointF(10, 10);
                    break;
                case Positions.LowerLeft:
                    p_position = new PointF(10, 600 - p_size.Y - 10);
                    break;
                case Positions.UpperRight:
                    p_position = new PointF(800 - p_size.X - 10, 10);
                    break;
                case Positions.LowerRight:
                    p_position = new PointF(800 - p_size.X - 10,
                        600 - p_size.Y - 10);
                    break;
            }
            Pen pen = new Pen(Color.FromArgb((int)(255 * 0.6), 
                255, 255, 255));
            p_game.Device.FillRectangle(pen.Brush, p_position.X, 
                p_position.Y, p_size.X, p_size.Y);
        }

    }
}
