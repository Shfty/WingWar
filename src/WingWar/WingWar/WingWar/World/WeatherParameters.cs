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
    class WeatherParameters
    {
        private static WeatherParameters instance;

        public WeatherParameters() { }

        public static WeatherParameters Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeatherParameters();
                }
                return instance;
            }
        }

        public Vector3 WindDirection()
        {
            return new Vector3(-1, 0, 0);
        }

        public float WaveLength()
        {
            return 0.1f;
        }

        public float WaveHeight()
        {
            return 0.3f;
        }

        public float WindForce()
        {
            return 0.0005f;
        }
    }
}