using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;
using Space1939.HUD;

namespace Space1939.Levels
{
    class Level2 : GameComponent, ILevel
    {
        Game1 game;
        Collision collisionDetector;
        TerrainCollision terrainCollisionDetector;
        Control input;

        #region game objects
        //Models used in this level
        Player player;
        Record newRecord;
        Terrain terrain;

        List<Record> recordList = new List<Record>();
        //End of models used
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

        public Level2(Game1 game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            float aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;

            #region Load Models
            //Load Models

            Vector3 initPlayerPosition = new Vector3(-500, 550, 500);
            player = new Player(game, initPlayerPosition, 5f);
            terrain = new Terrain(game);

            game.Components.Add(player);
            game.Components.Add(terrain);

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

            player.setCamera(currentCam);
            player.DrawOrder = 3;

            terrain.setCamera(currentCam);
            terrain.DrawOrder = 2;

            skybox = new Skybox(game, currentCam, "Space");
            skybox.DrawOrder = 1;
            game.Components.Add(skybox);
            #endregion

            #region initialize controls
            input = new Control(game, player, thirdPersonCam, false);
            game.Components.Add(input);
            #endregion

            collisionDetector = new Collision(game);
            game.Components.Add(collisionDetector);

            terrainCollisionDetector = new TerrainCollision(game, terrain);
            game.Components.Add(terrainCollisionDetector);

            #region Records
            //places records on the land scape randomly
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = new Vector3(rand.Next(-255*30, 0), 0, rand.Next(0, 255*30 ));
                pos.Y = terrain.getTerrainHeight(pos) + 30;
                Record newRecord = new Record(game, pos);
                recordList.Add(newRecord);
                newRecord.setCamera(currentCam);
                newRecord.DrawOrder = 3;
                game.Components.Add(newRecord);
            }
            #endregion

            optionsHaveChanged();
        }

        public void UnloadContent()
        {
            input.Dispose();
            game.Components.Remove(input);
            player.Dispose();
            game.Components.Remove(player);
            skybox.Dispose();
            game.Components.Remove(skybox);
        }

        public override void Update(GameTime gameTime)
        {

            //input.checkControls(gameTime);
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
                terrain.setCamera(currentCam);
            }
        }

        public Player getPlayer()
        {
            return player;
        }
    }
}

