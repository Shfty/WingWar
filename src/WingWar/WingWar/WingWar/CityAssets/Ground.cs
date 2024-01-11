using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar.CityAssets
{
    class Ground
    {
        GraphicsDevice device;
        Wall[] surface;
        BasicEffect effect;

        static Random random = new Random();


        static int[] offsetOptions = {   0,
                                         100,
                                         -100
                                     };

        public Ground(GraphicsDevice device, Vector3 farLeft, int lotsWide, int lotsDeep, float lotWidth, float lotDepth, float gap, BasicEffect effect)
        {
            this.device = device;
            this.effect = effect;

            farLeft.X -= gap / 2;
            farLeft.Z -= gap / 2;

            Texture2D lotTexture = GenerateLotTexture((int)lotWidth, (int)lotDepth, new Color());

            surface = new Wall[lotsWide * lotsDeep];
            for (int row = 0; row < lotsDeep; ++row)
            {
                for (int col = 0; col < lotsWide; ++col)
                {
                    // get lot corner position
                    Vector3 lotPosition;
                    lotPosition.X = farLeft.X + col * (lotWidth + gap);
                    lotPosition.Y = -600;
                    lotPosition.Z = farLeft.Z + row * (lotDepth + gap);

                    int index = row * lotsWide + col;
                    surface[index] = new Wall(device, lotPosition, Vector3.Up, Vector3.Forward, lotWidth + gap, lotDepth + gap, effect);
                    surface[index].SetTexture(lotTexture);


                }
            }
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice device)
        {
            foreach (Wall wall in surface)
                wall.Draw(viewMatrix, projectionMatrix, device, effect);
        }

        private Texture2D GenerateLotTexture(int lotWidth, int lotDepth, Color lightColor)
        {
            Color[] colorData = new Color[lotWidth * lotDepth];
            for (int i = 0; i < lotWidth * lotDepth; ++i)
                colorData[i] = new Color(50,50,50);

            Texture2D texture = new Texture2D(device, lotWidth, lotDepth);
            texture.SetData(colorData);
            return texture;
        }
    }
}
