using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar.CityAssets
{
    class TexturedQuad
    {
        private Vector3 upperLeft;
        private Vector3 upperRight;
        private Vector3 lowerLeft;
        private Vector3 lowerRight;
        private Vector3 normal;
        private Vector3 up;
        private Vector3 left;
        private float width;
        private float height;

        private VertexPositionNormalTexture[] vertices;
        private int[] indices;

        public VertexPositionNormalTexture[] Vertices { get { return vertices; } }
        public int[] Indices { get { return indices; } }

        public TexturedQuad(float width, float height)
            : this(Vector3.Zero, Vector3.Backward, Vector3.Up, width, height)
        { }

        public TexturedQuad(Vector3 upperLeft, Vector3 normal, Vector3 up, float width, float height)
        {
            this.upperLeft = upperLeft;
            this.normal = normal;
            this.up = up;
            this.width = width;
            this.height = height;

            this.vertices = new VertexPositionNormalTexture[4];
            this.indices = new int[6];

            this.left = Vector3.Cross(normal, up);
            this.upperRight = upperLeft - left * width;
            this.lowerLeft = upperLeft - up * height;
            this.lowerRight = upperRight - up * height;

            FillVertices();
        }

        private void FillVertices()
        {
            // normals
            for (int i = 0; i < Vertices.Length; ++i)
                vertices[i].Normal = normal;

            // position and texture
            vertices[0].Position = this.lowerLeft;
            vertices[0].TextureCoordinate = Vector2.UnitY;
            vertices[1].Position = this.upperLeft;
            vertices[1].TextureCoordinate = Vector2.Zero;
            vertices[2].Position = this.lowerRight;
            vertices[2].TextureCoordinate = Vector2.One;
            vertices[3].Position = this.upperRight;
            vertices[3].TextureCoordinate = Vector2.UnitX;

            // indices, clockwise winding
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 3;
        }
    }
}
