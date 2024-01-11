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
    class GameObject
    {
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = new Vector3(1,1,1);

        public VerticesFactory verticesFac;
        public Effect effect;

        public Vector4 colour;

        public GameObject()
        {
            verticesFac = new VerticesFactory();
        }

        public virtual void BuildBuffer()
        {
            //No-op for base
            //Inherited classes should populate verticesFac here
        }

        public virtual void Draw(Camera camera)
        {
            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["Color"].SetValue(colour);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, verticesFac.vertices.Length - 2);
            }
        }
    }
}
