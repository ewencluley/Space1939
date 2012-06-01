using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Menus
{
    class SliderMenuButton:MenuButton
    {
        Texture2D sliderButton;
        Vector2 sliderButtonPosition;

        public SliderMenuButton(Texture2D texture, Texture2D sliderTexture, Vector2 position)
            : base(texture, position)
        {
            sliderButton = sliderTexture;
            sliderButtonPosition=position;
            sliderButtonPosition.Y += 1;
        }

        public void slide()
        {
            if (Mouse.GetState().X <= (base.position.X + base.texture.Width - sliderButton.Width / 2) && Mouse.GetState().X >= (base.position.X + sliderButton.Width / 2))
            {
                sliderButtonPosition.X = Mouse.GetState().X - sliderButton.Width / 2;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(sliderButton, sliderButtonPosition, Color.White);
            spriteBatch.End();
        }

        public int getValue()
        {
            return (int)(((sliderButtonPosition.X - base.position.X)/(base.texture.Width-sliderButton.Width))*100);
        }

        public void setValue(int value)
        {
            float val = (float)value / 100;
            sliderButtonPosition.X = base.position.X + (float)((base.texture.Width - sliderButton.Width) * val);
        }
    }
}
