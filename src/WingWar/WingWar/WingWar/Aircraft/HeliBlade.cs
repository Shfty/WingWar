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
    class HeliBlade : GameObject
    {
        public float angle = 0;

        Vector4 Colour = new Vector4(0.6f, 0.6f, 0.6f, 1f);

        public HeliBlade(Helicopter heli, ContentManager Content)
        {
            this.position = heli.position;
            this.rotation = heli.rotation;
            this.scale = new Vector3(1, 1, 1);

            verticesFac = new VerticesFactory();
            this.effect = Content.Load<Effect>("Effects//CelShader");
            BuildBuffer();

            colour = new Vector4(0.5f, 0.5f, 0.4f, 1f);
        }

        public override void BuildBuffer()
        {
            Vector3 normal = new Vector3(0, 1, 0);

            verticesFac.NUM_VERTICES = 8;
            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.vertices[0] = new VertexPositionNormal(new Vector3(-0.222f, 1.892f, -0.5f), normal);
            verticesFac.vertices[1] = new VertexPositionNormal(new Vector3(0.222f, 1.892f, -0.5f), normal);
            verticesFac.vertices[2] = new VertexPositionNormal(new Vector3(-5.196f, 1.885f, 0.5f), normal);
            verticesFac.vertices[3] = new VertexPositionNormal(new Vector3(5.196f, 1.885f, 0.5f), normal);

            verticesFac.vertices[4] = new VertexPositionNormal(new Vector3(-4.516f, 1.643f, -0.5f), normal);
            verticesFac.vertices[5] = new VertexPositionNormal(new Vector3(4.516f, 1.643f, -0.5f), normal);
            verticesFac.vertices[6] = new VertexPositionNormal(new Vector3(-5.349f, 1.635f, 0.5f), normal);
            verticesFac.vertices[7] = new VertexPositionNormal(new Vector3(5.349f, 1.635f, 0.5f), normal);

            verticesFac.CreateObjectBuffer();
        }

        public override void Draw(Camera camera)
        {
            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            effect.CurrentTechnique = effect.Techniques["Technique2"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["Colour"].SetValue(Colour);
            effect.Parameters["Line"].SetValue(0.01f);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, verticesFac.vertices.Length - 2);
            }
        }
    }
}
