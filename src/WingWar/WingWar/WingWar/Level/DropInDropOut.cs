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
    class DropInDropOut
    {
        private static DropInDropOut instance;
        bool prevAcceptState, prevAddPlayerState, prevRemovePlayerState;
        Vector2 prevLeftState;
        string[]  menuItems = new string[] { "JT-01 Firebird", "CS-3 Gull", "Cancel" };
        Vector2 position;
        Color normal = Color.DarkGray, highlight = new Color(20, 220, 20, 255);
        int selection = 0;

        public DropInDropOut()
        {
        }

        public static DropInDropOut Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DropInDropOut();
                }
                return instance;
            }
        }

        public void ManagePlayers(int viewports, ViewportManager viewManager, int maxPlayers)
        {
            int playerIndex = 0;

            for (int i = 0; i < maxPlayers; i++)
            {
                if (i > viewports - 1)
                {
                    if (playerIndex > viewports)
                    {
                    }
                    else
                    {
                        if (GamePad.GetState((PlayerIndex)playerIndex).IsConnected)
                        {
                            if (Controls.Instance.AddPlayer((PlayerIndex)playerIndex) && !prevAddPlayerState)
                                if (viewports < maxPlayers)
                                {
                                    viewManager.AddViewport();
                                }
                        }
                    }
                }
                    
                if (i == viewports - 1)
                {
                    if (Controls.Instance.RemovePlayer((PlayerIndex)playerIndex) && !prevRemovePlayerState)
                        if (viewports > 1)
                        {
                            viewManager.RemovePlayer(viewports - 1);
                        }
                }

                prevRemovePlayerState = Controls.Instance.RemovePlayer((PlayerIndex)playerIndex);
                prevAddPlayerState = Controls.Instance.AddPlayer((PlayerIndex)playerIndex);

                playerIndex++;
            }
        }

        public void ArcadeManagePlayers(int viewports, ViewportManager viewManager, int maxPlayers)
        {
            int playerIndex = 0;

            for (int i = 0; i < maxPlayers; i++)
            {
                if (i > viewports - 1)
                {
                    if (playerIndex > viewports)
                    {
                    }
                    else
                    {
                        if (Controls.Instance.AddPlayer((PlayerIndex)playerIndex) && !prevAddPlayerState)
                            if (viewports < maxPlayers)
                            {
                                viewManager.AddViewport();
                            }
                    }
                }

                if (i == viewports - 1)
                {
                    if (Controls.Instance.RemovePlayer((PlayerIndex)playerIndex) && !prevRemovePlayerState)
                        if (viewports > 0)
                        {
                            viewManager.RemovePlayer(viewports);
                        }
                }

                prevRemovePlayerState = Controls.Instance.RemovePlayer((PlayerIndex)playerIndex);
                prevAddPlayerState = Controls.Instance.AddPlayer((PlayerIndex)playerIndex);

                playerIndex++;
            }
        }

        public void DebugManagePlayers(int players, ViewportManager viewManager, int maxPlayers)
        {
            if (Controls.Instance.AddPlayer((PlayerIndex)players) && !prevAddPlayerState)
                if (players < maxPlayers - 1)
                {
                    viewManager.AddViewport();
                    viewManager.AddPlayer(0);
                }

            if (Controls.Instance.RemovePlayer((PlayerIndex)players) && !prevRemovePlayerState)
                if (players > 0)
                {
                    viewManager.RemovePlayer(players);
                }

            prevRemovePlayerState = Controls.Instance.RemovePlayer((PlayerIndex)players);
            prevAddPlayerState = Controls.Instance.AddPlayer((PlayerIndex)players);
        }

        public void Draw(Viewport viewport, int playerIndex, ViewportManager viewManager)
        {
            if (Controls.Instance.LeftStick((PlayerIndex)playerIndex).Y < 0 && prevLeftState.Y < 0)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection++;
                if (selection == menuItems.Length)
                    selection = 0;
            }
            if (Controls.Instance.LeftStick((PlayerIndex)playerIndex).Y > 0 && prevLeftState.Y > 0)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection--;
                if (selection < 0)
                    selection = menuItems.Length - 1;
            }

            if (Controls.Instance.Accept((PlayerIndex)playerIndex) && !prevAcceptState)
            {
                switch (selection)
                {
                    case 0:
                        viewManager.AddPlayer(0);
                        Game.GraphicsMgr.ApplyChanges();
                        break;
                    case 1:
                        viewManager.AddPlayer(1);
                        Game.GraphicsMgr.ApplyChanges();
                        break;
                    case 2:
                        viewManager.RemovePlayer(playerIndex);
                        break;
                }
            }

            Texture2D texture = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.Black });

            MeasureMenu();

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(texture, new Rectangle(0, 0, viewport.Width, viewport.Width), Color.White);

            for (int i = 0; i < menuItems.Length; i++)
            {
                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(position.X - (((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / 2) - 1), (position.Y + ((50 * Game.MasterScale2D) * i)) - 1),
                Color.White, 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);

                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(position.X - ((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / 2), position.Y + ((50 * Game.MasterScale2D) * i)),
                (i == selection ? highlight : normal), 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);
            }

            Game.SpriteBatch.End();

            prevLeftState = Controls.Instance.LeftStick((PlayerIndex)playerIndex);
            prevAcceptState = Controls.Instance.Accept((PlayerIndex)playerIndex);
        }

        private void MeasureMenu()
        {
            float height = 0;
            float width = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = Game.MenuFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;

                height += Game.MenuFont.LineSpacing + 5;
            }

            position = new Vector2(
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 2,
                (Game.GraphicsMgr.GraphicsDevice.Viewport.Height - height * Game.MasterScale2D) / 2);
        }
    }
}
