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
    class Shield : GameObject
    {
        private VertexPositionNormal[] vertices;
        public BoundingSphere shieldSphere;

        Vector4 colorMin, colorMax, finalColor;
        float variance = 0.0f, varianceAmt = 0.005f;

        public Shield(int sides, Quaternion rotation, Vector3 position, Vector3 scale)
        {
            colorMin = new Vector4(0.0f, 0.0f, 0.8f, 0.4f);
            colorMax = new Vector4(0.3f, 0.0f, 0.8f, 0.4f);

            this.rotation = rotation;
            this.position = position;
            this.scale = scale;
            this.effect = Game.ContentMgr.Load<Effect>("Effects//AlphaShader");

            verticesFac = new VerticesFactory();
            BuildShield(sides);
            SetPosition();

            shieldSphere = new BoundingSphere(position + new Vector3(14000,-14200, 0), 14200f);
        }

        private void BuildShield(int sides)
        {
            List<VertexPositionNormal> verts = CreateGeosphere(sides, new Vector3(100,100,100));
            vertices = verts.ToArray();

            verticesFac.vertices = vertices;

            verticesFac.CreateObjectBuffer();
        }

        public void Update()
        {
            variance += varianceAmt;

            if (variance >= 0.8f)
                varianceAmt = -varianceAmt;

            if (variance <= 0.0f)
                varianceAmt = -varianceAmt;

            finalColor = new Vector4(MathHelper.Lerp(0.0f, 0.38f, variance), 0.0f, MathHelper.Lerp(0.4f, 0.8f, variance), 0.4f);
        }

        public List<VertexPositionNormal> CreateGeosphere(int sides, Vector3 xyz)
        {
            List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
            double theta1 = 0, theta2 = 0, theta3 = 0;

            double ex = 0, px = 0, cx = xyz.X;
            double ey = 0, py = 0, cy = xyz.Y;
            double ez = 0, pz = 0, cz = 0, r = xyz.Z;

            for (int j = 0; j < sides / 4; j++)
            {
                theta1 = j * MathHelper.TwoPi / sides - MathHelper.PiOver2;
                theta2 = (j + 1) * MathHelper.TwoPi / sides - MathHelper.PiOver2;
                for (int i = 0; i <= sides; i++)
                {
                    theta3 = i * MathHelper.TwoPi / sides;

                    ex = Math.Cos(theta1) * Math.Cos(theta3);
                    ey = Math.Sin(theta1);
                    ez = Math.Cos(theta1) * Math.Sin(theta3);
                    px = cx + r * ex;
                    py = cy + r * ey;
                    pz = cz + r * ez;
                    vertices.Add(new VertexPositionNormal(new Vector3((float)px, (float)py, (float)pz), new Vector3(0,1,0)));
                    ex = Math.Cos(theta2) * Math.Cos(theta3);
                    ey = Math.Sin(theta2);
                    ez = Math.Cos(theta2) * Math.Sin(theta3);
                    px = cx + r * ex;
                    py = cy + r * ey;
                    pz = cz + r * ez;
                    vertices.Add(new VertexPositionNormal(new Vector3((float)px, (float)py, (float)pz), new Vector3(0,1,0)));
                }
            }
            return vertices;
        }

        private void SetPosition()
        {
            Quaternion startrot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), 0f)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), 0f);

            rotation = startrot;
        }

        public override void Draw(Camera camera)
        {
            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["Colour"].SetValue(finalColor);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, verticesFac.vertices.Length - 2);
            }

            if (DebugDisplay.Instance.DrawDebug)
            {
                DebugDisplay.Instance.Draw(shieldSphere, camera.ViewMatrix(), camera.ProjectionMatrix());
            }
        }
    }
}
