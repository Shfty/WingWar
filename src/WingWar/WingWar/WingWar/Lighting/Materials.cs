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
    class Materials
    {

    private static Materials instance;

        public Materials() 
        {
        }

        public static Materials Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Materials();
                }
                return instance;
            }
        }

        public Vector4 DiffuseLightColor()
        {
            return new Vector4(0.8f, 0.8f, 0.8f, 1);
        }
            
        public Vector4 SpecularLightColor()
        {
            return new Vector4(1, 1, 1, 1);
        }

        public Vector3 LightDirection()
        {
            return new Vector3(1, -1, -1);
        }

        public Vector4 PointLightColor()
        {
            return new Vector4(0.5f, 0.0f, 1.0f, 1.0f);
        }

        public float LightAttenuation()
        {
            return 5000f;
        }

        public float LightFalloff()
        {
            return 2f;
        }

        private float ambience;

        public float Ambience
        {
            get { return ambience; }
            set { ambience = value; }
        }

        public Color SceneBackground()
        {
            return new Color(120, 200, 230);
        }
    }
}
