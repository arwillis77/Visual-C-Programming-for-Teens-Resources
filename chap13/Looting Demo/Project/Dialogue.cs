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
        private Font p_fontTitle;
        private Font p_fontMessage;
        private Font p_fontButton;
        private PointF p_position;
        private Positions p_corner;
        private Size p_size;
        private string p_title;
        private string p_message;
        private Button[] p_buttons;
        private int p_numButtons;
        private int p_selection;
        private Point p_mousePos;
        private MouseButtons p_mouseBtn;
        private MouseButtons p_oldMouseBtn;
        private bool p_visible;

        public enum Positions
        {
            UpperLeft,
            LowerLeft,
            UpperRight,
            LowerRight
        }

        public struct Button
        {
            public string Text;
        }

        public Dialogue(ref Game game)
        {
            p_game = game;
            p_corner = Positions.UpperRight;
            p_size = new Size(360, 280);
            p_title = "Title";
            p_message = "Message Text";
            p_fontTitle = new Font("Arial", 20, FontStyle.Regular, 
                GraphicsUnit.Pixel);
            p_fontMessage = new Font("Arial", 14, FontStyle.Regular, 
                GraphicsUnit.Pixel);
            p_fontButton = new Font("Arial", 12, FontStyle.Regular, 
                GraphicsUnit.Pixel);
            p_numButtons = 10;
            p_buttons = new Button[p_numButtons+1];
            for (int n = 1; n < 11; n++)
                p_buttons[n].Text = "Button " + n.ToString();
            p_selection = 0;
            p_mouseBtn = MouseButtons.None;
            p_oldMouseBtn = p_mouseBtn;
            p_mousePos = new Point(0, 0);
            p_visible = false;
        }
        
        public string Title
        {
            get { return p_title; }
            set { p_title = value; }
        }

        public string Message
        {
            get { return p_message; }
            set { p_message = value; }
        }

        public int NumButtons
        {
            get { return p_numButtons; }
            set { p_numButtons = value; }
        }

        public void setButtonText(int index, string value)
        {
            p_buttons[index].Text = value;
        }

        public string getButtonText(int index)
        {
            return p_buttons[index].Text;
        }

        public Rectangle getButtonRect(int index)
        {
            int i = index - 1;
            Rectangle rect = new Rectangle((int)p_position.X, 
                (int)p_position.Y, 0, 0);
            rect.Width = p_size.Width / 2 - 4;
            rect.Height = (int)(p_size.Height * 0.4 / 5);
            rect.Y += (int)(p_size.Height * 0.6 - 4);
            switch (index)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 9:
                    rect.X += 4;
                    rect.Y += (int)(Math.Floor((double)i / 2) * 
                        rect.Height);
                    break;
                case 2:
                case 4:
                case 6:
                case 8:
                case 10:
                    rect.X += 4 + rect.Width;
                    rect.Y += (int)(Math.Floor((double)i / 2) * 
                        rect.Height);
                    break;
            }
            return rect;
        }

        public int Selection
        {
            get { return p_selection; }
            set { p_selection = value; }
        }

        //get/set position in pixels 
        public PointF Position
        {
            get { return p_position; }
            set { p_position = value; }
        }

        public bool Visible
        {
            get { return p_visible; }
            set { p_visible = value; }
        }

        public void setCorner(Positions corner)
        {
            p_corner = corner;
        }

        private void Print(int x, int y, string text, Brush color)
        {
            p_game.Device.DrawString(text, p_fontTitle, color, x, y);
        }

        public void updateMouse(Point mousePos, MouseButtons mouseBtn)
        {
            p_mousePos = mousePos;
            p_oldMouseBtn = p_mouseBtn;
            p_mouseBtn = mouseBtn;
        }
        
        public void Draw()
        {
            if (!p_visible) return;

            switch (p_corner)
            {
                case Positions.UpperLeft:
                    p_position = new PointF(4, 4);
                    break;
                case Positions.LowerLeft:
                    p_position = new PointF(4, 600 - p_size.Height - 4);
                    break;
                case Positions.UpperRight:
                    p_position = new PointF(800 - p_size.Width - 4, 4);
                    break;
                case Positions.LowerRight:
                    p_position = new PointF(800 - p_size.Width - 4,
                        600 - p_size.Height - 4);
                    break;
            }

            //adjust height to fit buttons snugly
            float height = 180 + ((p_numButtons+1) / 2) * 20;

            //draw background
            Pen pen = new Pen(Color.FromArgb((int)(255 * 0.6), 50, 50, 50));
            p_game.Device.FillRectangle(pen.Brush, p_position.X, 
                p_position.Y, p_size.Width, height);
            p_game.Device.DrawRectangle(Pens.Gold, p_position.X, 
                p_position.Y, p_size.Width, height);

            //draw title
            SizeF size = p_game.Device.MeasureString(p_title, p_fontTitle);
            int tx = (int)(p_position.X + p_size.Width / 2 - size.Width / 2);
            int ty = (int)p_position.Y + 6;
            p_game.Device.DrawString(p_title, p_fontTitle, Brushes.Gold, 
                tx, ty);

            //draw message text
            SizeF layoutArea = new SizeF(p_size.Width, 120);
            int lines = 4;
            int length = p_message.Length;
            size = p_game.Device.MeasureString(p_message, p_fontMessage, 
                layoutArea, null, out length, out lines);
            RectangleF layoutRect = new RectangleF(p_position.X + 4, 
                p_position.Y + 34, size.Width, size.Height);
            p_game.Device.DrawString(p_message, p_fontMessage, 
                Brushes.White, layoutRect);

            //draw the buttons
            for (int n = 1; n < p_numButtons+1; n++)
            {
                Rectangle rect = getButtonRect(n);

                //draw button background
                Color color;
                if (rect.Contains(p_mousePos))
                {
                    //clicked on this button?
                    if (p_mouseBtn == MouseButtons.None && p_oldMouseBtn == MouseButtons.Left)
                        p_selection = n;
                    else
                        p_selection = 0;

                    color = Color.FromArgb(200, 80, 100, 120);
                    p_game.Device.FillRectangle(new Pen(color).Brush, rect);
                }

                //draw button border 
                p_game.Device.DrawRectangle(Pens.Gray, rect);

                //print button label 
                size = p_game.Device.MeasureString(p_buttons[n].Text, 
                    p_fontButton);
                tx = (int)(rect.X + rect.Width / 2 - size.Width / 2);
                ty = rect.Y + 2;
                p_game.Device.DrawString(p_buttons[n].Text, p_fontButton, 
                    Brushes.White, tx, ty);
            }
        }
    }
}
