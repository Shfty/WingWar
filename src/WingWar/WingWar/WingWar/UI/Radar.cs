using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar
{
    public class Radar 
    {
        //List<Aircraft> Enemies;                   // A list to hold all enemies

        //private Texture2D PlayerDotImage;
        //private Texture2D EnemyDotImage;
        //private Texture2D RadarImage;

        //// Local coords of the radar image's center, used to offset image when being drawn
        //private Vector2 RadarImageCenter;

        //// Distance that the radar can "see"
        //private const float RadarRange = 1500.0f;
        //private const float RadarRangeSquared = RadarRange * RadarRange;

        //// Radius of radar circle on the screen
        //private const float RadarScreenRadius = 150.0f;

        //// This is the center position of the radar hud on the screen. 
        //static Vector2 RadarCenterPos = new Vector2(850, 175);

        //public Radar(ContentManager Content, string playerDotPath, string enemyDotPath, string radarImagePath)
        //{
        //    PlayerDotImage = Content.Load<Texture2D>("Textures//redDotSmall");
        //    EnemyDotImage = Content.Load<Texture2D>("Textures//yellowDotSmall");
        //    RadarImage = Content.Load<Texture2D>("Textures//blackDotLarge");

        //    RadarImageCenter = new Vector2(RadarImage.Width * 0.5f, RadarImage.Height * 0.5f);
        //}

        //public void Draw(SpriteBatch spriteBatch, float playerForwardRadians, Vector3 playerPos, ref List<Aircraft> enemies)
        //{
        //    // The last parameter of the color determines how transparent the radar circle will be
        //    Game.SpriteBatch.Draw(RadarImage, RadarCenterPos, null, new Color(100, 100, 100, 150), 0.0f, RadarImageCenter, RadarScreenRadius / (RadarImage.Height * 0.5f), SpriteEffects.None, 0.0f);

        //    // If enemy is in range
        //    foreach (Aircraft thisEnemy in enemies)
        //    {
        //        Vector2 diffVect = new Vector2(thisEnemy.position.X - playerPos.X, thisEnemy.position.Z - playerPos.Z);
        //        float distance = diffVect.LengthSquared();

        //        // Check if enemy is within RadarRange
        //        if (distance < RadarRangeSquared)
        //        {
        //            // scale the distance from world coords to radar coords
        //            diffVect *= RadarScreenRadius / RadarRange;

        //            // We rotate each point on the radar so that the player is always facing UP on the radar
        //            diffVect = Vector2.Transform(diffVect, Matrix.CreateRotationZ(playerForwardRadians));

        //            // Offset coords from radar's center
        //            diffVect += RadarCenterPos;

        //            // We scale each dot so that enemies that are at higher elevations have bigger dots, and enemies
        //            // at lower elevations have smaller dots.
        //            float scaleHeight = 1.0f + ((thisEnemy.position.Y - playerPos.Y) / 200.0f);

        //            // Draw enemy dot on radar
        //            Game.SpriteBatch.Draw(EnemyDotImage, diffVect, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scaleHeight, SpriteEffects.None, 0.0f);
        //        }
        //    }

        //    // Draw player's dot last
        //    Game.SpriteBatch.Draw(PlayerDotImage, RadarCenterPos, Color.White);
        //}


        //
        //
        //
        //
        //
        //

//        using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

//namespace WingWar
//{
//    class Radar 
//    {
//        private Texture2D PlayerDotImage;
//        private Texture2D EnemyDotImage;
//        private Texture2D RadarImage;

//        // Local coords of the radar image's center, used to offset image when being drawn
//        private Vector2 RadarImageCenter;

//        // Distance that the radar can "see"
//        private const float RadarRange = 1500.0f;
//        private const float RadarRangeSquared = RadarRange * RadarRange;

//        // Radius of radar circle on the screen
//        private const float RadarScreenRadius = 150.0f;

//        Vector2 diffVect;
//        float scaleHeight;

//        // This is the center position of the radar hud on the screen. 
//        static Vector2 RadarCenterPos = new Vector2(850, 175);

//        public void LoadContent(ContentManager Content, string playerDotPath, string enemyDotPath, string radarImagePath)
//        {
//            PlayerDotImage = Content.Load<Texture2D>("Textures//redDotSmall");
//            EnemyDotImage = Content.Load<Texture2D>("Textures//yellowDotSmall");
//            RadarImage = Content.Load<Texture2D>("Textures//blackDotLarge");

//            RadarImageCenter = new Vector2(RadarImage.Width * 0.5f, RadarImage.Height * 0.5f);
//        }

//        public void Update(List<Player> enemies, float playerForwardRadians, Vector3 playerPos)
//        {
//            foreach (Player thisEnemy in enemies)
//            {
//                diffVect = new Vector2(thisEnemy.aircraft.position.X - playerPos.X, thisEnemy.aircraft.position.Z - playerPos.Z);
//                float distance = diffVect.LengthSquared();

//                // Check if enemy is within RadarRange
//                if (distance < RadarRangeSquared)
//                {
//                    // scale the distance from world coords to radar coords
//                    diffVect *= RadarScreenRadius / RadarRange;

//                    // We rotate each point on the radar so that the player is always facing UP on the radar
//                    diffVect = Vector2.Transform(diffVect, Matrix.CreateRotationZ(playerForwardRadians));

//                    // Offset coords from radar's center
//                    diffVect += RadarCenterPos;

//                    // We scale each dot so that enemies that are at higher elevations have bigger dots, and enemies
//                    // at lower elevations have smaller dots.
//                    scaleHeight = 1.0f + ((thisEnemy.aircraft.position.Y - playerPos.Y) / 200.0f);
//                }
//            }
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            // The last parameter of the color determines how transparent the radar circle will be
//            Game.SpriteBatch.Draw(RadarImage, RadarCenterPos, null, new Color(100, 100, 100, 150), 0.0f, RadarImageCenter, RadarScreenRadius / (RadarImage.Height * 0.5f), SpriteEffects.None, 0.0f);

//            Game.SpriteBatch.Draw(EnemyDotImage, diffVect, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), scaleHeight, SpriteEffects.None, 0.0f);

//            // Draw player's dot last
//            Game.SpriteBatch.Draw(PlayerDotImage, RadarCenterPos, Color.White);
//        }
//    }
//}

////in load assets in game place
//foreach (Player player in playerList)
//            {
//                player.gui.radar.LoadContent(Content, playerDotPath, enemyDotPath, radarImagePath);
//            }

    }
}
