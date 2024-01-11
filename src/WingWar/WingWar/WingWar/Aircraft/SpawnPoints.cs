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
    class SpawnPoints
    {
        Random spawn;
        public Vector3 spawnPos, origin;
        Quaternion rotation;

        public SpawnPoints()
        {
            spawn = new Random();
            origin = new Vector3(16000, 2000, -16000);
        }

        public void PlayerSpawn()
        {
            Vector3 spawnPoint = new Vector3(
                (float)spawn.Next(10000, 22000),
                2000f,
                (float)spawn.Next(-22000, -10000)
                );

            this.spawnPos = spawnPoint;

            spawnPoint = Vector3.Zero;
        }

        public void FaceOrigin(Vector3 spawn)
        {
            float forward = Vector3.Dot(spawn, origin);

            forward = MathHelper.Clamp(forward, -1.0f, 1.0f);

            float facing = (float)Math.Acos(forward);

            rotation = new Quaternion(0, facing, 0, 1);
        }
    }
}
