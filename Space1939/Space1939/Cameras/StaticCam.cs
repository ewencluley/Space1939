using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Space1939.Cameras
{
    public class StaticCam : Camera
    {
        private Player target;
        private float zoom;
        public const float INITIAL_ZOOM = -50f;
        public const float MAX_ZOOM = -5f;
        public const float MIN_ZOOM = -80f;

        public StaticCam(Game1 game, Player tar, Vector3 up, float aspectRatio)
            : base(game, tar.getPosition()+new Vector3(50, 0, 50), tar.getPosition(), up, aspectRatio)
        {
            target = tar;
            zoom = INITIAL_ZOOM;
        }

        public override void Update(GameTime gameTime)
        {
            if(Vector3.Distance(base.position, target.getPosition()) > 500){
                Random rand = new Random();
                Vector3 positionOffset = new Vector3(rand.Next(-100, 100), rand.Next(100), rand.Next(-100, 100));
                base.position = target.getPosition()+positionOffset;
            }
            view = Matrix.CreateLookAt(base.position, target.getPosition(), up);

            base.Update(gameTime);
        }

        public void zoomCam(float zoomBy)
        {
            // If mouse buton is pressed, zoom camera along the Z axis
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
