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
    class Audio
    {
        private static Audio instance;

        public Audio()
        {
            currentSong = rand.Next(0, NUM_SONGS);
            MediaPlayer.Volume = 0.05f;
        }

        public static Audio Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Audio();
                }
                return instance;
            }
        }

        private Song strategicAerialBombardment, propheticArmy, decimalRevolt, binaryChildren;

        private int currentSong = 0;
        private const int NUM_SONGS = 2; //How many songs to shuffle in-game

        //sound volume variables.
        //public float volume = 0.2f, preVolume = 0.2f;
        //private bool isMuted = false;

        //selects a random number for which song
        //initializes once in constructor
        Random rand = new Random();

        public void LoadMenu()
        {
            binaryChildren = Game.ContentMgr.Load<Song>("Audio//Binarychildren");
            strategicAerialBombardment = Game.ContentMgr.Load<Song>("Audio//StrategicAerialBombardment");
        }

        public void LoadGame()
        {
            propheticArmy = Game.ContentMgr.Load<Song>("Audio//PropheticArmy");
            decimalRevolt = Game.ContentMgr.Load<Song>("Audio//DecimalRevolt");
        }


        //Plays music in media player
        public void PlayBackgroundMusic(float audioVolume)
        {
            if (MediaPlayer.State != MediaState.Playing)
                SelectMusic();
            MediaPlayer.Volume = audioVolume;
        }

        public void PlayMenuMusic(float audioVolume)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(binaryChildren);
            MediaPlayer.Volume = audioVolume;
        }

        public void StopMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }

        public void PlayAttractMusic(float audioVolume)
        {
            if (MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(strategicAerialBombardment);
            MediaPlayer.Volume = audioVolume;
        }

        //Selects music based on random number generated.
        //When reaches end of track, plays next in library.
        public void SelectMusic()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                switch (currentSong)
                {
                    case 0:
                        MediaPlayer.Play(propheticArmy);
                        currentSong++;
                        break;
                    case 1:
                        MediaPlayer.Play(decimalRevolt);
                        currentSong++;
                        break;
                    case NUM_SONGS:
                        currentSong = 0;
                        break;
                }
            }
        }
    }
}
