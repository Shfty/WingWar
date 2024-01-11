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
    class GUI
    {
        private int aircraftType;
        private float currentHealth;
        private int deathCount;
        private int killCount;
        private int maxSpeed = 15;
        private int missiles;
        private int currentSpeed;

        private Vector2 missilePos;
        private Vector2 speedPos;
        private Color targetColour;

        private Vector3 velocity;
        private Vector3 position;
        private Quaternion rotation;

        private bool bulletTarget = false;
        private bool missileTarget = false;

        private bool active = true;

        private Rectangle sourceRect = Rectangle.Empty;

        private Texture2D hudback, hudmiddle, h1, h2, h3, h4, h5, h6, h7, h8, h9, h10, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, reticle;
        private Texture2D colour = null;

        public float textScale;
        public float scale;
        public float rScale;

        public GUI(int aircraftType)
        {
            this.aircraftType = aircraftType;
        }

        public void LoadContent(ContentManager content1)
        {
            hudback = content1.Load<Texture2D>("Textures//HUDBack");
            hudmiddle = content1.Load<Texture2D>("Textures//HUDMiddle");
            h1 = content1.Load<Texture2D>("Textures//h1");
            h2 = content1.Load<Texture2D>("Textures//h2");
            h3 = content1.Load<Texture2D>("Textures//h3");
            h4 = content1.Load<Texture2D>("Textures//h4");
            h5 = content1.Load<Texture2D>("Textures//h5");
            h6 = content1.Load<Texture2D>("Textures//h6");
            h7 = content1.Load<Texture2D>("Textures//h7");
            h8 = content1.Load<Texture2D>("Textures//h8");
            h9 = content1.Load<Texture2D>("Textures//h9");
            h10 = content1.Load<Texture2D>("Textures//h10");
            s1 = content1.Load<Texture2D>("Textures//s1");
            s2 = content1.Load<Texture2D>("Textures//s2");
            s3 = content1.Load<Texture2D>("Textures//s3");
            s4 = content1.Load<Texture2D>("Textures//s4");
            s5 = content1.Load<Texture2D>("Textures//s5");
            s6 = content1.Load<Texture2D>("Textures//s6");
            s7 = content1.Load<Texture2D>("Textures//s7");
            s8 = content1.Load<Texture2D>("Textures//s8");
            s9 = content1.Load<Texture2D>("Textures//s9");
            s10 = content1.Load<Texture2D>("Textures//s10");
            reticle = content1.Load<Texture2D>("Textures//crosshair2");
           

        }

        public void Update(Player player)
        {
            float speed = aircraftType == 0 ? -velocity.Z : (velocity.Y + maxSpeed / 2);
            currentSpeed = (int)(Math.Round(speed / maxSpeed, 1) * 10);

            this.currentHealth = player.currentHealth;
            this.velocity = player.aircraft.velocity;
            this.position = player.aircraft.position;
            this.rotation = player.aircraft.modelRot;
            this.deathCount = player.DeathCount;
            this.killCount = player.KillCount;
            this.missiles = player.weaponFac.missileCount;
            this.missileTarget = player.targetIndex >= 0 ? true : false;
            this.bulletTarget = player.bulletLock;
            this.active = player.Active;

            targetColour = Color.White;

            if (missileTarget)
            {
                targetColour = Color.Blue;
            }

            if (bulletTarget)
            {
                targetColour = Color.Red;
            }
        }

        public void DrawGUI(Camera camera)
        {
            if (active)
            {
                speedPos = new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width * 0.89f), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height * 0.15f));
                missilePos = new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width * 0.89f), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height * 0.15f));

                //FIXME: Move this into the constructor, have it take spriteBatch etc on instantiate
                if (colour == null)
                {
                    colour = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    colour.SetData(new[] { Color.White });
                }

                //Project reticle position into viewport
                Vector3 reticlePos =
                    Game.GraphicsMgr.GraphicsDevice.Viewport.Project(
                        Vector3.Forward * Camera.FAR_PLANE,
                        camera.ProjectionMatrix(),
                        camera.ViewMatrix(),
                        Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position)
                    ) - new Vector3(Game.GraphicsMgr.GraphicsDevice.Viewport.X, Game.GraphicsMgr.GraphicsDevice.Viewport.Y, 0);

                Game.SpriteBatch.Draw(reticle, new Vector2(reticlePos.X, reticlePos.Y), null, targetColour, 0f, new Vector2(250, 250), scale / 4, SpriteEffects.None, 0f);
                Game.SpriteBatch.Draw(hudback, new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width * 0.89f), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height * 0.15f)), null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);

                switch (currentSpeed)
                {
                    case 10:
                        Game.SpriteBatch.Draw(s10, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 9:
                        Game.SpriteBatch.Draw(s9, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 8:
                        Game.SpriteBatch.Draw(s8, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 7:
                        Game.SpriteBatch.Draw(s7, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 6:
                        Game.SpriteBatch.Draw(s6, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 5:
                        Game.SpriteBatch.Draw(s5, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 4:
                        Game.SpriteBatch.Draw(s4, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        Game.SpriteBatch.Draw(s3, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        Game.SpriteBatch.Draw(s2, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 1:
                        Game.SpriteBatch.Draw(s1, speedPos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                }

                switch (missiles)
                {
                    case 5:
                        Game.SpriteBatch.Draw(h10, missilePos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 4:
                        Game.SpriteBatch.Draw(h8, missilePos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 3:
                        Game.SpriteBatch.Draw(h6, missilePos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 2:
                        Game.SpriteBatch.Draw(h4, missilePos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 1:
                        Game.SpriteBatch.Draw(h2, missilePos, null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
                        break;
                    case 0:
                        break;
                }

                Game.SpriteBatch.Draw(hudmiddle, new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width * 0.89f), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - ((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height * 0.15f)), null, Color.White, 0f, new Vector2(284, 284), scale, SpriteEffects.None, 0f);
            }

            //Readouts
            Game.SpriteBatch.DrawString(Game.MenuFont, "Kills: " + killCount.ToString(),
                new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - (260 * textScale), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - (80 * textScale)),
                Color.White, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            Game.SpriteBatch.DrawString(Game.MenuFont, "Deaths: " + deathCount.ToString(),
                new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width - (260 * textScale), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height - (50 * textScale)),
                Color.White, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);

            //Respawn Prompt
            if (!this.active)
            {
                Game.SpriteBatch.DrawString(Game.MenuFont, "DESTROYED",
                new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width/2 - (100 * textScale), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height/2 - (80 * textScale)),
                Color.White, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
                Game.SpriteBatch.DrawString(Game.MenuFont, "Press Fire to respawn",
                new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width/2 - (250 * textScale), (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height/2 - (20 * textScale)),
                Color.White, 0, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            }
        }
    }
}
