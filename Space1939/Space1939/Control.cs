using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Space1939.Cameras;

namespace Space1939
{
    public class Control:GameComponent
    {
        Game1 game;
        Player player;
        Camera currentCam;
        Options options;
        Boolean gamepad;
        protected float invertY;
        public double acceleratingFor;
        public MouseState lastMouse;
        KeyboardState lastKeyboard;
        GamePadState lastGamePadState;
        PlayerIndex controllerNo;

        public Control(Game1 g, Player p, Camera current, Boolean pad)
            :base(g)
        {
            game = g;
            
            player = p;
            gamepad = pad;
            acceleratingFor = 0; 
            lastMouse = Mouse.GetState();
            lastKeyboard = Keyboard.GetState();
            
            currentCam = current;

            Initialize();
        }

        public override void Initialize()
        {
            options = game.getOptions();
            if (options.getOptionsData().invertY)
            {
                invertY = -1;
            }
            else
            {
                invertY = 1;
            }
            controllerNo = options.getOptionsData().controller;
            lastGamePadState = GamePad.GetState(controllerNo);
        }

        public override void Update(GameTime gameTime)
        {
            checkControls(gameTime);
            base.Update(gameTime);
        }

        public void changeCamera(Camera c)
        {
            currentCam = c;
        }

        public void setControlMethod(Boolean gamepad)
        {
            this.gamepad = gamepad;
        }

        public void checkControls(GameTime gameTime)
        {
            if (gamepad)
            {
                checkGamePad(gameTime);
            }
            else
            {
                checkKeyboard(gameTime);
            }
        }


        private void checkGamePad(GameTime gameTime)
        {
            GamePadThumbSticks stickState = GamePad.GetState(controllerNo).ThumbSticks;
            float yaw = 0, pitch = 0, roll = 0;

            #region up/down
            pitch = (float)gameTime.ElapsedGameTime.TotalMilliseconds * (stickState.Left.Y * invertY);
            #endregion
            
            #region turn
            yaw = (float)gameTime.ElapsedGameTime.TotalMilliseconds * stickState.Left.X;
            roll = (float)gameTime.ElapsedGameTime.TotalMilliseconds * stickState.Right.X;
            #endregion

            #region accelerate/decelerate
            GamePadTriggers triggers = GamePad.GetState(controllerNo).Triggers;
            if (triggers.Left > 0 && acceleratingFor < player.MAX_SPEED)//accelerate is trigger is pressed and spaceship < max speed.
            {
                acceleratingFor += gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }
            else if (acceleratingFor > 0) //decelerate if trigger no longer pressed
            {
                acceleratingFor -= gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }
            #endregion


        }

        private void checkKeyboard(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float yaw=0, pitch=0, roll = 0;

            #region up/down
            if (keyboard.IsKeyDown(Keys.W))
            {
                pitch -= (float) gameTime.ElapsedGameTime.TotalMilliseconds * invertY;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                pitch += (float)gameTime.ElapsedGameTime.TotalMilliseconds * invertY;
            }
            #endregion

            #region turn
            if(keyboard.IsKeyDown(Keys.A)){
                yaw -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            if(keyboard.IsKeyDown(Keys.D)){
                yaw += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (keyboard.IsKeyDown(Keys.Q))
            {
                roll += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (keyboard.IsKeyDown(Keys.E))
            {
                roll -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            #endregion

            player.changeDirection(yaw, pitch, roll);

            #region accelerate/decelerate
            if (keyboard.IsKeyDown(Keys.Space) && acceleratingFor<player.MAX_SPEED){//accelerate is trigger is pressed and spaceship < max speed.
                acceleratingFor += gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }else if(acceleratingFor>0) //decelerate if W no longer pressed
            {
                acceleratingFor -= gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }

            if (keyboard.IsKeyDown(Keys.C) && acceleratingFor > -player.MAX_SPEED)
            {//accelerate is trigger is pressed and spaceship < max speed.
                acceleratingFor -= gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }
            else if (acceleratingFor < 0) //decelerate if W no longer pressed
            {
                acceleratingFor += gameTime.ElapsedGameTime.TotalSeconds;
                player.accelerate(acceleratingFor);
            }
            #endregion

            #region camera change

            if (keyboard.IsKeyDown(Keys.C) && lastKeyboard.IsKeyUp(Keys.C))
            {
                game.changeCamera();
            }

            #endregion
            lastKeyboard = keyboard;

            checkMouse();
        }

        public void checkMouse()
        {
            // If mouse buton is pressed, zoom camera along the Z axis
            if (!lastMouse.Equals(Mouse.GetState()))
            {
                float zoomBy = (Mouse.GetState().ScrollWheelValue - lastMouse.ScrollWheelValue) / 50;

                IZoomableCamera zoomCam = currentCam as IZoomableCamera; //tests if the camera implemets zoomable
                if (zoomCam != null)
                    if (zoomBy > 0 && zoomCam.getZoom() + zoomBy < zoomCam.getZoomBounds(0) || (zoomBy < 0 && zoomCam.getZoom() + zoomBy > zoomCam.getZoomBounds(1)))
                    {
                        zoomCam.zoomCam(zoomBy);
                    }
            }
            lastMouse = Mouse.GetState();
        }
    }
}
