using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;
using Space1939.HUD;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Levels
{
    class Level1 : GameComponent, ILevel
    {
        Game1 game;
        Collision collisonDetector;
        Control input;
        HealthMeter health;
        Score score;

        #region game objects
        //Models used in this level
        Player player;
        Planet moon;
        AsteroidField asteroidField;
        #endregion

        #region cameras
        //Cameras used in this level
        public _3rdPersonCam thirdPersonCam;
        FirstPersonCam firstPersonCam;
        public StaticCam flyByCam;

        public Camera currentCam;
        private Queue<Camera> availableCameras;
        //End of Cameras
        #endregion

        #region world objects
        
        Skybox skybox;

        #endregion

        public Level1(Game1 game)
            :base(game)
        {
            this.game = game;
            game.displayControllerSelectMenu(true);
        }

        public override void Initialize()
        {
            float aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;

            #region Load Models
            //Load Models
            



            Vector3 initPlayerPosition = new Vector3(0, 0, 0);
            player = new Player(game, initPlayerPosition, 5f);
            moon = new Planet(game, new Vector3(0, 0, 9000), 300f, "moon");

            game.Components.Add(player);
            game.Components.Add(moon);

            #endregion

            #region Load Cameras
            //Load Cameras
            thirdPersonCam = new _3rdPersonCam(game, (player.getPosition()), player, Vector3.Up, aspectRatio);
            flyByCam = new StaticCam(game, player, Vector3.Up, aspectRatio);
            firstPersonCam = new FirstPersonCam(game, player.getPosition(), Vector3.Up, aspectRatio);
            availableCameras = new Queue<Camera>();
            availableCameras.Enqueue(thirdPersonCam);
            availableCameras.Enqueue(flyByCam);

            currentCam = availableCameras.Dequeue();
            #endregion

            #region setup game objects cameras and draw order
            asteroidField = new AsteroidField(game, currentCam);
            game.Components.Add(asteroidField);

            player.setCamera(currentCam);
            player.DrawOrder = 3;

            moon.setCamera(currentCam);
            moon.DrawOrder = 3;

            skybox = new Skybox(game, currentCam, "Space");
            skybox.DrawOrder = 1;
            game.Components.Add(skybox);
            #endregion

            #region initialize controls
            input = new Control(game, player, thirdPersonCam, false);
            game.Components.Add(input);
            #endregion



            collisonDetector = new Collision(game);
            game.Components.Add(collisonDetector);

            #region Audio
            //game.getMusicPlayer().playRandomTrack();
            #endregion

            optionsHaveChanged();
            

        }

        public void UnloadContent()
        {
            input.Dispose();
            game.Components.Remove(input);
            collisonDetector.Dispose();
            game.Components.Remove(collisonDetector);

            player.Dispose();
            game.Components.Remove(player);
            moon.Dispose();
            game.Components.Remove(moon);
            skybox.Dispose();
            game.Components.Remove(skybox);
            asteroidField.Dispose();
            game.Components.Remove(asteroidField);
            

        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                game.displayLoadingScreen();
            }

            currentCam.Update(gameTime);

            base.Update(gameTime);

        }

        public void optionsHaveChanged()
        {
            foreach (Camera c in availableCameras)
            {
                c.initialize();
            }
            currentCam.initialize();

            input.Initialize();
        }

        public void changeCamera()
        {
            if (availableCameras != null)
            {
                availableCameras.Enqueue(currentCam);
                currentCam = availableCameras.Dequeue();
                foreach (GameComponent component in game.Components)
                {
                    ICollidable comp = component as ICollidable;
                    if (comp != null)
                    {
                        comp.setCamera(currentCam);
                    }
                }
            }
        }

        public Player getPlayer()
        {
            return player;
        }
    }
}

