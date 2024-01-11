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
    class TerrainFactory : GameObject
    {
        public float[,] heightData;
        Perlin perlin;
        Random rand;
        private int size, seed;
        float freq, erodeSmooth, multiplicity, weight;
        VertexPositionNormal[] terrainVertices;

        public Vector3 cityPosition;
        public float citySize;

        bool menu;

        public TerrainFactory()
        {
            verticesFac = new VerticesFactory();
            rand = new Random((int)DateTime.Now.Second);
            //base.drawEffect = new BasicEffect(Game.GraphicsMgr.GraphicsDevice);

            position = Vector3.Zero;
            scale = new Vector3(200f, 50f, 200f);
            rotation = new Quaternion(0,0,0,1);
        }

        public void LoadContent(float multiplicity, float citySize, float weight, bool menu)
        {
            this.menu = menu;
            this.freq = rand.Next(10, 22);
            this.erodeSmooth = rand.Next(30, 80);
            this.weight = (weight - 50) / 200;
            this.citySize = citySize;
            this.multiplicity = ((multiplicity + 30) * 1f) + -this.weight * 100;
            this.effect = Game.ContentMgr.Load<Effect>("Effects//TerrainShader");

            GenerateTerrain();
        }

        private void GenerateTerrain()
        {
            seed = rand.Next(0, 512);
            perlin = new Perlin(seed);

            size = 160;
            heightData = new float[size, size];

            AddPerlinNoise(freq);
            for (int i = 0; i < 10; i++) { Erode(erodeSmooth); }
            Lake();
            Smoothen();

            verticesFac.vertices = CreateVertices();
            verticesFac.indices = CreateIndices();

            CalculateNormals();
        }

        private VertexPositionNormal[] CreateVertices()
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);

            terrainVertices = new VertexPositionNormal[(width * height)];

            int i = 0;

            for (int x = 0; x < height; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    Vector3 position = new Vector3(x, heightData[x, z], -z);
                    Vector3 normal = new Vector3(0, 0, 0);
                    terrainVertices[i++] = new VertexPositionNormal(position, normal);
                }
            }

            return terrainVertices;
        }

        private int[] CreateIndices()
        {
            int width = heightData.GetLength(0);
            int height = heightData.GetLength(1);

            int[] terrainIndices = new int[(width) * 2 * (height - 1)];

            int i = 0;
            int z = 0;

            while (z < height - 1)
            {
                for (int x = 0; x < width; x++)
                {
                    terrainIndices[i++] = x + z * width;
                    terrainIndices[i++] = x + (z + 1) * width;
                }
                z++;

                if (z < height - 1)
                {
                    for (int x = width - 1; x >= 0; x--)
                    {
                        terrainIndices[i++] = x + (z + 1) * width;
                        terrainIndices[i++] = x + z * width;
                    }
                }
                z++;
            }

            return terrainIndices;       
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < verticesFac.indices.Length / 3; i++)
            {
                int index1 = verticesFac.indices[i * 3];
                int index2 = verticesFac.indices[i * 3 + 1];
                int index3 = verticesFac.indices[i * 3 + 2];

                Vector3 side1 = verticesFac.vertices[index1].Position - verticesFac.vertices[index3].Position;
                Vector3 side2 = verticesFac.vertices[index1].Position - verticesFac.vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                verticesFac.vertices[index1].Normal = normal;
                verticesFac.vertices[index2].Normal = normal;
                verticesFac.vertices[index3].Normal = normal;
            }

            for (int i = 0; i < verticesFac.vertices.Length; i++)
            {
                verticesFac.vertices[i].Normal.Normalize();
            }
        }

        public void AddPerlinNoise(float f)
        {
            float temp = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    temp = (perlin.Noise(f * x / (float)size, f * y / (float)size, 0) + weight) * multiplicity;
                    if (temp < -20) { temp = -20; }
                    heightData[x,y] += temp;
                }
            }
        }

        private void Lake()
        {
            int startPosY = -10;
            int startPosX;
            int startPosZ;

            if (menu)
            {
                startPosX = 70;
                startPosZ = 70;
            }
            else
            {
                rand = new Random();
                startPosX = rand.Next(60, size - 60);
                rand = new Random();
                startPosZ = rand.Next(60, size - 60);
            }
            
            int volume = (int)citySize;

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (x >= startPosX && z >= startPosZ)
                    {
                        if (x <= (startPosX + volume) && z <= (startPosZ + volume))
                        {
                            heightData[x, z] = startPosY;
                        }
                    }
                }
            }

            citySize = volume / 2;
            cityPosition = new Vector3((startPosX * scale.X) + 1000, startPosY * scale.Y + 10, (-startPosZ * scale.Z) - 1000);
        }

        private void Erode(float smoothness)
        {
            for (int i = 1; i < size - 1; i++)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    float d_max = 0.0f;
                    int[] match = { 0, 0 };

                    for (int u = -1; u <= 1; u++)
                    {
                        for (int v = -1; v <= 1; v++)
                        {
                            if (Math.Abs(u) + Math.Abs(v) > 0)
                            {
                                float d_i = heightData[i, j] - heightData[i + u, j + v];
                                if (d_i > d_max)
                                {
                                    d_max = d_i;
                                    match[0] = u; match[1] = v;
                                }
                            }
                        }
                    }

                    if (0 < d_max && d_max <= (smoothness / (float)size))
                    {
                        float d_h = 0.5f * d_max;
                        heightData[i, j] -= d_h;
                        heightData[i + match[0], j + match[1]] += d_h;
                    }
                }
            }
        }

        private void Smoothen()
        {
            for (int i = 1; i < size - 1; ++i)
            {
                for (int j = 1; j < size - 1; ++j)
                {
                    float total = 0.0f;
                    for (int u = -1; u <= 1; u++)
                    {
                        for (int v = -1; v <= 1; v++)
                        {
                            total += heightData[i + u, j + v];
                        }
                    }

                    heightData[i, j] = total / 9.0f;
                }
            }
        }

        public override void Draw(Camera camera)
        {
            if (effect == null)
            {
                return;
            }

            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            effect.Parameters["DiffuseLightDirection"].SetValue(Materials.Instance.LightDirection());
            effect.Parameters["DiffuseLightColor"].SetValue(Materials.Instance.DiffuseLightColor());

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                //Apply the Passes
                pass.Apply();                 

                //Draw the Primitives of vertices
                Game.GraphicsMgr.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormal>
                    (PrimitiveType.TriangleStrip, verticesFac.vertices, 0, verticesFac.vertices.GetLength(0),
                    verticesFac.indices, 0, (int)(heightData.GetLength(0) * 2 * heightData.GetLength(1)) - 1000);
            }
        }
    }
}
