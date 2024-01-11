
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar
{
    public class DebugShapeRenderer
    {   
        static VertexBuffer vertBuffer;
        static int sphereResolution;
        /*
        VertexPositionColor[] boxVertices;
        short[] boxIndices;
        
        private void InitializeBox(BoundingBox collisionBox, Color color)
        {
            Vector3 v1 = collisionBox.Min;
            Vector3 v2 = collisionBox.Max;

            boxVertices = new VertexPositionColor[8];

            boxVertices[0] = new VertexPositionColor(v1, color);
            boxVertices[1] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v1.Z), color);
            boxVertices[2] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v2.Z), color);
            boxVertices[3] = new VertexPositionColor(new Vector3(v1.X, v1.Y, v2.Z), color);

            boxVertices[4] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v1.Z), color);
            boxVertices[5] = new VertexPositionColor(new Vector3(v2.X, v2.Y, v1.Z), color);
            boxVertices[6] = new VertexPositionColor(v2, color);
            boxVertices[7] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v2.Z), color);

            short[] boxIndices = { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

            this.boxIndices = boxIndices;
        }*/        

        private void InitializeSphere(int sphereResolution, Color sphereColor)
        {
            DebugShapeRenderer.sphereResolution = sphereResolution;
            
            VertexPositionColor[] verts = new VertexPositionColor[(sphereResolution + 1) * 3];

            int index = 0;

            float step = MathHelper.TwoPi / (float)sphereResolution;

            //create the loop on the XY plane first
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                    sphereColor);
            }

            //next on the XZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                    sphereColor);
            }

            //finally on the YZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                    sphereColor);
            }

            vertBuffer = new VertexBuffer(Game.GraphicsMgr.GraphicsDevice, typeof(VertexPositionColor), verts.Length, BufferUsage.None);
            vertBuffer.SetData(verts);
        }

        public void DrawBox(BoundingBox collisionBox, Color color, Matrix viewMatrix, Matrix projectionMatrix, BasicEffect effect)
        {/*
            if (boxVertices == null)
                InitializeBox(collisionBox, Color.White);
            
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = false;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, boxVertices, 0, 8, boxIndices, 0, 12);
            }*/

            Vector3 v1 = collisionBox.Min;
            Vector3 v2 = collisionBox.Max;

            VertexPositionColor[] boxVertices = new VertexPositionColor[8];

            boxVertices[0] = new VertexPositionColor(v1, color);
            boxVertices[1] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v1.Z), color);
            boxVertices[2] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v2.Z), color);
            boxVertices[3] = new VertexPositionColor(new Vector3(v1.X, v1.Y, v2.Z), color);

            boxVertices[4] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v1.Z), color);
            boxVertices[5] = new VertexPositionColor(new Vector3(v2.X, v2.Y, v1.Z), color);
            boxVertices[6] = new VertexPositionColor(v2, color);
            boxVertices[7] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v2.Z), color);
            short[] boxIndices = { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = false;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, boxVertices, 0, 8, boxIndices, 0, 12);
            }

        }

        public void DrawSphere(BoundingSphere sphere, Color color, Matrix viewMatrix, Matrix projectionMatrix, BasicEffect effect)
        {
            if (vertBuffer == null)
                InitializeSphere(30, color);

            Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(vertBuffer);

            Matrix oldWorldTransform = effect.World;

            effect.World =
                 Matrix.CreateScale(sphere.Radius) *
                 Matrix.CreateTranslation(sphere.Center);
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.LightingEnabled = false;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //render each circle individually
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);
            }

            effect.World = oldWorldTransform;
        }
    }
}
