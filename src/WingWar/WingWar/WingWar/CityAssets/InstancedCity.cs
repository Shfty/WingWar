using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class InstancedCity : GameObject
    {
        int size;
        int height;

        ContentManager content;
        float offset = 300f;

        private float[,] noise;
        private float f = 10f;

        private Perlin perlin;

        public List<BoundingBox> bbList;
        public BoundingBox collisionBox;
        public BoundingBox[] buildingBoxes;
        public BoundingSphere citySphere;

        private Vector3 cityCentre;
        
        InstanceManager instanceManager;

        int arraySize = 0, arrayPos = 0;

        public InstancedCity(ContentManager content, Vector3 position, int size, float height)
        {
            this.content = content;
            this.position = position;
            this.size = size;

            this.height = ((int)height + 30) / 5;

            instanceManager = new InstanceManager(content);
            
            cityCentre = position;
            cityCentre.X = cityCentre.X + (size * 200) / 2;
            cityCentre.Z = cityCentre.Z - (size * 200) / 2;

            bbList = new List<BoundingBox>();
            buildingBoxes = new BoundingBox[size * size];
            citySphere = new BoundingSphere(cityCentre, size * 300);

            perlin = new Perlin(512);

            AddNoise();
            for (int i = 0; i < 10; i++) { Erode(50); }

            CalculateArraySize();
            GenerateBuilding();  
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

        public void CalculateArraySize()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (noise[i, j] > 2)
                    {
                        arraySize++;
                    }
                }
            }
        }

        public void GenerateBuilding()
        {
            instanceManager.vertexData = new InstanceDataVertex[arraySize];

            Random random = new Random();

            Building01();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (noise[i, j] > 2)
                    {
                        Matrix scale = Matrix.CreateScale(40f, noise[i, j] * 100, 40f);

                        collisionBox = new BoundingBox((new Vector3(-40 + (i + position.X), (-50 * noise[i, j] * 2) - 500, -40 + (j + position.Z))),
                            new Vector3(40 + (i + position.X), (50 * noise[i, j] * 2) - 500, 40 + (j + position.Z)));
                        bbList.Add(collisionBox);

                        instanceManager.vertexData[arrayPos] = new InstanceDataVertex(scale * Matrix.CreateTranslation
                            (new Vector3(i + position.X, -500, j + position.Z)), SetColour(random.Next(0, 3)));

                        arrayPos++;

                    }
                    position.Z -= offset;
                }

                position.X += offset;
                position.Z += offset * size;
            }
            buildingBoxes = bbList.ToArray();

            instanceManager.InstancedVertexBuffer();
        }

        public void Update()
        {
        }      

        private Color SetColour(int randNum)
        {
            Color cubeColor = new Color();

            switch (randNum)
            {
                case 0:
                    cubeColor = new Color(255, 15, 15, 255);
                    break;
                case 1:
                    cubeColor = new Color(255, 255, 15, 255);
                    break;
                case 2:
                    cubeColor = new Color(255, 165, 0, 255);
                    break;
            }

            return cubeColor;
        }

        void Building01()
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex((normal - side1 - side2), normal);
                instanceManager.AddVertex((normal - side1 + side2), normal);
                instanceManager.AddVertex((normal + side1 + side2), normal);
                instanceManager.AddVertex((normal + side1 - side2), normal);
            }

            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex((normal - side1 - side2) * new Vector3(1.5f, .5f, 1.5f), normal);
                instanceManager.AddVertex((normal - side1 + side2) * new Vector3(1.5f, .5f, 1.5f), normal);
                instanceManager.AddVertex((normal + side1 + side2) * new Vector3(1.5f, .5f, 1.5f), normal);
                instanceManager.AddVertex((normal + side1 - side2) * new Vector3(1.5f, .5f, 1.5f), normal);
            }

            instanceManager.InitializePrimitive();
        }
        public override void Draw(Camera camera)
        {
            instanceManager.Draw(camera);

            if (DebugDisplay.Instance.DrawDebug)
            {
                foreach (BoundingBox collisionBox in bbList)
                {
                    //DebugDisplay.Instance.Draw(collisionBox, camera.ViewMatrix(), camera.ProjectionMatrix());

                }
            }
        }
    }
}