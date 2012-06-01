using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space1939.Menus
{
    class MenuButton
    {

        protected Texture2D texture;
        protected Vector2 position;

        public MenuButton(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }


        public virtual Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
