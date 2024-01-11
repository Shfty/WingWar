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
    class AttractScene
    {
        public delegate void LeaveAttractCallback();
        LeaveAttractCallback exitCallback;

        Camera attractCamera;
        VehicleSpawner attractSpawner;
        Effect attractAircraftEffect;
        Jet attractJet;
        Helicopter attractHeli;
        bool attractModeShowHeli = false;

        struct AttractCameraPosition
        {
            public Vector3 Rotation;
            public float Distance;
            public Vector3 CameraOffset;
            public int Duration;
            public int Delay;

            public AttractCameraPosition(Vector3 inRotation, float inDistance, Vector3 inCameraOffset, int inDuration, int inDelay)
            {
                Rotation = inRotation;
                Distance = inDistance;
                CameraOffset = inCameraOffset;
                Duration = inDuration;
                Delay = inDelay;
            }
        };

        List<AttractCameraPosition> attractCameraPositions;
        int attractPositionIndex = 0;
        int attractAnimationStartMs = 0;

        public AttractScene(ContentManager Content, LeaveAttractCallback exitCallback)
        {
            this.exitCallback = exitCallback;

            //Initial attract state
            attractCamera = new Camera(0);
            attractSpawner = new VehicleSpawner();
            attractAircraftEffect = Content.Load<Effect>("Effects//AircraftShader");
            attractJet = new Jet(
                new Vector3(0, 0, 0),
                Quaternion.Identity,
                1,
                attractSpawner.jetBuffer,
                attractAircraftEffect,
                0
            );

            attractHeli = new Helicopter(
                new Vector3(0, 0, 0),
                Quaternion.Identity,
                3,
                attractSpawner.heliBuffer,
                attractAircraftEffect,
                1,
                Content
            );

            //Attract animation positions
            attractCameraPositions = new List<AttractCameraPosition>();
            setupCameraPositions();
        }

        private void setupCameraPositions()
        {
            float pi = (float)Math.PI;

            //Back (Far)
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(0, 0, 0),
                    10000f,
                    new Vector3(0, 0, 0),
                    1300,
                    550
                )
            );
            //Front
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(0, pi, 0),
                    40f,
                    new Vector3(0, 0, 0),
                    1000,
                    4000
                )
            );
            //Right Back
            /* Jet Info text:
             * JT-01 High-Speed Interception Vehicle
             * 
             * Genetically evolved chassis
             *    |- Low physical resolution
             *    |- Optimized aerodynamics
             *    |- Geometrically reinforced structure
             */
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(-pi * 0.1f, pi * 1.75f, 0),
                    60f,
                    new Vector3(20, -2, 0),
                    1600,
                    3500
                )
            );
            //Bottom
            /* Jet Info text:
             * Armaments:
             * 2x ICU-2 Smart Cannons
             *     |- Target leading AI system
             *     |- Axial spin projectile trajectory control
             * 1x KBM Mk.III Frag Missile Pod
             *     |- Intelligent seeking system
             *     |- 0.5GJ Explosive Payload
             */
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(pi * 0.5f, 0, 0),
                    35f,
                    new Vector3(10, 0, 0),
                    1600,
                    4000
                )
            );
            //Left Front
            /* Jet Info text:
             * FR3-BRD Augmented reality pilot chamber
             *     |- Direct Central Nervous Interface
             *     |- Sensory Relay
             *     |- Operator Isolation
             */
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(-pi * 0.1f, pi * 1.25f, 0),
                    60f,
                    new Vector3(20, 2, 0),
                    2000,
                    1000
                )
            );

            //Top
            /* Jet Info Text:
             * Second-Generation Interceptor Class
             * SKM-Compliant Design
             * Codename "Firebird"
             * 
             * Heli Info Text:
             * Codename "Gull"
             */
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(-pi * 0.5f, 0, 0),
                    35f,
                    new Vector3(10, 0, 0),
                    2000,
                    1000
                )
            );

            //Back
            /* Jet Info Text:
             * VRM 3 Atmospheric Thruster
             *     |- Spatial Decompression Engine
             *     |- Low Power High Output Configuration
             */
            attractCameraPositions.Add(
                new AttractCameraPosition(
                    new Vector3(0, 0, 0),
                    40f,
                    new Vector3(0, 0, 0),
                    2000,
                    1000
                )
            );
        }

        public void Start(GameTime gameTime)
        {
            attractAnimationStartMs = (int)gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime)
        {
            int animationDurationMs =
                attractAnimationStartMs
                + attractCameraPositions[attractPositionIndex].Duration
                + attractCameraPositions[attractPositionIndex].Delay;

            if (gameTime.TotalGameTime.TotalMilliseconds > animationDurationMs)
            {
                if (attractPositionIndex == attractCameraPositions.Count - 1)
                {
                    attractModeShowHeli = !attractModeShowHeli;
                }

                attractAnimationStartMs = (int)gameTime.TotalGameTime.TotalMilliseconds;
                attractPositionIndex = (attractPositionIndex + 1) % attractCameraPositions.Count;
            }

            if (Controls.Instance.ActionButtons(PlayerIndex.One)
            || Controls.Instance.ActionButtons(PlayerIndex.Two)
            || Controls.Instance.ActionButtons(PlayerIndex.Three)
            || Controls.Instance.ActionButtons(PlayerIndex.Four))
            {
                exitCallback();
            }

            float aspect = (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width / (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height;
            attractCamera.UpdateCamera(aspect, new Vector3(0, 0, 0), Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI), false);
        }

        public void Draw(GameTime time)
        {
            Game.GraphicsMgr.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            Vector3 attractCameraRotation;
            float attractCameraDistance;
            Vector3 attractCameraOffset;

            float attractAnimationProgress =
                (float)(time.TotalGameTime.TotalMilliseconds - attractAnimationStartMs)
              / (float)attractCameraPositions[attractPositionIndex].Duration;

            if (attractPositionIndex < attractCameraPositions.Count - 1)
            {
                attractCameraRotation = Vector3.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].Rotation,
                                            attractCameraPositions[attractPositionIndex + 1].Rotation,
                                            attractAnimationProgress
                                        );
                attractCameraDistance = MathHelper.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].Distance,
                                            attractCameraPositions[attractPositionIndex + 1].Distance,
                                            attractAnimationProgress
                                        );
                attractCameraOffset = Vector3.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].CameraOffset,
                                            attractCameraPositions[attractPositionIndex + 1].CameraOffset,
                                            attractAnimationProgress
                                        );
            }
            else
            {
                attractCameraRotation = Vector3.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].Rotation,
                                            attractCameraPositions[0].Rotation,
                                            attractAnimationProgress
                                        );
                attractCameraDistance = MathHelper.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].Distance,
                                            attractCameraPositions[0].Distance,
                                            attractAnimationProgress
                                        );
                attractCameraOffset = Vector3.SmoothStep(
                                            attractCameraPositions[attractPositionIndex].CameraOffset,
                                            attractCameraPositions[0].CameraOffset,
                                            attractAnimationProgress
                                        );
            }

            Vector3 cameraPosition = Vector3.Transform(
                                         new Vector3(0, 0, attractCameraDistance),
                                         Matrix.CreateFromYawPitchRoll( //Need to swap X and Y due to YawPitchRoll
                                            attractCameraRotation.Y,
                                            attractCameraRotation.X,
                                            attractCameraRotation.Z
                                         )
                                     );

            Matrix viewMatrix = Matrix.CreateLookAt(
                                    cameraPosition + attractCameraOffset,
                                    attractCameraOffset,
                                    Vector3.Up
                                );

            if (!attractModeShowHeli)
            {
                attractJet.Draw(viewMatrix, attractCamera.cameraProjectionMatrix, 1.0f);
            }
            else
            {
                attractHeli.Draw(viewMatrix, attractCamera.cameraProjectionMatrix, 1.0f);
            }
        }
    }
}
