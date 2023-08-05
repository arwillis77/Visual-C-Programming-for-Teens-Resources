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

        public struct DrawableItem
        {
            public Item item;
            public Sprite sprite;
        }

        Game game;
        int drawLast = 0;
        Level level;
        keyStates keyState;
        bool gameover = false;
        Dialogue dialogue;
        bool lootFlag = false;
        List<DrawableItem> treasure;
        int lootTarget=0;
        Items items;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Looting Demo";

            //create game object
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);

            //create tilemap
            level = new Level(ref game, 25, 19, 32);
            level.loadTilemap("sample.level");
            level.loadPalette("palette.bmp", 5);

            //load items
            items = new Items();
            if (!items.Load("items.item"))
            {
                MessageBox.Show("Error loading file items.item");
                Application.Exit();
            }

            //load hero
            game.Hero = new Player(ref game);
            game.Hero.Load("paladin.char");
            game.Hero.Position = new Point(400 - 48, 300 - 48);
            game.Hero.AnimationState = Character.AnimationStates.Standing;

            //create inventory
            game.Inven = new Inventory(ref game, new Point((800-532)/2, 50));
            game.Inven.AddItem(items.getItem("Iron Chainmail"));
            game.Inven.AddItem(items.getItem("Long Sword"));
            game.Inven.AddItem(items.getItem("Small Shield"));
            game.Inven.AddItem(items.getItem("Cape of Stamina"));

            //create treasure drop list
            treasure = new List<DrawableItem>();
            lootFlag = false;

            //create drops of loot based on monsters
            for (int n=1; n<21; n++)
            {
                Character monster = new Character(ref game);
                monster.Load("skeleton sword shield.char");
                monster.Position = new Point(game.Random(100,1200), game.Random(100,1200));
                monster.AnimationState = Character.AnimationStates.Dead;
                monster.Alive = false;

                //add some loot
                DropLoot(ref monster);
            }

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
                case Keys.Escape: gameover = true; break; 
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
                case Keys.Up:
                case Keys.W: keyState.up = false; break;
                case Keys.Down:
                case Keys.S: keyState.down = false; break;
                case Keys.Left:
                case Keys.A: keyState.left = false; break;
                case Keys.Right:
                case Keys.D: keyState.right = false; break;
                case Keys.Space: lootFlag = true; break;
                case Keys.I:
                    game.Inven.Visible = !game.Inven.Visible;
                    break;
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
                doTreasure();
                doHero();
                doDialogue();
                doInventory();

                lootFlag = false;

                game.Print(280, 580, "Controls: (Space) Loot, (I) Inventory");

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
                if (game.Hero.Y > 300 - 48) game.Hero.Y -= steps;
                else
                {
                    pos.Y -= steps;
                    if (pos.Y <= 0) game.Hero.Y -= steps;
                }

            }
            //down key movement
            else if (keyState.down)
            {
                if (game.Hero.Y < 300 - 48) game.Hero.Y += steps;
                else
                {
                    pos.Y += steps;
                    if (pos.Y >= (127 - 19) * 32) game.Hero.Y += steps;
                }
            }

            //left key movement
            if (keyState.left)
            {
                if (game.Hero.X > 400 - 48) game.Hero.X -= steps;
                else
                {
                    pos.X -= steps;
                    if (pos.X <= 0) game.Hero.X -= steps;
                }
            }

            //right key movement
            else if (keyState.right)
            {
                if (game.Hero.X < 400 - 48) game.Hero.X += steps;
                else
                {
                    pos.X += steps;
                    if (pos.X >= (127 - 25) * 32) game.Hero.X += steps;
                }
            }

            //draw the tilemap
            level.ScrollPos = pos;
            level.Update();
            level.Draw(0, 0, 800, 600);
        }

        private void doHero()
        {
            //limit player sprite to the screen boundary
            if (game.Hero.X < -32) game.Hero.X = -32;
            else if (game.Hero.X > 800 - 65) game.Hero.X = 800 - 65;

            if (game.Hero.Y < -48) game.Hero.Y = -48;
            else if (game.Hero.Y > 600 - 81) game.Hero.Y = 600 - 81;

            //orient the player in the right direction
            if (keyState.up && keyState.right) 
            {
                game.Hero.Direction = 1;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.right && keyState.down) 
            {
                game.Hero.Direction = 3;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.down && keyState.left) 
            {
                game.Hero.Direction = 5;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.left && keyState.up) 
            {
                game.Hero.Direction = 7;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.up) 
            {
                game.Hero.Direction = 0;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.right) 
            {
                game.Hero.Direction = 2;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.down) 
            {
                game.Hero.Direction = 4;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (keyState.left)
            {
                game.Hero.Direction = 6;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else
                game.Hero.AnimationState = Character.AnimationStates.Standing;

            //draw the hero
            game.Hero.Draw();
        }

        private void doTreasure()
        {
            PointF relativePos = new PointF(0,0);
            const int lootRadius = 40;
            PointF heroCenter = game.Hero.CenterPos;
            PointF itemCenter = new PointF(0,0);
            double dist;

            foreach (DrawableItem it in treasure)
            {
                //is item in view?
                if (it.sprite.X > level.ScrollPos.X - 64 
                    && it.sprite.X < level.ScrollPos.X + 23 * 32 + 64 
                    && it.sprite.Y > level.ScrollPos.Y - 64 
                    && it.sprite.Y < level.ScrollPos.Y + 17 * 32 + 64)
                {
                    //get relative position of item on screen
                    relativePos.X = it.sprite.X - level.ScrollPos.X;
                    relativePos.Y = it.sprite.Y - level.ScrollPos.Y;

                    //get center of item
                    itemCenter = relativePos;
                    itemCenter.X += it.sprite.Width / 2;
                    itemCenter.Y += it.sprite.Height / 2;

                    //get distance to the item
                    dist = game.Hero.CenterDistance(itemCenter);

                    //is player trying to pick up this loot?
                    if (dist < lootRadius)
                    {
                        game.Device.DrawEllipse(new Pen(Color.Magenta, 2), 
                            itemCenter.X - it.sprite.Width / 2,
                            itemCenter.Y - it.sprite.Height / 2,
                            it.sprite.Width, it.sprite.Height);

                        if (lootFlag)
                        {
                            //collect gold or item
                            if (it.item.Name == "gold" && it.item.Value > 0)
                            {
                                game.Hero.Gold += (int)it.item.Value;
                                treasure.Remove(it);
                                showDialogue("LOOT", it.item.Value.ToString() + " GOLD", "OK");
                            }
                            else
                            {
                                if (game.Inven.AddItem(it.item))
                                {
                                    treasure.Remove(it);
                                    showDialogue("LOOT", it.item.Summary, "OK");
                                }
                                else
                                    showDialogue("OVERLOADED!", "You are overloaded with too much stuff!", "OK");
                            }

                            //wait for user 
                            if (dialogue.Selection == 1)
                            {
                                lootFlag = false;
                                dialogue.Selection = 0;
                            }
                            break;
                        }
                    }

                    //draw the monster sprite
                    it.sprite.Draw((int)relativePos.X, (int)relativePos.Y);
                }
            }
        }

        private void showDialogue(string title, string message,
            string button1)
        {
            dialogue.Title = title;
            dialogue.Message = message;
            dialogue.NumButtons = 1;
            dialogue.setButtonText(1, button1);
            dialogue.Visible = true;
        }

        private void showDialogue(string title, string message, 
            string button1, string button2)
        {
            dialogue.Title = title;
            dialogue.Message = message;
            dialogue.NumButtons = 2;
            dialogue.setButtonText(1, button1);
            dialogue.setButtonText(2, button2);
            dialogue.Visible = true;
        }

        private void doDialogue()
        {
            if (game.Hero.CenterPos.Y < 300)
                dialogue.setCorner(Dialogue.Positions.LowerRight);
            else
                dialogue.setCorner(Dialogue.Positions.UpperRight);

            dialogue.updateMouse(game.MousePos, game.MouseButton);
            dialogue.Draw();

            if (dialogue.Selection > 0)
            {
                dialogue.Visible = false;
                dialogue.Selection = 0;
            }
        }

       private void doInventory()
       {
           if (!game.Inven.Visible) return;
           game.Inven.updateMouse(game.MousePos, game.MouseButton);
           game.Inven.Draw();
       }

        //drop loot specified in the monster's character data file
        public void DropLoot(ref Character srcMonster)
        {
            int count = 0;
            int rad = 64;

            //any gold to drop?
            Item itm = new Item();
            int gold = game.Random(srcMonster.DropGoldMin, srcMonster.DropGoldMax);
            itm.Name = "gold";
            itm.DropImageFilename = "gold.png";
            itm.InvImageFilename = "gold.png";
            itm.Value = gold;
            Point p = new Point(0, 0);
            p.X = (int)srcMonster.X + game.Random(rad) - rad / 2;
            p.Y = (int)srcMonster.Y + game.Random(rad) - rad / 2;
            DropTreasureItem(ref itm, p.X, p.Y);
            
            //any items to drop?
            if (srcMonster.DropNum1 > 0 && srcMonster.DropItem1 != "")
            {
                count = game.Random(1, srcMonster.DropNum1);
                for (int n = 1; n < count; n++)
                {
                    //25% chance for drop
                    if (game.Random(100) < 25)
                    {
                        itm = items.getItem(srcMonster.DropItem1);
                        p.X = (int)srcMonster.X + game.Random(rad) - rad / 2;
                        p.Y = (int)srcMonster.Y + game.Random(rad) - rad / 2;
                        DropTreasureItem(ref itm, p.X, p.Y);
                    }
                }
            }
            if (srcMonster.DropNum2 > 0 && srcMonster.DropItem2 != "")
            {
                count = game.Random(1, srcMonster.DropNum2);
                for (int n = 1; n < count; n++)
                {
                    //25% chance for drop
                    if (game.Random(100) < 25)
                    {
                        itm = items.getItem(srcMonster.DropItem2);
                        p.X = (int)srcMonster.X + game.Random(rad) - rad / 2;
                        p.Y = (int)srcMonster.Y + game.Random(rad) - rad / 2;
                        DropTreasureItem(ref itm, p.X, p.Y);
                    }
                }
            }
            if (srcMonster.DropNum3 > 0 && srcMonster.DropItem3 != "")
            {
                count = game.Random(1, srcMonster.DropNum3);
                for (int n = 1; n < count; n++)
                {
                    //25% chance for drop
                    if (game.Random(100) < 25)
                    {
                        itm = items.getItem(srcMonster.DropItem3);
                        p.X = (int)srcMonster.X + game.Random(rad) - rad / 2;
                        p.Y = (int)srcMonster.Y + game.Random(rad) - rad / 2;
                        DropTreasureItem(ref itm, p.X, p.Y);
                    }
                }
            }
        }

        public void DropTreasureItem(ref Item itm, int x, int y)
        {
            DrawableItem drit;
            drit.item = itm;

            drit.sprite = new Sprite(ref game);
            drit.sprite.Position = new Point(x, y);

            if (drit.item.DropImageFilename == "")
            {
                MessageBox.Show("Error: Item '" + drit.item.Name + "' image file is invalid.");
                Application.Exit();
            }

            drit.sprite.Image = game.LoadBitmap(drit.item.DropImageFilename);
            drit.sprite.Size = drit.sprite.Image.Size;

            treasure.Add(drit);
        }

    }
}
