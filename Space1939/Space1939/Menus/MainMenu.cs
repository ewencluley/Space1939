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
    class MainMenu:Menu
    { 
        
        MenuButton exitButton;
        MenuButton optionsButton;
        MenuButton resumeButton, toneArm;

        SpinningMenuButton recordButton;
        protected event MenuEventHandler eventHandler;

        Game1 game;
        public MainMenu(Game1 g, MenuEventHandler eventH)
            : base(g, "Menus\\MainMenu\\Menu")
        {

            eventHandler = eventH;
            game = g;
            Rectangle backgroundBound = base.getBound();
            Texture2D exitButtonTex = game.Content.Load<Texture2D>("Menus\\MainMenu\\Buttons\\exit_button");
            Texture2D optionsButtonTex = game.Content.Load<Texture2D>("Menus\\MainMenu\\Buttons\\options_button");
            Texture2D resumeButtonTex = game.Content.Load<Texture2D>("Menus\\MainMenu\\Buttons\\resume_button");
            Texture2D recordTex = game.Content.Load<Texture2D>("Menus\\MainMenu\\record");
            Texture2D tonearmTex = game.Content.Load<Texture2D>("Menus\\MainMenu\\toneArm");

            exitButton = new MenuButton(exitButtonTex, new Vector2(backgroundBound.X + (backgroundBound.Width / 2) - (exitButtonTex.Width / 2), backgroundBound.Y + 3 * (backgroundBound.Height / 4) - 3 * (exitButtonTex.Height / 4)));
            optionsButton = new MenuButton(optionsButtonTex, new Vector2(backgroundBound.X + (backgroundBound.Width / 2) - (optionsButtonTex.Width / 2), backgroundBound.Y + 2 * (backgroundBound.Height / 4) - 2 * (optionsButtonTex.Height / 4)));
            resumeButton = new MenuButton(resumeButtonTex, new Vector2(backgroundBound.X + (backgroundBound.Width / 2) - (resumeButtonTex.Width / 2), backgroundBound.Y + 1 * (backgroundBound.Height / 4) - 1 * (resumeButtonTex.Height / 4)));

            recordButton = new SpinningMenuButton(recordTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.8251f), backgroundBound.Y + (backgroundBound.Height * 0.7606f)));
            toneArm = new MenuButton(tonearmTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.8180f), backgroundBound.Y + (backgroundBound.Height * 0.5172f)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);//draws the backround

            //draws each button in the main menu
            exitButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            resumeButton.Draw(spriteBatch);
            recordButton.Draw(spriteBatch);

            toneArm.Draw(spriteBatch);

            base.DrawMouse(spriteBatch);//draws the mouse

        }

        public override void Update(GameTime gameTime)
        {
            recordButton.Update(gameTime);
        }

        public override void clickOption(Vector2 mouseClickPos)
        {

            if (resumeButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
               // game.displayMainMenu(false);
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.none));
            }
            else if (optionsButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                //game.displayOptionsMenu();
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.options));
            }
            else if (exitButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                //game.Exit();
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.exit));
            }
            else if (recordButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.record));
                game.getMusicPlayer().skipTrack();
            }
        }

        /// <summary>
        /// Inner class defining the event args for this menu
        /// </summary>
        public class MenuEventArgs : EventArgs
        {
            public enum NextMenu { none, options, exit, record };
            public NextMenu nextMenu;

            public MenuEventArgs(NextMenu nextMenu)
            {
                this.nextMenu = nextMenu;
            }
        }
        public delegate void MenuEventHandler(object sender, MenuEventArgs e);
    }
}
