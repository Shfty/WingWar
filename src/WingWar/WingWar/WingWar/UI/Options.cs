//#define XBOX
#undef XBOX

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
    public class Options
    {
        Color normal = Color.DarkGray, highlight = new Color(20, 220, 20, 255), overlay = new Color(40,40,40,160);
        bool prevUpState, prevDownState, prevLeftState, prevRightState;
        Vector2 position, positionTwo;
        public string[] menuItems;
        private float height, width;
        public float audioVolume;
        public float soundEffectVolume;
        public int menuChoice, selection;

        private Texture2D texture = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1);

        public float terrainHeight, cityHeight;

        public string paused = "OPTIONS", resume = "Back", quit = "Quit Game";
        public bool bFullscreen, bDebug = true;
        string fullscreen = "Fullscreen: Off >";
        string debugMode = "Debug Mode: OFF >";

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

        public Options(Game game, int menuChoice)
        {
            // TODO: Construct any child components here
            this.menuChoice = menuChoice;
            audioVolume = 50.0f;
            soundEffectVolume = 50.0f;
            MenuItems();

            texture.SetData(new[] { Color.White });

            if (Game.GraphicsMgr.IsFullScreen)
                bFullscreen = true;
            else
                bFullscreen = false;
        }

        private void MenuItems()
        {
            menuItems = new string[] { paused, fullscreen, debugMode, "Music Volume: < " + audioVolume + " >", "FX Volume: < " + soundEffectVolume + " >", resume, quit };
        }

        public void MeasureMenu()
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
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width / 7,
                (Game.GraphicsMgr.GraphicsDevice.Viewport.Height - height * Game.MasterScale2D) / 2);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            MenuItems();
            MeasureMenu();

            MathHelper.Clamp(audioVolume, 1, 100);
            MathHelper.Clamp(soundEffectVolume, 1, 100);

            if (!bFullscreen) fullscreen = "Fullscreen: < On";
            if (bFullscreen) fullscreen = "Fullscreen: Off >";

            if (!bDebug) debugMode = "Debug Mode: < ON";
            if (bDebug) debugMode = "Debug Mode OFF >";

            if (selection == 0) selection = 1;

            if (selection >= menuItems.Length)
                selection = 1;

            if (Controls.Instance.Down(0) && !prevDownState)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection++;
                if (selection == menuItems.Length)
                    selection = 1;
            }
            if (Controls.Instance.Up(0) && !prevUpState)
            {
                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuMove);
                selection--;
                if (selection < 1)
                    selection = menuItems.Length - 1;
            }

            prevUpState = Controls.Instance.Up(0);
            prevDownState = Controls.Instance.Down(0);
            prevLeftState = Controls.Instance.Left(0);
            prevRightState = Controls.Instance.Right(0);
        }

        public void Draw()
        {
            DrawOverlay();

            for (int i = 0; i < menuItems.Length; i++)
            {
                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(position.X - ((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / 2) - 1, position.Y + ((50 * Game.MasterScale2D) * i) - 1),
                Color.White, 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);

                Game.SpriteBatch.DrawString(
                Game.MenuFont,
                menuItems[i],
                new Vector2(position.X - ((Game.MenuFont.MeasureString(menuItems[i]).X * Game.MasterScale2D) / 2), position.Y + ((50 * Game.MasterScale2D) * i)),
                (i == selection ? highlight : normal), 0, new Vector2(0, 0), Game.MasterScale2D, SpriteEffects.None, 0);
            }
        }

        private void DrawOverlay()
        {
            float w = (Game.MenuFont.MeasureString(menuItems[2]).X * Game.MasterScale2D) * 2;
            float h = ((50 * menuItems.Length) * Game.MasterScale2D) + (100 * Game.MasterScale2D);
            float positionX = position.X - w / 2;
            float positionY = position.Y + (50 * Game.MasterScale2D) - (100 * Game.MasterScale2D);

            Rectangle rect = new Rectangle((int)positionX, (int)positionY, (int)w, (int)h);

            Game.SpriteBatch.Draw(texture, rect, overlay);

            int bw = 2;

            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, bw, rect.Height), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Right - bw, rect.Top, bw, rect.Height), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, rect.Width, bw), Color.White);
            Game.SpriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Bottom - bw, rect.Width, bw), Color.White);
        }
    }
}
