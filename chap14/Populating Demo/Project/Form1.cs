using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RPG
{
    public partial class Form1 : Form
    {
        Game game;
        int drawLast = 0;
        Dialogue dialogue;
        bool lootFlag = false;
        Character[] monsters;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Dungeon Crawler - Populate Demo";

            /**
             * create game object
            **/
            Form form = (Form)this;
            game = new Game(ref form, 800, 600);
            game.SetFont("Arial", 14, FontStyle.Regular);

            /**
             * load hero
            **/
            game.Hero = new Player(ref game);
            if (!game.Hero.Load("paladin.char"))
            {
                MessageBox.Show("Error loading paladin.char");
                Application.Exit();
            }
            game.Hero.AnimationState = Character.AnimationStates.Standing;
            game.Hero.Position = new Point(32, 32);

            /**
             * create tilemap
            **/
            game.World = new Level(ref game, 25, 19, 32);
            if (!game.World.loadTilemap("level1.level"))
            {
                MessageBox.Show("Error loading level");
                Application.Exit();
            }
            if (!game.World.loadPalette("palette.bmp", 5))
            {
                MessageBox.Show("Error loading palette");
                Application.Exit();
            }
            game.World.GridPos = new Point(0, 0);
            game.World.Update();


            /**
             * look for the player's spawn tile (ENTRY flag)
            **/
            Point target = new Point(0, 0);
            bool found = false;
            for (int y = 0; y < 128; y++)
            {
                if (found) break;
                for (int x = 0; x < 128; x++)
                {
                    target = new Point(x-1, y-1);
                    Level.tilemapStruct tile = game.World.getTile(x,y);
                    if (tile.data1.ToUpper() == "ENTRY")
                    {
                        found = true;
                        game.Hero.Position = new Point(
                            target.X * 32, target.Y * 32-16);
                        break;
                    }
                }
            }


            /**
             * load items database
            **/
            game.Items = new Items();
            if (!game.Items.Load("items.item"))
            {
                MessageBox.Show("Error loading items");
                Application.Exit();
            }

            /**
             * create inventory
            **/
            game.Inven = new Inventory(ref game, new Point((800-532)/2, 50));
            if (!game.Inven.AddItem(game.Items.getItem("Short Sword")))
            {
                MessageBox.Show("Error adding item to inventory");
                Application.Exit();
            }


            /**
             * create treasure drop list
            **/
            game.Treasure = new List<Game.DrawableItem>();
            lootFlag = false;

            //add one item of treasure to the dungeon
            Item item = game.Items.getItem("Small Shield");
            DropTreasureItem(ref item, 10*32, 1*32);

            //add loot treasure from a .char file
            Character loot = new Character(ref game);
            if (!loot.Load("dummydrop1.char"))
            {
                MessageBox.Show("Error loading loot file");
                Application.Exit();
            }
            loot.Position = new Point(8*32,2*32);
            DropLoot(ref loot);

            //search dungeon level for drop item codes
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    Item it = null;
                    Level.tilemapStruct tile = game.World.getTile(x, y);
                    if (tile.data1.ToUpper() == "ITEM" && tile.data2 != "")
                    {
                        it = game.Items.getItem(tile.data2);
                        DropTreasureItem(ref it, x*32, y*32);
                    }
                }
            }


            /**
             * create monster list
            **/
            monsters = new Character[10];

            //search dungeon level for monsters!
            int count = 0;
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    Level.tilemapStruct tile = game.World.getTile(x, y);
                    if (tile.data1.ToUpper() == "MONSTER" && tile.data2 != "")
                    {
                        monsters[count] = new Character(ref game);
                        monsters[count].Load(tile.data2);
                        monsters[count].Position = new PointF((x-1) * 32, (y-1) * 32);
                        count++;
                    }
                }
            }



            
            /**
             * create dialogue
            **/
            dialogue = new Dialogue(ref game);

            /**
             * game loop
            **/
            while (!game.GameOver)
            {
                doUpdate();
            }
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape: game.GameOver = true; break; 
                case Keys.Up:
                case Keys.W: game.keyState.up = true; break;
                case Keys.Down:
                case Keys.S: game.keyState.down = true; break;
                case Keys.Left:
                case Keys.A: game.keyState.left = true; break;
                case Keys.Right:
                case Keys.D: game.keyState.right = true; break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W: game.keyState.up = false; break;
                case Keys.Down:
                case Keys.S: game.keyState.down = false; break;
                case Keys.Left:
                case Keys.A: game.keyState.left = false; break;
                case Keys.Right:
                case Keys.D: game.keyState.right = false; break;
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
                doMonsters();
                doDialogue();
                doInventory();

                lootFlag = false;

                //get tile data at player's foot
                PointF feet = game.Hero.FootPos;
                int tilex = (int)(game.World.ScrollPos.X + feet.X) / 32;
                int tiley = (int)(game.World.ScrollPos.Y + feet.Y) / 32;
                Level.tilemapStruct ts = game.World.getTile(tilex, tiley);

                string text = "";
                text = "Scroll " + game.World.ScrollPos.ToString() +
                    " Tile " + game.World.GridPos.ToString() +
                    "\nPlayer = " + game.Hero.Position.ToString();
                text += " Tile " + tilex.ToString() + "," + tiley.ToString();

                if (ts.collidable)
                {
                    text += "\nCollidable";
                }
                if (ts.portal)
                {
                    text += "\nPortal " + ts.portalx.ToString() +
                        "," + ts.portaly.ToString() + " " + ts.portalfile;
                }

                game.Print(0,0,text);

                game.Print(280, 580, "Controls: (Space) Loot, (I) Inventory");

                game.Update();
                Application.DoEvents();
            }
            else Thread.Sleep(1);
        }

        private void doScrolling()
        {
            game.World.Update();
            game.World.Draw(0, 0, 800, 600);
        }

        private void doHero()
        {
            //limit player sprite to the screen boundary
            if (game.Hero.X < -32) game.Hero.X = -32;
            else if (game.Hero.X > 800 - 65) game.Hero.X = 800 - 65;

            if (game.Hero.Y < -32) game.Hero.Y = -32;
            else if (game.Hero.Y > 600 - 81) game.Hero.Y = 600 - 81;

            //orient the player in the right direction
            if (game.keyState.up && game.keyState.right) 
            {
                game.Hero.Direction = 1;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.right && game.keyState.down) 
            {
                game.Hero.Direction = 3;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.down && game.keyState.left) 
            {
                game.Hero.Direction = 5;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.left && game.keyState.up) 
            {
                game.Hero.Direction = 7;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.up) 
            {
                game.Hero.Direction = 0;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.right) 
            {
                game.Hero.Direction = 2;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.down) 
            {
                game.Hero.Direction = 4;
                game.Hero.AnimationState = Character.AnimationStates.Walking;
            }
            else if (game.keyState.left)
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

            foreach (Game.DrawableItem it in game.Treasure)
            {
                //is item in view?
                if (it.sprite.X > game.World.ScrollPos.X - 64 
                    && it.sprite.X < game.World.ScrollPos.X + 23 * 32 + 64 
                    && it.sprite.Y > game.World.ScrollPos.Y - 64 
                    && it.sprite.Y < game.World.ScrollPos.Y + 17 * 32 + 64)
                {
                    //get relative position of item on screen
                    relativePos.X = it.sprite.X - game.World.ScrollPos.X;
                    relativePos.Y = it.sprite.Y - game.World.ScrollPos.Y;

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
                                game.Treasure.Remove(it);
                                showDialogue("LOOT", it.item.Value.ToString() + " GOLD", "OK");
                            }
                            else
                            {
                                if (game.Inven.AddItem(it.item))
                                {
                                    game.Treasure.Remove(it);
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
                        itm = game.Items.getItem(srcMonster.DropItem1);
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
                        itm = game.Items.getItem(srcMonster.DropItem2);
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
                        itm = game.Items.getItem(srcMonster.DropItem3);
                        p.X = (int)srcMonster.X + game.Random(rad) - rad / 2;
                        p.Y = (int)srcMonster.Y + game.Random(rad) - rad / 2;
                        DropTreasureItem(ref itm, p.X, p.Y);
                    }
                }
            }
        }

        public void DropTreasureItem(ref Item itm, int x, int y)
        {
            Game.DrawableItem drit;
            drit.item = itm;

            drit.sprite = new Sprite(ref game);
            drit.sprite.Position = new Point(x, y);

            if (drit.item.DropImageFilename == "")
            {
                MessageBox.Show("Error: Item '" + drit.item.Name + 
                    "' image file is invalid.");
                Application.Exit();
            }

            drit.sprite.Image = game.LoadBitmap(drit.item.DropImageFilename);
            drit.sprite.Size = drit.sprite.Image.Size;

            game.Treasure.Add(drit);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            game.GameOver = true;
        }

        private void doMonsters()
        {
            PointF relativePos;
            PointF heroCenter;
            PointF monsterCenter;

            heroCenter = game.Hero.CenterPos;

            for (int n = 0; n < monsters.Length; n++)
            {
                if (monsters[n] != null)
                {
                //is monster in view?
                    if (monsters[n].X > game.World.ScrollPos.X &&
                        monsters[n].X < game.World.ScrollPos.X + 23 * 32 &&
                        monsters[n].Y > game.World.ScrollPos.Y &&
                        monsters[n].Y < game.World.ScrollPos.Y + 17 * 32)
                    {
                        //get relative position on screen
                        relativePos = new PointF(
                            Math.Abs(game.World.ScrollPos.X - monsters[n].X),
                            Math.Abs(game.World.ScrollPos.Y - monsters[n].Y));

                        //get center
                        monsterCenter = relativePos;
                        monsterCenter.X += monsters[n].GetSprite.Width / 2;
                        monsterCenter.Y += monsters[n].GetSprite.Height / 2;

                        //draw the monster sprite
                        monsters[n].Draw(relativePos);
                    }
                }
            }
                   
        }    
    }
}
