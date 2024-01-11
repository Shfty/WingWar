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

        VehicleSpawner attractSpawner;
        Effect attractAircraftEffect;
        Jet attractJet;
        Helicopter attractHeli;
        bool attractModeShowHeli = false;

        struct AttractKeyframe
        {
            public Vector3 Rotation;
            public float Distance;
            public Vector3 CameraOffset;
            public int Duration;
            public int Delay;

            public AttractKeyframe(Vector3 inRotation, float inDistance, Vector3 inCameraOffset, int inDuration, int inDelay)
            {
                Rotation = inRotation;
                Distance = inDistance;
                CameraOffset = inCameraOffset;
                Duration = inDuration;
                Delay = inDelay;
            }
        };

        struct AttractLabel
        {
            public int Index;
            public string Content;
            public Vector2 Position;
            public Vector2 HighlightPosition;
            public bool Heli;

            public AttractLabel(int index, string content, Vector2 position, Vector2 highlightPosition, bool heli)
            {
                Index = index;
                Content = content;
                Position = position;
                HighlightPosition = highlightPosition;
                Heli = heli;
            }
        };

        List<AttractKeyframe> attractKeyframes;
        List<AttractLabel> attractLabels;
        List<TextLabel> activeLabels;
        int attractAnimationIndex = 0;
        int attractAnimationStartMs = 0;
        float attractAnimationProgress = 0;
        Vector3 attractCameraRotation = Vector3.Zero;
        float attractCameraDistance = 0.0f;
        Vector3 attractCameraOffset = Vector3.Zero;
        Vector3 cameraPosition = Vector3.Zero;
        Matrix viewMatrix = Matrix.Identity;
        Matrix projectionMatrix = Matrix.Identity;

        public AttractScene(ContentManager Content, LeaveAttractCallback exitCallback)
        {
            this.exitCallback = exitCallback;

            //Initial attract state
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
            attractKeyframes = new List<AttractKeyframe>();
            attractLabels = new List<AttractLabel>();
            activeLabels = new List<TextLabel>();
            setupCameraPositions();
        }

        private void setupCameraPositions()
        {
            float pi = (float)Math.PI;

            //Back (Far)
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(0, 0, 0),
                    Camera.FAR_PLANE,
                    new Vector3(0, 0, 0),
                    1000, //Fly-in
                    350
                )
            );

            //Front
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(0, pi, 0),
                    40f,
                    new Vector3(0, 0, 0),
                    800, //Turn to back
                    3200
                )
            );

            //Right Back
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(-pi * 0.1f, pi * 1.75f, 0),
                    60f,
                    new Vector3(20, -2, 0),
                    1200, //Flip to bottom
                    2800
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    1,
                    "JT-01 High-Speed Interception Vehicle",
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.25f, 0.55f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    1,
                    "Genetically-evolved chassis\n- Low physical Resolution\n- Optimized aerodynamics\n- Geometrically reinforced structure",
                    new Vector2(0.75f, 0.75f),
                    new Vector2(0.3f, 0.6f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    1,
                    "CS-3 Tactical Assault Vehicle",
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.3f, 0.55f),
                    true
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    1,
                    "Vertical Flight Model\n- 4DOF System\n- VTOL Capable",
                    new Vector2(0.75f, 0.75f),
                    new Vector2(0.29f, 0.475f),
                    true
                )
            );

            //Bottom
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(pi * 0.5f, 0, 0),
                    35f,
                    new Vector3(10, 0, 0),
                    800, //Spin to front left
                    3200
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    2,
                    "2x ICU-2 Smart Cannons\n- Target Leading AI System\n- Axial Spin Trajectory Control",
                    new Vector2(0.75f, 0.3f),
                    new Vector2(0.4f, 0.45f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    2,
                    "1x KBM Mk.III Frag Missile Pod\n- Intelligent Seeking System\n- 0.5GJ Explosive Payload",
                    new Vector2(0.75f, 0.6f),
                    new Vector2(0.3f, 0.7f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    2,
                    "2x ICU-2 Smart Cannons\n- Target Leading AI System\n- Axial Spin Trajectory Control",
                    new Vector2(0.75f, 0.3f),
                    new Vector2(0.225f, 0.55f),
                    true
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    2,
                    "1x KBM Mk.III Frag Missile Pod\n- Intelligent Seeking System\n- 0.5GJ Explosive Payload",
                    new Vector2(0.75f, 0.6f),
                    new Vector2(0.3f, 0.7f),
                    true
                )
            );

            //Left Front
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(-pi * 0.1f, pi * 1.25f, 0),
                    60f,
                    new Vector3(20, 2, 0),
                    1200, //Flip to top
                    2800
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    3,
                    "FR3-BRD Aumented Reality Pilot Chamber\n- Direct Central Nervous Interface\n- Sensory Relay Technology\n- Full Operator Isolation",
                    new Vector2(0.4f, 0.3f),
                    new Vector2(0.68f, 0.65f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    3,
                    "CGS Armoured Cockpit\n- Virtual Reality Interface\n- Hyper-Response Input Devices\n- Maximum Operator Protection",
                    new Vector2(0.4f, 0.3f),
                    new Vector2(0.64f, 0.68f),
                    true
                )
            );

            //Top
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(-pi * 0.5f, 0, 0),
                    35f,
                    new Vector3(10, 0, 0),
                    900, //Tilt to front
                    2400
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    4,
                    "Second Generation Interceptor Class\nSKM-Compliant Design\nCodename \"Firebird\"",
                    new Vector2(0.75f, 0.5f),
                    new Vector2(0.3f, 0.4f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    4,
                    "Experimental Ravager Class\nNon-Compliant Prototype Design\nCodename \"Gull\"",
                    new Vector2(0.75f, 0.5f),
                    new Vector2(0.3f, 0.4f),
                    true
                )
            );

            //Back
            attractKeyframes.Add(
                new AttractKeyframe(
                    new Vector3(0, 0, 0),
                    40f,
                    new Vector3(0, 0, 0),
                    2000,
                    1000
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    5,
                    "VRM-3 Atmospheric Thruster\n- Spatial Decompression Engine\n- Low Power High Output Configuration",
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.5f, 0.5f),
                    false
                )
            );

            attractLabels.Add(
                new AttractLabel(
                    5,
                    "TX-90 Cube Infused Polygonal Boosters\n- Advanced Geometric Propulsion\n- Anti-Grav Technology",
                    new Vector2(0.5f, 0.25f),
                    new Vector2(0.5f, 0.525f),
                    true
                )
            );
        }

        public void Start(GameTime gameTime)
        {
            attractAnimationIndex = 0;
            attractAnimationStartMs = (int)gameTime.TotalGameTime.TotalMilliseconds;
        }

        public void Update(GameTime gameTime)
        {
            if (Controls.Instance.ActionButtons(PlayerIndex.One)
            || Controls.Instance.ActionButtons(PlayerIndex.Two)
            || Controls.Instance.ActionButtons(PlayerIndex.Three)
            || Controls.Instance.ActionButtons(PlayerIndex.Four))
            {
                activeLabels = null;
                exitCallback();
            }

            CheckKeyframeChange(gameTime);

            UpdateCamera(gameTime);

            //Update active labels
            if (activeLabels != null)
            {
                foreach (TextLabel label in activeLabels)
                {
                    label.Update(gameTime);
                }
            }
        }

        private void CheckKeyframeChange(GameTime gameTime)
        {
            int animationDurationMs =
                attractAnimationStartMs
                + attractKeyframes[attractAnimationIndex].Duration;

            if (gameTime.TotalGameTime.TotalMilliseconds > animationDurationMs)
            {
                if (activeLabels == null)
                {
                    activeLabels = new List<TextLabel>();
                    foreach (AttractLabel label in attractLabels)
                    {
                        if (label.Index == attractAnimationIndex && label.Heli == attractModeShowHeli)
                        {
                            activeLabels.Add(new TextLabel(label.Content, label.Position, label.HighlightPosition));
                        }
                    }
                }
            }

            int animationDurationDelayMs =
                animationDurationMs
                + attractKeyframes[attractAnimationIndex].Delay;

            if (gameTime.TotalGameTime.TotalMilliseconds > animationDurationDelayMs)
            {
                if (attractAnimationIndex == attractKeyframes.Count - 1)
                {
                    attractModeShowHeli = !attractModeShowHeli;
                }

                attractAnimationStartMs = (int)gameTime.TotalGameTime.TotalMilliseconds;
                attractAnimationIndex = (attractAnimationIndex + 1) % attractKeyframes.Count;

                activeLabels = null;
            }
        }

        private void UpdateCamera(GameTime gameTime)
        {
            attractAnimationProgress =
                (float)(gameTime.TotalGameTime.TotalMilliseconds - attractAnimationStartMs)
              / (float)attractKeyframes[attractAnimationIndex].Duration;

            if (attractAnimationIndex < attractKeyframes.Count - 1)
            {
                attractCameraRotation = Vector3.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].Rotation,
                                            attractKeyframes[attractAnimationIndex + 1].Rotation,
                                            attractAnimationProgress
                                        );
                attractCameraDistance = MathHelper.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].Distance,
                                            attractKeyframes[attractAnimationIndex + 1].Distance,
                                            attractAnimationProgress
                                        );
                attractCameraOffset = Vector3.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].CameraOffset,
                                            attractKeyframes[attractAnimationIndex + 1].CameraOffset,
                                            attractAnimationProgress
                                        );
            }
            else
            {
                attractCameraRotation = Vector3.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].Rotation,
                                            attractKeyframes[0].Rotation,
                                            attractAnimationProgress
                                        );
                attractCameraDistance = MathHelper.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].Distance,
                                            attractKeyframes[0].Distance,
                                            attractAnimationProgress
                                        );
                attractCameraOffset = Vector3.SmoothStep(
                                            attractKeyframes[attractAnimationIndex].CameraOffset,
                                            attractKeyframes[0].CameraOffset,
                                            attractAnimationProgress
                                        );
            }

            cameraPosition = Vector3.Transform(
                                         new Vector3(0, 0, attractCameraDistance),
                                         Matrix.CreateFromYawPitchRoll( //Need to swap X and Y due to YawPitchRoll
                                            attractCameraRotation.Y,
                                            attractCameraRotation.X,
                                            attractCameraRotation.Z
                                         )
                                     );

            viewMatrix = Matrix.CreateLookAt(
                                    cameraPosition + attractCameraOffset,
                                    attractCameraOffset,
                                    Vector3.Up
                                );

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(50),
                (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Width / (float)Game.GraphicsMgr.GraphicsDevice.Viewport.Height,
                Camera.NEAR_PLANE,
                Camera.FAR_PLANE
            );
        }

        public void Draw()
        {
            Game.GraphicsMgr.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            if (!attractModeShowHeli)
            {
                attractJet.Draw(viewMatrix, projectionMatrix);
            }
            else
            {
                attractHeli.Draw(viewMatrix, projectionMatrix);
            }

            if (activeLabels != null)
            {
                foreach (TextLabel label in activeLabels)
                {
                    label.Draw();
                }
            }
        }
    }
}
