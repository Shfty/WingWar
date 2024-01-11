//#define ARCADE
#undef ARCADE

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

namespace WingWar
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menus
    {
        Color normal = Color.DarkGray, highlight = new Color(20, 220, 20, 255), overlay = new Color(40, 40, 40, 160);
        Texture2D texture = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1);
        bool prevUpState, prevDownState, prevLeftState, prevRightState;
        SpriteFont activeFont;
        Vector2 position, positionTwo, labels;

        public Vector2[] rectangles = new Vector2[5];
        public Vector2[] sliders = new Vector2[5];

        Texture2D peakRect, peakSlider, cityRect, citySlider, waterRect, waterSlider, distRect, distSlider, treeRect, treeSlider;
        Color[] data, data2, data3, data4, data5, data6, data7, data8, data9, data10;

        float width = 0f, height = 0f;
        public string[] menuItems;
        public int menuChoice, selection, maxKills = 10;

        public float terrainHeight, cityHeight, waterAmt, citySize, foliage;
        string gameType = "GameType: Normal >";

        public int Selection
        {
            get { return selection; }
            set
            {
                selection = value;
                if (selection < 0)
                    selection = 0;
                if (selection >= menuItems.Length)
                    selection = menuItems.Length - 1;
            }
        }

        public Menus(Game game, int menuChoice)
        {
            // TODO: Construct any child components here
            this.menuChoice = menuChoice;
            MenuItems();
            MeasureMenu();

            GameType.Instance.ToggleNormal(true);

            labels = new Vector2(55, 10);
            texture.SetData(new[] { Color.White });

            int offset = 0;

            for (int i = 0; i < 5; i++)
            {
                sliders[i] = new Vector2(positionTwo.X - (Game.MenuFont.MeasureString(menuItems[0]).X / 50) + (Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 3) + 50,
                    positionTwo.Y + (Game.MenuFont.LineSpacing / 2) + offset);

                offset += 50;
            }

            //Create Top Slider
            peakRect = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 100, 5);
            data = new Color[100 * 5];
            for (int j = 0; j < data.Length; ++j) data[j] = Color.Green;
            peakRect.SetData(data);

            //Creates Top DragBox
            peakSlider = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 5, 15);
            data2 = new Color[5 * 15];
            for (int j = 0; j < data2.Length; ++j) data2[j] = Color.Blue;
            peakSlider.SetData(data2);

            //Creates Bottom Slider
            cityRect = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 100, 5);
            data3 = new Color[100 * 5];
            for (int j = 0; j < data3.Length; ++j) data3[j] = Color.Green;
            cityRect.SetData(data3);

            //Creates Bottom DragBox
            citySlider = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 5, 15);
            data4 = new Color[5 * 15];
            for (int j = 0; j < data4.Length; ++j) data4[j] = Color.Blue;
            citySlider.SetData(data4);

            //Create  Slider
            waterRect = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 100, 5);
            data5 = new Color[100 * 5];
            for (int j = 0; j < data5.Length; ++j) data5[j] = Color.Green;
            waterRect.SetData(data5);

            //Creates  DragBox
            waterSlider = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 5, 15);
            data6 = new Color[5 * 15];
            for (int j = 0; j < data6.Length; ++j) data6[j] = Color.Blue;
            waterSlider.SetData(data6);

            //Create cVl Slider
            distRect = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 100, 5);
            data7 = new Color[100 * 5];
            for (int j = 0; j < data7.Length; ++j) data7[j] = Color.Green;
            distRect.SetData(data7);

            //Creates cVl DragBox
            distSlider = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 5, 15);
            data8 = new Color[5 * 15];
            for (int j = 0; j < data8.Length; ++j) data8[j] = Color.Blue;
            distSlider.SetData(data8);

            //Create tree Slider
            treeRect = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 100, 5);
            data9 = new Color[100 * 5];
            for (int j = 0; j < data9.Length; ++j) data9[j] = Color.Green;
            treeRect.SetData(data9);

            //Creates tree DragBox
            treeSlider = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 5, 15);
            data10 = new Color[5 * 15];
            for (int j = 0; j < data10.Length; ++j) data10[j] = Color.Blue;
            treeSlider.SetData(data10);
        }

        private void MenuItems()
        {
            if (menuChoice == 0) { menuItems = new string[] { "Begin", "Quit" }; }
            if (menuChoice == 1) { menuItems = new string[] { "Quick Play", "Custom Map", "Options", "Back" }; }
            if (menuChoice == 2) { menuItems = new string[] { "Screensaver", "D:" }; }
            if (menuChoice == 3) { menuItems = new string[] { "Terrain Height", "City Height", "Water Amount", "City Size", "Foliage Amount", 
                "Score Limit: < " + maxKills + " >", gameType, "Reset", "Random", "Confirm", "Back" }; }
            if (menuChoice == 4) { menuItems = new string[] { "JT-01 Firebird", "CS-3 Gull", "Cancel" }; }
            if (menuChoice == 5) { menuItems = new string[] { "Loading" }; }
#if ARCADE
            if (menuChoice == 6) { menuItems = new string[] { "Loaded: Press Green to Begin" }; }
#else
            if (menuChoice == 6) { menuItems = new string[] { "Loaded: Press A to Begin" }; }
#endif
        }

        // Moved menu systems into useful resources on wiki.

        private void MeasureMenu()
        {
            height = 0;
            width = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = Game.MenuFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;

                height += Game.MenuFont.LineSpacing + 5;
            }

            position = new Vector2(
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 2,
                (Game.GraphicsMgr.GraphicsDevice.Viewport.Height - height * Game.MasterScale2D) / 2);

            positionTwo = new Vector2(
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 4,
                (Game.GraphicsMgr.GraphicsDevice.Viewport.Height - height * Game.MasterScale2D) / 2);

            int offset = 0;

            for (int i = 0; i < 5; i++)
            {
                rectangles[i] = new Vector2(positionTwo.X - (Game.MenuFont.MeasureString(menuItems[0]).X / 50) + (Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 3),
                    positionTwo.Y + (Game.MenuFont.LineSpacing / 2 * Game.MasterScale2D) + (offset * Game.MasterScale2D));

                sliders[i].Y = rectangles[i].Y - (Game.MenuFont.LineSpacing / 10);

                offset += 50;
            }
        }

        public void Update(GameTime gameTime)
        {
            MeasureMenu();
            MenuItems();

            if (selection >= menuItems.Length)
                selection = 0;

            if (Controls.Instance.Down(0) && !prevDownState)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection++;
                if (selection == menuItems.Length)
                    selection = 0;
            }
            if (Controls.Instance.Up(0) && !prevUpState)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection--;
                if (selection < 0)
                    selection = menuItems.Length - 1;
            }

            if (menuChoice == 3)
            {
                activeFont = Game.MenuFontSmall;
                switch (selection)
                {
                    case 0:
                        if (Controls.Instance.Right(0))
                        {
                            sliders[0].X += 1;
                            if (sliders[0].X >= rectangles[0].X + 100) sliders[0].X = rectangles[0].X + 100;
                        }
                        if (Controls.Instance.Left(0))
                        {
                            sliders[0].X -= 1;
                            if (sliders[0].X <= rectangles[0].X) sliders[0].X = rectangles[0].X;
                        }
                        break;

                    case 1:
                        if (Controls.Instance.Right(0))
                        {
                            sliders[1].X += 1;
                            if (sliders[1].X >= rectangles[1].X + 100) sliders[1].X = rectangles[1].X + 100;
                        }
                        if (Controls.Instance.Left(0))
                        {
                            sliders[1].X -= 1;
                            if (sliders[1].X <= rectangles[1].X) sliders[1].X = rectangles[1].X;
                        }
                        break;

                    case 2:
                        if (Controls.Instance.Right(0))
                        {
                            sliders[2].X += 1;
                            if (sliders[2].X >= rectangles[2].X + 100) sliders[2].X = rectangles[2].X + 100;
                        }
                        if (Controls.Instance.Left(0))
                        {
                            sliders[2].X -= 1;
                            if (sliders[2].X <= rectangles[2].X) sliders[2].X = rectangles[2].X;
                        }
                        break;
                    case 3:
                        if (Controls.Instance.Right(0))
                        {
                            sliders[3].X += 1;
                            if (sliders[3].X >= rectangles[3].X + 100) sliders[3].X = rectangles[3].X + 100;
                        }
                        if (Controls.Instance.Left(0))
                        {
                            sliders[3].X -= 1;
                            if (sliders[3].X <= rectangles[3].X) sliders[3].X = rectangles[3].X;
                        }
                        break;
                    case 4:
                        if (Controls.Instance.Right(0))
                        {
                            sliders[4].X += 1;
                            if (sliders[4].X >= rectangles[4].X + 100) sliders[4].X = rectangles[4].X + 100;
                        }
                        if (Controls.Instance.Left(0))
                        {
                            sliders[4].X -= 1;
                            if (sliders[4].X <= rectangles[4].X) sliders[4].X = rectangles[4].X;
                        }
                        break;
                    case 5:
                        if (Controls.Instance.Right(0) && !prevRightState)
                        {
                            maxKills += 5;
                            if (maxKills >= 25) maxKills = 25;
                        }
                        if (Controls.Instance.Left(0) && !prevLeftState)
                        {
                            maxKills -= 5;
                            if (maxKills <= 5) maxKills = 5;
                        }
                        break;
                    case 6:
                        if (Controls.Instance.Right(0) && !prevRightState)
                        {
                            gameType = "GameType: < Hardcore";
                            GameType.Instance.ToggleNormal(false);
                        }
                        if (Controls.Instance.Left(0) && !prevLeftState)
                        {
                            gameType = "GameType: Normal >";
                            GameType.Instance.ToggleNormal(true);
                        }
                        break;
                }

                terrainHeight = (sliders[0].X - rectangles[0].X);
                if (terrainHeight == 0) terrainHeight = 1;

                cityHeight = (sliders[1].X - rectangles[1].X);
                cityHeight = cityHeight + 10;

                waterAmt = (sliders[2].X - rectangles[2].X);
                if (waterAmt == 0) waterAmt = 1;

                citySize = (sliders[3].X - rectangles[3].X) * 0.8f;
                citySize = citySize + 20;

                foliage = (sliders[4].X - rectangles[4].X) + 10;
                foliage = foliage / 10;
            }
            else
            {
                activeFont = Game.MenuFont;
            }

            prevUpState = Controls.Instance.Up(0);
            prevDownState = Controls.Instance.Down(0);
            prevLeftState = Controls.Instance.Left(0);
            prevRightState = Controls.Instance.Right(0);
        }

        public void Draw()
        {
            if (menuChoice == 3)
            {
                DrawOverlay(true);
                DrawMenuStrings(50, positionTwo);
                DrawSliders();
            }
            else
            {
                DrawOverlay(false);
                DrawMenuStrings(2, position);
            }

        }

        private void DrawOverlay(bool mapMenu)
        {
            float w = 0;
            float h = 0;
            float positionX = 0;
            float positionY = 0;

            if (mapMenu)
            {
                w = (Game.MenuFont.MeasureString(menuItems[0]).X * Game.MasterScale2D) * 3.5f;
                h = ((50 * menuItems.Length) * Game.MasterScale2D) + (100 * Game.MasterScale2D);
                positionX = position.X - w / 2.1f;
                positionY = position.Y + (50 * Game.MasterScale2D) - (100 * Game.MasterScale2D);

                RectangleOverlay(positionX, positionY, w, h);
            }
            else
            {
                w = (Game.MenuFont.MeasureString(menuItems[0]).X * Game.MasterScale2D) * 2;
                h = ((50 * menuItems.Length) * Game.MasterScale2D) + (100 * Game.MasterScale2D);
                positionX = position.X - w / 2;
                positionY = position.Y + (50 * Game.MasterScale2D) - (100 * Game.MasterScale2D);

                RectangleOverlay(positionX, positionY, w, h);
            }

            if (menuChoice == 0)
            {
                w = (Game.MenuFont.MeasureString(Game.Title).X * 2) * Game.MasterScale2D;
                h = (50 * Game.MasterScale2D) + (100 * Game.MasterScale2D);
                positionX = position.X - w / 2;
                positionY = (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height / 4 - 
                    ((Game.MenuFont.LineSpacing) * Game.MasterScale2D);

                RectangleOverlay(positionX, positionY, w, h);

                Game.SpriteBatch.DrawString(Game.MenuFont, Game.Title,
                new Vector2((float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 2 - ((Game.MenuFont.MeasureString(Game.Title).X) * Game.MasterScale2D / 2), 
                    (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height / 4),
                Color.White, 0, new Vector2(0, 0), 1 * Game.MasterScale2D, SpriteEffects.None, 0);

            }
        }

        private void RectangleOverlay(float positionX, float positionY, float w, float h)
        {
            Rectangle rect = new Rectangle((int)positionX, (int)positionY, (int)w, (int)h);

            Game.SpriteBatch.Draw(texture, rect, overlay);

            int bw = 2;

            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, bw, rect.Height), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Right - bw, rect.Top, bw, rect.Height), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, rect.Width, bw), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Bottom - bw, rect.Width, bw), Color.White);
        }

        private void DrawMenuStrings(int alignment, Vector2 pos)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(pos.X - (((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / alignment) - 1), (position.Y + ((50 * Game.MasterScale2D) * i)) - 1),
                Color.White, 0, new Vector2(0,0), Game.MasterScale2D, SpriteEffects.None, 0);

                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(pos.X - ((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / alignment), position.Y + ((50 * Game.MasterScale2D) * i)),
                (i == selection ? highlight : normal), 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);
            }
        }

        private void DrawSliders()
        {
            int i = 0;

            Game.SpriteBatch.Draw(peakRect, rectangles[0], Color.White);
            Game.SpriteBatch.Draw(peakSlider, sliders[0], Color.White);
            Game.SpriteBatch.DrawString(Game.MenuFont, "Low                High", rectangles[0] - labels, i == selection ? highlight : normal, 0.0f, new Vector2(0f, 0f), 0.4f, SpriteEffects.None, 0);

            i = 1;
            Game.SpriteBatch.Draw(cityRect, rectangles[1], Color.White);
            Game.SpriteBatch.Draw(citySlider, sliders[1], Color.White);
            Game.SpriteBatch.DrawString(Game.MenuFont, "Low                High", rectangles[1] - labels, i == selection ? highlight : normal, 0.0f, new Vector2(0f, 0f), 0.4f, SpriteEffects.None, 0);

            i = 2;
            Game.SpriteBatch.Draw(waterRect, rectangles[2], Color.White);
            Game.SpriteBatch.Draw(waterSlider, sliders[2], Color.White);
            Game.SpriteBatch.DrawString(Game.MenuFont, "Water              Land", rectangles[2] - labels, i == selection ? highlight : normal, 0.0f, new Vector2(0f, 0f), 0.4f, SpriteEffects.None, 0);

            i = 3;
            Game.SpriteBatch.Draw(distRect, rectangles[3], Color.White);
            Game.SpriteBatch.Draw(distSlider, sliders[3], Color.White);
            Game.SpriteBatch.DrawString(Game.MenuFont, "Small              Large", rectangles[3] - labels, i == selection ? highlight : normal, 0.0f, new Vector2(0f, 0f), 0.4f, SpriteEffects.None, 0);

            i = 4;
            Game.SpriteBatch.Draw(treeRect, rectangles[4], Color.White);
            Game.SpriteBatch.Draw(treeSlider, sliders[4], Color.White);
            Game.SpriteBatch.DrawString(Game.MenuFont, "Less               More", rectangles[4] - labels, i == selection ? highlight : normal, 0.0f, new Vector2(0f, 0f), 0.4f, SpriteEffects.None, 0);
        }
    }
}


