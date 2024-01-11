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
    class Camera
    {
        //Read-onlies
        public const float NEAR_PLANE = 1f;
        public const float FAR_PLANE = 32000f;

        //Public
        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 LookAt = Vector3.Zero;
        public float PositionLerpFactor = 0.4f;
        public float RotationLerpFactor = 0.4f;
        public float FOV = 50;
        public float AspectRatio = 1.0f;

        public enum CameraType
        {
            FirstPerson,
            ThirdPerson
        }

        public CameraType Type = CameraType.ThirdPerson;


        //Private
        Vector3 currentPosition = Vector3.Zero;
        Quaternion currentRotation = Quaternion.Identity;
        Quaternion targetRotation = Quaternion.Identity;
        Vector3 cameraTypeOffset = Vector3.Zero;

        public Camera()
        {
        }

        public void Update()
        {
            if (Type == CameraType.FirstPerson)
            {
                cameraTypeOffset = Vector3.Zero;
            }
            else
            {
                cameraTypeOffset = new Vector3(0, 10f, 100f);
            }

            currentPosition = Vector3.SmoothStep(currentPosition, Position, PositionLerpFactor);

            if (LookAt != Vector3.Zero)
            {
                Vector3 avatarForwardUnit = Vector3.Forward;
                Vector3 newForwardUnit = Vector3.Normalize(LookAt - (currentPosition + Vector3.Transform(cameraTypeOffset, Rotation)));
                float dot = Vector3.Dot(avatarForwardUnit, newForwardUnit);

                if(dot == 1)
                {
                    targetRotation = Quaternion.Identity;
                }
                else if (dot == -1)
                {
                    targetRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI);
                }
                else
                {
                    Vector3 rotAxis = Vector3.Cross(avatarForwardUnit, newForwardUnit);
                    rotAxis.Normalize();
                    targetRotation = Quaternion.CreateFromAxisAngle(rotAxis, (float)Math.Acos(dot));
                }
            }

            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, RotationLerpFactor);
        }

        public Matrix ViewMatrix()
        {
            return Matrix.CreateLookAt(
                currentPosition + Vector3.Transform(cameraTypeOffset, Rotation),
                currentPosition + Vector3.Transform(cameraTypeOffset, Rotation) + Vector3.Transform(Vector3.Forward * FAR_PLANE, currentRotation),
                Vector3.Transform(Vector3.Up, Rotation)
            );
        }

        //Ignores offset, used for targeting
        public Matrix FirstPersonViewMatrix()
        {
            return Matrix.CreateLookAt(
                currentPosition,
                currentPosition + Vector3.Transform(Vector3.Forward * FAR_PLANE, currentRotation),
                Vector3.Transform(Vector3.Up, Rotation)
            );
        }

        public Matrix ProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(FOV),
                AspectRatio,
                NEAR_PLANE,
                FAR_PLANE
            );
        }

        public Matrix CustomProjectionMatrix(float fov, float aspectRatio)
        {
            return Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(fov),
                aspectRatio,
                NEAR_PLANE,
                FAR_PLANE
            );
        }

        public BoundingFrustum ViewFrustum()
        {
            return new BoundingFrustum(ViewMatrix() * ProjectionMatrix());
        }

        public void ToggleFirstPerson()
        {
            if(this.Type == CameraType.ThirdPerson)
            {
                this.Type = CameraType.FirstPerson;
            }
            else
            {
                this.Type = CameraType.ThirdPerson;
            }
        }

        //Immediately move to the target values without lerping
        public void Reset()
        {
            Update();
            currentPosition = Position;
            currentRotation = targetRotation;
        }
    }
}
