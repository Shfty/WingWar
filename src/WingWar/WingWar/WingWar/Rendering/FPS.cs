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
    class FPS
    {
        private static FPS instance;

        public FPS() {}

        public static FPS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FPS();
                }
                return instance;
            }
        }

        private float frameCount = 0, elapsedTime = 0;
        public float fps = 0;

        private void GetFPS(GameTime gameTime)
        {
            if (elapsedTime >= 1000)
            {
                fps = frameCount;
                frameCount = -3;
                elapsedTime = 0;
                return;
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            frameCount++;
        }

        public void DrawFPS(GameTime gameTime)
        {
            GetFPS(gameTime);

            Game.SpriteBatch.DrawString(Game.MenuFont, "FPS: " + fps.ToString(),
                new Vector2(10, 10),
                Color.Black, 0, new Vector2(0, 0), 1 * Game.MasterScale2D, SpriteEffects.None, 0);

            Game.SpriteBatch.DrawString(Game.MenuFont, "FPS: " + fps.ToString(),
                new Vector2(12, 12),
                Color.White, 0, new Vector2(0, 0), 1 * Game.MasterScale2D, SpriteEffects.None, 0);
        }
    }
}
