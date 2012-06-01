using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Levels
{
    public class LoadingScreen : DrawableGameComponent, ILevel
    {
        Game1 game;
        Texture2D loadingTex;
        Vector2 position;
        bool drawn = false;

        public LoadingScreen(Game1 game)
            :base(game)
        {
            this.game = game;
            
        }

        public override void Initialize()
        {
            LoadContent();
            //Draw(new GameTime());
        }

        protected override void LoadContent()
        {
            loadingTex = game.Content.Load<Texture2D>("Menus\\loading");
            position = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - (loadingTex.Width / 2), (game.GraphicsDevice.Viewport.Height / 2) - (loadingTex.Height / 2));
            base.LoadContent();
        }

        public void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (drawn)
            {
                if (game.currentLevel is LoadingScreen)
                {
                    game.nextLevel();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(game.GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(loadingTex, position, Color.White);
            spriteBatch.End();
            drawn = true;
        }

        public void optionsHaveChanged()
        {

        }

        public MusicPlayer getMusicPlayer()
        {
            return null;
        }

        public SoundPlayer getSoundPlayer()
        {
            return null;
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

