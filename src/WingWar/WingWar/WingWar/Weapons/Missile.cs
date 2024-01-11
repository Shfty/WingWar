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
    class Missile : GameObject
    {
        //The fraction of the total active time that the missile should not seek for
        const int SEEK_DELAY_FACTOR = 10;

        public int targetIndex;
        private const float speed = 30f;
        public int activeTime = 0;
        public int totalActiveTime = 5000;

        public BoundingSphere hitSphere;

        public bool LostTarget = false;

        //Parses in jet data for origin
        public Missile(Vector3 position, Quaternion rotation, int targetIndex)
        {
            this.rotation = rotation;
            this.position = position;
            this.scale = new Vector3(5, 5, 5);

            this.targetIndex = targetIndex;
            hitSphere = new BoundingSphere(position, 20f);
        }

        //Translates depending on jet position/rotation
        public void Update(GameTime gameTime, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetVelocity)
        {
            Color particleColour;

            if (targetPosition != Vector3.Zero && !LostTarget)
            {
                //Seeker logic
                Vector3 newForwardUnit = Vector3.Normalize(targetPosition - position);
                Vector3 avatarForwardUnit = Vector3.Forward;
                Vector3 rotAxis = Vector3.Cross(avatarForwardUnit, newForwardUnit);
                rotAxis.Normalize();
                float rotAngle = (float)Math.Acos(Vector3.Dot(avatarForwardUnit, newForwardUnit));

                Quaternion destQ = Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);

                float seekTime = totalActiveTime / Math.Max(SEEK_DELAY_FACTOR, 1);
                if (activeTime >= seekTime)
                {
                    float accuracy = (activeTime - seekTime) / (totalActiveTime - seekTime);
                    rotation = Quaternion.Slerp(rotation, destQ, accuracy);
                }

                //Colour
                particleColour = new Color(1, 0, 0, 0.2f);
            }
            else
            {
                particleColour = new Color(1, 1, 1, 0.1f);
            }

            Vector3 missileMovement = Vector3.Transform(Vector3.Forward, rotation);
            if ((targetPosition - position).Length() > 10)
            {
                position += missileMovement * speed;
            }

            //Spawn Trail
            ParticleManager.Instance().AddParticle(
                    particleColour,
                    position + Vector3.Transform(Vector3.Backward, rotation),
                    Quaternion.CreateFromAxisAngle(Vector3.Forward, gameTime.TotalGameTime.Milliseconds % 360),
                    new Vector3(5, 5, 5),
                    50,
                    Vector3.Transform(new Vector3(0, 0, 0.5f), rotation),
                    Vector3.Zero,
                    Quaternion.Identity,
                    Quaternion.Identity,
                    new Vector3(-0.1f, -0.1f, 0),
                    new Vector3(0, 0, 0)
            );

            activeTime += gameTime.ElapsedGameTime.Milliseconds;

            hitSphere.Center = position;
        }
    }
}