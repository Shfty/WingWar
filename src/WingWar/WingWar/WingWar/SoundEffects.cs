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
    class SoundEffects
    {
        public AudioEngine audioEngine;
        public WaveBank waveBank;
        public SoundBank soundBank;

        public Cue jetSound;
        public Cue heliSound;

        public bool active = false;
        public AudioCategory FXSound;
        private static SoundEffects instance;
        public float FXVolume = 0.5f;

        List<Player> playerList;

        public enum SoundCues
        {
            Explosion,
            Laser,
            LaserImpact,
            MenuAccept,
            MenuMove,
            MenuNO,
            MissileFire,
            MissileImpact,
            RadarLocking,
            RadarLocked
        }

        public SoundEffects()
        {
            
        }

        public static SoundEffects Instance()
        {
            if (instance == null)
            {
                instance = new SoundEffects();
                return instance;
            }
            else
            {
                return instance;
            }
        }

        public void LoadGame()
        {
            audioEngine = new AudioEngine("Content\\Audio\\SoundEffects\\SoundXact.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\SoundEffects\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\SoundEffects\\Sound Bank.xsb");

            FXSound = audioEngine.GetCategory("Global");
            FXSound.SetVolume(0.5f);

            jetSound = soundBank.GetCue("JetLoopAlt");
            heliSound = soundBank.GetCue("HelicopterLoop");
        }

        public void FXSoundDown()
        {
            FXVolume -= 0.01f;
            FXVolume = MathHelper.Clamp(FXVolume, 0.0f, 1.0f);
            FXSound.SetVolume(FXVolume);
        }

        public void FXSoundUp()
        {
            FXVolume += 0.01f;
            FXVolume = MathHelper.Clamp(FXVolume, 0.0f, 1.0f);
            FXSound.SetVolume(FXVolume);
        }

        public void PlaySound(SoundCues cue)
        {
            Cue soundCue = soundBank.GetCue(cue.ToString());
            soundCue.Play();
        }

        public void PlaySound3D(SoundCues cue, Vector3 soundPosition)
        {
            Vector3 nearestListenerPosition = NearestListener(soundPosition);

            AudioEmitter emitter = new AudioEmitter();
            emitter.Position = soundPosition;

            AudioListener listener = new AudioListener();
            listener.Position = nearestListenerPosition;

            soundBank.PlayCue(cue.ToString(), listener, emitter);
        }

        public Vector3 NearestListener(Vector3 soundPosition)
        {
            float shortestDistance = -1;
            int nearestIndex = -1;

            foreach (Player player in playerList)
            {
                float distance = Vector3.Distance(player.aircraft.position, soundPosition);

                if (shortestDistance == -1 || distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestIndex = playerList.IndexOf(player);
                }
            }

            return playerList[nearestIndex].aircraft.position;
        }

        public void Update(GameTime gameTime, List<Player> playerList)
        {
            // Update the players' positions
            this.playerList = playerList;

            EngineSound();

            // Update the audio engine.
            audioEngine.Update();
        }

        public void EngineSound()
        {
            //How many jets/helis are there, and how fast are they moving?
            int activeJets = 0;
            int activeHelis = 0;
            float totalJetVelocity = 0;
            float totalHeliVelocity = 0;
            foreach (Player player in playerList)
            {
                if (player.Active)
                {
                    if (player.AircraftType == 0)
                    {
                        activeJets++;
                        totalJetVelocity += -player.aircraft.velocity.Z;
                    }
                    else
                    {
                        activeHelis++;
                        totalHeliVelocity += player.aircraft.velocity.Y;
                    }
                }
            }

            //State Change
            if (active)
            {
                if (!jetSound.IsPlaying && activeJets > 0)
                {
                    jetSound = soundBank.GetCue("JetLoopAlt");
                    jetSound.Play();
                }
                else if (jetSound.IsPlaying && activeJets == 0)
                {
                    jetSound.Stop(AudioStopOptions.AsAuthored);
                }

                if (!heliSound.IsPlaying && activeHelis > 0)
                {
                    heliSound = soundBank.GetCue("HelicopterLoop");
                    heliSound.Play();
                }
                else if (heliSound.IsPlaying && activeHelis == 0)
                {
                    heliSound.Stop(AudioStopOptions.AsAuthored);
                }
            }

            //Pitch shift
            if (jetSound.IsPlaying)
            {
                float averageJetVelocity = totalJetVelocity / activeJets;
                jetSound.SetVariable("Pitch", averageJetVelocity);
            }

            if (heliSound.IsPlaying)
            {
                float averageHeliVelocity = totalHeliVelocity / activeHelis;
                heliSound.SetVariable("Pitch", averageHeliVelocity);
            }
        }

        public void StopSounds()
        {
            active = false;
            heliSound.Stop(AudioStopOptions.AsAuthored);
            jetSound.Stop(AudioStopOptions.AsAuthored);
        }
    }
}
