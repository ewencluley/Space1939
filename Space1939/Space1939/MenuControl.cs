using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Menus
{
    class MenuControl
    {
        Game1 game;
        Boolean escPressed = false;
        MouseState currentState, previousState;

        public MenuControl(Game1 g)
        {
            game = g;
        }

        public void checkEscOpenMenu()
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (!escPressed)
            {
                if (keyboard.IsKeyDown(Keys.Escape) && game.getCurrentMenu() is MainMenu)
                {
                    game.displayMainMenu(false);
                    escPressed = true;
                }
                else if(keyboard.IsKeyDown(Keys.Escape))
                {
                    game.displayMainMenu(true);
                    escPressed = true;
                }
            }
            if (keyboard.IsKeyUp(Keys.Escape))
            {
                escPressed = false;
            }
        }

        public void checkMouseClick()
        {
            currentState = Mouse.GetState();
            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                Vector2 clickPos = new Vector2(currentState.X, currentState.Y);
                game.getCurrentMenu().clickOption(clickPos);
            }
            previousState = currentState;
            
        }

        public void checkMouseHeld()
        {
            currentState = Mouse.GetState();
            if (currentState.LeftButton == ButtonState.Pressed)
            {
                Vector2 heldPos = new Vector2(currentState.X, currentState.Y);
                game.getCurrentMenu().dragOption(heldPos);
            }
            previousState = currentState;
        }
    }
}
