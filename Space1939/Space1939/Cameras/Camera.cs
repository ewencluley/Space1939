using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Space1939.Cameras
{
    public class Camera
    {
        public Matrix view { get; set; }
        public Matrix projection { get; protected set; }
        public Vector3 position { get; protected set; }
        private Vector3 target;
        protected Vector3 up;
        private MouseState lastMouse;
        private float aspectRatio;

        private Game1 game;

        public Camera(Game1 game, Vector3 pos, Vector3 tar, Vector3 up, float aspectRatio)
        {
            this.game = game;
            this.aspectRatio = aspectRatio;

            view = Matrix.CreateLookAt(pos, tar, up);
            initialize();
            
            position = pos;
            target = tar;
            this.aspectRatio = aspectRatio;
            this.up = up;
            lastMouse = Mouse.GetState();
        }

        public void initialize()
        {
            int farClipping = (3000 * game.getOptions().getOptionsData().clippingDistance) +2;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, farClipping);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
