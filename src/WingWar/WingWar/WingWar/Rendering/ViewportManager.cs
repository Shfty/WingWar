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
    class ViewportManager
    {
        private SpawnPoints spawn;

        private List<Viewport> viewportList;
        private List<Player> playerList;
        private List<Rectangle> rectList;
        private Texture2D texture;

        private ContentManager Content;
        public Viewport defaultViewport;

        public ViewportManager(
            List<Viewport> viewportList,
            List<Player> playerList,
            ContentManager Content
        )
        {
            this.Content = Content;

            spawn = new SpawnPoints();

            this.viewportList = viewportList;
            this.playerList = playerList;
        }

        public void SliceViewports()
        {
            rectList = new List<Rectangle>();
            int w = Game.GraphicsMgr.PreferredBackBufferWidth;
            int h = Game.GraphicsMgr.PreferredBackBufferHeight;

            defaultViewport = new Viewport(new Rectangle(0, 0, w, h));

            switch (viewportList.Count())
            {
                case 1:
                    rectList.Add(new Rectangle(0, 0, w, h));
                    break;
                case 2:
                    rectList.Add(new Rectangle(0, 0, w, h / 2));
                    rectList.Add(new Rectangle(0, h / 2, w, h / 2));
                    break;
                case 3:
                    rectList.Add(new Rectangle(0, 0, w, h / 2));
                    rectList.Add(new Rectangle(0, h / 2, w / 2, h / 2));
                    rectList.Add(new Rectangle(w / 2, h / 2, w / 2, h / 2));
                    break;
                case 4:
                    rectList.Add(new Rectangle(0, 0, w / 2, h / 2));
                    rectList.Add(new Rectangle(w / 2, 0, w / 2, h / 2));
                    rectList.Add(new Rectangle(0, h / 2, w / 2, h / 2));
                    rectList.Add(new Rectangle(w / 2, h / 2, w / 2, h / 2));
                    break;
                default:
                    break;
            }

            for (int i = 0; i < viewportList.Count(); i++)
            {
                viewportList[i] = new Viewport(rectList[i]);
            }

            texture = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
        }

        public void AddViewport()
        {
            Viewport viewport = Game.GraphicsMgr.GraphicsDevice.Viewport;
            viewportList.Add(viewport);

            SliceViewports();
        }

        public void AddPlayer(int aircraftType)
        {
            Player player = new Player(viewportList.Count, aircraftType, Content);
            playerList.Add(player);

            spawn.PlayerSpawn();
            Vector3 spawnPoint = spawn.spawnPos;
        }

        public bool RemovePlayer(int idx)
        {
            //Try to remove the requested player, if it fails then there isn't one in that slot
            try
            {
                viewportList.RemoveAt(idx);
                SliceViewports();
                playerList.RemoveAt(idx);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Draw()
        {
            int bw = 2;

            for (int i = 0; i < viewportList.Count; i++)
            {
                Game.SpriteBatch.Draw(texture, new Rectangle(rectList[i].Left, rectList[i].Top, bw, rectList[i].Height), Color.White);
                Game.SpriteBatch.Draw(texture, new Rectangle(rectList[i].Right - bw, rectList[i].Top, bw, rectList[i].Height), Color.White);
                Game.SpriteBatch.Draw(texture, new Rectangle(rectList[i].Left, rectList[i].Top, rectList[i].Width, bw), Color.White);
                Game.SpriteBatch.Draw(texture, new Rectangle(rectList[i].Left, rectList[i].Bottom - bw, rectList[i].Width, bw), Color.White);
            }
        }
    }
}
