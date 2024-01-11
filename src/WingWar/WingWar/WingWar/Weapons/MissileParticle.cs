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
    class MissileParticle
    {
        public VerticesFactory verticesFac;

        Vector3 topLeft, topRight, botLeft, botRight, normal;

        public MissileParticle()
        {
            verticesFac = new VerticesFactory();

            normal = new Vector3(0, 1, 0);

            BuildBullet();
            verticesFac.CreateObjectBuffer();
        }

        private void BuildBullet()
        {
            verticesFac.NUM_VERTICES = 4;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            // Calculate the vertexPos of the vertices on the top face.
            topRight = new Vector3(0.5f, 0.5f, 0);
            topLeft = new Vector3(-0.5f, 0.5f, 0);
            botRight = new Vector3(0.5f, 0.5f, 0);
            botLeft = new Vector3(-0.5f, 0.5f, 0);

            AddVertices();
        }

        private void AddVertices()
        {
            verticesFac.vertices[0] = new VertexPositionNormal(topRight, normal);
            verticesFac.vertices[1] = new VertexPositionNormal(topLeft, normal);
            verticesFac.vertices[2] = new VertexPositionNormal(botRight, normal);
            verticesFac.vertices[3] = new VertexPositionNormal(botLeft, normal);
        }
    }
}
