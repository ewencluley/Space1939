using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;

namespace Space1939
{
    class Planet:DrawableGameComponent, ICollidable
    {
        Game1 game;
        Model theModel;
        Vector3 position;
        Camera camera;
        float scale;

        public Planet(Game1 game, Vector3 position, float scale, String planetName)
            : base(game)
        {
            this.game = game;
            this.scale = scale;
            this.position = position;
            theModel = game.Content.Load<Model>("Models\\Planets\\" + planetName);
        }

        #region getter/setter methods
        /// <summary>
        /// Gets the model for the asteroid
        /// </summary>
        /// <returns>Model of the asteroid </returns>
        public Model getModel()
        {
            return theModel;
        }

        /// <summary>
        /// Sets the current camera that the object is being viewed through
        /// </summary>
        /// <param name="c">Camera to draw the model using</param>
        public void setCamera(Camera c)
        {
            camera = c;
        }

        /// <summary>
        /// Returns the asteroids position
        /// </summary>
        /// <returns>Vector3 position of the asteroid</returns>
        public Vector3 getPosition()
        {
            return position;
        }

        /// <summary>
        /// Gets the scale of the current object
        /// </summary>
        /// <returns>float the asteroids size</returns>
        public float getScale()
        {
            return scale;
        }

        public Vector3 getVelocity()
        {
            return Vector3.Zero;
        }
        #endregion

        public override void Draw(GameTime gameTime)
        {
            Matrix[] transforms = new Matrix[theModel.Bones.Count];
            theModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in theModel.Meshes)
            {
                
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting(); //enables default lighting on the effect for the mesh besing considered
                    effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale, scale, scale) * Matrix.CreateTranslation(position);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;

                }
                mesh.Draw();
            }
            //base.Draw(gameTime);
        }

        public void collision(ICollidable other)
        {
            game.displayLoadingScreen();
        }

        public bool isAlive()
        {
            return true;
        }
    }
}
