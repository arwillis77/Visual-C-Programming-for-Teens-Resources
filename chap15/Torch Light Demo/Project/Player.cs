using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace RPG
{
    public class Player : Character
    {
        private Game p_game;
        private int p_gold;

        public Player(ref Game game) : base(ref game)
        {
            p_game = game;
            p_gold = 0;
        }

        public int Gold
        {
            get { return p_gold; }
            set { p_gold = value; }
        }

        public Point GetGlobalTilePos()
        {
            PointF feet = p_game.Hero.FootPos;
            int tilex = (int)(p_game.World.ScrollPos.X + feet.X) / 32;
            int tiley = (int)(p_game.World.ScrollPos.Y + feet.Y) / 32;
            return new Point(tilex, tiley);
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
