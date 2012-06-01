using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Levels
{
    class Intro:GameComponent, ILevel
    {
        Game1 game;
        ScreenBackdrop titleScreen;

        public Intro(Game1 game)
            :base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            titleScreen = new ScreenBackdrop(game);
            titleScreen.DrawOrder = 100;
            game.Components.Add(titleScreen);
            #region Audio

            game.getMusicPlayer().playTrack(99);

            #endregion
        }

        public void UnloadContent()
        {
            game.Components.Remove(titleScreen);
            titleScreen.Dispose();
            game.getMusicPlayer().stop();
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                game.displayLoadingScreen();
            }
        }

        /// <summary>
        /// Re initializes components of the level when the options have changed.
        /// </summary>
        public void optionsHaveChanged()
        {
        }

        public void changeCamera()
        {
            //blank as the intro screen doesnt implement cameras
        }

        public Player getPlayer()
        {
            return null;
        }
    }
}
