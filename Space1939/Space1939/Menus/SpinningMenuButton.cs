using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space1939.Menus
{
    class SpinningMenuButton : MenuButton
    {
        float rotation;
        Vector2 origin;

        public SpinningMenuButton(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            rotation=0;
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override Rectangle getBound()
        {
            Rectangle bounds = base.getBound();
            bounds.Offset((int)-origin.X, (int)-origin.Y);
            return bounds;
        }
    }
}
