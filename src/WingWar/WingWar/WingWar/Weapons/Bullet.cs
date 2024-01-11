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
    class Bullet : GameObject
    {
        public int targetIndex;

        //for calcs of position, and removing bullet.
        private const float velocity = 80f;
        public int activeTime = 4000, totalActiveTime;
        bool tracer;

        public bool LostTarget {get; set;}

        public BoundingSphere hitSphere;

        //Parses in jet data for origin
        public Bullet(Vector3 position, Quaternion rotation, int targetIndex, bool tracer)
        {
            this.rotation = rotation;
            this.position = position;
            this.scale = new Vector3(0.5f, 0.5f, 0.5f);
            this.tracer = tracer;

            this.targetIndex = targetIndex;

            hitSphere = new BoundingSphere(position, 50f);
        }

        //Translates depending on jet position/rotation
        public void Update(GameTime gameTime, Vector3 targetPosition)
        {
            if (targetPosition != Vector3.Zero && !LostTarget)
            {
                //Seeker logic
                Vector3 newForwardUnit = Vector3.Normalize(targetPosition - position);
                Vector3 avatarForwardUnit = Vector3.Forward;
                Vector3 rotAxis = Vector3.Cross(avatarForwardUnit, newForwardUnit);
                rotAxis.Normalize();
                float rotAngle = (float)Math.Acos(Vector3.Dot(avatarForwardUnit, newForwardUnit));

                Quaternion destQ = Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);

                rotation = Quaternion.Slerp(rotation, destQ, 1);
            }

            //Spawn Trail
            if (tracer)
            {
                ParticleManager.Instance().AddParticle(
                        new Color(0.75f, 0.75f, 0.25f, 0.1f),
                        position + Vector3.Transform(Vector3.Backward, rotation),
                        Quaternion.CreateFromAxisAngle(Vector3.Forward, gameTime.TotalGameTime.Milliseconds % 360),
                        new Vector3(2, 2, 2),
                        50,
                        Vector3.Transform(new Vector3(0, 0, 0.5f), rotation),
                        Vector3.Zero,
                        Quaternion.Identity,
                        Quaternion.Identity,
                        new Vector3(-0.02f, -0.02f, 0),
                        new Vector3(0, 0, 0)
                );
            }

            Vector3 bulletMovement = Vector3.Transform(Vector3.Forward, rotation);
            position += bulletMovement * velocity;

            totalActiveTime += gameTime.ElapsedGameTime.Milliseconds;
            hitSphere.Center = position + new Vector3(1, 1, 1);
        }
    }
}
