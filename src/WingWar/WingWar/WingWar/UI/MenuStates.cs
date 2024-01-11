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
    class MenuStates
    {
        public Menus menu;
        public Options options;
        private Game game;
        private ViewportManager viewManager;

        private int menuChoice = 0, optionsChoice = 0;
        private int[] playerAircraft = new int[4];
        private bool prevAcceptState;
        private Vector2 prevLeftState;
        public float audioVolume = 50.0f / 255f;
        public bool menuDraw = false, optionsDraw = false;
        
        private enum AircraftType { Jet = 0, Heli }
        public enum MenuState { TitleScreen = 0, RootMenu, Options, Attract, MapMenu, AircraftMenu, LoadState, Loaded, Game };
        public MenuState menuState;

        public MenuStates(Game game, ViewportManager viewManager)
        {
            this.game = game;
            this.viewManager = viewManager;

            menu = new Menus(game, menuChoice);
            options = new Options(game, optionsChoice);
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
            switch (menuState)
            {
                case MenuState.TitleScreen:
                    menuDraw = true;
                    menu.menuChoice = 0;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                        switch (menu.selection)
                        {
                            case 0:
                                menuState = MenuState.RootMenu;
                                break;
                            case 1:
                                game.Exit();
                                break;
                        }
                    }
                    break;

                case MenuState.RootMenu:
                    optionsDraw = false;
                    menuDraw = true;
                    menu.menuChoice = 1;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                        switch (menu.selection)
                        {
                            case 0:
                                Random rand = new Random(DateTime.Now.Second);
                                Game.terrainHeight = rand.Next(15, 50);
                                Game.cityHeight = rand.Next(50, 130);
                                Game.weight = rand.Next(1, 10);
                                Game.citySize = rand.Next(30, 70);
                                Game.foliage = rand.Next(2, 5);
                                WinCondition.Instance.MaxScore = 10;
                                GameType.Instance.ToggleNormal(true);

                                menuState = MenuState.AircraftMenu;
                                menu.selection = 0;
                                break;
                            case 1:
                                menu.selection = 0;
                                menuState = MenuState.MapMenu;
                                break;
                            case 2:
                                menuState = MenuState.Options;
                                break;
                            case 3:
                                menuState = MenuState.TitleScreen;
                                break;
                        }
                    }
                    break;

                case MenuState.Options:
                    menuDraw = false;
                    optionsDraw = true;
                    options.Update(gameTime);

                    if (Game.InGame) 
                    { 
                        options.paused = "PAUSED";
                        options.resume = "Resume Game";
                        options.quit = "Quit to Menu";
                    }
                    else 
                    { 
                        options.paused = "OPTIONS";
                        options.resume = "Accept";
                        options.quit = "Quit Game";
                    }

                    switch (options.selection)
                    {
                        case 1:
                            if (Controls.Instance.LeftStick(0).X > 0 && !Game.GraphicsMgr.IsFullScreen && !options.bFullscreen)
                            {
                                options.bFullscreen = true;
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                Game.GraphicsMgr.ToggleFullScreen();
                            }

                            if (Controls.Instance.LeftStick(0).X < 0 && Game.GraphicsMgr.IsFullScreen && options.bFullscreen)
                            {
                                options.bFullscreen = false;
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                Game.GraphicsMgr.ToggleFullScreen();  
                            }
                            break;

                        case 2:
                            if (Controls.Instance.LeftStick(0).X < 0 && !options.bDebug)
                            {
                                options.bDebug = true;
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                DebugDisplay.Instance.DrawDebug = false;
                            }
                            if (Controls.Instance.LeftStick(0).X > 0 && options.bDebug)
                            {
                                options.bDebug = false;
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                DebugDisplay.Instance.DrawDebug = true;
                            }
                            break;

                        case 3:
                            if (Controls.Instance.LeftStick(0).X > 0) 
                            { 
                                options.audioVolume++;
                                
                            }
                            if (Controls.Instance.LeftStick(0).X < 0) 
                            { 
                                options.audioVolume--;
                            }

                            options.audioVolume = MathHelper.Clamp(options.audioVolume, 0, 100);
                            audioVolume = (options.audioVolume / 255f);
                            MediaPlayer.Volume = audioVolume;
                            break;

                        case 4:
                            if (Controls.Instance.LeftStick(0).X > 0) 
                            {     
                                SoundEffects.Instance().FXSoundUp();
                                options.soundEffectVolume++;
 
                            }
                            if (Controls.Instance.LeftStick(0).X < 0) 
                            { 
                                SoundEffects.Instance().FXSoundDown();
                                options.soundEffectVolume--;
                            }

                            options.soundEffectVolume = MathHelper.Clamp(options.soundEffectVolume, 0, 100);
                            float soundEffectVolume = (options.soundEffectVolume / 250);
                            break;

                        case 5:
                            if (Controls.Instance.Accept(0) && !prevAcceptState)
                            {
                                optionsDraw = false;
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                if (Game.InGame)
                                {
                                    menuState = MenuState.Game;
                                }
                                else
                                {
                                    menuState = MenuState.RootMenu;
                                }
                            }
                            break;
                        case 6:
                            if (Controls.Instance.Accept(0) && !prevAcceptState)
                            {
                                SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                                if (Game.InGame)
                                {
                                    Game.InGame = false;
                                    game.UnloadAssets();
                                    MenuCamera.Instance.ConstructMenuScene();
                                    game.LoadAssets(true);
                                    SoundEffects.Instance().StopSounds();
                                    MediaPlayer.Stop();
                                    Audio.Instance.PlayMenuMusic(audioVolume);
                                    menuState = MenuState.RootMenu;
                                }
                                else
                                {
                                    game.Exit();
                                }
                            }
                            break;
                    }
                    break;

                case MenuState.Attract:
                    menuDraw = true;
                    menu.menuChoice = 2;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                        menuState = MenuState.TitleScreen;
                    }
                    break;

                case MenuState.MapMenu:
                    menuDraw = true;
                    menu.menuChoice = 3;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                        switch (menu.selection)
                        {
                            case 7:
                                for (int i = 0; i < 5; i++)
                                {
                                    menu.sliders[i].X = menu.rectangles[i].X + 50;
                                }

                                menu.maxKills = 10;
                                break;

                            case 8:
                                Random rand;
                                rand = new Random((int)DateTime.Now.Millisecond);

                                int min = (int)menu.rectangles[0].X;
                                int max = (int)menu.rectangles[0].X + 100;

                                float terrainLvl = (float)rand.Next(min, max);
                                float waterLvl = (float)rand.Next(min, max);
                                float cityLvl = (float)rand.Next(min, max);
                                float distribution = (float)rand.Next(min, max);
                                float trees = (float)rand.Next(min, max);
                                int kills = rand.Next(1, 6);

                                menu.sliders[0].X = cityLvl;
                                menu.sliders[1].X = terrainLvl;
                                menu.sliders[2].X = waterLvl;
                                menu.sliders[3].X = distribution;
                                menu.sliders[4].X = trees;
                                menu.maxKills = kills * 5;
                                break;

                            case 9:
                                Game.terrainHeight = menu.terrainHeight;
                                Game.cityHeight = menu.cityHeight;
                                Game.weight = menu.waterAmt;
                                Game.citySize = menu.citySize;
                                Game.foliage = menu.foliage;
                                WinCondition.Instance.MaxScore = menu.maxKills;
                                menuState = MenuState.AircraftMenu;
                                break;

                            case 10:
                                menuState = MenuState.RootMenu;
                                break;
                        }
                    }
                    break;

                case MenuState.AircraftMenu:
                    menuDraw = true;
                    menu.menuChoice = 4;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        SoundEffects.Instance().PlaySound(SoundEffects.SoundCues.MenuAccept);
                        switch (menu.selection)
                        {
                            case 0:
                                playerAircraft[0] = (int)AircraftType.Jet;
                                menuState = MenuState.LoadState;
                                break;
                            case 1:
                                playerAircraft[0] = (int)AircraftType.Heli;
                                menuState = MenuState.LoadState;
                                break;
                            case 2:
                                menuState = MenuState.RootMenu;
                                break;
                        }
                    }
                    break;

                case MenuState.LoadState:

                    menuDraw = true;
                    menu.menuChoice = 5;
                    menu.Update(gameTime);
                    
                    menuState = MenuState.Loaded;

                    break;

                case MenuState.Loaded:

                    menuDraw = true;
                    menu.menuChoice = 6;
                    menu.Update(gameTime);

                    if (Controls.Instance.Accept(0) && !prevAcceptState)
                    {
                        GameType.Instance.SetGameType();
                        viewManager.AddViewport();
                        viewManager.AddPlayer(playerAircraft[0]);
                        Audio.Instance.StopMusic();
                        SoundEffects.Instance().active = true;
                        menuState = MenuState.Game;
                    }
                    break;
            }
            prevAcceptState = Controls.Instance.Accept(0);
            prevLeftState = Controls.Instance.LeftStick(0);
        }
    }
}
