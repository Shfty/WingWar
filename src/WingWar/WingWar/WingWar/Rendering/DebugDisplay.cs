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
    class DebugDisplay
    {
        private static DebugDisplay instance;

        public static DebugDisplay Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DebugDisplay();
                }

                return instance;
            }
        }

        private DebugShapeRenderer debugDraw;
        private BasicEffect effect;
        private bool drawDebug = false;
        private Color color;

        public DebugDisplay() 
        {
            debugDraw = new DebugShapeRenderer();
            effect = new BasicEffect(Game.GraphicsMgr.GraphicsDevice);
            effect.VertexColorEnabled = true;
            color = Color.White;
        }

        public bool DrawDebug
        {
            get { return drawDebug; }
            set { drawDebug = value; }
        }

        public void Draw(BoundingBox collisionBox, Matrix viewMatrix, Matrix projectionMatrix)
        {
            debugDraw.DrawBox(collisionBox, color, viewMatrix, projectionMatrix, effect);
        }

        public void Draw(BoundingSphere sphere, Matrix viewMatrix, Matrix projectionMatrix)
        {
            debugDraw.DrawSphere(sphere, color, viewMatrix, projectionMatrix, effect);
        }

        public void Update()
        {
            if (DrawDebug)
            {
                GraphicsDeviceSettings.Instance.SetWire();
            }
            else
            {
                GraphicsDeviceSettings.Instance.SetSolid();
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (DrawDebug)
            {
                FPS.Instance.DrawFPS(gameTime);
            }
        }
    }
}
