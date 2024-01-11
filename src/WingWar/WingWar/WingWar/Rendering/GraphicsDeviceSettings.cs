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
    class GraphicsDeviceSettings
    {
        private static GraphicsDeviceSettings instance;

        public GraphicsDeviceSettings() {}

        public static GraphicsDeviceSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GraphicsDeviceSettings();
                }
                return instance;
            }
        }

        public bool wireMode;
        bool wireframeKeyPrevState;

        public void Wireframe()
        {
            if (Controls.Instance.Wireframe(0) && !wireframeKeyPrevState)
            {
                wireMode = !wireMode;
            }

            wireframeKeyPrevState = Controls.Instance.Wireframe(0);
        }

        public void SetWire()
        {
            RasterizerState rasterizerState = new RasterizerState();

            rasterizerState.FillMode = FillMode.WireFrame;
            rasterizerState.CullMode = CullMode.None;
            Game.GraphicsMgr.GraphicsDevice.RasterizerState = rasterizerState;
        }

        public void SetSolid()
        {
            RasterizerState rasterizerState = new RasterizerState();

            rasterizerState.FillMode = FillMode.Solid;
            rasterizerState.CullMode = CullMode.None;
            Game.GraphicsMgr.GraphicsDevice.RasterizerState = rasterizerState;
        }

        public void ResetAfterSpriteBatch()
        {
            Game.GraphicsMgr.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsMgr.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game.GraphicsMgr.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        }
    }
}
