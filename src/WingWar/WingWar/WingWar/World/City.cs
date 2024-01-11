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
using WingWar.CityAssets.Buildings;
using WingWar.CityAssets;

namespace WingWar
{
    class City : GameObject
    {
        Ground ground;
        Ground ground2;
        Ground ground3;
        Ground ground4;
        Ground ground5;
        public BoundingSphere citySphere;
        public RectangularBuilding[] rectangularBuildings;
        static Random random = new Random();
        static int[] offsetOptions = {   0,
                                         100,
                                         -100
                                     };
        static int offset2;

        DebugShapeRenderer debugShape;
        BasicEffect boundingEffect;
        BasicEffect drawEffect;

        // TODO: factor all constrained random number generation into a separate class
        public City(GraphicsDevice device, int numWide, int numDeep, float gap, float minWidth, float maxWidth, float minDepth, float maxDepth, float minHeight, float maxHeight)
        {
            drawEffect = new BasicEffect(device);
            boundingEffect = new BasicEffect(device);

            boundingEffect.VertexColorEnabled = true;
            boundingEffect.PreferPerPixelLighting = true;
            boundingEffect.LightingEnabled = true;
            boundingEffect.EnableDefaultLighting();

            citySphere = new BoundingSphere(new Vector3(16000, -1000, -16000), 3000);

            debugShape = new DebugShapeRenderer();

            
            ground = new Ground(device, new Vector3(15390, 0, -17120), numWide, numDeep, maxWidth * 1.3f, maxDepth * 1.3f, gap, drawEffect);
            ground2 = new Ground(device, new Vector3(13480, 0, -17120), numWide, numDeep, maxWidth * 1.3f, maxDepth * 1.3f, gap, drawEffect);
            ground3 = new Ground(device, new Vector3(17300, 0, -17120), numWide, numDeep, maxWidth * 1.3f, maxDepth * 1.3f, gap, drawEffect);
            ground4 = new Ground(device, new Vector3(15390, 0, -15380), numWide, numDeep, maxWidth * 1.3f, maxDepth * 1.3f, gap, drawEffect);
            ground5 = new Ground(device, new Vector3(15390, 0, -19040), numWide, numDeep, maxWidth * 1.3f, maxDepth * 1.3f, gap, drawEffect);

            rectangularBuildings = new RectangularBuilding[numWide * numDeep];
            Random rand = new Random();
            for (int row = 0; row < numDeep; ++row)
            {
                for (int col = 0; col < numWide; ++col)
                {
                    float buildingWidth = MathHelper.Lerp(minWidth, maxWidth, (float)rand.NextDouble());
                    float buildingDepth = MathHelper.Lerp(minDepth, maxDepth, (float)rand.NextDouble());
                    float buildingHeight = MathHelper.Lerp(minHeight, maxHeight, (float)rand.NextDouble());
                    // center building on lot
                    Vector2 positionInLot;
                    positionInLot.X = (maxWidth - buildingWidth) / 2;
                    positionInLot.Y = (maxDepth - buildingDepth) / 2;
                    // get lot corner position
                    Vector2 lotPosition;
                    lotPosition.X = col * (maxWidth + gap);
                    lotPosition.Y = row * (maxDepth + gap);
                    // actual position of building
                    Vector2 buildingPosition = lotPosition + positionInLot;
                    offset2 = offsetOptions[random.Next(offsetOptions.Length)];
                    int offset3 = offset2;
                    int index = row * numWide + col;
                    rectangularBuildings[index] = new RectangularBuilding(device,
                        new Vector3(buildingPosition.X - offset3, 0, buildingPosition.Y - offset3), buildingWidth, buildingDepth, buildingHeight, drawEffect);
                    // set window textures
                    float textureDeltaRange = 1f;
                    float textureSizeFactor = 1 + textureDeltaRange * (float)rand.NextDouble();
                    int windowHeight = 8;
                    int windowWidth = 5;
                    Texture2D frontBack = WallTexture.Initialize(device,
                        (int)(textureSizeFactor * buildingHeight),
                        (int)(textureSizeFactor * buildingWidth),
                        windowHeight,
                        windowWidth
                    );
                    Texture2D leftRight = WallTexture.Initialize(device,
                        (int)(textureSizeFactor * buildingHeight),
                        (int)(textureSizeFactor * buildingWidth),
                        windowHeight,
                        windowWidth
                    );
                    Texture2D top = CityAssets.Texture.Generate(device, 1, 1, new Color(0, 0, 0));
                    rectangularBuildings[index].SetTextures(frontBack, leftRight, top);
                    Texture2D baseTexture = CityAssets.Texture.Generate(device, 1, 1, new Color(100, 100, 100));
                    rectangularBuildings[index].SetBaseTexture(baseTexture);
                }
            }
            for (int row = 1; row < numDeep; ++row)
            {
                for (int col = 1; col < numWide; ++col)
                {
                    float buildingWidth = MathHelper.Lerp(minWidth, maxWidth, (float)rand.NextDouble());
                    float buildingDepth = MathHelper.Lerp(minDepth, maxDepth, (float)rand.NextDouble());
                    float buildingHeight = MathHelper.Lerp(minHeight, maxHeight * 2.5f, (float)rand.NextDouble());
                    // center building on lot
                    Vector2 positionInLot;
                    positionInLot.X = (maxWidth - buildingWidth) / 2;
                    positionInLot.Y = (maxDepth - buildingDepth) / 2;
                    // get lot corner position
                    Vector2 lotPosition;
                    lotPosition.X = col * (maxWidth + gap);
                    lotPosition.Y = row * (maxDepth + gap);
                    // actual position of building
                    Vector2 buildingPosition = lotPosition + positionInLot;
                    offset2 = offsetOptions[random.Next(offsetOptions.Length)];
                    int offset3 = offset2;
                    int index = row * numWide + col;
                    rectangularBuildings[index] = new RectangularBuilding(device,
                        new Vector3(buildingPosition.X - offset3, 0, buildingPosition.Y - offset3), buildingWidth, buildingDepth, buildingHeight, drawEffect);
                    // set window textures
                    float textureDeltaRange = 1f;
                    float textureSizeFactor = 1 + textureDeltaRange * (float)rand.NextDouble();
                    int windowHeight = 8;
                    int windowWidth = 5;
                    Texture2D frontBack = WallTexture.Initialize(device,
                        (int)(textureSizeFactor * buildingHeight),
                        (int)(textureSizeFactor * buildingWidth),
                        windowHeight,
                        windowWidth
                    );
                    Texture2D leftRight = WallTexture.Initialize(device,
                        (int)(textureSizeFactor * buildingHeight),
                        (int)(textureSizeFactor * buildingWidth),
                        windowHeight,
                        windowWidth
                    );
                    Texture2D top = CityAssets.Texture.Generate(device, 1, 1, new Color(0, 0, 0));
                    rectangularBuildings[index].SetTextures(frontBack, leftRight, top);
                    Texture2D baseTexture = CityAssets.Texture.Generate(device, 1, 1, new Color(100, 100, 100));
                    rectangularBuildings[index].SetBaseTexture(baseTexture);
                }
            }
        }
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice device, float ambience)
        {
            foreach (RectangularBuilding building in rectangularBuildings)
            {
                building.Draw(viewMatrix, projectionMatrix, device);
            }
            ground.Draw(viewMatrix, projectionMatrix, device);
            ground2.Draw(viewMatrix, projectionMatrix, device);
            ground3.Draw(viewMatrix, projectionMatrix, device);
            ground4.Draw(viewMatrix, projectionMatrix, device);
            ground5.Draw(viewMatrix, projectionMatrix, device);

            //debugShape.Draw(citySphere, Color.White, viewMatrix, projectionMatrix, device, boundingEffect);
        }
    }
}

