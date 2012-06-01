using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space1939.Levels
{
    class ScreenBackdrop:DrawableGameComponent
    {
        Game1 game;
        Texture2D titleScreen;

        public ScreenBackdrop(Game1 game)
            : base(game)
        {
            this.game = game;
            titleScreen = game.Content.Load<Texture2D>("IntroAssets\\titleScreen");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(game.GraphicsDevice);
            float scale = ((float)game.GraphicsDevice.Viewport.Width / (float)titleScreen.Width);
            float vertCent = (float)((float)game.GraphicsDevice.Viewport.Height / 2f) - (float)((float)(titleScreen.Height *scale) / 2f);
            spriteBatch.Begin();
            spriteBatch.Draw(titleScreen, new Vector2(0,vertCent), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

        }
    }
}
