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
    class Collisions
    {
        private SpawnPoints spawn;
        Water water; TerrainFactory terrain; InstancedCity city; Shield shield;

        bool check;

        public Collisions(Water water, TerrainFactory terrain, InstancedCity city, Shield shield)
        {
            spawn = new SpawnPoints();
            this.water = water;
            this.terrain = terrain;
            this.city = city;
            this.shield = shield;
            check = true;
        }

        public void Update(Player player, List<Player> playerList, GameTime gameTime)
        {
            CityCollision(player);
            TerrainCollision(player, check);
            WaterCollisison(player);
            AircraftCollision(playerList);
            ShieldCollision(player);
            BulletCollision(player, playerList);
            MissileCollision(player, playerList);
            TargetIntersection(player, playerList);
            CheckRespawns(player);
        }

        private void TargetIntersection(Player player, List<Player> playerList)
        {
            BoundingFrustum targetFrustum = new BoundingFrustum(player.camera.FirstPersonViewMatrix() * player.camera.CustomProjectionMatrix(10, 1.0f));
            Player closestTarget = null;

            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i] == player)
                {
                    continue;
                }

                if (playerList[i].Active)
                {
                    if (targetFrustum.Contains(playerList[i].aircraft.aircraftSphere) != ContainmentType.Disjoint)
                    {
                        float distance = Vector3.Distance(playerList[i].aircraft.position, player.aircraft.position);
                        if (closestTarget == null || distance < Vector3.Distance(closestTarget.aircraft.position, player.aircraft.position))
                        {
                            closestTarget = playerList[i];
                        }
                    }
                }
            }

            if (closestTarget != null)
            {
                player.targetIndex = closestTarget.playerIndex;

                targetFrustum = new BoundingFrustum(player.camera.FirstPersonViewMatrix() * player.camera.CustomProjectionMatrix(5, 1.0f));
                player.bulletLock = targetFrustum.Contains(closestTarget.aircraft.aircraftSphere) != ContainmentType.Disjoint;
            }
            else
            {
                player.targetIndex = -1;
                player.bulletLock = false;
            }
        }

        private void BulletCollision(Player player, List<Player> playerList)
        {
            //NOTE: This may still be unstable, but a few minutes of testing revealed no crashes
            //Repro: Fly around for a while holding shoot + missile
            for (int i = 0; i < player.weaponFac.bullets.Count - 1; i++)
            {
                bool br = false;

                //Bullet to aircraft collision
                for (int a = 0; a < playerList.Count; a++)
                {
                    //Don't collide with self or dead players
                    if (playerList[a] == player || !playerList[a].Active)
                    {
                        continue;
                    }
                    if (player.weaponFac.bullets[i].hitSphere.Intersects(
                        playerList[a].aircraft.aircraftSphere))
                    {
                        DestroyBullet(player, i);
                        playerList[a].currentHealth -= GameType.Instance.BulletDamage;

                        if (playerList[a].currentHealth <= 0 && playerList[a].Active)
                        {
                            player.kills++;
                            player.weaponFac.missileCount += 1;
                            Death(playerList[a]);
                        }
                        br = true;
                        break;
                    }
                }
                if (br)
                {
                    continue;
                }

                //Bullet to building collision

                if (BulletCollisionsOn())
                {
                    if (player.weaponFac.bullets[i].hitSphere.Intersects(city.citySphere))
                    {
                        foreach (BoundingBox collisionBox in city.bbList)
                        {
                            if (player.weaponFac.bullets[i].hitSphere.Intersects(
                                collisionBox))
                            {
                                DestroyBullet(player, i);
                                br = true;
                                break;
                            }
                        }
                        if (br)
                        {
                            continue;
                        }
                    }
                }

                //Bullet to water collision
                if (player.weaponFac.bullets[i].hitSphere.Intersects(
                    water.waterBox))
                {
                    DestroyBullet(player, i);
                    continue;
                }

                //Bullet to terrain collision
                if ((player.weaponFac.bullets[i].position.X >= 1) && (player.weaponFac.bullets[i].position.Z <= -1) &&
                (player.weaponFac.bullets[i].position.X <= 31999) && (player.weaponFac.bullets[i].position.Z >= -31999))
                {
                    float heightAtPoint = (int)(terrain.heightData[
                        (int)(player.weaponFac.bullets[i].position.X / 200),
                        (int)(-player.weaponFac.bullets[i].position.Z / 200)] * 50);

                    if (player.weaponFac.bullets[i].position.Y < heightAtPoint)
                    {
                        DestroyBullet(player, i);
                        continue;
                    }
                }

                if (!player.weaponFac.bullets[i].hitSphere.Intersects(shield.shieldSphere))
                {
                    DestroyBullet(player, i);
                    continue;
                }
            }
        }

        //Because of the large amount of geometry added at last minute, code to stabilise gameplay.
        private bool BulletCollisionsOn()
        {
            if (FPS.Instance.fps <= 24)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void MissileCollision(Player player, List<Player> playerList)
        {
            //NOTE: This may still be unstable, but a few minutes of testing revealed no crashes
            //Repro: Fly around for a while holding shoot + missile
            for (int m = 0; m < player.weaponFac.missiles.Count; m++)
            {
                bool br = false;

                //Missile to aircraft collision
                for (int a = 0; a < playerList.Count; a++)
                {
                    //Don't collide with self or dead players
                    if (playerList[a] == player || !playerList[a].Active)
                    {
                        continue;
                    }
                    if (player.weaponFac.missiles[m].hitSphere.Intersects(
                        playerList[a].aircraft.aircraftSphere))
                    {
                        DestroyMissile(player, m);
                        playerList[a].currentHealth -= GameType.Instance.MissileDamage;

                        if (playerList[a].currentHealth <= 0 && playerList[a].Active)
                        {
                            player.kills++;
                            player.weaponFac.missileCount += 1;
                            Death(playerList[a]);
                        }
                        br = true;
                        break;
                    }
                }
                if (br)
                {
                    continue;
                }
                
                //Missile to building collision
                if (player.weaponFac.missiles[m].hitSphere.Intersects(city.citySphere))
                {
                    foreach (BoundingBox collisionBox in city.bbList)
                    {
                        if (player.weaponFac.missiles[m].hitSphere.Intersects(
                            collisionBox))
                        {
                            DestroyMissile(player, m);
                            br = true;
                            break;
                        }
                    }
                    if (br)
                    {
                        continue;
                    }
                }
                
                //Missile to water collision
                if (player.weaponFac.missiles[m].hitSphere.Intersects(
                    water.waterBox))
                {
                    DestroyMissile(player, m);
                    continue;
                }

                //Missile to terrain collision
                if ((player.weaponFac.missiles[m].position.X >= 1) && (player.weaponFac.missiles[m].position.Z <= -1) &&
                (player.weaponFac.missiles[m].position.X <= 31999) && (player.weaponFac.missiles[m].position.Z >= -31999))
                {
                    float heightAtPoint = (int)(terrain.heightData[
                        (int)(player.weaponFac.missiles[m].position.X / 200),
                        (int)(-player.weaponFac.missiles[m].position.Z / 200)] * 50);

                    if (player.weaponFac.missiles[m].position.Y < heightAtPoint)
                    {
                        DestroyMissile(player, m);
                        continue;
                    }
                }

                if (!player.weaponFac.missiles[m].hitSphere.Intersects(shield.shieldSphere))
                {
                    DestroyMissile(player, m);
                    continue;
                }
            }
        }

        //Testing collisions between player aircrafts.IF they hit eachother they die.
        private void AircraftCollision(List<Player> playerList)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < playerList.Count; j++)
                {
                    //Don't need to compare an aircraft with itself, so can skip that one.
                    if (i != j)
                    {
                        if (playerList[i].aircraft.aircraftSphere.Intersects(playerList[j].aircraft.aircraftSphere)
                            && playerList[i].Active)
                        {
                            Death(playerList[i]);
                        }
                    }
                }
            }
        }
        
        private void CityCollision(Player player)
        {  
            if (player.aircraft.aircraftSphere.Intersects(city.citySphere))
            {
                foreach (BoundingBox collisionBox in city.bbList)
                {
                    if (player.aircraft.aircraftSphere.Intersects(collisionBox)
                        && player.Active)
                    {
                        Death(player);
                    }
                }
            }
        }
        
        private void WaterCollisison(Player player)
        {
            if (player.aircraft.aircraftSphere.Intersects(water.waterBox) && player.Active)
            {
                Death(player);
            }
        }

        private void TerrainCollision(Player player, bool check)
        {
            if ((player.aircraft.position.X >= 1) && (player.aircraft.position.Z <= -1) &&
                (player.aircraft.position.X <= 31999) && (player.aircraft.position.Z >= -31999))
            {
                float heightAtPoint = (int)(terrain.heightData[
                    (int)(player.aircraft.position.X / 200),
                    (int)(-player.aircraft.position.Z / 200)] * 50);

                if (player.aircraft.position.Y < heightAtPoint && player.Active)
                {
                    Death(player);
                }
            }
        }

        private void ShieldCollision(Player player)
        {
            if (!player.aircraft.aircraftSphere.Intersects(shield.shieldSphere) && player.Active)
            {
                Death(player);
            }
        }

        private void Respawn(Player player)
        {
            player.Active = true;

            spawn.PlayerSpawn();
            player.currentHealth = 10;
            player.aircraft.Reset();
            player.aircraft.position = spawn.spawnPos;
            player.StartFlyIn();
            player.weaponFac.missileCount = player.weaponFac.startingMissiles;
            player.deaths++;
        }

        private void Death(Player player)
        {
            SoundEffects.Instance().PlaySound3D(SoundEffects.SoundCues.Explosion, player.aircraft.position);
            SpawnExplosion(
                player.aircraft.position,
                player.aircraft.modelRot,
                3,
                new Color(0.8f, 0.5f, 0.0f, 0.5f),
                16,
                32
            );
            player.Active = false;
            player.aircraft.velocity = Vector3.Zero;
            player.aircraft.Thrust(0.0f);
        }

        private void DestroyBullet(Player player, int index)
        {
            SoundEffects.Instance().PlaySound3D(SoundEffects.SoundCues.LaserImpact, player.weaponFac.bullets.ElementAt(index).position);
            SpawnExplosion(
                player.weaponFac.bullets.ElementAt(index).position,
                player.weaponFac.bullets.ElementAt(index).rotation,
                1,
                new Color(0.75f, 0.75f, 0.25f, 0.1f),
                1,
                32
            );
            player.weaponFac.bullets.RemoveAt(index);
        }

        private void DestroyMissile(Player player, int index)
        {
            SoundEffects.Instance().PlaySound3D(SoundEffects.SoundCues.MissileImpact, player.weaponFac.missiles.ElementAt(index).position);
            SpawnExplosion(
                player.weaponFac.missiles.ElementAt(index).position,
                player.weaponFac.missiles.ElementAt(index).rotation,
                2,
                new Color(0.8f, 0.5f, 0.0f, 0.5f),
                16,
                32
            );
            player.weaponFac.missiles.RemoveAt(index);
        }

        private void SpawnExplosion(Vector3 position,
                                    Quaternion rotation,
                                    float scale,
                                    Color colour,
                                    int slices = 8,
                                    int resolution = 16)
        {
            Vector3 sliceAxis = Vector3.Transform(Vector3.Forward, rotation);
            Vector3 damping = Vector3.One * 0.9f;

            for (int i = 0; i < slices; i++)
            {
                Vector3 velocity = Vector3.Transform(sliceAxis * scale, Matrix.CreateFromAxisAngle(Vector3.Transform(Vector3.Right, rotation), (float)Math.PI / 2));
                for (int o = 0; o < resolution; o++)
                {
                    ParticleManager.Instance().AddParticle(
                        colour,
                        position,
                        Quaternion.Identity,
                        new Vector3(scale, scale, scale),
                        40,
                        velocity,
                        damping,
                        Quaternion.CreateFromAxisAngle(Vector3.Forward, (float)Math.PI/180),
                        Quaternion.Identity
                    );

                    velocity = Vector3.Transform(velocity, Matrix.CreateFromAxisAngle(sliceAxis, (float)(Math.PI * 2) / resolution));
                }

                sliceAxis = Vector3.Transform(sliceAxis, Matrix.CreateFromAxisAngle(Vector3.Right, (float)(Math.PI) / slices));
            }
        }

        private void CheckRespawns(Player player)
        {
            PlayerIndex gamePad = new PlayerIndex();

            switch (player.playerIndex)
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
                default:
                    break;
            }

            if (!player.Active && (Controls.Instance.PrimaryFire(gamePad) || Controls.Instance.SecondaryFire(gamePad)))
            {
                Respawn(player);
            }
        }
    }
}
