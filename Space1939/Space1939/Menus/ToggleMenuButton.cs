using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space1939.Menus
{
    class ToggleMenuButton:MenuButton
    {
        Texture2D textureTrue;
        Texture2D textureFalse;
        bool toggled = true;

        public ToggleMenuButton(Texture2D texture, Texture2D texture2, Vector2 position)
            : base(texture, position)
        {
            textureTrue = texture;
            textureFalse = texture2;
        }

        public void toggle()
        {
            toggled = !toggled;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (toggled)
            {
                spriteBatch.Draw(textureTrue, base.position, Color.White);
            }
            else
            {
                spriteBatch.Draw(textureFalse, base.position, Color.White);
            }
            spriteBatch.End();
        }

        public bool getValue()
        {
            return toggled;
        }

        public void setValue(bool tog)
        {
            toggled = tog;
        }
    }
}
