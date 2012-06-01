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
    public class Menu
    {
        Texture2D background;

        public Texture2D mouse;
        public Vector2 position;

        public Menu(Game1 g, String backgroundAssetName)
        {
            background = g.Content.Load<Texture2D>(backgroundAssetName);
            mouse = g.Content.Load<Texture2D>("Menus\\arrow");
            position = new Vector2((g.GraphicsDevice.Viewport.Width / 2) - (background.Width / 2), (g.GraphicsDevice.Viewport.Height / 2) - (background.Height / 2));
            
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, background.Width, background.Height);
        }

        public virtual void clickOption(Vector2 mouseClickPos)
        {
        }

        public virtual void dragOption(Vector2 mouseClickPos)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, position, Color.White);
            spriteBatch.End();
        }

        public void DrawMouse(SpriteBatch spriteBatch)//draws the mouse this should be called after other elements are drawn.
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mouse, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            spriteBatch.End();
        }
    }
}
