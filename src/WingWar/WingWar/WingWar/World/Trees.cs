using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class Trees : GameObject
    {
        int size;
        float[,] height;
        float foliage;

        ContentManager content;
        //changes dif between trees 200's no more than 10
        float offset = 200.04f;

        private Perlin perlin;

        public List<BoundingBox> treeList;
        public BoundingBox[] treeBoxes;
        
        InstanceManager instanceManager;

        int arraySize = 0, arrayPos = 0;

        public Trees(ContentManager content, int size, float[,] height, float foliage)
        {
            this.content = content;
            position = new Vector3(-60, 0, -80);
            this.size = size;
            this.foliage = foliage;
            this.height = height;
            instanceManager = new InstanceManager(content);
            treeList = new List<BoundingBox>();
            treeBoxes = new BoundingBox[size * size];

            perlin = new Perlin(DateTime.Now.Millisecond);

            CalculateArraySize();
            GenerateTree(height);
        }

        private void CalculateArraySize()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (height[i, j] > -4 && height[i, j] < foliage)
                    {
                        arraySize++;
                    }
                }
            }
        }

        public void GenerateTree(float[,] height)
        {
            instanceManager.vertexData = new InstanceDataVertex[arraySize];

            Random random = new Random();

            BuildTree();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //amount of trees placed
                    if (height[i, j] > -4 && height[i, j] < foliage)
                    {
                        Matrix scale = Matrix.CreateScale(20f, 20f, 20f);
                        /*
                        treeBoxes[(j * size) + i] = new BoundingBox((new Vector3(-24 + (i + position.X), -24 + (height[i, j] * 50 + 51), -24 + (j + position.Z))),
                            new Vector3(24 + (i + position.X), 24 + (height[i, j] * 52 + 51), 24 + (j + position.Z)));
                        */
                        instanceManager.vertexData[arrayPos] = new InstanceDataVertex(scale * Matrix.CreateTranslation
                            (new Vector3(i + position.X, height[i, j] * 50 + 56.4f, j + position.Z)), SetColour(random.Next(0, 3)));
                        //51 change Y axis float .0f / number after does y and keep curv

                        arrayPos++;
                    }

                    position.Z -= offset;
                }
                position.X += offset;
                position.Z += offset * size;

                instanceManager.InstancedVertexBuffer();
            }
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
                    cubeColor = new Color(0, 180, 0, 255);
                    break;
                case 1:
                    cubeColor = new Color(0, 280, 0, 255);
                    break;
                case 2:
                    cubeColor = new Color(0, 240, 220, 255);
                    break;
            }

            return cubeColor;
        }

        void BuildTree()
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
                instanceManager.AddVertex(((normal + new Vector3(0, 0, 0)) - side1 - side2) * new Vector3(1.2f, 1.2f, 1.2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 0, 0)) - side1 + side2) * new Vector3(1.2f, 1.2f, 1.2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 0, 0)) + side1 + side2) * new Vector3(1.2f, 1.2f, 1.2f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, 0, 0)) + side1 - side2) * new Vector3(1.2f, 1.2f, 1.2f), normal);
            }

            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                instanceManager.CreateIndex();

                //first new Vector moves position, second scales
                // Four vertices per face.
                instanceManager.AddVertex(((normal + new Vector3(0, -1, 0)) - side1 - side2) * new Vector3(.25f, 3f, .25f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -1, 0)) - side1 + side2) * new Vector3(.25f, 3f, .25f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -1, 0)) + side1 + side2) * new Vector3(.25f, 3f, .25f), normal);
                instanceManager.AddVertex(((normal + new Vector3(0, -1, 0)) + side1 - side2) * new Vector3(.25f, 3f, .25f), normal);
            }

            instanceManager.InitializePrimitive();
        }

        public override void Draw(Camera camera)
        {
            instanceManager.Draw(camera);
        }
    }
}