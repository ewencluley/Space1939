using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Space1939.Cameras
{
    public class _3rdPersonCam : Camera, IZoomableCamera
    {
        private Player target;
        //private MouseState lastMouse;
        private float zoom;
        public const float INITIAL_ZOOM = -50f;
        public const float MAX_ZOOM = -5f;
        public const float MIN_ZOOM = -80f;

        public _3rdPersonCam(Game1 game, Vector3 pos, Player tar, Vector3 up, float aspectRatio)
            : base(game, pos, tar.getPosition(), up, aspectRatio)
        {
            target = tar;
            zoom = INITIAL_ZOOM;
        }

        public override void Update(GameTime gameTime)
        {
            
            Vector3 desiredPosition;
            Vector3 offset = new Vector3(0, 50, zoom);
                
            if(Keyboard.GetState().IsKeyDown(Keys.LeftControl)) {
             
                offset = new Vector3(0, 40, 100);
            }

            desiredPosition = Vector3.Transform(offset, Matrix.CreateFromQuaternion(target.getDirection()));
            desiredPosition += target.getPosition();

            float smoothStepSpeed = Vector3.Distance(base.position, desiredPosition)/200;


            Vector3 position = Vector3.SmoothStep(base.position,desiredPosition,smoothStepSpeed);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                position = desiredPosition;
            }

            Vector3 up = Vector3.Up;
            up = Vector3.Transform(up, Matrix.CreateFromQuaternion(target.getDirection()));

            view = Matrix.CreateLookAt(position, target.getPosition(), up);
            base.position = position;

            base.Update(gameTime);
        }
        /// <summary>
        /// zoom camera towards/away from the ship
        /// </summary>
        /// <param name="zoomBy">Amount to zoom in or out by</param>
        public void zoomCam(float zoomBy)
        {
            
            zoom += zoomBy;
        }

        public float getZoom()
        {
            return zoom;
        }


        public float getZoomBounds(int x)
        {
            if (x == 0)
            {
                return MAX_ZOOM;
            }
            else
            {
                return MIN_ZOOM;
            }
        }
    }
}
