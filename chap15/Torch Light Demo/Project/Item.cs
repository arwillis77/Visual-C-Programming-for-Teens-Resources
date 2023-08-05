using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public class Item
    {
        private string p_name;
        private string p_desc;
        private string p_dropfile;
        private string p_invfile;
        private string p_category;
        private float p_weight;
        private float p_value;
        private int p_attacknumdice;
        private int p_attackdie;
        private int p_defense;
        private int p_buffStr;
        private int p_buffDex;
        private int p_buffSta;
        private int p_buffInt;
        private int p_buffCha;

        public Item()
        {
            p_name = "new item";
            p_desc = "";
            p_dropfile = "";
            p_invfile = "";
            p_category = "";
            p_weight = 0;
            p_value = 0;
            p_attacknumdice = 0;
            p_attackdie = 0;
            p_defense = 0;
            p_buffStr = 0;
            p_buffDex = 0;
            p_buffSta = 0;
            p_buffInt = 0;
            p_buffCha = 0;
        }

        public string Name
        {
            get { return p_name; }
            set { p_name = value; }
        }

        public string Description
        {
            get { return p_desc; }
            set { p_desc = value; }
        }

        public string DropImageFilename
        {
            get { return p_dropfile; }
            set { p_dropfile = value; }
        }

        public string InvImageFilename
        {
            get { return p_invfile; }
            set { p_invfile = value; }
        }

        public string Category
        {
            get { return p_category; }
            set { p_category = value; }
        }

        public float Weight
        {
            get { return p_weight; }
            set { p_weight = value; }
        }

        public float Value
        {
            get { return p_value; }
            set { p_value = value; }
        }

        public int AttackNumDice
        {
            get { return p_attacknumdice; }
            set { p_attacknumdice = value; }
        }

        public int AttackDie
        {
            get { return p_attackdie; }
            set { p_attackdie = value; }
        }

        public int Defense
        {
            get { return p_defense; }
            set { p_defense = value; }
        }

        public int STR
        {
            get { return p_buffStr; }
            set { p_buffStr = value; }
        }

        public int DEX
        {
            get { return p_buffDex; }
            set { p_buffDex = value; }
        }

        public int STA
        {
            get { return p_buffSta; }
            set { p_buffSta = value; }
        }

        public int INT
        {
            get { return p_buffInt; }
            set { p_buffInt = value; }
        }

        public int CHA
        {
            get { return p_buffCha; }
            set { p_buffCha = value; }
        }

        public string Summary
        {
            get {
                string text = "This '" + p_name + "', ";

                string weight = "";
                if (p_weight > 50) weight = "a very heavy ";
                else if (p_weight > 25) weight = "a heavy ";
                else if (p_weight > 15) weight = "a ";
                else if (p_weight > 7) weight = "a light ";
                else if (p_weight > 0) weight = "a very light ";
                text += weight;

                if (p_category == "Weapon") text += "weapon";
                else if (p_category == "Armor") text += "armor item";
                else if (p_category == "Necklace") text += "necklace";
                else if (p_category == "Ring") text += "ring";
                else text += p_category.ToLower() + " item";
                
                if (p_attacknumdice != 0)
                {
                    text += ", attacks at " + p_attacknumdice.ToString() 
                        + "D" + p_attackdie.ToString() 
                        + " (" + p_attacknumdice.ToString() + " - " 
                        + (p_attackdie * p_attacknumdice).ToString() 
                        + " damage)";
                }

                if (p_defense != 0)
                    text += ", adds " + p_defense.ToString() + 
                        " armor points";

                string fmt = "+#;-#";
                if (p_buffStr != 0)
                    text += ", " + p_buffStr.ToString(fmt) + " STR";
                
                if (p_buffDex != 0)
                    text += ", " + p_buffDex.ToString(fmt) + " DEX";
                
                if (p_buffSta != 0)
                    text += ", " + p_buffSta.ToString(fmt) + " STA";
                
                if (p_buffInt != 0)
                    text += ", " + p_buffInt.ToString(fmt) + " INT";
                
                if (p_buffCha != 0)
                    text += ", " + p_buffCha.ToString(fmt) + " CHA";

                return text + ".";
            }
        }

        public override string ToString()
        {
            return p_name;
        }

    }
}
