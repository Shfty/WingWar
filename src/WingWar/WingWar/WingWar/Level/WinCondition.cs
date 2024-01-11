using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar
{
    class WinCondition
    {
        private static WinCondition instance;
        private int maxScore = 10;
        private string playerName;
        private int playerIdx;

        public WinCondition() { }

        public static WinCondition Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WinCondition();
                }
                return instance;
            }
        }

        public bool HasPlayerWon(int playerScore, int playerIndex)
        {
            if (playerScore >= maxScore)
            {
                this.playerIdx = playerIndex;
                PlayerName = "GG: Player " + playerIndex + " has won!";
                return true;
            }
            else
            {
                return false;
            }
        }

        public string PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

        public int MaxScore
        {
            get
            {
                return maxScore;
            }

            set
            {
                maxScore = value;
            }
        }

        public void Draw(List<Viewport> viewportList)
        {
            Game.SpriteBatch.Begin();

            for (int i = 0; i < viewportList.Count; i++)
            {
                Game.GraphicsMgr.GraphicsDevice.Viewport = viewportList[playerIdx - 1];

                Game.SpriteBatch.DrawString(Game.MenuFont, PlayerName,
                    new Vector2((float)viewportList[i].Width / 2 - ((Game.MenuFont.MeasureString(PlayerName).X * Game.MasterScale2D) / 2),
                        (float)viewportList[i].Height / 2 - (float)Game.MenuFont.LineSpacing / 2),
                    Color.White, 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);             
            }

            Game.SpriteBatch.End();

            GraphicsDeviceSettings.Instance.ResetAfterSpriteBatch();
        }
    }
}
