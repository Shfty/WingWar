using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class Tunnels : GameObject
    {
        int size;
        int height;

        ContentManager content;
        float offset = 300f;

        private float[,] noise;
        private float f = 10f;

        private Perlin perlin;

        public List<BoundingBox> tunnelList;
        public BoundingBox collisionBox;
        public BoundingBox[] tunnelBoxes;
        
        InstanceManager instanceManager;

        int arraySize = 0, arrayPos = 0;

        public Tunnels(ContentManager content, Vector3 position, int size, float height, List<BoundingBox> tunnelList)
        {
            this.content = content;
            this.position = position;
            this.size = size;
            this.height = ((int)height + 30) / 5;
            
            instanceManager = new InstanceManager(content);

            this.tunnelList = tunnelList;
            tunnelBoxes = new BoundingBox[size * size];

            perlin = new Perlin(512);

            AddNoise();
            for (int i = 0; i < 10; i++) { Erode(50); }

            CalculateArraySize();
            GenerateBridge();
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
                    if (noise[i, j] > 10.8f && noise[i, j] < 12)
                    {
                        arraySize++;
                    }
                }
            }
        }

        public void GenerateBridge()
        {
            instanceManager.vertexData = new InstanceDataVertex[arraySize];

            Random random = new Random();

            Building01();
            Building02();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (noise[i, j] > 10.8f && noise[i, j] < 12)
                    {
                        Matrix scale = Matrix.CreateScale(40f, 40f, 40f);

                        collisionBox = new BoundingBox((new Vector3(-80 + (i + position.X + 140), -8 + (noise[i, j] * 440 - 5000), -400 + (j + position.Z))),
                            new Vector3(80 + (i + position.X + 140), 8 + (noise[i, j] * 440 - 5000), 400 + (j + position.Z)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-80 + (i + position.X + 140), -8 + (noise[i, j] * 440 - 5235), -400 + (j + position.Z))),
                            new Vector3(80 + (i + position.X + 140), 8 + (noise[i, j] * 440 - 5235), 400 + (j + position.Z)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-8 + (i + position.X + 60), -120 + (noise[i, j] * 440 - 5120), -400 + (j + position.Z))),
                            new Vector3(8 + (i + position.X + 60), 120 + (noise[i, j] * 440 - 5120), 400 + (j + position.Z)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-8 + (i + position.X + 230), -120 + (noise[i, j] * 440 - 5120), -400 + (j + position.Z))),
                            new Vector3(8 + (i + position.X + 230), 120 + (noise[i, j] * 440 - 5120), 400 + (j + position.Z)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-400 + (i + position.X), -8 + (noise[i, j] * 440 - 4760), -80 + (j + position.Z + 140))),
                            new Vector3(400 + (i + position.X), 8 + (noise[i, j] * 440 - 4760), 80 + (j + position.Z + 140)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-400 + (i + position.X), -8 + (noise[i, j] * 440 - 4995), -80 + (j + position.Z + 140))),
                            new Vector3(400 + (i + position.X), 8 + (noise[i, j] * 440 - 4995), 80 + (j + position.Z + 140)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-400 + (i + position.X), -120 + (noise[i, j] * 440 - 4880), -8 + (j + position.Z + 60))),
                            new Vector3(400 + (i + position.X), 120 + (noise[i, j] * 440 - 4880), 8 + (j + position.Z + 60)));
                        tunnelList.Add(collisionBox);

                        collisionBox = new BoundingBox((new Vector3(-400 + (i + position.X), -120 + (noise[i, j] * 440 - 4880), -8 + (j + position.Z + 230))),
                            new Vector3(400 + (i + position.X), 120 + (noise[i, j] * 440 - 4880), 8 + (j + position.Z + 230)));
                        tunnelList.Add(collisionBox);

                        instanceManager.vertexData[arrayPos] = new InstanceDataVertex(scale * Matrix.CreateTranslation
                            (new Vector3(i + position.X, noise[i, j] * 440 - 5000, j + position.Z)), SetColour());

                        arrayPos++;

                    }
                    position.Z -= offset;
                }

                position.X += offset;
                position.Z += offset * size;
            }

            tunnelBoxes = tunnelList.ToArray();

            instanceManager.InstancedVertexBuffer();
        }

        public void Update()
        {
        }

        private Color SetColour()
        {
            Color cubeColor = new Color();

            cubeColor = new Color(192, 192, 192, 225);

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

            // Create each face in turn.
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(1.9f, 0, 0)) - side1 - side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, 0, 0)) - side1 + side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, 0, 0)) + side1 + side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, 0, 0)) + side1 - side2) * new Vector3(2f, .2f, 10), normal);
            }

            // Create each face in turn.
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(1.9f, -30, 0)) - side1 - side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, -30, 0)) - side1 + side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, -30, 0)) + side1 + side2) * new Vector3(2f, .2f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(1.9f, -30, 0)) + side1 - side2) * new Vector3(2f, .2f, 10), normal);
            }


            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(8f, -1, 0)) - side1 - side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(8f, -1, 0)) - side1 + side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(8f, -1, 0)) + side1 + side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(8f, -1, 0)) + side1 - side2) * new Vector3(.2f, 3f, 10), normal);
            }
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(30f, -1, 0)) - side1 - side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(30f, -1, 0)) - side1 + side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(30f, -1, 0)) + side1 + side2) * new Vector3(.2f, 3f, 10), normal);
                instanceManager.AddVertex(((normal + new Vector3(30f, -1, 0)) + side1 - side2) * new Vector3(.2f, 3f, 10), normal);
            }
            instanceManager.InitializePrimitive();
        }

        void Building02()
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

            // Create each face in turn.
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(0, 30, 1.9f)) - side1 - side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 30, 1.9f)) - side1 + side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 30, 1.9f)) + side1 + side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 30, 1.9f)) + side1 - side2) * new Vector3(10, .2f, 2), normal);
            }

            // Create each face in turn.
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(0, -0, 1.9f)) - side1 - side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -0, 1.9f)) - side1 + side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -0, 1.9f)) + side1 + side2) * new Vector3(10, .2f, 2), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -0, 1.9f)) + side1 - side2) * new Vector3(10, .2f, 2), normal);
            }


            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 8f)) - side1 - side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 8f)) - side1 + side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 8f)) + side1 + side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 8f)) + side1 - side2) * new Vector3(10, 3f, .2f), normal);
            }
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 30f)) - side1 - side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 30f)) - side1 + side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 30f)) + side1 + side2) * new Vector3(10, 3f, .2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 1, 30f)) + side1 - side2) * new Vector3(10, 3f, .2f), normal);
            }
            instanceManager.InitializePrimitive();
        }

        public override void Draw(Camera camera)
        {
            instanceManager.Draw(camera);
        }
    }
}
