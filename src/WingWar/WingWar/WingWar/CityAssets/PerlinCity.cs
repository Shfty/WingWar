using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class PerlinCity
    {
        private Vector3 position;
        private Vector3 cityCentre;
        private Vector3 scale;
        private Vector4 color;
        private Effect effect;
        private Random rand;
        private int size, height, heightWeight, buildingSelect, colorSelect, offset;

        private bool cull;
        private float[,] noise;
        private float f = 10f;

        public List<Buildings> buildingList;
        public BoundingBox[] buildingBoxes;
        public BoundingSphere citySphere;
        private Perlin perlin;

        DebugShapeRenderer debugShape;
        BasicEffect Zeffect;

        public PerlinCity(Vector3 position, Vector3 scale, ContentManager Content, int size, float height, bool cull)
        {
            this.position = position;
            this.scale = scale;
            this.size = size;
            this.cull = cull;

            debugShape = new DebugShapeRenderer();
            Zeffect = new BasicEffect(Game.GraphicsMgr.GraphicsDevice);

            cityCentre = position;
            cityCentre.X = cityCentre.X + (size * 200) / 2;
            cityCentre.Z = cityCentre.Z - (size * 200) / 2;

            rand = new Random(16);
            buildingSelect = rand.Next(0, 3);
            heightWeight = rand.Next(0, 7);
            colorSelect = rand.Next(0, 3);
            offset = 300;

            perlin = new Perlin(512);
            effect = Content.Load<Effect>("Effects//CityShader");

            this.height = ((int)height + 30) / 5;

            buildingList = new List<Buildings>();
            buildingBoxes = new BoundingBox[size * size];
            citySphere = new BoundingSphere(cityCentre, size * 300);

            AddNoise();
            for (int i = 0; i < 10; i++) { Erode(50); }
            Generate();
        }

        private void AddNoise()
        {
            float temp = 0;

            noise = new float[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    temp = ((perlin.Noise(f * x / size, f * y / size, 0) + 0.2f) * height);
                    noise[x, y] += temp;
                }
            }
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
                                float d_i = noise[i, j] - noise[i + u, j + v];
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
                        noise[i, j] -= d_h;
                        noise[i + match[0], j + match[1]] += d_h;
                    }
                }
            }
        }

        public void Generate()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int k = 0;

                    switch (colorSelect)
                    {
                        case 0: color = new Vector4(0.8f, 0.1f, 0.1f, 1.0f); break;
                        case 1: color = new Vector4(1.0f, 1.0f, 0.2f, 1.0f); break;
                        case 2: color = new Vector4(1.0f, 0.6f, 0.1f, 1.0f); break;
                    }

                    if (noise[i, j] > 2)
                    {
                        Buildings b = new Buildings(effect, (int)noise[i, j], color, position, scale);
                        buildingBoxes[k] = b.collisionBox;


                        switch (buildingSelect)
                        {
                            case 0:
                                b.Building01();
                                break;
                            case 1:
                                b.Building02();
                                break;
                            case 2:
                                b.Building03();
                                break;
                        }

                        buildingList.Add(b);
                    }

                    buildingSelect = rand.Next(0, 3);
                    heightWeight = rand.Next(0, 3);
                    colorSelect = rand.Next(0, 5);

                    position.Z -= offset;
                    k++;
                }

                position.X += offset;
                position.Z += offset * size;
            }

        }

        public void Draw(Camera camera, float ambience, int playerIndex)
        {
            foreach (Buildings building in buildingList)
            {
                if (building.cameraView[playerIndex - 1] == true)
                {
                    building.Draw(camera);
                }

                building.cameraView[playerIndex - 1] = false;
            }
        }

        public void Draw(Camera camera)
        {
            foreach (Buildings building in buildingList)
            {
                building.Draw(camera);
            }

            //debugShape.Draw(citySphere, Color.White, viewMatrix, projectionMatrix, Zeffect);
        }
    }
}