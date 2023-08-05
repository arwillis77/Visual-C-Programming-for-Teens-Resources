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

        public enum AttackStates
        {
            ATTACK_NONE,
            ATTACK_TRIGGER,
            ATTACK_ATTACK,
            ATTACK_RESULT,
            ATTACK_LOOT
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
        AttackStates attackState = AttackStates.ATTACK_NONE;
        bool attacking = false;
        int target = 0;
        string attackText = "";
        int monstersInRange = 0;
        int monstersInView = 0;
        Character[] monsters;
        Dialogue dialogue;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Combat Demo 4";

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
                monsters[n].Position = new Point(game.Random(100, 1000), 
                    game.Random(100, 1000));
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
                case Keys.Space: attackFlag = true; break;
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
                doAttack();
                doDialogue();

                attackFlag = false;

                game.Print(0, 0, "Monsters in view: " + 
                    monstersInView.ToString());
                game.Print(0, 20, "Attack state: " + attackState.ToString());
                game.Print(0, 40, "Selection: " + dialogue.Selection.ToString());
                game.Print(320, 570, "Press SPACE to Attack");

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

        private void showDialogue(string title, string message, string button1)
        {
            dialogue.Title = title;
            dialogue.Message = message;
            dialogue.NumButtons = 1;
            dialogue.setButtonText(1, button1);
            dialogue.Visible = true;
        }

        private void doDialogue()
        {
            if (dialogue.Visible)
            {
                if (hero.CenterPos.Y < 300)
                    dialogue.setCorner(Dialogue.Positions.LowerRight);
                else
                    dialogue.setCorner(Dialogue.Positions.UpperRight);

                dialogue.updateMouse(game.MousePos, game.MouseButton);
                dialogue.Draw();
            }
        }

        private void doMonsters()
        {
            PointF relativePos;
            PointF heroCenter;
            PointF monsterCenter;
            double dist=0;
            float spriteSize;
            Point center;
            RectangleF circleRect;

            heroCenter = hero.CenterPos;
            monstersInRange = 0;
            monstersInView = 0;

            for (int n = 1; n < NUM_MONSTERS; n++)
            {
                //is monster in view?
                if (monsters[n].X > level.ScrollPos.X &&
                    monsters[n].X < level.ScrollPos.X + 23 * 32 &&
                    monsters[n].Y > level.ScrollPos.Y &&
                    monsters[n].Y < level.ScrollPos.Y + 17 * 32)
                {
                    monstersInView += 1;

                    relativePos = new PointF(
                        Math.Abs(level.ScrollPos.X - monsters[n].X),
                        Math.Abs(level.ScrollPos.Y - monsters[n].Y));

                    //get center of NPC
                    monsterCenter = relativePos;
                    monsterCenter.X += monsters[n].GetSprite.Width / 2;
                    monsterCenter.Y += monsters[n].GetSprite.Height / 2;

                    //get distance to the NPC
                    dist = hero.CenterDistance(monsterCenter);

                    if (monsters[n].Alive)
                        monsters[n].AnimationState = Character.AnimationStates.Walking;
                    else
                        monsters[n].AnimationState = Character.AnimationStates.Dead;
                    

                    //is player trying to attack this monster?
                    if (dist < ATTACK_RADIUS && monsters[n].Alive)
                    {
                        monstersInRange++;

                        if (target > 0)
                            game.Device.DrawEllipse(new Pen(Brushes.Red, 2.0f), 
                                monsterCenter.X - 24, monsterCenter.Y, 48, 48);
                        else 
                            game.Device.DrawEllipse(new Pen(Brushes.Blue, 2.0f), 
                                monsterCenter.X - 24, monsterCenter.Y, 48, 48);

                        if (attackState == AttackStates.ATTACK_NONE)
                        {
                            if (attackFlag)
                            {
                                attackState = AttackStates.ATTACK_TRIGGER;
                                target = n;
                                attackFlag = false;
                                //dialogue.Visible = true;

                                //make PC and NPC face each other
                                int dir = getTargetDirection(monsterCenter, hero.CenterPos);
                                monsters[target].Direction = dir;
                                monsters[target].Draw();

                                dir = getTargetDirection(hero.CenterPos, monsterCenter);
                                hero.Direction = dir;
                                hero.Draw();

                                break;
                            }
                        }
                    }

                    //draw the monster sprite
                    monsters[n].Draw(relativePos);

                }
            }

            if (monstersInRange == 0 && attackState == AttackStates.ATTACK_NONE)
            {
                target = 0;
                attackText = "";
                dialogue.Visible = false;
                dialogue.Selection = 0;
            }
        }


        private int getTargetDirection(PointF source, PointF target)
        {
            int direction = 0;
            if (source.X < target.X - 16)
            {
                //facing eastward
                if (source.Y < target.Y - 8)
                    direction = 3; //south east
                else if (source.Y > target.Y + 8)
                    direction = 1; //north east
                else
                    direction = 2; //east
            }
            else if (source.X > target.X + 16)
            {
                //facing westward
                if (source.Y < target.Y - 8)
                    direction = 5; //south west
                else if (source.Y > target.Y + 8)
                    direction = 7; //north west
                else
                    direction = 6; //west
            }
            else
            {
                //facing north or south
                if (source.Y < target.Y - 8)
                    direction = 4; //south
                else if (source.Y > target.Y + 8)
                    direction = 0; //north

            }
            return direction;
       }

       private void doAttack()
       {
            const int DEF_ARMOR = 10;
            const int DEF_SHIELD = 0;
            const int WEAPON_DMG = 5;
            bool hit = false;
            bool critical = false;
            bool fail = false;
            int roll = 0;
            int AC = 0;
            int damage = 0;
            string text="";

            //cancel button clicked?
            if (dialogue.Selection == 2)
            {
                attackState = AttackStates.ATTACK_NONE;
                dialogue.Selection = 0;
                dialogue.Visible = false;
                return; 
            }

            switch (attackState)
            {
                case AttackStates.ATTACK_NONE:
                    hero.AnimationState = Character.AnimationStates.Walking;
                    dialogue.Visible = false;
                    break;

                case AttackStates.ATTACK_TRIGGER:
                    if (target > 0)
                    {
                        text = "You are facing a " + monsters[target].Name + 
                            ". " + monsters[target].Description;
                        showDialogue("Prepare to Attack", text, "ATTACK", 
                            "CANCEL");
                    }

                    //attack button clicked?
                    if (dialogue.Selection == 1)
                        attackState = AttackStates.ATTACK_ATTACK;

                    break;

                case AttackStates.ATTACK_ATTACK:
                    //calculate target's AC
                    AC = monsters[target].DEX + DEF_ARMOR + DEF_SHIELD;

                    //calculate chance to-hit for PC
                    roll = game.Random(1, 20);
                    text += "To-Hit Roll: " + roll.ToString();
                    if (roll == 20)
                    {
                        //critical hit!
                        hit = true;
                        critical = true;
                        text += " (CRITICAL!)\n";
                    }
                    else if (roll == 1)
                    {
                        fail = true;
                        text += " (EPIC FAIL!)\n";
                    }
                    else
                    {
                        //normal hit
                        roll += hero.STR;
                        if (roll > AC) hit = true;
                        text += " + STR(" + hero.STR.ToString() + ") = " +
                            roll.ToString() + "\n";
                    }

                    //did attack succeed?
                    if (hit)
                    {
                        //calculate base damage
                        damage = game.Random(1, 8);

                        //add critical
                        if (critical) damage *= 2;

                        text += "Damage roll: " + damage.ToString() + "\n";

                        //add STR
                        damage += hero.STR;
                        text += " + STR(" + hero.STR.ToString() + ") = " +
                            damage.ToString() + "\n";

                        //add weapon damage (usually a die roll)
                        damage += WEAPON_DMG;
                        text += " + weapon(" + WEAPON_DMG.ToString() + 
                            ") = " + damage.ToString() + "\n";

                        //subtract AC
                        damage -= AC;
                        text += " - monster AC(" + AC.ToString() + ") = " +
                            damage.ToString() + "\n";

                        //minimal hit
                        if (damage < 1) damage = 1;
                        monsters[target].HitPoints -= damage;

                        //show result
                        text += "Attack succeeds for " + damage.ToString() +
                            " damage.";
                    }
                    else
                        text += "Attack failed.\n";

                    attackText = text;
                    attackState = AttackStates.ATTACK_RESULT;
                    break;

                case AttackStates.ATTACK_RESULT:
                    hero.AnimationState = Character.AnimationStates.Walking;

                    //is monster dead?
                    if (monsters[target].HitPoints <= 0)
                    {
                        monsters[target].Alive = false;
                        int xp = game.Random(50, 100);
                        addExperience(xp);
                        text = monsters[target].Name + " Defeated!";
                        attackText = "You have slain the " + monsters[target].Name +
                            "! You gained " + xp.ToString() + " experience.";
                        showDialogue(text, attackText, "CLOSE");
                        attackState = AttackStates.ATTACK_LOOT;
                    }
                    else
                    {
                        showDialogue("Attack Roll", attackText, "ATTACK AGAIN",
                            "CANCEL");
                        if (dialogue.Selection == 1)
                            attackState = AttackStates.ATTACK_ATTACK;
                    }
                    break;

                case AttackStates.ATTACK_LOOT:
                    hero.AnimationState = Character.AnimationStates.Walking;
                    if (dialogue.Selection == 1)
                    {
                        attackState = AttackStates.ATTACK_NONE;
                        target = 0;
                    }
                    break;
            }
       }

       private void addExperience(int xp)
       {
           hero.Experience += xp;
           if (hero.Experience > 200)
           {
               hero.Level += 1;
               hero.Experience -= 200;
           }
       }

       private void Form1_FormClosed(object sender, FormClosedEventArgs e)
       {
           gameover = true;
       }
    }
}
