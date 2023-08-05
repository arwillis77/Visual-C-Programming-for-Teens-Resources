using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace RPG
{
public class Character
{

    public enum AnimationStates
    {
        Walking = 0,
        Attacking = 1,
        Dying = 2,
        Dead = 3,
        Standing = 4
    }

    private Game p_game;
    private PointF p_position;
    private int p_direction;
    private AnimationStates p_state;

    //character file properties;
    private string p_name;
    private string p_class;
    private string p_race;
    private string p_desc;
    private int p_str;
    private int p_dex;
    private int p_sta;
    private int p_int;
    private int p_cha;
    private int p_hitpoints;
    private int p_dropGold1;
    private int p_dropGold2;
    private string p_walkFilename;
    private Sprite p_walkSprite;
    private Size p_walkSize;
    private int p_walkColumns;
    private string p_attackFilename;
    private Sprite p_attackSprite;
    private Size p_attackSize;
    private int p_attackColumns;
    private string p_dieFilename;
    private Sprite p_dieSprite;
    private Size p_dieSize;
    private int p_dieColumns;
    private int p_experience;
    private int p_level;
    private bool p_alive;
    private int p_dropnum1;
    private int p_dropnum2;
    private int p_dropnum3;
    private string p_dropitem1;
    private string p_dropitem2;
    private string p_dropitem3;

    public Character(ref Game game)
    {
        p_game = game;
        p_position = new PointF(0, 0);
        p_direction = 1;
        p_state = AnimationStates.Standing;

        //initialize loadable properties
        p_name = "";
        p_class = "";
        p_race = "";
        p_desc = "";
        p_str = 0;
        p_dex = 0;
        p_sta = 0;
        p_int = 0;
        p_cha = 0;
        p_hitpoints = 0;
        p_dropGold1 = 0;
        p_dropGold2 = 0;
        p_walkSprite = null;
        p_walkFilename = "";
        p_walkSize = new Size(0, 0);
        p_walkColumns = 0;
        p_attackSprite = null;
        p_attackFilename = "";
        p_attackSize = new Size(0, 0);
        p_attackColumns = 0;
        p_dieSprite = null;
        p_dieFilename = "";
        p_dieSize = new Size(0, 0);
        p_dieColumns = 0;
        p_experience = 0;
        p_level = 1;
        p_dropnum1 = 0;
        p_dropnum2 = 0;
        p_dropnum3 = 0;
        p_dropitem1 = "";
        p_dropitem2 = "";
        p_dropitem3 = "";
    }

    public string Name
    {
        get { return p_name; }
        set { p_name = value; }
    }

    public string PlayerClass
    {
        get { return p_class; }
        set { p_class = value; }
    }

    public string Race
    {
        get { return p_race; }
        set { p_race = value; }
    }

    public string Description
    {
        get { return p_desc; }
        set { p_desc = value; }
    }

    public int STR
    {
        get { return p_str; }
        set { p_str = value; }
    }

    public int DEX
    {
        get { return p_dex; }
        set { p_dex = value; }
    }

    public int STA
    {
        get { return p_sta; }
        set { p_sta = value; }
    }

    public int INT
    {
        get { return p_int; }
        set { p_int = value; }
    }

    public int CHA
    {
        get { return p_cha; }
        set { p_cha = value; }
    }

    public int HitPoints
    {
        get { return p_hitpoints; }
        set { p_hitpoints = value; }
    }

    public int DropGoldMin
    {
        get { return p_dropGold1; }
        set { p_dropGold1 = value; }
    }

    public int DropGoldMax
    {
        get { return p_dropGold2; }
        set { p_dropGold2 = value; }
    }

    public int DropNum1
    {
        get { return p_dropnum1; }
        set { p_dropnum1 = value; }
    }

    public int DropNum2
    {
        get { return p_dropnum2; }
        set { p_dropnum2 = value; }
    }

    public int DropNum3
    {
        get { return p_dropnum3; }
        set { p_dropnum3 = value; }
    }

    public string DropItem1
    {
        get { return p_dropitem1; }
        set { p_dropitem1 = value; }
    }

    public string DropItem2
    {
        get { return p_dropitem2; }
        set { p_dropitem2 = value; }
    }

    public string DropItem3
    {
        get { return p_dropitem3; }
        set { p_dropitem3 = value; }
    }

    public Sprite GetSprite
    {
        get {
            switch (p_state)
            {
                case AnimationStates.Walking:
                    return p_walkSprite;
                case AnimationStates.Attacking:
                    return p_attackSprite;
                case AnimationStates.Dying:
                    return p_dieSprite;
                default:
                    return p_walkSprite;
            }
        }
    }

    public PointF Position
    {
        get { return p_position; }
        set { p_position = value; }
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

    public int Direction
    {
        get { return p_direction; }
        set { p_direction = value; }
    }

    public AnimationStates AnimationState
    {
        get { return p_state; }
        set { p_state = value; }
    }

    public void Draw()
    {
        Draw((int)p_position.X, (int)p_position.Y);
    }

    public void Draw(PointF pos)
    {
        Draw((int)pos.X, (int)pos.Y);
    }

    public void Draw(int x, int y)
    {
        int startFrame, endFrame;
        switch (p_state)
        {
            case AnimationStates.Standing:
                p_walkSprite.Position = p_position;
                if (p_direction > -1)
                {
                    startFrame = p_direction * p_walkColumns;
                    endFrame = startFrame + p_walkColumns - 1;
                    p_walkSprite.CurrentFrame = endFrame;
                }
                p_walkSprite.Draw(x,y);
                break;

            case AnimationStates.Walking:
                p_walkSprite.Position = p_position;
                if (p_direction > -1)
                {
                    startFrame = p_direction * p_walkColumns;
                    endFrame = startFrame + p_walkColumns - 1;
                    p_walkSprite.AnimationRate = 30;
                    p_walkSprite.Animate(startFrame, endFrame);
                }
                p_walkSprite.Draw(x,y);
                break;

            case AnimationStates.Attacking:
                p_attackSprite.Position = p_position;
                if (p_direction > -1)
                {
                    startFrame = p_direction * p_attackColumns;
                    endFrame = startFrame + p_attackColumns - 1;
                    p_attackSprite.AnimationRate = 30;
                    p_attackSprite.Animate(startFrame, endFrame);
                }
                p_attackSprite.Draw(x,y);
                break;

            case AnimationStates.Dying:
                p_dieSprite.Position = p_position;
                if (p_direction > -1)
                {
                    startFrame = p_direction * p_dieColumns;
                    endFrame = startFrame + p_dieColumns - 1;
                    p_dieSprite.AnimationRate = 30;
                    p_dieSprite.Animate(startFrame, endFrame);
                }
                p_dieSprite.Draw(x,y);
                break;

            case AnimationStates.Dead:
                p_dieSprite.Position = p_position;
                if (p_direction > -1)
                {
                    startFrame = p_direction * p_dieColumns;
                    endFrame = startFrame + p_dieColumns-1;
                    p_dieSprite.CurrentFrame = endFrame;
                }
                p_dieSprite.Draw(x,y);
                break;
        }
    }

    private string getElement(string field, ref XmlElement element)
    {
        string value = "";
        try
        {
            value = element.GetElementsByTagName(field)[0].InnerText;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return value;
    }


    public bool Load(string filename)
    {
        try
        {
            //open the xml file 
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList list = doc.GetElementsByTagName("character");
            XmlElement element = (XmlElement)list[0];

            //read data fields
            string data;
            p_name = getElement("name", ref element);
            p_class = getElement("class", ref element);
            p_race = getElement("race", ref element);
            p_desc = getElement("desc", ref element);
            
            data = getElement("str", ref element);
            p_str = Convert.ToInt32(data);
            
            data = getElement("dex", ref element);
            p_dex = Convert.ToInt32(data);
            
            data = getElement("sta", ref element);
            p_sta = Convert.ToInt32(data);
            
            data = getElement("int", ref element);
            p_int = Convert.ToInt32(data);
            
            data = getElement("cha", ref element);
            p_cha = Convert.ToInt32(data);
            
            data = getElement("hitpoints", ref element);
            p_hitpoints = Convert.ToInt32(data);
            
            data = getElement("anim_walk_filename", ref element);
            p_walkFilename = data;
            
            data = getElement("anim_walk_width", ref element);
            p_walkSize.Width = Convert.ToInt32(data);
            
            data = getElement("anim_walk_height", ref element);
            p_walkSize.Height = Convert.ToInt32(data);
            
            data = getElement("anim_walk_columns", ref element);
            p_walkColumns = Convert.ToInt32(data);

            data = getElement("anim_attack_filename", ref element);
            p_attackFilename = data;

            data = getElement("anim_attack_width", ref element);
            p_attackSize.Width = Convert.ToInt32(data);

            data = getElement("anim_attack_height", ref element);
            p_attackSize.Height = Convert.ToInt32(data);

            data = getElement("anim_attack_columns", ref element);
            p_attackColumns = Convert.ToInt32(data);

            data = getElement("anim_die_filename", ref element);
            p_dieFilename = data;

            data = getElement("anim_die_width", ref element);
            p_dieSize.Width = Convert.ToInt32(data);

            data = getElement("anim_die_height", ref element);
            p_dieSize.Height = Convert.ToInt32(data);

            data = getElement("anim_die_columns", ref element);
            p_dieColumns = Convert.ToInt32(data);

            data = getElement("dropgold1", ref element);
            p_dropGold1 = Convert.ToInt32(data);

            data = getElement("dropgold2", ref element);
            p_dropGold2 = Convert.ToInt32(data);

            data = getElement("drop1_num", ref element);
            p_dropnum1 = Convert.ToInt32(data);

            data = getElement("drop2_num", ref element);
            p_dropnum2 = Convert.ToInt32(data);

            data = getElement("drop3_num", ref element);
            p_dropnum3 = Convert.ToInt32(data);

            p_dropitem1 = getElement("drop1_item", ref element);
            p_dropitem2 = getElement("drop2_item", ref element);
            p_dropitem3 = getElement("drop3_item", ref element);


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        //create character sprites
        try
        {
            if (p_walkFilename != "")
            {
                p_walkSprite = new Sprite(ref p_game);
                p_walkSprite.Image = LoadBitmap(p_walkFilename);
                p_walkSprite.Size = p_walkSize;
                p_walkSprite.Columns = p_walkColumns;
                p_walkSprite.TotalFrames = p_walkColumns * 8;
            }

            if (p_attackFilename != "")
            {
                p_attackSprite = new Sprite(ref p_game);
                p_attackSprite.Image = LoadBitmap(p_attackFilename);
                p_attackSprite.Size = p_attackSize;
                p_attackSprite.Columns = p_attackColumns;
                p_attackSprite.TotalFrames = p_attackColumns * 8;
            }

            if (p_dieFilename != "")
            {
                p_dieSprite = new Sprite(ref p_game);
                p_dieSprite.Image = LoadBitmap(p_dieFilename);
                p_dieSprite.Size = p_dieSize;
                p_dieSprite.Columns = p_dieColumns;
                p_dieSprite.TotalFrames = p_dieColumns * 8;
            }
        }

        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
        return true;
    }

    private Bitmap LoadBitmap(string filename)
    {
        Bitmap bmp=null;
        try
        {
            bmp = new Bitmap(filename);
        }
        catch (Exception){}
        return bmp;
    }
     
    public PointF FootPos
    {
        get { return new Point((int)this.X + 32, (int)this.Y + 32 + 16); }
    }

    public PointF CenterPos
    {
        get
        {
            PointF pos = this.Position;
            pos.X += this.GetSprite.Width / 2;
            pos.Y += this.GetSprite.Height / 2;
            return pos;
        }
    }

    public double FootDistance(ref Character other)
    {
        return p_game.Distance(this.FootPos, other.FootPos);
    }

    public double FootDistance(PointF pos)
    {
        return p_game.Distance(this.FootPos, pos);
    }

    public double CenterDistance(ref Character other)
    {
        return p_game.Distance(CenterPos, other.CenterPos);
    }

    public double CenterDistance(PointF pos)
    {
        return p_game.Distance(this.CenterPos, pos);
    }

    public int Experience
    {
        get { return p_experience; }
        set { p_experience = value; }
    }

    public int Level
    {
        get { return p_level; }
        set { p_level = value; }
    }

    public bool Alive
    {
        get { return p_alive; }
        set { p_alive = value; }
    }
    
}
}
