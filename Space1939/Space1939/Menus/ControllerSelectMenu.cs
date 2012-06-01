using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Menus
{
    class ControllerSelectMenu:Menu
    {
        Game1 game;
        public event MenuEventHandler eventHandler;

        public ControllerSelectMenu(Game1 g, MenuEventHandler eventHandler)
            : base(g, "Menus\\ControlMenu\\controllerSelect")
        {
            this.eventHandler = eventHandler;
            game = g;
        }

        public override void Update(GameTime gameTime)
        {
            for (int aPlayer = 0; aPlayer < 4; aPlayer++)
            {

                if (GamePad.GetState((PlayerIndex)aPlayer).Buttons.A == ButtonState.Pressed)
                {
                    eventHandler.Invoke(this, new MenuEventArgs(true, (PlayerIndex)aPlayer));
                   // game.getSoundPlayer().playSound("click");
                    return;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A) == true)
                {
                    eventHandler.Invoke(this, new MenuEventArgs(false, (PlayerIndex)1));
                   // game.getSoundPlayer().playSound("click");
                    return;
                }

            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// Inner class defining the event args for this menu
        /// </summary>
        public class MenuEventArgs : EventArgs
        {
            public bool gamepad { get; protected set; }
            public PlayerIndex controllerNo { get; protected set; }

            public MenuEventArgs(bool gamepad, PlayerIndex controllerNo)
                :base()
            {
                this.gamepad = gamepad;
                this.controllerNo = controllerNo;
            }
        }

        public delegate void MenuEventHandler(object sender, MenuEventArgs e);
    }
}
