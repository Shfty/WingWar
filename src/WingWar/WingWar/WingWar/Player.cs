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
    class Player
    {
        private SpawnPoints spawn;
        private VehicleSpawner vehicleSpawn;

        public Camera camera;
        public GUI gui;
        public WeaponFactory weaponFac;
        public Aircraft aircraft;
        public Effect planeEffect;
        public Effect weaponEffect;
        private ContentManager Content;

        public const float MAX_HEALTH = 10;

        public int deaths, kills, score = 0;
        public float currentHealth = MAX_HEALTH;
        public int playerIndex;
        PlayerIndex gamePadIndex;

        //Trail Colours
        Vector4 FULL_HEALTH = new Vector4(0, 1.0f, 0, 0.1f);
        Vector4 NO_HEALTH = new Vector4(1.0f, 0, 0, 0.1f);

        //Which playerIndex are we aiming at? -1 = nobody
        public int targetIndex = -1;
        public bool bulletLock = false;

        public int AircraftType;
        public float CurrentHealth { get { return currentHealth; } }
        public int DeathCount { get { return deaths; } }
        public int KillCount { get { return kills; } }
        public int ScoreCount { get { return score; } }

        public bool Active;
        public bool FlyIn = true;

        bool prevCameraToggleState = false;
        Vector3 flyInOffset = new Vector3(100, 50, 0);

        public Player(int playerIndex, int aircraftType, ContentManager Content)
        {
            this.playerIndex = playerIndex;

            switch (playerIndex)
            {
                case 1:
                    gamePadIndex = PlayerIndex.One;
                    break;
                case 2:
                    gamePadIndex = PlayerIndex.Two;
                    break;
                case 3:
                    gamePadIndex = PlayerIndex.Three;
                    break;
                case 4:
                    gamePadIndex = PlayerIndex.Four;
                    break;
            }

            this.AircraftType = aircraftType;
            this.Content = Content;
            this.planeEffect = Content.Load<Effect>("Effects//AircraftShader");
            this.weaponEffect = Content.Load<Effect>("Effects//WeaponShader");

            vehicleSpawn = new VehicleSpawner();
            spawn = new SpawnPoints();
            spawn.PlayerSpawn();

            InitialiseAircraft();

            camera = new Camera();
            StartFlyIn();
            gui = new GUI(aircraftType);
            weaponFac = new WeaponFactory(playerIndex, weaponEffect, Content);

            gui.LoadContent(Content);

            Active = true;
        }

        public void StartFlyIn()
        {
            FlyIn = true;
            camera.Position = aircraft.position + flyInOffset;
            camera.LookAt = aircraft.position + aircraft.modelOffset;
            camera.PositionLerpFactor = 0.1f;
            camera.Reset();
        }

        public void Update(float aspectRatio, GameTime gameTime,Player player, List<Player> playerList)
        {
            if (Active)
            {
                spawnTrail(gameTime);

                if (FlyIn)
                {
                    aircraft.aircraftSphere.Center = aircraft.position;
                    if (aircraft.modelOffset.Z > 300)
                    {
                        aircraft.modelOffset.Z *= 0.92f;
                    }
                    else if (aircraft.modelOffset.Z > 0)
                    {
                        aircraft.modelOffset.Z -= 15f;
                    }
                    else
                    {
                        aircraft.modelOffset.Z = 0;
                    }
                }
                else
                {
                    aircraft.Update(this);
                    weaponFac.Update(gameTime, aircraft.position, aircraft.modelRot, playerList, targetIndex);
                }
            }

            //Camera
            camera.Rotation = aircraft.rotation;
            if (!FlyIn)
            {
                camera.Position = aircraft.position;
                camera.Rotation = aircraft.rotation;
                camera.LookAt = aircraft.position + (Vector3.Transform(Vector3.Forward * Camera.FAR_PLANE, aircraft.modelRot));

                if (camera.PositionLerpFactor < 0.5)
                {
                    camera.PositionLerpFactor += 0.01f;
                }
            }
            else
            {
                camera.Position = aircraft.position + flyInOffset;
                camera.LookAt = aircraft.position + aircraft.modelOffset;
                if (aircraft.modelOffset.Z == 0)
                {
                    FlyIn = false;
                }
            }

            camera.AspectRatio = aspectRatio;

            //Toggle FPV/TPV
            if (Controls.Instance.FirstPerson(gamePadIndex) && !prevCameraToggleState)
            {
                camera.ToggleFirstPerson();
            }
            prevCameraToggleState = Controls.Instance.FirstPerson(gamePadIndex);

            //Work out rotation and lerping
            camera.Update();

            gui.Update(this);
        }
        

        private void InitialiseAircraft()
        {
            switch (AircraftType)
            {
                case 0:
                    aircraft = new Jet(spawn.spawnPos, new Quaternion(0, 0, 0, 1), playerIndex,
                        vehicleSpawn.jetBuffer, planeEffect, AircraftType);
                    break;

                case 1:
                    aircraft = new Helicopter(spawn.spawnPos, new Quaternion(0, 0, 0, 1), playerIndex,
                        vehicleSpawn.heliBuffer, planeEffect, AircraftType, Content);
                    break;
            }
        }

        private void spawnTrail(GameTime gameTime)
        {
            Vector4 healthColour = Vector4.SmoothStep(NO_HEALTH, FULL_HEALTH, (float)currentHealth / (float)MAX_HEALTH);

            if (aircraft.aircraftType == 0)
            {
                spawnJetTrail(healthColour, gameTime);
            }
            else
            {
                spawnHeliTrail(healthColour, gameTime);
            }
        }

        private void spawnJetTrail(Vector4 healthColour, GameTime gameTime)
        {
            Random random = new Random();

            float particleScale = aircraft.Velocity.Z;
            particleScale /= Jet.MAX_VELOCITY.Z;

            Vector3 particleOffset =
                Vector3.Transform(
                    new Vector3(0, 0, 20),
                    aircraft.modelRot
                );

            //Central trail
            ParticleManager.Instance().AddParticle(
                new Color(healthColour.X, healthColour.Y, healthColour.Z, healthColour.W),
                aircraft.position + aircraft.modelOffset + particleOffset,
                Quaternion.CreateFromYawPitchRoll(0, 0, random.Next((int)(Math.PI * 20)) / 10),
                new Vector3(particleScale, particleScale, particleScale),
                100,
                Vector3.Zero,
                Vector3.One,
                Quaternion.CreateFromYawPitchRoll(0, 0, 0.1f),
                Quaternion.Identity,
                new Vector3(-0.1f, -0.1f, -0.1f),
                new Vector3(0, 0, 0)
            );

            //Helix Offsets
            for (int i = 0; i < 2; i++)
            {
                Vector3 particleVelocity =
                    Vector3.Transform(
                        new Vector3(0, 0.2f, 0),
                        aircraft.modelRot *
                            Quaternion.CreateFromAxisAngle(
                                Vector3.Forward,
                                (float)gameTime.TotalGameTime.TotalMilliseconds / 100 % (float)(Math.PI * 2)
                            ) *
                            Quaternion.CreateFromAxisAngle(Vector3.Forward, (float)Math.PI * i)
                    );

                ParticleManager.Instance().AddParticle(
                    new Color(healthColour.X, healthColour.Y, healthColour.Z, healthColour.W),
                    aircraft.position + aircraft.modelOffset + particleOffset,
                    Quaternion.CreateFromYawPitchRoll(0, 0, random.Next((int)(Math.PI * 20)) / 10),
                    new Vector3(particleScale, particleScale, particleScale),
                    100,
                    particleVelocity,
                    Vector3.One,
                    Quaternion.CreateFromYawPitchRoll(0, 0, 0.1f),
                    Quaternion.Identity,
                    new Vector3(-0.1f, -0.1f, -0.1f),
                    new Vector3(0, 0, 0)
                );

                particleVelocity = Vector3.Transform(Vector3.Forward, aircraft.modelRot * Quaternion.CreateFromAxisAngle(Vector3.Forward, (float)Math.PI));
            }
        }

        private void spawnHeliTrail(Vector4 healthColour, GameTime gameTime)
        {
            Random random = new Random();

            float particleScale = Math.Abs(aircraft.Velocity.Y);
            particleScale /= Helicopter.MAX_VELOCITY;
            particleScale++;

            Vector3 particleOffset =
                Vector3.Transform(
                    new Vector3(0, 10, 0),
                    aircraft.modelRot
                );

            //Helix Offsets
            for (int i = 0; i < 2; i++)
            {
                Vector3 particleVelocity =
                    Vector3.Transform(
                        new Vector3(0, 0.2f, 0.2f),
                        aircraft.modelRot *
                            Quaternion.CreateFromAxisAngle(
                                Vector3.Up,
                                (float)gameTime.TotalGameTime.TotalMilliseconds / 100 % (float)(Math.PI * 2)
                            ) *
                            Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI * i)
                    );

                ParticleManager.Instance().AddParticle(
                    new Color(healthColour.X, healthColour.Y, healthColour.Z, healthColour.W),
                    aircraft.position + aircraft.modelOffset + particleOffset,
                    Quaternion.CreateFromYawPitchRoll(0, 0, random.Next((int)(Math.PI * 20)) / 10),
                    new Vector3(particleScale, particleScale, particleScale),
                    100,
                    particleVelocity,
                    Vector3.One,
                    Quaternion.CreateFromYawPitchRoll(0, 0, 0.1f),
                    Quaternion.Identity,
                    new Vector3(0.05f, 0.05f, 0.05f),
                    Vector3.Zero
                );

                particleVelocity = Vector3.Transform(Vector3.Up, aircraft.modelRot * Quaternion.CreateFromAxisAngle(Vector3.Up, (float)Math.PI));
            }
        }
    }
}

