using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar
{
    class Jet : Aircraft
    {
        public static Vector3 MAX_VELOCITY = new Vector3(10.0f, 1.0f, 15.0f);
        public static float ACCELL_FACTOR = 0.05f;
        public static float DECELL_FACTOR = 0.025f;

        public static float YAW_ACCELL = 0.001f;
        public static float PITCH_ACCELL = 0.002f;
        public static float ROLL_ACCELL = 0.004f;

        public static float YAW_FACTOR = 0.1f;
        public static float PITCH_FACTOR = 0.1f;
        public static float ROLL_FACTOR = 0.1f;

        public static float AIRBRAKE_FACTOR = 0.05f;
        public static float GRAVITY_FACTOR = 0.1f;
        public static float LIFT_OFFSET = 0.1f;
        public static float LIFT_FACTOR = 0.15f;

        float stickSpeedCap = 0.0f;

        public Jet(Vector3 offset, Quaternion rotation, int playerIndex, VertexBuffer jetBuffer, Effect effect, int aircraftType)
            : base(offset, rotation, playerIndex)
        {
            //offset = new Vector3(8f, -2f, 5f);
            aircraftSphere = new BoundingSphere(position, 10f);

            scale = new Vector3(2, 2, 2);

            velocity.Z = -MAX_VELOCITY.Z;
            stickSpeedCap = 15.0f;

            verticesFac.objectBuffer = jetBuffer;
            NUM_VERTICES = 216;

            base.effect = effect;
            this.aircraftType = aircraftType;
        }

        public override void Thrust(float thrust)
        {
            if (thrust * MAX_VELOCITY.Z >= stickSpeedCap)
            {
                stickSpeedCap = thrust * MAX_VELOCITY.Z;
            }
            else
            {
                stickSpeedCap = Math.Max(stickSpeedCap - DECELL_FACTOR, 0.0f);
            }

            velocity.Z = Math.Max(
                velocity.Z - Math.Min(ACCELL_FACTOR * thrust, ACCELL_FACTOR)
                , -stickSpeedCap
                );
        }

        public override void Airbrake(float intensity)
        {
            if (intensity > 0.0f)
                stickSpeedCap = Math.Max(stickSpeedCap - (intensity * AIRBRAKE_FACTOR), 0.0f);
        }

        public override void Rotate(float yaw, float pitch, float roll)
        {
            rotSpeed.Y += yaw * YAW_ACCELL;
            rotSpeed.Y = MathHelper.Lerp(rotSpeed.Y, 0.0f, 0.05f);
            
            rotSpeed.X += pitch * PITCH_ACCELL;
            rotSpeed.X = MathHelper.Lerp(rotSpeed.X, 0.0f, 0.05f);

            rotSpeed.Z += roll * ROLL_ACCELL;
            rotSpeed.Z = MathHelper.Lerp(rotSpeed.Z, 0.0f, 0.05f);
        }

        public override void applyPhysics()
        {
            //Model rotation
            modelRot = rotation;
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), -base.rotSpeed.X * 4);
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), -base.rotSpeed.Z * 4);
            modelRot *= Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -base.rotSpeed.Y * 4);

            //Apply gravity
            base.velocity.Y -= GRAVITY_FACTOR;

            //Apply lift
            Vector3 sourceHeading = Vector3.Transform(Vector3.Forward, rotation);
            sourceHeading.Normalize();
            float radianOfAttack = (float)Math.Acos(Vector3.Dot(sourceHeading, Vector3.Down));
            radianOfAttack -= MathHelper.ToRadians(15); //Shift slightly to make the ideal angle 15 degrees
            float attackScalar = (1.0f + LIFT_OFFSET) - (Math.Abs(MathHelper.ToDegrees(radianOfAttack) - 90) / 90);
            float lift = Math.Min((base.velocity.Z / MAX_VELOCITY.Z) * attackScalar, 0.0f);
            base.velocity.Y -= lift * LIFT_FACTOR;

            //Limit Lift
            base.velocity.Y = Math.Min(base.velocity.Y, MAX_VELOCITY.Y);
            base.velocity.Z = Math.Min(base.velocity.Z, MAX_VELOCITY.Z);
            base.velocity.Z = Math.Max(base.velocity.Z, -MAX_VELOCITY.Z);
        }

        public override void applyMovement()
        {
            //Thrust
            position += Vector3.Transform(new Vector3(0, 0, velocity.Z), rotation);

            //Gravity
            position += new Vector3(0, velocity.Y, 0);

            //Rotate the ship
            totalRot.Y -= rotSpeed.Y;
            totalRot.X -= rotSpeed.X;
            totalRot.Z -= rotSpeed.Z;

            //Build Rotation Quaternion
            Quaternion rotQuat =
                  Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), totalRot.X)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), totalRot.Y)
                * Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), totalRot.Z);

            //Multiply it into the jet's rotation
            rotation *= rotQuat;

            //Reset rotation in preparation for next frame
            totalRot = Vector3.Zero;
        }

        public override void Reset()
        {
            velocity = new Vector3(0, 0, -MAX_VELOCITY.Z);
            totalRot = Vector3.Zero;
            rotSpeed = Vector3.Zero;
            stickRotCap = Vector3.Zero;
            modelRot = Quaternion.Identity;
            rotation = Quaternion.Identity;
            modelOffset = new Vector3(0, 0, Camera.FAR_PLANE);
        }
    }
}
