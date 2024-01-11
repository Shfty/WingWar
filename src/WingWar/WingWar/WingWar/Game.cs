#define DEBUG
//#undef DEBUG

#define FULLSCREEN
//#undef FULLSCREEN

//#define ARCADE
#undef ARCADE

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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        const int PREFERRED_2D_VERTICAL_RESOLUTION = 1080;

        public static ContentManager ContentMgr;
        public static GraphicsDeviceManager GraphicsMgr;
        public static SpriteBatch SpriteBatch;
        public static SpriteFont MenuFont;
        public static SpriteFont MenuFontSmall;
        public static GameTime GameTime;
        public static float MasterScale2D, terrainHeight, cityHeight, citySize, weight, foliage;
        public static bool InGame;
        public static string Title = "OPERATION: SKYFLARE";

        List<Viewport> viewportList;
        List<Player> playerList;

        //Menu Camera setup
        Camera menuCamera;

        //Attract State
        const int ATTRACT_MODE_TIMEOUT = 10000;
        int attractModeTimer = 0;
        bool attractMode = false;
        AttractScene attractScene;
        Intro intro;

        int winTimer;

        TerrainFactory terrain; InstancedCity instancedCity; 
        Bridges bridges; Tunnels tunnels; Water water; Trees trees;
        MenuStates menuStates; Shield shield; Collisions collision;
        ViewportManager viewManager; BloomComponent bloom;

        bool guiDraw = false;
        int maxPlayers;

        enum GameState { Intro = 0, Menu, Game, Win }
        GameState gameState;

        bool freezeCam = false;
        bool prevFreezeCamState = false;

        public Game()
        {
            GraphicsMgr = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ContentMgr = Content;
            this.Window.Title = Title;

            //Relatively low resolution to make sure the window does not exceed screen bounds
#if FULLSCREEN
            GraphicsMgr.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            GraphicsMgr.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#else
            GraphicsMgr.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            GraphicsMgr.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
#endif
            GraphicsMgr.PreferMultiSampling = true;
            MasterScale2D = (float)GraphicsMgr.PreferredBackBufferHeight / PREFERRED_2D_VERTICAL_RESOLUTION;
        }

        protected override void Initialize()
        {
            viewportList = new List<Viewport>();
            playerList = new List<Player>();
            terrain = new TerrainFactory();

#if ARCADE
            maxPlayers = 2;
#else
            maxPlayers = 4;
#endif

#if FULLSCREEN
            GraphicsMgr.ToggleFullScreen();
#endif

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            GraphicsDevice.Clear(Color.Red);
#if !MUTE_MUSIC
            Audio.Instance.LoadMenu();
#endif

            MenuCamera.Instance.ConstructMenuScene();
            LoadAssets(true);
            menuCamera = new Camera();
            menuCamera.AspectRatio = GraphicsMgr.GraphicsDevice.Viewport.AspectRatio;
            Materials.Instance.Ambience = 0.7f;
            SoundEffects.Instance().LoadGame();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MenuFont = Game.ContentMgr.Load<SpriteFont>("Fonts//menuFont");
            MenuFontSmall = Game.ContentMgr.Load<SpriteFont>("Fonts//menuFontSmall");
            viewManager = new ViewportManager(viewportList, playerList, Content);
            menuStates = new MenuStates(this, viewManager);
            menuStates.LoadContent(Content);

            //Attract Scene
            attractScene = new AttractScene(Content, LeaveAttractCallback);

            //Intro
            intro = new Intro();

            //Post Processing
            bloom = new BloomComponent(this);
            bloom.Load();
            Components.Add(bloom);

            menuStates.menuState = 0;
            
#if !MUTE_MUSIC
            //Play menu music
            Audio.Instance.PlayMenuMusic(menuStates.audioVolume);
#endif
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            switch (gameState)
            {
                case GameState.Intro:
                    intro.Update();
                    if (intro.IsDone() || Controls.Instance.ActionButtons(0))
                    {
                        gameState = GameState.Menu;
                    }
                    break;
                case GameState.Menu:

                    menuStates.menuDraw = true; guiDraw = false;
                    menuStates.Update(gameTime);                        

                    float aspect = (float)GraphicsMgr.GraphicsDevice.Viewport.Width / (float)GraphicsMgr.GraphicsDevice.Viewport.Height;

                    if (!InGame)
                    {
                        Materials.Instance.Ambience = 0.7f;

                        if (!attractMode)
                        {
                            //Calculate rotation around city animation
                            float rotationTimer = (float)gameTime.TotalGameTime.TotalMilliseconds * 0.0001f;
                            float distance = terrain.citySize * 400;
                            Vector3 cityOffset = new Vector3(terrain.citySize, 0, -terrain.citySize) * 150;

                            menuCamera.Position = terrain.cityPosition +  cityOffset + new Vector3((float)(Math.Cos(rotationTimer) * distance), 1500, (float)(Math.Sin(rotationTimer) * distance));
                            menuCamera.LookAt = terrain.cityPosition + cityOffset;

                            menuCamera.Update();

                            if (Controls.Instance.AttractModeTimer())
                                attractModeTimer = 0;

                            attractModeTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (attractModeTimer > ATTRACT_MODE_TIMEOUT)
                            {
                                attractModeTimer = 0;
                                attractMode = true;
#if !MUTE_MUSIC
                                Audio.Instance.StopMusic();
                                Audio.Instance.PlayAttractMusic(menuStates.audioVolume);
#endif
                                attractScene.Start(gameTime);
                            }
                        }
                        else
                        {
                            attractScene.Update(gameTime);
                        }
                    }

                    if ((int)menuStates.menuState == 6)
                    {
                        UnloadAssets();
                        LoadAssets(false);
                    }
                    if ((int)menuStates.menuState == 8)
                    {
                        gameState = GameState.Game;
                        attractModeTimer = 0;
#if !MUTE_MUSIC
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            Audio.Instance.StopMusic();
                            Audio.Instance.PlayBackgroundMusic(menuStates.audioVolume);
                        }
#endif
                    }

                    break;

                case GameState.Game:
                    if (!freezeCam)
                    {
                        updateGame(gameTime);
                    }
                    else
                    {
                        updateFreezeCam();
                    }
                    break;

                case GameState.Win:

                    winTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (winTimer > 5000)
                    {
                        InGame = false;
                        winTimer = 0;
                        gameState = GameState.Menu;
                        menuStates.menuState = MenuStates.MenuState.RootMenu;
                        UnloadAssets();
                        MenuCamera.Instance.ConstructMenuScene();
                        SoundEffects.Instance().StopSounds();
                        GraphicsDevice.Viewport = viewManager.defaultViewport;
                        LoadAssets(true);
                    }
                    break;
            }

            shield.Update();
            SoundEffects.Instance().Update(gameTime, playerList);
            prevFreezeCamState = Controls.Instance.FreezeCam(0);
            base.Update(gameTime);
        }

        void LeaveAttractCallback()
        {
            attractMode = false;

            Audio.Instance.StopMusic();
            Audio.Instance.PlayMenuMusic(menuStates.audioVolume);
        }

        private void updateGame(GameTime gameTime)
        {
            menuStates.menuDraw = false; guiDraw = true; InGame = true;

            Audio.Instance.SelectMusic();
            bloom.Visible = true;
            Materials.Instance.Ambience = 0.7f;

            foreach (Player player in playerList)
            {
                int i = player.playerIndex - 1;

                float aspectRatio;

                try
                {
                    aspectRatio = (float)viewportList[i].Width / (float)viewportList[i].Height;
                }
                catch
                {
                    aspectRatio = (float)viewManager.defaultViewport.Width / (float)viewManager.defaultViewport.Height;
                }

                player.camera.RotationLerpFactor = 0.4f;
                player.Update(aspectRatio, gameTime, player, playerList);
                if (!player.FlyIn)
                {
                    collision.Update(player, playerList, gameTime);
                }

                if (WinCondition.Instance.HasPlayerWon(player.kills, player.playerIndex))
                    gameState = GameState.Win;
            }

            ParticleManager.Instance().Update();
            GUIscale.Instance.Update(playerList, viewportList.Count);
            GraphicsDeviceSettings.Instance.Wireframe();

#if ARCADE
            DropInDropOut.Instance.ArcadeManagePlayers(viewportList.Count - 1, viewManager, maxPlayers);
#else
                        if (!GamePad.GetState(PlayerIndex.One).IsConnected)
                            DropInDropOut.Instance.DebugManagePlayers(viewportList.Count - 1, viewManager, maxPlayers);
                        else
                            DropInDropOut.Instance.ManagePlayers(viewportList.Count, viewManager, maxPlayers);
#endif


            //Quit w/escape or arcade sidebuttons
            if (Controls.Instance.Start(0))
            {
                gameState = GameState.Menu;
                menuStates.menuState = MenuStates.MenuState.Options;
            }

            //Toggle Freeze Cam with D-Pad Down or F1
            if (Controls.Instance.FreezeCam(0) && !prevFreezeCamState)
            {
                freezeCam = true;
            }
        }

        private void updateFreezeCam()
        {
            foreach (Player player in playerList)
            {
                int i = player.playerIndex - 1;

                float aspectRatio;

                try
                {
                    aspectRatio = (float)viewportList[i].Width / (float)viewportList[i].Height;
                }
                catch
                {
                    aspectRatio = (float)viewManager.defaultViewport.Width / (float)viewManager.defaultViewport.Height;
                }

                player.camera.RotationLerpFactor = 1;
                player.camera.LookAt = player.aircraft.position;
                player.camera.Rotation *= Quaternion.CreateFromYawPitchRoll((float)(Math.PI / 90f) * -Controls.Instance.LeftStick(0).X, (float)(Math.PI / 90f) * Controls.Instance.LeftStick(0).Y, (float)(Math.PI / 90f) * -Controls.Instance.RightStick(0).X);
                player.camera.AspectRatio = aspectRatio;
                player.camera.Update();
            }

            //Toggle Freeze Cam with D-Pad Down or F1
            if (Controls.Instance.FreezeCam(0) && !prevFreezeCamState)
            {
                freezeCam = false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Bloom Drawing
            bloom.BeginDraw();

            //3D Rendering
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                Materials.Instance.SceneBackground(), 1.0f, 0);

            GraphicsDeviceSettings.Instance.ResetAfterSpriteBatch();

            //Intro
            if (gameState == GameState.Intro)
            {
                intro.Draw();
            }

            //Menu Background
            if ((menuStates.menuDraw || menuStates.optionsDraw) && !InGame)
            {
                //Title Screen
                if (!attractMode)
                {
                    Materials.Instance.Ambience = 0.7f;
                    DrawMenuScene(menuCamera, gameTime);
                }
                else //Attract State
                {
                    Materials.Instance.Ambience = 1.0f;
                    attractScene.Draw();
                }
            }

            //In-game
            if (!menuStates.menuDraw)
            {
                for (int i = 0; i < viewportList.Count(); i++)
                {

                    GraphicsDeviceSettings.Instance.ResetAfterSpriteBatch();
                    DebugDisplay.Instance.Update();

                    GraphicsDevice.Viewport = viewportList[i];

                    try
                    {
                        if (!playerList[i].Active || menuStates.optionsDraw)
                            Materials.Instance.Ambience = 0.1f;
                        else
                            Materials.Instance.Ambience = 0.7f;

                        DrawScene(playerList[i].camera, gameTime);

                        Game.SpriteBatch.Begin();
                        if (guiDraw)
                            if (!playerList[i].FlyIn)
                                playerList[i].gui.DrawGUI(playerList[i].camera);
                        Game.SpriteBatch.End();
                    }
                    catch
                    {
                        DropInDropOut.Instance.Draw(viewportList[i], i, viewManager);
                    }

                    GraphicsDevice.Viewport = viewManager.defaultViewport;
                }
            }

            //2D Rendering
            Game.SpriteBatch.Begin();

            if (InGame && !menuStates.optionsDraw)
            {
                DebugDisplay.Instance.Draw(gameTime);
                viewManager.Draw();
            }

            if (menuStates.optionsDraw)
            {
                if (InGame)
                {
                    Materials.Instance.Ambience = 0.1f;
                }

                menuStates.options.Draw();
            }

            if (!attractMode)
            {
                if (menuStates.menuDraw)
                {
                    menuStates.menu.Draw();
                }

                if ((int)menuStates.menuState == 7)
                {
                    GraphicsDevice.Clear(Color.Black);
                }
            }

            SpriteBatch.End();

            if (gameState == GameState.Win)
            {
                WinCondition.Instance.Draw(viewportList);
                //Game.GraphicsMgr.GraphicsDevice.Viewport = viewManager.defaultViewport;
            }

            base.Draw(gameTime);
        }

        private void DrawScene(Camera camera, GameTime gameTime)
        {
            terrain.Draw(camera);
            instancedCity.Draw(camera);
            trees.Draw(camera);
            tunnels.Draw(camera);
            
            foreach (Player player in playerList)
            {
                if (player.Active)
                {
                    player.weaponFac.Draw(camera);

                    if (camera != player.camera || camera.Type == Camera.CameraType.ThirdPerson || player.FlyIn)
                    {
                        player.aircraft.Draw(camera);
                    }
                }
            }

            ParticleManager.Instance().Draw(camera);
            water.Draw(camera, gameTime);
            bridges.Draw(camera);
            DrawShield(camera);
        }

        private void DrawMenuScene(Camera camera, GameTime time)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                Materials.Instance.SceneBackground(), 1.0f, 0);

            terrain.Draw(camera);
            instancedCity.Draw(camera);
            trees.Draw(camera);
            bridges.Draw(camera);
            tunnels.Draw(camera);
            water.Draw(camera, time);
            DrawShield(camera);
        }

        private void DrawShield(Camera camera)
        {
            GraphicsDeviceSettings.Instance.SetWire();
            shield.Draw(camera);
            GraphicsDeviceSettings.Instance.SetSolid();
            shield.Draw(camera);
        }

        public void UnloadAssets()
        {
            shield = null;
            water = null;
            collision = null;
            trees = null;
            tunnels = null;
            bridges = null;
            instancedCity = null;
            collision = null;

            for (int i = 0; i < viewportList.Count; i++)
            {
                viewManager.RemovePlayer(i);                
            }

            viewManager.SliceViewports();
        }

        public void LoadAssets(bool menu)
        {
            Audio.Instance.LoadGame();
            SoundEffects.Instance().LoadGame();
            terrain.LoadContent(Game.terrainHeight, Game.citySize, Game.weight, menu);
            instancedCity = new InstancedCity(Game.ContentMgr, terrain.cityPosition, (int)Game.citySize / 2, Game.cityHeight);
            trees = new Trees(Game.ContentMgr, 160, terrain.heightData, Game.foliage);
            bridges = new Bridges(Game.ContentMgr, terrain.cityPosition, (int)Game.citySize / 2, Game.cityHeight, instancedCity.bbList);
            tunnels = new Tunnels(Game.ContentMgr, terrain.cityPosition, (int)Game.citySize / 2, Game.cityHeight, instancedCity.bbList);
            shield = new Shield(100, new Quaternion(0, 0, 0, 1),
                new Vector3(2000, 6390, -16000), new Vector3(140, 140, 140));
            water = new Water(new Vector3(0, -609, -32000),
                new Quaternion(0, 0, 0, 1), new Vector3(500, 1, 500));
            collision = new Collisions(water, terrain, instancedCity, shield);
        }
    }
}