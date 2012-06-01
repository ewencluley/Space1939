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
    class OptionsMenu : Menu
    {
        MenuButton changeControlsButton, backButton, saveButton;
        ToggleMenuButton fullscreenButton;
        ToggleMenuButton invertYButton;
        SliderMenuButton clippingSlider, soundSlider, musicSlider;

        Game1 game;

        protected event MenuEventHandler eventHandler;

        public OptionsMenu(Game1 g, MenuEventHandler eventH)
            : base(g, "Menus\\OptionsMenu\\menu_options")
        {
            game = g;

            eventHandler = eventH;

            Rectangle backgroundBound = base.getBound();

            #region textures
            Texture2D toggleOffTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\off");
            Texture2D toggleOnTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\on");
            Texture2D changeControlsButtonTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\changeControllerButton");

            Texture2D sliderTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\slider");
            Texture2D sliderBGTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\sliderBackGround");

            Texture2D saveTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\saveButton");
            Texture2D backTex = game.Content.Load<Texture2D>("Menus\\OptionsMenu\\Buttons\\backButton");
            #endregion

            #region buttons and sliders
            changeControlsButton = new MenuButton(changeControlsButtonTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.1975f)-11, backgroundBound.Y + (backgroundBound.Height * 0.7404f)-9));
            
            fullscreenButton = new ToggleMenuButton(toggleOnTex, toggleOffTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.5430f)-15, backgroundBound.Y + (backgroundBound.Height * 0.5375f)-10));
            invertYButton = new ToggleMenuButton(toggleOnTex, toggleOffTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.5430f) - 15, backgroundBound.Y + (backgroundBound.Height * 0.6592f) - 12));

            backButton = new MenuButton(backTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.7475f) - 0, backgroundBound.Y + (backgroundBound.Height * 0.7505f) - 0));
            saveButton = new MenuButton(saveTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.6348f) - 0, backgroundBound.Y + (backgroundBound.Height * 0.7505f) - 0));

            clippingSlider = new SliderMenuButton(sliderBGTex, sliderTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.5430f), backgroundBound.Y + (backgroundBound.Height * 0.4462f)));
            soundSlider = new SliderMenuButton(sliderBGTex, sliderTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.5430f), backgroundBound.Y + (backgroundBound.Height * 0.3448f)));
            musicSlider = new SliderMenuButton(sliderBGTex, sliderTex, new Vector2(backgroundBound.X + (backgroundBound.Width * 0.5430f), backgroundBound.Y + (backgroundBound.Height * 0.2535f)));
            #endregion

            loadOptions();
        }

        private void loadOptions()
        {
            Options.OptionsData optionsData = game.getOptions().getOptionsData();
            soundSlider.setValue(optionsData.soundVolume);
            musicSlider.setValue(optionsData.musicVolume);
            clippingSlider.setValue(optionsData.clippingDistance);
            fullscreenButton.setValue(optionsData.fullscreen);
            invertYButton.setValue(optionsData.invertY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);//draws the backround
            
            //draws each button in the main menu
            changeControlsButton.Draw(spriteBatch);
            fullscreenButton.Draw(spriteBatch);
            invertYButton.Draw(spriteBatch);

            clippingSlider.Draw(spriteBatch);
            soundSlider.Draw(spriteBatch);
            musicSlider.Draw(spriteBatch);

            saveButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);

            base.DrawMouse(spriteBatch);//draws the mouse

        }

        public override void clickOption(Vector2 mouseClickPos)
        {

            if (invertYButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                invertYButton.toggle();
            }
            else if (fullscreenButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                fullscreenButton.toggle();
            }
            else if (changeControlsButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                game.displayControllerSelectMenu(true);
            }
            else if (saveButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                save();
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.save));
            }
            else if (backButton.getBound().Contains(new Point((int)mouseClickPos.X, (int)mouseClickPos.Y)))
            {
                game.getSoundPlayer().playSound("click");
                eventHandler.Invoke(this, new MenuEventArgs(MenuEventArgs.NextMenu.back));
                loadOptions(); //if the player is canceling the options change, we need to reset the sliders to their original values.
            }
        }

        /// <summary>
        /// Gets the values of all the sliders/ buttons and passes it back to options object and then saves them to file.
        /// </summary>
        private void save()
        {
            Options o = game.getOptions();
            Options.OptionsData optionsData = o.getOptionsData();
            optionsData.invertY = invertYButton.getValue();
            optionsData.musicVolume = musicSlider.getValue();
            optionsData.soundVolume = soundSlider.getValue();
            optionsData.fullscreen = fullscreenButton.getValue();
            optionsData.clippingDistance = clippingSlider.getValue();
            
            o.setOptionsData(optionsData);
            o.saveOptions();
        }

        public override void dragOption(Vector2 mouseHeldPos)
        {
            if (clippingSlider.getBound().Contains(new Point((int)mouseHeldPos.X, (int)mouseHeldPos.Y)))
            {
                clippingSlider.slide();
            }
            else if (musicSlider.getBound().Contains(new Point((int)mouseHeldPos.X, (int)mouseHeldPos.Y)))
            {
                musicSlider.slide();
            }
            else if (soundSlider.getBound().Contains(new Point((int)mouseHeldPos.X, (int)mouseHeldPos.Y)))
            {
                soundSlider.slide();
            }
        }

        /// <summary>
        /// Inner class defining the event args for this menu
        /// </summary>
        public class MenuEventArgs : EventArgs
        {
            public enum NextMenu { back, save };
            public NextMenu theEvent;

            public MenuEventArgs(NextMenu theEvent)
            {
                this.theEvent = theEvent;
            }
        }
        public delegate void MenuEventHandler(object sender, MenuEventArgs e);
    }
}

