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
    class TextLabel
    {
        public const int UNDERLINE_VERTICAL_OFFSET = -10;
        public const float HIGHLIGHT_POINT_SCALE = 5;
        public const int ANIMATION_DURATION = 600;

        string content;
        Vector2 position;
        Vector2 highlightPosition;
        Texture2D lineTexture;

        float animationStartMs = 0;
        int animationIndex;

        public TextLabel(string content, Vector2 position, Vector2 highlightPosition)
        {
            this.content = content;

            this.position = new Vector2(
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width * position.X,
                Game.GraphicsMgr.GraphicsDevice.Viewport.Height * position.Y
            );

            this.highlightPosition = new Vector2(
                Game.GraphicsMgr.GraphicsDevice.Viewport.Width * highlightPosition.X,
                Game.GraphicsMgr.GraphicsDevice.Viewport.Height * highlightPosition.Y
            );

            lineTexture = new Texture2D(Game.GraphicsMgr.GraphicsDevice, 1, 1);
            lineTexture.SetData(new[]{Color.White});
        }

        public void Update(GameTime gameTime)
        {
            if (animationStartMs == 0)
            {
                animationStartMs = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }

            float elapsedTime = (float)gameTime.TotalGameTime.TotalMilliseconds - animationStartMs;
            float scalarProgress = elapsedTime / ANIMATION_DURATION;
            animationIndex = (int)(content.Length * scalarProgress);
        }

        public void Draw()
        {
            Game.SpriteBatch.Begin();

            Vector2 stringBounds = Game.MenuFont.MeasureString(content) * Game.MasterScale2D;

            //Main text
            Game.SpriteBatch.DrawString(Game.MenuFont, content.Substring(0, Math.Min(animationIndex, content.Length)), position - (stringBounds / 2), Color.White, 0, Vector2.Zero, Game.MasterScale2D, SpriteEffects.None, 0);

            //Underline
            Rectangle underlineRect = new Rectangle(
                (int)position.X - (int)(stringBounds.X / 2),
                (int)position.Y + (int)stringBounds.Y + (int)(UNDERLINE_VERTICAL_OFFSET * Game.MasterScale2D) - (int)(stringBounds.Y / 2),
                (int)stringBounds.X,
                1
            );
            Game.SpriteBatch.Draw(lineTexture, underlineRect, Color.White);

            //Highlight line
            Vector2 source = new Vector2(position.X - (stringBounds.X / 2), position.Y + stringBounds.Y + (UNDERLINE_VERTICAL_OFFSET * Game.MasterScale2D) - (stringBounds.Y / 2));
            float angle = (float)Math.Atan2(highlightPosition.Y - source.Y, highlightPosition.X - source.X);
            float length = Vector2.Distance(source, highlightPosition);

            Game.SpriteBatch.Draw(lineTexture, source, null, Color.White,
                       angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, 0);


            //Source/Highlight dots
            Vector2 offset = new Vector2((HIGHLIGHT_POINT_SCALE * Game.MasterScale2D) / 2, (HIGHLIGHT_POINT_SCALE * Game.MasterScale2D) / 2);
            Game.SpriteBatch.Draw(lineTexture, source - offset, null, Color.White, 0, new Vector2(0, 0), (HIGHLIGHT_POINT_SCALE * Game.MasterScale2D), SpriteEffects.None, 0);
            Game.SpriteBatch.Draw(lineTexture, highlightPosition - offset, null, Color.White, 0, new Vector2(0, 0), (HIGHLIGHT_POINT_SCALE * Game.MasterScale2D), SpriteEffects.None, 0);

            Game.SpriteBatch.End();
        }
    }
}
