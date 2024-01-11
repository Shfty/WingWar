using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WingWar.CityAssets.Buildings
{
    class RectangularBuilding
    {
        GraphicsDevice device;
        BasicEffect effect;

        Wall[] verticalWalls;
        Wall[] verticalWalls2;
        Wall[] verticalWalls3;
        Wall[] verticalWalls4;
        Wall[] verticalWalls5;
        Wall roof;
        Wall roof2;
        Wall roof3;
        Wall roof4;
        Wall roof5;
        Wall buildingBase;
        Wall buildingBase2;
        Wall buildingBase3;
        Wall buildingBase4;
        Wall buildingBase5;

        public BoundingBox[] collisionBox = new BoundingBox[5];
        DebugShapeRenderer debugShape;

        /// <summary>
        /// Creates a simple rectangular building, oriented in the cardinal directions.
        /// </summary>
        /// <param name="position">far, bottom, left corner of building.</param>
        public RectangularBuilding(GraphicsDevice device, Vector3 position, float width, float depth, float height, BasicEffect effect)
        {
            this.effect = effect;

            debugShape = new DebugShapeRenderer();

            Building1(device, position, width, depth, height * 1.5f);
            Building2(device, position, width, depth, height);
            Building3(device, position, width, depth, height);
            Building4(device, position, width, depth, height);
            Building5(device, position, width, depth, height);
        }

        public void Building1(GraphicsDevice device, Vector3 position, float width, float depth, float height)
        {
            {
                this.device = device;
                Vector3 leftUpperLeft = (position + new Vector3(15500, -600, -17000)) + Vector3.Up * height;
                Vector3 nearUpperLeft = leftUpperLeft + Vector3.Backward * depth;
                Vector3 rightUpperLeft = nearUpperLeft + Vector3.Right * width;
                Vector3 farUpperLeft = rightUpperLeft + Vector3.Forward * depth;
                verticalWalls = new Wall[4];
                verticalWalls[0] = new Wall(device, nearUpperLeft, Vector3.Backward, Vector3.Up, width, height, effect);
                verticalWalls[1] = new Wall(device, rightUpperLeft, Vector3.Right, Vector3.Up, depth, height, effect);
                verticalWalls[2] = new Wall(device, farUpperLeft, Vector3.Forward, Vector3.Up, width, height, effect);
                verticalWalls[3] = new Wall(device, leftUpperLeft, Vector3.Left, Vector3.Up, depth, height, effect);
                roof = new Wall(device, leftUpperLeft, Vector3.Up, Vector3.Forward, width, depth, effect);
                buildingBase = new Wall(device, (position + new Vector3(15500, -490, -17000)) + Vector3.Up - new Vector3(50, 0, 50), Vector3.Up, Vector3.Forward, width * 2, depth * 2, effect);
                collisionBox[0] = new BoundingBox((position + new Vector3(15500, -600, -17000)) + new Vector3(0, 0, 0), (position + new Vector3(15500, -600, -17000)) + new Vector3(width, height, depth));
            }
        }
        public void Building2(GraphicsDevice device, Vector3 position, float width, float depth, float height)
        {
            {
                this.device = device;
                Vector3 leftUpperLeft = (position + new Vector3(13680, -600, -17000)) + Vector3.Up * height;
                Vector3 nearUpperLeft = leftUpperLeft + Vector3.Backward * depth;
                Vector3 rightUpperLeft = nearUpperLeft + Vector3.Right * width;
                Vector3 farUpperLeft = rightUpperLeft + Vector3.Forward * depth;
                verticalWalls2 = new Wall[4];
                verticalWalls2[0] = new Wall(device, nearUpperLeft, Vector3.Backward, Vector3.Up, width, height, effect);
                verticalWalls2[1] = new Wall(device, rightUpperLeft, Vector3.Right, Vector3.Up, depth, height, effect);
                verticalWalls2[2] = new Wall(device, farUpperLeft, Vector3.Forward, Vector3.Up, width, height, effect);
                verticalWalls2[3] = new Wall(device, leftUpperLeft, Vector3.Left, Vector3.Up, depth, height, effect);
                roof2 = new Wall(device, leftUpperLeft, Vector3.Up, Vector3.Forward, width, depth, effect);
                buildingBase2 = new Wall(device, (position + new Vector3(13680, -490, -17000)) + Vector3.Up - new Vector3(50, 0, 50), Vector3.Up, Vector3.Forward, width * 2, depth * 2, effect);
                collisionBox[1] = new BoundingBox((position + new Vector3(13680, -600, -17000)) + new Vector3(0, 0, 0), (position + new Vector3(13680, -600, -17000)) + new Vector3(width, height, depth));
            }
        }

        public void Building3(GraphicsDevice device, Vector3 position, float width, float depth, float height)
        {
            {
                this.device = device;
                Vector3 leftUpperLeft = (position + new Vector3(17320, -600, -17000)) + Vector3.Up * height;
                Vector3 nearUpperLeft = leftUpperLeft + Vector3.Backward * depth;
                Vector3 rightUpperLeft = nearUpperLeft + Vector3.Right * width;
                Vector3 farUpperLeft = rightUpperLeft + Vector3.Forward * depth;
                verticalWalls3 = new Wall[4];
                verticalWalls3[0] = new Wall(device, nearUpperLeft, Vector3.Backward, Vector3.Up, width, height, effect);
                verticalWalls3[1] = new Wall(device, rightUpperLeft, Vector3.Right, Vector3.Up, depth, height, effect);
                verticalWalls3[2] = new Wall(device, farUpperLeft, Vector3.Forward, Vector3.Up, width, height, effect);
                verticalWalls3[3] = new Wall(device, leftUpperLeft, Vector3.Left, Vector3.Up, depth, height, effect);
                roof3 = new Wall(device, leftUpperLeft, Vector3.Up, Vector3.Forward, width, depth, effect);
                buildingBase3 = new Wall(device, (position + new Vector3(17320, -490, -17000)) + Vector3.Up - new Vector3(50, 0, 50), Vector3.Up, Vector3.Forward, width * 2, depth * 2, effect);
                collisionBox[2] = new BoundingBox((position + new Vector3(17320, -600, -17000)) + new Vector3(0, 0, 0), (position + new Vector3(17320, -600, -17000)) + new Vector3(width, height, depth));
            }
        }
        public void Building4(GraphicsDevice device, Vector3 position, float width, float depth, float height)
        {
            {
                this.device = device;
                Vector3 leftUpperLeft = (position + new Vector3(15500, -600, -15180)) + Vector3.Up * height;
                Vector3 nearUpperLeft = leftUpperLeft + Vector3.Backward * depth;
                Vector3 rightUpperLeft = nearUpperLeft + Vector3.Right * width;
                Vector3 farUpperLeft = rightUpperLeft + Vector3.Forward * depth;
                verticalWalls4 = new Wall[4];
                verticalWalls4[0] = new Wall(device, nearUpperLeft, Vector3.Backward, Vector3.Up, width, height, effect);
                verticalWalls4[1] = new Wall(device, rightUpperLeft, Vector3.Right, Vector3.Up, depth, height, effect);
                verticalWalls4[2] = new Wall(device, farUpperLeft, Vector3.Forward, Vector3.Up, width, height, effect);
                verticalWalls4[3] = new Wall(device, leftUpperLeft, Vector3.Left, Vector3.Up, depth, height, effect);
                roof4 = new Wall(device, leftUpperLeft, Vector3.Up, Vector3.Forward, width, depth, effect);
                buildingBase4 = new Wall(device, (position + new Vector3(15500, -490, -15180)) + Vector3.Up - new Vector3(50, 0, 50), Vector3.Up, Vector3.Forward, width * 2, depth * 2, effect);
                collisionBox[3] = new BoundingBox((position + new Vector3(15500, -600, -15180)) + new Vector3(0, 0, 0), (position + new Vector3(15500, -600, -15180)) + new Vector3(width, height, depth));
            }
        }
        public void Building5(GraphicsDevice device, Vector3 position, float width, float depth, float height)
        {
            {
                this.device = device;
                Vector3 leftUpperLeft = (position + new Vector3(15500, -600, -18820)) + Vector3.Up * height;
                Vector3 nearUpperLeft = leftUpperLeft + Vector3.Backward * depth;
                Vector3 rightUpperLeft = nearUpperLeft + Vector3.Right * width;
                Vector3 farUpperLeft = rightUpperLeft + Vector3.Forward * depth;
                verticalWalls5 = new Wall[4];
                verticalWalls5[0] = new Wall(device, nearUpperLeft, Vector3.Backward, Vector3.Up, width, height, effect);
                verticalWalls5[1] = new Wall(device, rightUpperLeft, Vector3.Right, Vector3.Up, depth, height, effect);
                verticalWalls5[2] = new Wall(device, farUpperLeft, Vector3.Forward, Vector3.Up, width, height, effect);
                verticalWalls5[3] = new Wall(device, leftUpperLeft, Vector3.Left, Vector3.Up, depth, height, effect);
                roof5 = new Wall(device, leftUpperLeft, Vector3.Up, Vector3.Forward, width, depth, effect);
                buildingBase5 = new Wall(device, (position + new Vector3(15500, -490, -18820)) + Vector3.Up - new Vector3(50, 0, 50), Vector3.Up, Vector3.Forward, width * 2, depth * 2, effect);
                collisionBox[4] = new BoundingBox((position + new Vector3(15500, -600, -18820)) + new Vector3(0, 0, 0), (position + new Vector3(15500, -600, -18820)) + new Vector3(width, height, depth));
            }
        }
        public void SetTextures(Texture2D frontBack, Texture2D leftRight, Texture2D top)
        {
            // TODO: mirror the opposing textures
            verticalWalls[0].SetTexture(frontBack);
            verticalWalls[2].SetTexture(frontBack);
            verticalWalls[1].SetTexture(leftRight);
            verticalWalls[3].SetTexture(leftRight);
            roof.SetTexture(top);
            verticalWalls2[0].SetTexture(frontBack);
            verticalWalls2[2].SetTexture(frontBack);
            verticalWalls2[1].SetTexture(leftRight);
            verticalWalls2[3].SetTexture(leftRight);
            roof2.SetTexture(top);
            verticalWalls3[0].SetTexture(frontBack);
            verticalWalls3[2].SetTexture(frontBack);
            verticalWalls3[1].SetTexture(leftRight);
            verticalWalls3[3].SetTexture(leftRight);
            roof3.SetTexture(top);
            verticalWalls4[0].SetTexture(frontBack);
            verticalWalls4[2].SetTexture(frontBack);
            verticalWalls4[1].SetTexture(leftRight);
            verticalWalls4[3].SetTexture(leftRight);
            roof4.SetTexture(top);
            verticalWalls5[0].SetTexture(frontBack);
            verticalWalls5[2].SetTexture(frontBack);
            verticalWalls5[1].SetTexture(leftRight);
            verticalWalls5[3].SetTexture(leftRight);
            roof5.SetTexture(top);
        }
        public void SetBaseTexture(Texture2D baseTexture)
        {
            buildingBase.SetTexture(baseTexture);
            buildingBase2.SetTexture(baseTexture);
            buildingBase3.SetTexture(baseTexture);
            buildingBase4.SetTexture(baseTexture);
            buildingBase5.SetTexture(baseTexture);
        }
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice device)
        {
            foreach (Wall wall in verticalWalls)
            {
                wall.Draw(viewMatrix, projectionMatrix, device, effect);
                roof.Draw(viewMatrix, projectionMatrix, device, effect);
            }
            //buildingBase.Draw(viewMatrix, projectionMatrix, device, effect);
            foreach (Wall wall2 in verticalWalls2)
            {
                wall2.Draw(viewMatrix, projectionMatrix, device, effect);
                roof2.Draw(viewMatrix, projectionMatrix, device, effect);
            }
            //buildingBase2.Draw(viewMatrix, projectionMatrix, device, effect);
            foreach (Wall wall3 in verticalWalls3)
            {
                wall3.Draw(viewMatrix, projectionMatrix, device, effect);
                roof3.Draw(viewMatrix, projectionMatrix, device, effect);
            }
            //buildingBase3.Draw(viewMatrix, projectionMatrix, device, effect);
            foreach (Wall wall4 in verticalWalls4)
            {
                wall4.Draw(viewMatrix, projectionMatrix, device, effect);
                roof4.Draw(viewMatrix, projectionMatrix, device, effect);
            }
            //buildingBase4.Draw(viewMatrix, projectionMatrix, device, effect);
            foreach (Wall wall5 in verticalWalls5)
            {
                wall5.Draw(viewMatrix, projectionMatrix, device, effect);
                roof5.Draw(viewMatrix, projectionMatrix, device, effect);
            }
            //buildingBase5.Draw(viewMatrix, projectionMatrix, device, effect);
            
            //Add our shapes to be rendered
            //for (int i = 0; i < collisionBox.Length; i++)
            //{
            //    debugShape.AddBoundingBox(collisionBox[i], Color.Yellow, viewMatrix, projectionMatrix, device, effect);
            //}
        }
    }
}

