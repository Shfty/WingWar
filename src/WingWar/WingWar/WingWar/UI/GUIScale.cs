using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WingWar
{
    class GUIscale
    {
        private static GUIscale instance;

        public GUIscale() {}

        public static GUIscale Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GUIscale();
                }
                return instance;
            }
        }

        public void Update(List<Player> playerList, int viewports)
        {
            for (int i = 0; i < playerList.Count(); i++)
            {
                if (viewports == 1)
                {
                    playerList[i].gui.textScale = 1.0f;
                    playerList[i].gui.rScale = .25f;
                    playerList[i].gui.scale = .5f;
                }

                if (viewports >= 2)
                {
                    playerList[i].gui.textScale = 0.75f;
                    playerList[i].gui.rScale = .125f;
                    playerList[i].gui.scale = .25f;
                }

                playerList[i].gui.scale *= Game.MasterScale2D;
                playerList[i].gui.rScale *= Game.MasterScale2D;
            }
        }
    }
}
