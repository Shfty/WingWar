using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar.CityAssets
{
    static class WallTexture
    {
        static Color[] colorList;
        static int width;
        static int height;
        static int windowWidth;
        static int windowHeight;
        static Random rand = new Random();


        static Color[] colorOptions = { Color.LightYellow,
                                        Color.White,
                                        Color.Green,
                                        Color.Blue,
                                        Color.Red,
                                        Color.Yellow,
                                        Color.Orange,
                                        Color.SteelBlue,
                                        Color.LightPink,
                                        Color.DarkCyan,                                          
                                        Color.PowderBlue
                                      };

        static Color litColor;

        static public Texture2D Initialize(GraphicsDevice graphicsDevice, int height, int width, int windowHeight, int windowWidth)
        {
            WallTexture.height = height;
            WallTexture.width = width;
            WallTexture.windowHeight = windowHeight;
            WallTexture.windowWidth = windowWidth;

            colorList = new Color[width * height];            

            // Fill with black
            for (int i = 0; i < colorList.Length; ++i)
            {
                colorList[i] = Color.Black;                
            }

            if (rand.NextDouble() < 0.5)
            {
                DrawRandomWindows();
            }
            else
            {
                DrawRandomGroups();
            }

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData<Color>(colorList);

            return texture;
        }

        static private void DrawRandomWindows()
        {
            for (int row = 1; row < height; row = row + windowHeight + 2)
            {
                for (int col = 1; col < width; col = col + windowWidth + 2)
                {
                    if (row + windowHeight < height && col + windowWidth < width)
                    {
                        if (rand.NextDouble() < 0.15)
                        {
                            DrawLitWindow(row, col);
                        }
                        else
                        {
                            DrawUnlitWindow(row, col);
                        }
                    }
                }
            }
        }

        static private void DrawRandomGroups()
        {
            Boolean isLit = false;

            for (int row = 1; row < height; row = row + windowHeight + 2)
            {
                for (int col = 1; col < width; col = col + windowWidth + 2)
                {
                    if (row + windowHeight < height && col + windowWidth < width)
                    {
                        if ((isLit == false && rand.NextDouble() < 0.02) ||
                            (isLit == true && rand.NextDouble() < 0.1))
                        {
                            isLit = !isLit;
                        }

                        if (isLit == true)
                        {
                            DrawLitWindow(row, col);
                        }
                        else
                        {
                            DrawUnlitWindow(row, col);
                        }
                    }
                }
            }
        }

        static private void DrawLitWindow(int row, int col)
        {
                litColor = colorOptions[rand.Next(colorOptions.Length)];
                Color color = litColor;

                Double colorChange = rand.NextDouble() / 2 + 0.75;
                color.R = (byte)(Math.Min(color.R * colorChange, 255));
                color.G = (byte)(Math.Min(color.G * colorChange, 255));
                color.B = (byte)(Math.Min(color.B * colorChange, 255));

                DrawWindow(row, col, color);            
        }

        static private void DrawUnlitWindow(int row, int col)
        {
                litColor = colorOptions[rand.Next(colorOptions.Length)];
                Color color = litColor;

                Double colorChange = rand.NextDouble() / 2 + 0.75;
                color.R = (byte)(Math.Min(color.R * colorChange, 100));
                color.G = (byte)(Math.Min(color.G * colorChange, 100));
                color.B = (byte)(Math.Min(color.B * colorChange, 100));

                DrawWindow(row, col, color);            
        }

        static private void DrawWindow(int row, int col, Color color)
        {
            for (int r = row; r < row + windowHeight; ++r)
            {
                for (int c = col; c < col + windowWidth; ++c)
                {
                    colorList[r * width + c] = color;
                }
            }
        }
    }
}
