using System;
using System.Collections.Generic;
using System.Xml;

namespace RPG
{
    public class Player : Character
    {
        private int p_gold;

        public Player(ref Game game) : base(ref game)
        {
            p_gold = 0;
        }

        public int Gold
        {
            get { return p_gold; }
            set { p_gold = value; }
        }

        public override string ToString()
        {
            return base.Name;
        }

        public void LoadGame(string filename)
        {
        }

        public void SaveGame(string filename)
        {
        }

    }
}
