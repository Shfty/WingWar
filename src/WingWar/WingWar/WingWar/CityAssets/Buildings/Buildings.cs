using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class Buildings : GameObject
    {
        public int height;
        private Vector4 Colour;
        public BoundingBox collisionBox;

        public bool[] cameraView = new bool[4];

        public Buildings(Effect effect, int height, Vector4 Color, Vector3 position, Vector3 scale)
        {
            this.scale = scale;
            this.position = position;
            
            verticesFac = new VerticesFactory();

            this.Colour = Color;
            this.height = height;
            this.effect = effect;

            for (int i = 0; i < cameraView.Length; i++) { cameraView[i] = false; }
        }

        public void Building01()
        {
            verticesFac.NUM_VERTICES = 36 * 2 * height + 100;
            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            float base1 = 2f;
            float base2 = -2f;
            float offset = 1.5f;

            for (int i = 0; i < height; i++)
            {
                // Calculate the vertexPos of the vertices on the top face.
                verticesFac.topLeftFront = new Vector3(-1.0f, base1, -1.0f);
                verticesFac.topLeftBack = new Vector3(-1.0f, base1, 1.0f);
                verticesFac.topRightFront = new Vector3(1.0f, base1, -1.0f);
                verticesFac.topRightBack = new Vector3(1.0f, base1, 1.0f);

                // Calculate the vertexPos of the vertices on the bottom face.
                verticesFac.btmLeftFront = new Vector3(-1.0f, base2, -1.0f);
                verticesFac.btmLeftBack = new Vector3(-1.0f, base2, 1.0f);
                verticesFac.btmRightFront = new Vector3(1.0f, base2, -1.0f);
                verticesFac.btmRightBack = new Vector3(1.0f, base2, 1.0f);
                                                
                verticesFac.AddVerticies();

                // Calculate the vertexPos of the vertices on the top face.
                verticesFac.topLeftFront = (new Vector3(0.0f, offset + 2, 0.0f)) + new Vector3(-1.2f, 1.2f, -1.2f);
                verticesFac.topLeftBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.2f, 1.2f, 1.2f);
                verticesFac.topRightFront = (new Vector3(0.0f, offset + 2, 0.0f)) + new Vector3(1.2f, 1.2f, -1.2f);
                verticesFac.topRightBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.2f, 1.2f, 1.2f);

                // Calculate the vertexPos of the vertices on the bottom face.
                verticesFac.btmLeftFront = (new Vector3(0.0f, offset + 2, 0.0f)) + new Vector3(-1.2f, -1.2f, -1.2f);
                verticesFac.btmLeftBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.2f, -1.2f, 1.2f);
                verticesFac.btmRightFront = (new Vector3(0.0f, offset + 2, 0.0f)) + new Vector3(1.2f, -1.2f, -1.2f);
                verticesFac.btmRightBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.2f, -1.2f, 1.2f);

                verticesFac.AddVerticies();

                base1 += 4.5f; base2 += 4.5f; offset += 4.5f;
            }
            collisionBox = new BoundingBox((position + new Vector3(-45)), (position + new Vector3(45, 130 * height, 45)));
            verticesFac.CreateObjectBuffer();            
        }

        public void Building02()
        {
            verticesFac.NUM_VERTICES = 36 * 3 * height + 100;
            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            float base1 = 1f;
            float base2 = -1f;

            for (int i = 0; i < height * 3; i++)
            {                
                // Calculate the vertexPos of the vertices on the top face.
                verticesFac.topLeftFront = new Vector3(-1.711f, 0.289f + base1, 0.005f);
                verticesFac.topLeftBack = new Vector3(-0.706f, -0.716f + base1, 1.416f);
                verticesFac.topRightFront = new Vector3(-0.299f, 1.711f + base1, 0.005f);
                verticesFac.topRightBack = new Vector3(0.706f, 0.706f + base1, 1.416f);

                // Calculate the vertexPos of the vertices on the bottom face.
                verticesFac.btmLeftFront = new Vector3(-0.706f, -0.706f + base2, -1.416f);
                verticesFac.btmLeftBack = new Vector3(0.299f, -1.711f + base2, -0.005f);
                verticesFac.btmRightFront = new Vector3(0.706f, 0.716f + base2, -1.416f);
                verticesFac.btmRightBack = new Vector3(1.711f, -0.289f + base2, -0.005f);

                verticesFac.AddVerticies();
                
                base1 += 1f; base2 += 1f;
            }
            Quaternion startRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 45);
            this.rotation *= startRot;

            collisionBox = new BoundingBox((position + new Vector3(-50)), (position + new Vector3(50, 100 * height, 50)));

            verticesFac.CreateObjectBuffer();
        }

        public void Building03()
        {
            verticesFac.NUM_VERTICES = 36 * 2 * height + 100;
            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            float base1 = 2f;
            float base2 = -2f;
            float offset = 1.5f;

            for (int i = 0; i < height; i++)
            {
                // Calculate the vertexPos of the vertices on the top face.
                verticesFac.topLeftFront = new Vector3(-1.0f, base1, -1.0f);
                verticesFac.topLeftBack = new Vector3(-1.0f, base1, 1.0f);
                verticesFac.topRightFront = new Vector3(1.0f, base1, -1.0f);
                verticesFac.topRightBack = new Vector3(1.0f, base1, 1.0f);

                // Calculate the vertexPos of the vertices on the bottom face.
                verticesFac.btmLeftFront = new Vector3(-1.0f, base2, -1.0f);
                verticesFac.btmLeftBack = new Vector3(-1.0f, base2, 1.0f);
                verticesFac.btmRightFront = new Vector3(1.0f, base2, -1.0f);
                verticesFac.btmRightBack = new Vector3(1.0f, base2, 1.0f);

                verticesFac.AddVerticies();

                // Calculate the vertexPos of the vertices on the top face.
                verticesFac.topLeftFront = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.5f, 1.5f, -1.5f);
                verticesFac.topLeftBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.5f, 1.5f, 1.5f);
                verticesFac.topRightFront = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.5f, 1.5f, -1.5f);
                verticesFac.topRightBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.5f, 1.5f, 1.5f);

                // Calculate the vertexPos of the vertices on the bottom face.
                verticesFac.btmLeftFront = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.5f, -1.5f, -1.5f);
                verticesFac.btmLeftBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(-1.5f, -1.5f, 1.5f);
                verticesFac.btmRightFront = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.5f, -1.5f, -1.5f);
                verticesFac.btmRightBack = (new Vector3(0.0f, offset, 0.0f)) + new Vector3(1.5f, -1.5f, 1.5f);

                verticesFac.AddVerticies();

                base1 += 4.5f; base2 += 4.5f; offset += 4.5f;
            }

            collisionBox = new BoundingBox((position + new Vector3(-50)), (position + new Vector3(60, 130 * height, 50)));

            verticesFac.CreateObjectBuffer();
        }

        public override void Draw(Camera camera)
        {
            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["Colour"].SetValue(Colour);
            effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            effect.Parameters["Line"].SetValue(5.0f);
            effect.Parameters["DiffuseLightDirection"].SetValue(Materials.Instance.LightDirection());
            effect.Parameters["DiffuseLightColor"].SetValue(Materials.Instance.DiffuseLightColor());

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, verticesFac.vertices.Length - 2);
            }
        }
    }
}
