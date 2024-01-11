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
    class BulletParticle
    {
        public VerticesFactory verticesFac;

        Vector3 normal;

        public BulletParticle()
        {
            verticesFac = new VerticesFactory();

            normal = new Vector3(0, 1, 0);

            BuildBullet();
            verticesFac.CreateObjectBuffer();
        }

        private void BuildBullet()
        {
            verticesFac.NUM_VERTICES = 8;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.vertices[0] = new VertexPositionNormal(new Vector3(-2,-2,-2), normal);
            verticesFac.vertices[1] = new VertexPositionNormal(new Vector3( 2,-2,-2), normal);
            verticesFac.vertices[2] = new VertexPositionNormal(new Vector3(-2, 2,-2), normal);
            verticesFac.vertices[3] = new VertexPositionNormal(new Vector3( 2, 2,-2), normal);
            verticesFac.vertices[4] = new VertexPositionNormal(new Vector3(-2,-2, 2), normal);
            verticesFac.vertices[5] = new VertexPositionNormal(new Vector3( 2,-2, 2), normal);
            verticesFac.vertices[6] = new VertexPositionNormal(new Vector3(-2, 2, 2), normal);
            verticesFac.vertices[7] = new VertexPositionNormal(new Vector3( 2, 2, 2), normal);
        }
    }
}
