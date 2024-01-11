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
    class WeaponFactory
    {
        static char TRACER_FREQUENCY = (char)2;

        SoundEffects weaponSounds; 
        BulletParticle bulletPart;
        MissileParticle missilePart;
        VertexBuffer bulletBuffer, missileBuffer;

        Vector3 rightOffset = new Vector3(4, -4, 2);
        Vector3 leftOffset = new Vector3(-6, -4, 2);
        Vector3 missileOffset = new Vector3(0, -5, 4);
        Vector3 gunPosition, missilePosition;
        Effect effect;

        //Creates an array in which to store individual bullets/missiles
        public List<Bullet> bullets;
        public List<Missile> missiles;

        int tracerCounter = 0;

        public int startingMissiles = 5;
        public int missileCount;

        //Values to calculate rate of fire depending on weapon
        int bulletReloadTime = 150;
        int missileReloadTime = 2000;
        int missileReload, bulletReload;

        PlayerIndex gamePad;

        //Initializes Default Values
        public WeaponFactory(int playerIndex, Effect effect, ContentManager Content)
        {
            this.effect = effect;
            missileCount = startingMissiles;
            weaponSounds = new SoundEffects();
            bulletPart = new BulletParticle();
            missilePart = new MissileParticle();

            bulletBuffer = bulletPart.verticesFac.objectBuffer;
            missileBuffer = missilePart.verticesFac.objectBuffer;

            bullets = new List<Bullet>();
            missiles = new List<Missile>();

            bulletReload = bulletReloadTime;
            missileReload = missileReloadTime;

            switch (playerIndex)
            {
                case 1:
                    gamePad = PlayerIndex.One;
                    break;
                case 2:
                    gamePad = PlayerIndex.Two;
                    break;
                case 3:
                    gamePad = PlayerIndex.Three;
                    break;
                case 4:
                    gamePad = PlayerIndex.Four;
                    break;
            }
        }

        private void fireBullets(Vector3 position, Quaternion rotation, int targetIndex)
        {
            gunPosition = (Vector3.Transform(rightOffset, Matrix.CreateFromQuaternion(rotation)) + position);
            AddBullet(rotation, targetIndex, tracerCounter % TRACER_FREQUENCY == 0);

            gunPosition = (Vector3.Transform(leftOffset, Matrix.CreateFromQuaternion(rotation)) + position);
            AddBullet(rotation, targetIndex, tracerCounter % TRACER_FREQUENCY == 0);

            SoundEffects.Instance().PlaySound3D(SoundEffects.SoundCues.Laser, position);

            tracerCounter++;
        }

        private void fireMissile(Vector3 position, Quaternion rotation, int targetIndex)
        {
            missileCount -= 1;
            missilePosition = (Vector3.Transform(missileOffset, Matrix.CreateFromQuaternion(rotation)) + position);

            Missile m = new Missile(missilePosition, rotation, targetIndex);
            missiles.Add(m);

            SoundEffects.Instance().PlaySound3D(SoundEffects.SoundCues.MissileFire, position);
        }

        public void Update(GameTime gameTime, Vector3 position, Quaternion rotation, List<Player> playerList, int targetIndex)
        {
            if (Controls.Instance.PrimaryFire(gamePad))
            {
                //Fire on press for responsiveness
                if (bulletReload == 0)
                {
                    fireBullets(position, rotation, targetIndex);
                }

                if (bulletReload < bulletReloadTime)
                {
                    bulletReload += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    bulletReload = 1; //Reset to 1 to avoid triggering fire-on-press
                    fireBullets(position, rotation, targetIndex);
                }
            }
            else
            {
                //Reset timer when released
                bulletReload = 0;
            }

            //If shooting, only release a single shot per 100 ms
            if (Controls.Instance.SecondaryFire(gamePad) && missileCount > 0)
            {
                if (missileReload >= missileReloadTime)
                {
                    missileReload = 0;
                    fireMissile(position, rotation, targetIndex);
                }
            }

            if (missileReload < missileReloadTime)
            {
                missileReload += gameTime.ElapsedGameTime.Milliseconds;
            }

            MathHelper.Clamp(startingMissiles, 0, 5);

            //Update bullet seeking
            foreach (Bullet bullet in bullets)
            {
                Vector3 targetPosition = Vector3.Zero;

                foreach (Player player in playerList)
                {
                    if (player.playerIndex != bullet.targetIndex)
                    {
                        continue;
                    }
                    else if (!bullet.LostTarget)
                    {
                        if (player.Active)
                        {
                            targetPosition = player.aircraft.position;
                        }
                        else
                        {
                            bullet.LostTarget = true;
                        }
                    }
                }

                bullet.Update(gameTime, targetPosition);
            }

            //If bullet has gone past active time in ms, remove it.
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].totalActiveTime > bullets[i].activeTime)
                    bullets.RemoveAt(i);
            }

            //Update missile seeking
            foreach(Missile missile in missiles)
            {
                Vector3 targetPosition = Vector3.Zero;
                Quaternion targetRotation = Quaternion.Identity;
                Vector3 targetVelocity = Vector3.Zero;

                foreach (Player player in playerList)
                {
                    if (player.playerIndex != missile.targetIndex)
                    {
                        continue;
                    }
                    else if(!missile.LostTarget)
                    {
                        if (player.Active)
                        {
                            targetPosition = player.aircraft.position;
                            targetRotation = player.aircraft.rotation;
                            targetVelocity = player.aircraft.velocity;
                        }
                        else
                        {
                            missile.LostTarget = true;
                        }
                    }
                }

                missile.Update(gameTime, targetPosition, targetRotation, targetVelocity);
            }

            //If missile has gone past active time in ms, remove it.
            for (int i = 0; i < missiles.Count; i++)
            {
                if (missiles[i].activeTime > missiles[i].totalActiveTime)
                    missiles.RemoveAt(i);
            }

            //Console.WriteLine(bullets.Count);
        }

        private void AddBullet(Quaternion rotation, int targetIndex, bool tracer)
        {
            Bullet b = new Bullet(gunPosition, rotation, targetIndex, tracer);
            bullets.Add(b);
        }


        public void Draw(Camera camera)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);

            foreach (Bullet b in bullets)
            {
                    Matrix World = Matrix.CreateFromQuaternion(b.rotation) *
                            Matrix.CreateScale(b.scale) *
                            Matrix.Invert(camera.ViewMatrix());
                    World.Translation = b.position;

                    effect.Parameters["World"].SetValue(World);

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(bulletBuffer);
                        Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 6);

                    }
            }

            foreach (Missile m in missiles)
            {
                Matrix World = Matrix.CreateFromQuaternion(m.rotation) *
                    Matrix.CreateScale(m.scale) *
                    Matrix.Invert(camera.ViewMatrix());
                World.Translation = m.position;

                effect.Parameters["World"].SetValue(World);

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(missileBuffer);
                    Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 6);
                }
            }

            if (DebugDisplay.Instance.DrawDebug)
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    DebugDisplay.Instance.Draw(bullets[i].hitSphere, camera.ViewMatrix(), camera.ProjectionMatrix());
                }

                for (int i = 0; i < missiles.Count; i++)
                {
                    DebugDisplay.Instance.Draw(missiles[i].hitSphere, camera.ViewMatrix(), camera.ProjectionMatrix());
                }
            }
        }
    }
}
