using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WingWar
{
    class Helicopter : Aircraft
    {
        public static float MAX_VELOCITY = 7.5f;
        public static float ACCELL_FACTOR = 0.020f;
        public static float DECELL_FACTOR = 0.01f;
        public static float ROT_ACCELL = 0.0025f;
        public static float ROT_FACTOR = 0.004f;
        public static float AIRBRAKE_FACTOR = 0.05f;
        public static float IDLE_BLADE_SPEED = 1.0f;

        HeliBlade blade;
        float bladeSpeed = 0;

        public Helicopter(Vector3 offset, Quaternion rotation, int playerIndex, VertexBuffer heliBuffer, 
            Effect effect, int aircraftType, ContentManager Content)
            : base(offset, rotation, playerIndex)
        {
            blade = new HeliBlade(this, Content);

            scale = new Vector3(2, 2, 2);

            aircraftSphere = new BoundingSphere(position, 10f);

            verticesFac.objectBuffer = heliBuffer;
            NUM_VERTICES = 384;

            this.effect = effect;
            this.aircraftType = aircraftType;
        }

        public override void Thrust(float thrust)
        {
            if (thrust > 0 && bladeSpeed < 1.0f)
            {
                bladeSpeed += Math.Min(ACCELL_FACTOR * thrust, ACCELL_FACTOR);
            }
            else if (bladeSpeed >= 0)
            {
                if (bladeSpeed >= 0.01f)
                {
                    bladeSpeed -= DECELL_FACTOR;
                }
                else
                {
                    bladeSpeed = 0;
                }
            }
        }

        public override void Airbrake(float intensity)
        {
            if (intensity > 0 && bladeSpeed > -1.0f)
            {
                bladeSpeed -= Math.Min(ACCELL_FACTOR * intensity, ACCELL_FACTOR);
            }
            else if (bladeSpeed <= 0)
            {
                if (bladeSpeed <= -0.01f)
                {
                    bladeSpeed += DECELL_FACTOR;
                }
                else
                {
                    bladeSpeed = 0;
                }
            }
        }

        public override void Rotate(float yaw, float pitch, float roll)
        {
            rotSpeed.Y += yaw * ROT_FACTOR;
            rotSpeed.Y = MathHelper.Lerp(rotSpeed.Y, 0.0f, 0.05f);

            rotSpeed.X += pitch * ROT_FACTOR;
            rotSpeed.X = MathHelper.Lerp(rotSpeed.X, 0.0f, 0.05f);

            rotSpeed.Z += roll * ROT_FACTOR;
            rotSpeed.Z = MathHelper.Lerp(rotSpeed.Z, 0.0f, 0.05f);
        }

        public override void applyMovement()
        {
            velocity.Y = bladeSpeed * MAX_VELOCITY;

            //Create the thrust vector)
            Vector3 thrust = Vector3.Transform(new Vector3(0,1,0), rotation);

            //Move the ship
            position += (thrust * 2) * velocity;

            //Rotate the ship
            totalRot.Y -= rotSpeed.Y;
            totalRot.X -= rotSpeed.X;
            totalRot.Z -= rotSpeed.Z;

            position += Vector3.Transform(new Vector3(-totalRot.Y * 100, 0, 0), rotation);
            position += Vector3.Transform(new Vector3(0, 0, totalRot.X * 100), rotation);

            //Build Rotation Quaternion
            Quaternion rotQuat =
                  Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), totalRot.X)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), totalRot.Y)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), totalRot.Z);

            //Multiply it into the craft's rotation
            rotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -base.rotSpeed.Z);
            modelRot *= rotQuat;

            //Reset rotation in preparation for next frame
            totalRot = Vector3.Zero;
        }

        public override void applyPhysics()
        {
            //Model rotation
            modelRot = rotation;
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -base.rotSpeed.X * 4);
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -base.rotSpeed.Z * 4);
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), -base.rotSpeed.Y * 4);

            //Limit Velocity
            base.velocity.Y = Math.Min(base.velocity.Y, MAX_VELOCITY);
            base.velocity.Y = Math.Max(base.velocity.Y, -MAX_VELOCITY);

        }

        public override void Reset()
        {
            velocity = Vector3.Zero;
            totalRot = Vector3.Zero;
            rotSpeed = Vector3.Zero;
            stickRotCap = Vector3.Zero;
            modelRot = Quaternion.Identity;
            rotation = Quaternion.Identity;
            modelOffset = new Vector3(0, 0, Camera.FAR_PLANE);
        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
            blade.position = position + modelOffset;
            blade.rotation = modelRot;
            blade.rotation *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), blade.angle);
            blade.angle += bladeSpeed + IDLE_BLADE_SPEED;
            blade.scale = new Vector3(2, 2, 2);
            blade.Draw(camera);
        }
    }
}