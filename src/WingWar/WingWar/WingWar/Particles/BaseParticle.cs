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
    class BaseParticle
    {
        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 Scale = new Vector3(1, 1, 1);
        public Color Colour;

        Vector3 velocity = Vector3.Zero;
        Vector3 damping = Vector3.Zero;

        Quaternion RotationSpeed = Quaternion.Identity;
        Quaternion RotationDamping = Quaternion.Identity;

        Vector3 ScaleSpeed = Vector3.Zero;
        Vector3 ScaleDamping = Vector3.Zero;

        public int lifespan;

        public BaseParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan)
        {
            this.Colour = colour;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.lifespan = lifespan;
        }

        public BaseParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan, Vector3 velocity, Vector3 damping)
        {
            this.Colour = colour;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.lifespan = lifespan;
            this.velocity = velocity;
            this.damping = damping;
        }

        public BaseParticle(
            Color colour,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            int lifespan,
            Vector3 velocity,
            Vector3 damping,
            Quaternion RotationSpeed,
            Quaternion RotationDamping)
        {
            this.Colour = colour;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.lifespan = lifespan;
            this.velocity = velocity;
            this.damping = damping;
            this.RotationSpeed = RotationSpeed;
            this.RotationDamping = RotationDamping;
        }

        public BaseParticle(
            Color colour,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            int lifespan,
            Vector3 velocity,
            Vector3 damping,
            Quaternion RotationSpeed,
            Quaternion RotationDamping,
            Vector3 ScaleSpeed,
            Vector3 ScaleDamping)
        {
            this.Colour = colour;
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.lifespan = lifespan;
            this.velocity = velocity;
            this.damping = damping;
            this.RotationSpeed = RotationSpeed;
            this.RotationDamping = RotationDamping;
            this.ScaleSpeed = ScaleSpeed;
            this.ScaleDamping = ScaleDamping;
        }

        public void Update()
        {
            velocity *= damping;
            Position += velocity;
            RotationSpeed *= RotationDamping;
            Rotation *= RotationSpeed;

            //ScaleSpeed *= ScaleDamping;
            Scale += ScaleSpeed;

            lifespan--;
        }
    }
}
