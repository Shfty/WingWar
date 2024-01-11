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
    public class BloomSettings
    {
        public readonly float BloomThreshold;
        public readonly float BlurAmount;
        public readonly float BloomIntensity;
        public readonly float BaseIntensity;
        public readonly float BloomSaturation;
        public readonly float BaseSaturation;


        public BloomSettings(float bloomThreshold, float blurAmount,
                             float bloomIntensity, float baseIntensity,
                             float bloomSaturation, float baseSaturation)
        {
            BloomThreshold = bloomThreshold;
            BlurAmount = blurAmount;
            BloomIntensity = bloomIntensity;
            BaseIntensity = baseIntensity;
            BloomSaturation = bloomSaturation;
            BaseSaturation = baseSaturation;
        }

        public static BloomSettings PresetSettings = new BloomSettings(0.5f, 1, 1, 1, 1, 1);

    }
}