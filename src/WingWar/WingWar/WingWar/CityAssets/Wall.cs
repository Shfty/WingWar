using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar.CityAssets
{
    class Wall
    {
        private GraphicsDevice device;
        private  BasicEffect effect;
        private TexturedQuad quad;
        private Vector3 upperLeft;
        private Vector3 normal;
        private Vector3 up;
        private float width;
        private float height;
        private Texture2D texture;

        public Wall(GraphicsDevice device, Vector3 upperLeft, Vector3 normal, Vector3 up, float width, float height, BasicEffect effect)
        {
            this.effect = effect;
            this.device = device;
            this.upperLeft = upperLeft;
            this.normal = normal;
            this.up = up;
            this.width = width;
            this.height = height;
           
            this.quad = new TexturedQuad(upperLeft, normal, up, width, height);

            if (effect == null)
                effect = new BasicEffect(device);
        }

        public void SetTexture(Texture2D texture)
        {
                this.texture = texture;            
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice device, BasicEffect effect)
        {

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            effect.PreferPerPixelLighting = true;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    quad.Vertices, 0, quad.Vertices.Length,
                    quad.Indices, 0, quad.Indices.Length / 3
                );
            }
        }
    }
}
