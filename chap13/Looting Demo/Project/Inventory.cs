using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RPG
{
    public class Inventory
    {

        public struct Button
        {
            public Rectangle rect;
            public string text;
            public Bitmap image;
            public string imagefile;
        }

        private int BTN_HEAD;
        private int BTN_CHEST;
        private int BTN_LEGS;
        private int BTN_RTHAND;
        private int BTN_LTHAND;
        private int BTN_RTFINGER;
        private int BTN_LTFINGER;

        private Game p_game;
        private Font p_font;
        private Font p_font2;
        private PointF p_position;
        private Button[] p_buttons;
        private int p_selection;
        private int p_sourceIndex;
        private int p_targetIndex;
        private Point p_mousePos;
        private MouseButtons p_mouseBtn;
        private int p_lastButton;
        private MouseButtons p_oldMouseBtn;
        private bool p_visible;
        private Bitmap p_bg;
        private Item[] p_inventory;


        public Inventory(ref Game game, Point pos)
        {
            p_game = game;
            p_position = pos;
            p_bg = game.LoadBitmap("char_bg3.png");
            p_font = new Font("Arial", 24, FontStyle.Bold, GraphicsUnit.Pixel);
            p_font2 = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.Pixel);
            p_selection = 0;
            p_mouseBtn = MouseButtons.None;
            p_oldMouseBtn = p_mouseBtn;
            p_mousePos = new Point(0, 0);
            p_visible = false;
            p_lastButton = -1;
            CreateInventory();
            CreateButtons();
        }

        public void CreateInventory()
        {
            p_inventory = new Item[30];
            for (int n = 0; n < p_inventory.Length - 1; n++)
            {
                p_inventory[n] = new Item();
                p_inventory[n].Name = "";
            }
        }

        public bool AddItem(Item itm)
        {
            for (int n = 0; n < 20; n++)
            {
                if (p_inventory[n].Name == "")
                {
                    CopyInventoryItem(ref itm, ref p_inventory[n]);
                    return true;
                }
            }
            return false;
        }

        public void CreateButtons()
        {
            int rx=0, ry=0, rw=0, rh=0, index=0;

            //create inventory buttons
            p_buttons = new Button[30];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    rx = (int)p_position.X + 6 + x * 76;
                    ry = (int)p_position.Y + 278 + y * 76;
                    rw = 64;
                    rh = 64;
                    p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
                    p_buttons[index].text = index.ToString();
                    index += 1;
                }
            }

            //create left gear buttons
            rx = (int)p_position.X + 6;
            ry = (int)p_position.Y + 22;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "cape";
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "weapon 1";
            BTN_RTHAND = index;
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "ring";
            index += 1;

            //create center gear buttons
            rx = (int)p_position.X + 82;
            ry = (int)p_position.Y + 6;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "helm";
            BTN_HEAD = index;
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "chest";
            BTN_CHEST = index;
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "boots";
            BTN_LEGS = index;
            index += 1;

            //create right gear buttons
            rx = (int)p_position.X + 158;
            ry = (int)p_position.Y + 22;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "amulet";
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "weapon 2";
            BTN_LTHAND = index;
            index += 1;

            ry += 76;
            p_buttons[index].rect = new Rectangle(rx, ry, rw, rh);
            p_buttons[index].text = "gauntlets";
            index += 1;
        }

        public bool Visible
        {
            get { return p_visible; }
            set { p_visible = value; }
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

        public int LastButton
        {
            get { return p_lastButton; }
            set { p_lastButton = value; }
        }

        private void Print(int x, int y, string text)
        {
            Print(x, y, text, Brushes.White);
        }

        private void Print(int x, int y, string text, Brush color)
        {
            p_game.Device.DrawString(text, p_font, color, x, y);
        }

        //print text right-justified from top-right x,y
        private void PrintRight(int x, int y, string text, Brush color)
        {
            SizeF rsize = p_game.Device.MeasureString(text, p_font);
            p_game.Device.DrawString(text, p_font, color, x - rsize.Width, y);
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
            int tx, ty;

            //draw background 
            p_game.DrawBitmap(ref p_bg, p_position.X, p_position.Y);
            p_game.Device.DrawRectangle(new Pen(Color.Gold, 2), p_position.X - 1, p_position.Y - 1, p_bg.Width + 2, p_bg.Height + 2);

            //print player stats
            int x = 400;
            int y = (int)p_position.Y;
            int ht = 26;
            Print(x, y, p_game.Hero.Name, Brushes.Gold);
            y += ht + 8;
            PrintRight(660, y, p_game.Hero.Level.ToString(), Brushes.LightGreen);
            Print(x, y, "Level", Brushes.LightGreen);
            y += ht;
            PrintRight(660, y, p_game.Hero.Experience.ToString(), Brushes.LightBlue);
            Print(x, y, "Experience", Brushes.LightBlue);
            y += ht + 8;
            PrintRight(660, y, p_game.Hero.STR.ToString(), Brushes.LightGreen);
            Print(x, y, "Strength", Brushes.LightGreen);
            y += ht;
            PrintRight(660, y, p_game.Hero.DEX.ToString(), Brushes.LightBlue);
            Print(x, y, "Dexterity", Brushes.LightBlue);
            y += ht;
            PrintRight(660, y, p_game.Hero.STA.ToString(), Brushes.LightGreen);
            Print(x, y, "Stamina", Brushes.LightGreen);
            y += ht;
            PrintRight(660, y, p_game.Hero.INT.ToString(), Brushes.LightBlue);
            Print(x, y, "Intellect", Brushes.LightBlue);
            y += ht;
            PrintRight(660, y, p_game.Hero.CHA.ToString(), Brushes.LightGreen);
            Print(x, y, "Charisma", Brushes.LightGreen);
            y += ht + 8;
            PrintRight(660, y, p_game.Hero.Gold.ToString(), Brushes.LightGoldenrodYellow);
            Print(x, y, "Gold", Brushes.LightGoldenrodYellow);
            y += ht;

            //draw the buttons
            for (int n = 0; n < p_buttons.Length - 1; n++)
            {
                Rectangle rect = p_buttons[n].rect;

                //draw button border 
                p_game.Device.DrawRectangle(Pens.Gray, rect);

                //print button label 
                if (p_buttons[n].image == null)
                {
                    SizeF rsize = p_game.Device.MeasureString(p_buttons[n].text, p_font2);
                    tx = (int)(rect.X + rect.Width / 2 - rsize.Width / 2);
                    ty = rect.Y + 2;
                    p_game.Device.DrawString(p_buttons[n].text, p_font2, Brushes.DarkGray, tx, ty);
                }
            }

            //check for (button click
            for (int n = 0; n < p_buttons.Length - 1; n++)
            {
                Rectangle rect = p_buttons[n].rect;
                if (rect.Contains(p_mousePos))
                {
                    if (p_mouseBtn == MouseButtons.None && p_oldMouseBtn == MouseButtons.Left)
                    {
                        p_selection = n;
                        if (p_sourceIndex == -1)
                            p_sourceIndex = p_selection;
                        else if (p_targetIndex == -1)
                            p_targetIndex = p_selection;
                        else
                        {
                            p_sourceIndex = p_selection;
                            p_targetIndex = -1;
                        }
                        break;
                    }
                    p_game.Device.DrawRectangle(new Pen(Color.Red, 2.0f), rect);
                }
            }

            string text = "Source: " + p_sourceIndex.ToString() + ", Target: " + p_targetIndex.ToString();
            if (p_sourceIndex == p_targetIndex) 
                text += " : same item";
            
            if (p_selection != -1 && p_sourceIndex != -1 && p_targetIndex != -1)
            {
                if (p_buttons[p_sourceIndex].image == null)
                    text += " : source is empty";
                else if (p_buttons[p_targetIndex].image != null)
                    text += " : target is in use";
                else
                {
                    text += " : good to move!";
                    MoveInventoryItem(p_sourceIndex, p_targetIndex);
                    p_selection = -1;
                }
            }
            p_game.Device.DrawString(text, p_font2, Brushes.White, p_position.X + 20, p_position.Y + 255);

            //draw equipment
            for (int n = 0; n < p_inventory.Length - 1; n++)
            {
                DrawInventoryItem( n );
            }
        }

        private void DrawInventoryItem(int index)
        {
            string filename = p_inventory[index].InvImageFilename;
            if (filename.Length > 0)
            {
                //try to avoid repeatedly loading image
                if (p_buttons[index].image == null || p_buttons[index].imagefile != filename)
                {
                    p_buttons[index].imagefile = filename;
                    p_buttons[index].image = p_game.LoadBitmap(filename);
                }
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF srcRect = p_buttons[index].image.GetBounds( ref unit );
                RectangleF dstRect = p_buttons[index].rect;
                p_game.Device.DrawImage(p_buttons[index].image, dstRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        private void MoveInventoryItem(int source, int dest)
        {
            CopyInventoryItem(ref p_inventory[source], ref p_inventory[dest]);
            p_inventory[source].Name = "";
            p_inventory[source].InvImageFilename = "";
            p_buttons[source].imagefile = "";
            p_buttons[source].image = null;
        }

        public void CopyInventoryItem(int source, int dest)
        {
            CopyInventoryItem(ref p_inventory[source], ref p_inventory[dest]);
        }

        public void CopyInventoryItem(ref Item srcItem, ref Item dstItem)
        {
            dstItem.Name = srcItem.Name;
            dstItem.Description = srcItem.Description;
            dstItem.AttackDie = srcItem.AttackDie;
            dstItem.AttackNumDice = srcItem.AttackNumDice;
            dstItem.Category = srcItem.Category;
            dstItem.Defense = srcItem.Defense;
            dstItem.DropImageFilename = srcItem.DropImageFilename;
            dstItem.InvImageFilename = srcItem.InvImageFilename;
            dstItem.Value = srcItem.Value;
            dstItem.Weight = srcItem.Weight;
            dstItem.STR = srcItem.STR;
            dstItem.DEX = srcItem.DEX;
            dstItem.CHA = srcItem.CHA;
            dstItem.STA = srcItem.STA;
            dstItem.INT = srcItem.INT;
        }
   
    }
}
