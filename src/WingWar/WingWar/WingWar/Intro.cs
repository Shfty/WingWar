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
    class Intro
    {
        Vector2 stringOffset = Vector2.Zero;
        Vector2 stringBounds = Vector2.Zero;
        string introText =
@"It is the year 4114





Following global nuclear war, the earth has become a barren wasteland
Only small pockets of civilization remain
         
These cities, sealed inside protective biodomes, hold the last of humanity
Resources are scarce, and for some there is only so much time until the end
         
As a city's population gradually falls toward zero, the vultures begin to circle
These vultures come in the form of scavenging paramilitary organizations
         
Looking to expand and survive, these PMCs wait until a city is all but defenseless
Then, they move to take control of it
         
However, it is not always that simple
When multiple PMCs move to take the same city,
the result is all out small-scale war until only one remains 
         
This simulation details the longest-fought battle in our database





Operation: Skyflare";

        public Intro()
        {
            stringBounds = Game.MenuFont.MeasureString(introText) * 0.75f * Game.MasterScale2D ;
            stringOffset = new Vector2(0, Game.GraphicsMgr.GraphicsDevice.Viewport.Height);
        }

        public void Update()
        {
            stringOffset.Y -= Game.GameTime.ElapsedGameTime.Milliseconds / 25f;
        }

        public bool IsDone()
        {
            return stringOffset.Y <= -Game.GraphicsMgr.GraphicsDevice.Viewport.Height;
        }

        public void Draw()
        {
            Vector2 stringPosition =
                new Vector2(
                    Game.GraphicsMgr.GraphicsDevice.Viewport.Bounds.Center.X,
                    Game.GraphicsMgr.GraphicsDevice.Viewport.Bounds.Center.Y
                ) - (stringBounds / 2);

            Game.GraphicsMgr.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.DrawString(Game.MenuFont, introText, stringPosition + stringOffset, Color.White, 0, Vector2.Zero, 0.75f * Game.MasterScale2D, SpriteEffects.None, 0);
            Game.SpriteBatch.End();
        }
    }
}