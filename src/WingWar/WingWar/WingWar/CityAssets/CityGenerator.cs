using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class CityGenerator
    {
        private Vector3 position;
        private Vector3 scale;
        private Vector4 color;
        private Effect effect;
        private Random rand;
        private int height, heightWeight, buildingSelect, colorSelect, offset;
        
        private bool cull;
       
        public List<Buildings> buildingList;
        public BoundingBox[] buildingBoxes;
        public BoundingSphere citySphere;

        int[,] cityPlan;
        int size;
        static Random random = new Random();
        
        public CityGenerator(Vector3 position, Vector3 scale, ContentManager Content, int size, float height, bool cull)
        {
            this.position = position;
            this.scale = scale;
            this.size = size;
            this.cull = cull;

            rand = new Random(16);
            buildingSelect = rand.Next(0, 3);
            heightWeight = rand.Next(0, 7);
            colorSelect = rand.Next(0, 3);
            offset = 300;

            effect = Content.Load<Effect>("Effects//CityShader");

            this.height = ((int)height + 30) / 10;

            cityPlan = new int[size*2, size];

            buildingList = new List<Buildings>();
            buildingBoxes = new BoundingBox[size];
            citySphere = new BoundingSphere(position + new Vector3(3000, -2500, -3000), 6000f);
            LoadCityPlan();
            City();
        }
        
        private void LoadCityPlan()
        {/*
            cityPlan = new int[,]
             {
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0},
                  {0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0},
                  {0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0},
                  {0,0,0,0,1,0,1,1,0,0,0,1,1,0,1,0,0,0,0},
                  {0,0,0,1,1,0,1,0,0,1,0,0,1,0,1,1,0,0,0},
                  {0,0,0,1,1,0,0,0,1,1,1,0,0,0,1,1,0,0,0},
                  {0,0,0,1,1,0,1,0,0,1,0,0,1,0,1,1,0,0,0},
                  {0,0,0,0,1,0,1,1,0,0,0,1,1,0,1,0,0,0,0},
                  {0,0,0,0,0,0,0,1,1,0,1,1,0,0,0,0,0,0,0},
                  {0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0},
                  {0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
             };*/
            
            for (int x = 0; x < size*2; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cityPlan[x, y] += BuildingProbality();
                }
            }
        }
        
        static int BuildingProbality()
        {
            if (random.NextDouble() < 0.8)
                return 1;

            return random.Next(0, 1);
        }
        
        void City()
        {
            int cityWidth = cityPlan.GetLength(0);
            int cityLength = cityPlan.GetLength(1);
            
            for (int x = 0; x < cityWidth; x++)
            {
                for (int z = 0; z < cityLength; z++)
                {
                    if (cityPlan[x, z] == 1)
                    {
                        Building();
                    }
                    if (cityPlan[x, z] == 0)
                    { 
                        position.Z -= offset*size;
                    }
                }
                position.X += offset;
                position.Z += offset * cityLength*size;
            }
        }

        void Building()
        {
            for (int i = 1; i < 2; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int origHeight = height;

                    int k = 0;

                    switch (heightWeight)
                    {
                        case 0: height -= 6; break;
                        case 1: height -= 4; break;
                        case 2: height -= 2; break;
                        case 3: height = origHeight; break;
                        case 4: height += 2; break;
                        case 5: height += 4; break;
                        case 6: height += 6; break;
                    }

                    if (height < 2) height = 2;

                    switch (colorSelect)
                    {
                        case 0: color = new Vector4(0.8f, 0.1f, 0.1f, 1.0f); break;
                        case 1: color = new Vector4(1.0f, 1.0f, 0.2f, 1.0f); break;
                        case 2: color = new Vector4(1.0f, 0.6f, 0.1f, 1.0f); break;
                    }

                    Buildings b = new Buildings(effect, height, color, position, scale);
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

                    height = origHeight;

                    buildingSelect = rand.Next(0, 3);
                    heightWeight = rand.Next(0, 3);
                    colorSelect = rand.Next(0, 5);
                    position.Z -= offset;
                    k++;
                }                
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
        }
    }
}
