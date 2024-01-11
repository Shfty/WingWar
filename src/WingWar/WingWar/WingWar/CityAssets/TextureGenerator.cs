using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar.CityAssets
{
    static class Texture
    {
        public static Texture2D Generate(GraphicsDevice device, int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(device, width, height);
            Color[] colorData = { color };
            for (int row = 0; row < height; ++row)
            {
                for (int col = 0; col < width; ++col)
                {
                    int index = row * width + col;
                    texture.SetData(0, new Rectangle(col, row, 1, 1), colorData, 0, 1);
                }
            }
            return texture;
        }
    }
}

