using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space1939.HUD
{
    public class Score : DrawableGameComponent
    {
        Game1 game;
        Texture2D background;
        Vector2 position;
        Vector2 fontPosition;

        SpriteFont font;
        String scoreString;

        int score = 0;

        public Score(Game1 game)
            : base(game)
        {
            this.game = game;
            background = game.Content.Load<Texture2D>("HUD\\Score\\score");
            position = new Vector2(game.GraphicsDevice.Viewport.Width - background.Width, game.GraphicsDevice.Viewport.Height - background.Height);
            fontPosition = new Vector2((game.GraphicsDevice.Viewport.Width - background.Width) + (background.Width * 0.726f), (game.GraphicsDevice.Viewport.Height - background.Height) + (background.Height * 0.493f));
            font = game.Content.Load<SpriteFont>("HUD\\Score\\ScoreFont");
            scoreString = "" + score;
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 FontOrigin = font.MeasureString(scoreString) / 2;
            SpriteBatch spriteBatch = new SpriteBatch(game.GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(background, position, Color.White);
            spriteBatch.DrawString(font, scoreString, fontPosition, Color.White, 0f, FontOrigin, 1f, SpriteEffects.None,0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void setValue(int score)
        {
            scoreString = "" + score;
        }

        public void incrementScore()
        {
            score++;
            scoreString = "" + score;
        }
    }
}
