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
    class MenuCamera
    {
        private static MenuCamera instance;

        public Quaternion menuCameraRot = Quaternion.Identity;
        bool menuCameraDirection = false;
        float frameCount = 0;

        public MenuCamera() 
        {
        }

        public static MenuCamera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuCamera();
                }

                return instance;
            }
        }

        public void ConstructMenuScene()
        {
            Game.terrainHeight = 20;
            Game.cityHeight = 70;
            Game.weight = 2;
            Game.citySize = 30;
            Game.foliage = 3;
        }

        public void Update()
        {
            frameCount += 1;

            if (frameCount >= 900)
            {
                menuCameraDirection = !menuCameraDirection;
                frameCount = 0;
            }

            if (menuCameraDirection)
                menuCameraRot.Y += 0.1f;
            else
                menuCameraRot.Y -= 0.1f;
        }
    }
}
