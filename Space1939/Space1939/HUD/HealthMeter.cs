using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space1939.HUD
{
    public class HealthMeter:DrawableGameComponent
    {
        Game1 game;
        Texture2D background;
        Texture2D needle;
        Texture2D foreground;

        Vector2 position;
        Vector2 needle_origin, needle_position;
        float needle_rotation = -0.1f;

        int health = 100;

        public HealthMeter(Game1 game)
            :base(game)
        {
            this.game = game;
            background = game.Content.Load<Texture2D>("HUD\\Health\\background");
            foreground = game.Content.Load<Texture2D>("HUD\\Health\\foreground");
            needle = game.Content.Load<Texture2D>("HUD\\Health\\needle");

            needle_position = new Vector2(80, 210);
            needle_origin = new Vector2(4, 130);
            position = new Vector2(0, 0);
        }


        public override void Update(GameTime gameTime)
        {
            needle_rotation = health / 100f;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(game.GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(background, position, Color.White);
            spriteBatch.Draw(needle, needle_position, null, Color.White, needle_rotation, needle_origin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(foreground, position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void setValue(int health)
        {
            this.health = health;
        }

        public void decrementValue(int health)
        {
            this.health -= health;
        }

        public int getValue()
        {
            return health;
        }
    }
}
