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
    class VerticesFactory
    {
        public VertexPositionNormal[] vertices;
        public int[] indices;
        public int NUM_VERTICES, i = 0;
        public VertexBuffer objectBuffer;
        public DynamicVertexBuffer dynamicBuffer;

        public Vector3 topLeftFront, topLeftBack, topRightFront, topRightBack, btmLeftFront, btmLeftBack, btmRightFront, btmRightBack;

        private Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f);
        private Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f);
        private Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f);
        private Vector3 normalLeft = new Vector3(-1.0f, 0.0f, 0.0f);
        private Vector3 normalRight = new Vector3(1.0f, 0.0f, 0.0f);

        public VerticesFactory()
        {
        }

        public void CreateObjectBuffer()
        {
            objectBuffer = new VertexBuffer(
                Game.GraphicsMgr.GraphicsDevice,
                VertexPositionNormal.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            objectBuffer.SetData(vertices);
        }

        public void CreateDynamicBuffer()
        {
            dynamicBuffer = new DynamicVertexBuffer(
                Game.GraphicsMgr.GraphicsDevice,
                VertexPositionNormal.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            // Load the buffer
            dynamicBuffer.SetData(vertices);
        }

        public void AddVerticies()
        {
            // Add the vertices for the FRONT face. NORMAL FRONT
            vertices[i] = new VertexPositionNormal(topLeftFront, normalFront); i++;
            vertices[i] = new VertexPositionNormal(btmLeftFront, normalFront); i++;
            vertices[i] = new VertexPositionNormal(topRightFront, normalFront); i++;
            vertices[i] = new VertexPositionNormal(btmLeftFront, normalFront); i++;
            vertices[i] = new VertexPositionNormal(btmRightFront, normalFront); i++;
            vertices[i] = new VertexPositionNormal(topRightFront, normalFront); i++;

            // Add the vertices for the BACK face. NORMAL BACK
            vertices[i] = new VertexPositionNormal(topLeftBack, normalBack); i++;
            vertices[i] = new VertexPositionNormal(topRightBack, normalBack); i++;
            vertices[i] = new VertexPositionNormal(btmLeftBack, normalBack); i++;
            vertices[i] = new VertexPositionNormal(btmLeftBack, normalBack); i++;
            vertices[i] = new VertexPositionNormal(topRightBack, normalBack); i++;
            vertices[i] = new VertexPositionNormal(btmRightBack, normalBack); i++;

            // Add the vertices for the TOP face. NORMAL TOP
            vertices[i] = new VertexPositionNormal(topLeftFront, normalTop); i++;
            vertices[i] = new VertexPositionNormal(topRightBack, normalTop); i++;
            vertices[i] = new VertexPositionNormal(topLeftBack, normalTop); i++;
            vertices[i] = new VertexPositionNormal(topLeftFront, normalTop); i++;
            vertices[i] = new VertexPositionNormal(topRightFront, normalTop); i++;
            vertices[i] = new VertexPositionNormal(topRightBack, normalTop); i++;

            // Add the vertices for the BOTTOM face. NORAL BOTTOM
            vertices[i] = new VertexPositionNormal(btmLeftFront, normalBottom); i++;
            vertices[i] = new VertexPositionNormal(btmLeftBack, normalBottom); i++;
            vertices[i] = new VertexPositionNormal(btmRightBack, normalBottom); i++;
            vertices[i] = new VertexPositionNormal(btmLeftFront, normalBottom); i++;
            vertices[i] = new VertexPositionNormal(btmRightBack, normalBottom); i++;
            vertices[i] = new VertexPositionNormal(btmRightFront, normalBottom); i++;

            // Add the vertices for the LEFT face.
            vertices[i] = new VertexPositionNormal(topLeftFront, normalLeft); i++;
            vertices[i] = new VertexPositionNormal(btmLeftBack, normalLeft); i++;
            vertices[i] = new VertexPositionNormal(btmLeftFront, normalLeft); i++;
            vertices[i] = new VertexPositionNormal(topLeftBack, normalLeft); i++;
            vertices[i] = new VertexPositionNormal(btmLeftBack, normalLeft); i++;
            vertices[i] = new VertexPositionNormal(topLeftFront, normalLeft); i++;

            // Add the vertices for the RIGHT face. 
            vertices[i] = new VertexPositionNormal(topRightFront, normalRight); i++;
            vertices[i] = new VertexPositionNormal(btmRightFront, normalRight); i++;
            vertices[i] = new VertexPositionNormal(btmRightBack, normalRight); i++;
            vertices[i] = new VertexPositionNormal(topRightBack, normalRight); i++;
            vertices[i] = new VertexPositionNormal(topRightFront, normalRight); i++;
            vertices[i] = new VertexPositionNormal(btmRightBack, normalRight); i++;
        }
    }
}
