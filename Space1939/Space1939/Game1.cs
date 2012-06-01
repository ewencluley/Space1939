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
using System.Collections;

using Space1939.Levels;
using Space1939.Menus;
using Space1939.HUD;

namespace Space1939
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region gfx_system
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion

        #region sound_system
        MusicPlayer musicEngine;
        SoundPlayer soundEngine;
        #endregion

        #region controls
        private MenuControl menuControl;
        #endregion

        HealthMeter healthMeter;
        Score scoreMeter;

        #region menus
        //Menus used in this level
        ControllerSelectMenu controllerSelect;
        MainMenu mainMenu;
        OptionsMenu optionsMenu;

        Menu currentMenu;
        //End of menus
        #endregion

        #region options
        Options options;
        #endregion

        #region levels

        ILevel intro;
        ILevel level1;
        ILevel level2;
        Queue<ILevel> levels;
        public ILevel currentLevel { get; private set; }
        

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            intro = new Intro(this);
            level1 = new Level1(this);
            level2 = new Level2(this);
            levels = new Queue<ILevel>();
            levels.Enqueue(intro);
            //levels.Enqueue(level1);
            levels.Enqueue(level2);
            currentLevel = levels.Dequeue();
            Components.Add(currentLevel);

            options = new Options();

            musicEngine = new MusicPlayer(this, "Content//Sound//Music//music", 0); //extensions are purposly missed off and are autmatically added by the MusicPlayer class
            soundEngine = new SoundPlayer(this, "Content//Sound//Sound//Sounds");

            healthMeter = new HealthMeter(this);
            healthMeter.DrawOrder = 10;
            Components.Add(healthMeter);

            scoreMeter = new Score(this);
            scoreMeter.DrawOrder = 10;
            Components.Add(scoreMeter);

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            

            #region Load Menus
            controllerSelect = new ControllerSelectMenu(this, new ControllerSelectMenu.MenuEventHandler(controllerDetectScreenEvent));
            mainMenu = new MainMenu(this, new MainMenu.MenuEventHandler(mainMenuScreenEvent));
            optionsMenu = new OptionsMenu(this, new OptionsMenu.MenuEventHandler(optionsMenuScreenEvent));
            currentMenu = null;
            #endregion

            #region Setup Controls
            menuControl = new MenuControl(this);
            #endregion
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            

            optionsHaveChanged();
        
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region Game control setup
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #endregion

            if (currentLevel == intro)
            {
                Components.Remove(healthMeter);
                Components.Remove(scoreMeter);
            }

            #region Menus updates
            if (currentMenu != controllerSelect)
            {
                menuControl.checkEscOpenMenu();
            }
            if (currentMenu != null)
            {
                currentMenu.Update(gameTime);
                if (currentMenu != controllerSelect)
                {
                    menuControl.checkMouseClick();

                    if (currentMenu != null) // checked if menu is still displayed (may have been closed by prev mouse click action) 
                    {
                        menuControl.checkMouseHeld();
                    }
                }
            }
            #endregion

            #region Game Updates
            else
            {
                currentLevel.Update(gameTime);
                base.Update(gameTime);
            }
            
            #endregion

            musicEngine.Update();
            soundEngine.Update();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);

            if (currentMenu != null)
            {
                currentMenu.Draw(spriteBatch);
            }

            
        }


        private void controllerDetectScreenEvent(object obj, ControllerSelectMenu.MenuEventArgs e)
        {
            Options.OptionsData currentSettings = options.getOptionsData();
            currentSettings.gamePad = e.gamepad;
            currentSettings.controller = e.controllerNo;
            options.setOptionsData(currentSettings);
            currentMenu = null; //closes the controller select menu
        }

        private void mainMenuScreenEvent(object obj, MainMenu.MenuEventArgs e)
        {
            switch (e.nextMenu)
            {
                case MainMenu.MenuEventArgs.NextMenu.exit:
                    this.Exit();
                    break;
                case MainMenu.MenuEventArgs.NextMenu.none:
                    currentMenu = null;
                    break;
                case MainMenu.MenuEventArgs.NextMenu.options:
                    currentMenu = optionsMenu;
                    break;
                case MainMenu.MenuEventArgs.NextMenu.record:
                    musicEngine.skipTrack();
                    break;
            }
        }

        private void optionsMenuScreenEvent(object obj, OptionsMenu.MenuEventArgs e)
        {
            switch (e.theEvent)
            {
                case OptionsMenu.MenuEventArgs.NextMenu.save:
                    currentMenu = mainMenu;
                    optionsHaveChanged();
                    break;
                case OptionsMenu.MenuEventArgs.NextMenu.back:
                    currentMenu = mainMenu;
                    break;
            }
        }

        /// <summary>
        /// Displays or hides the main menu
        /// </summary>
        /// <param name="display">Whether to display or hide the main menu</param>
        public void displayMainMenu(Boolean display)
        {
            if (display)
            {
                currentMenu = mainMenu;
            }
            else
            {
                currentMenu = null;
            }
        }

        /// <summary>
        /// Displays the options menu.  To hide, simply call to display the main menu
        /// </summary>
        public void displayOptionsMenu()
        {
            currentMenu = optionsMenu;
        }

        /// <summary>
        /// Gets the current menu being displayed.
        /// </summary>
        /// <returns>the current Menu object</returns>
        public Menu getCurrentMenu()
        {
            return currentMenu;
        }

        public void displayControllerSelectMenu(bool display)
        {
            if (display)
            {
                currentMenu = controllerSelect;
            }
        }

        //gets the options object for other modules to extract settings from this.
        public Options getOptions()
        {
            return options;
        }

        public MusicPlayer getMusicPlayer()
        {
            return musicEngine;
        }

        public SoundPlayer getSoundPlayer()
        {
            return soundEngine;
        }

        public HealthMeter getHealthMeter()
        {
            return healthMeter;
        }

        public Score getScoreMeter()
        {
            return scoreMeter;
        }

        public void optionsHaveChanged()
        {
            if ((options.getOptionsData().fullscreen && !graphics.IsFullScreen) || (!options.getOptionsData().fullscreen && graphics.IsFullScreen))
            {
                graphics.ToggleFullScreen();
            }

            currentLevel.optionsHaveChanged();
            musicEngine.initialize();
            soundEngine.initialize();

            
        }

        /// <summary>
        /// Loads and starts the next level held in the levels Queue after disposing of the assets associated with the current level.
        /// </summary>
        public void nextLevel()
        {
            currentLevel.UnloadContent();
            Components.Remove(currentLevel);
            currentLevel = levels.Dequeue();
            Components.Add(currentLevel);
            Components.Add(healthMeter);
            Components.Add(scoreMeter);
            base.Initialize();
        }


        /// <summary>
        /// Displays a static loading screen after disposing of the content of the current level. Once the loading screen has loaded it will begin loaidng the next nevel in the queue.
        /// </summary>
        public void displayLoadingScreen()
        {
            Components.Remove(healthMeter);
            Components.Remove(scoreMeter);
            currentLevel.UnloadContent();
            Components.Remove(currentLevel);
            ILevel loadingScreen = new LoadingScreen(this);
            currentLevel = loadingScreen;
            Components.Add(currentLevel);
        }

        public void changeCamera()
        {
            currentLevel.changeCamera();
            foreach(ILevel level in levels){
                level.changeCamera();
            }
        }

        
    }
}
