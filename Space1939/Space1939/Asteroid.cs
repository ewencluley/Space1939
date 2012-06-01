using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;
using Space1939.ParticalSystem;

namespace Space1939
{
    class Asteroid:DrawableGameComponent,ICollidable
    {
        Game1 game;

        Model theModel;
        Vector3 position;
        float scale;
        Quaternion direction = Quaternion.Identity;
        Vector3 velocity;
        float mass;

        bool alive = true;
        
        Camera camera;

        public Asteroid(Game1 game, Vector3 position, float scale, Vector3 driection, Vector3 velocity)
            :base(game)
        {
            this.game = game;
            theModel = game.Content.Load<Model>("Models\\Asteroid\\LargeAsteroid");
            this.direction = Quaternion.CreateFromYawPitchRoll(direction.X, direction.Y, direction.Z);
            this.velocity = velocity;
            this.position = position;
            this.scale = scale;
            mass = 10 * scale;

        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            position += new Vector3(velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds, velocity.Z * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        public override void Draw(GameTime gameTime)
        {
            if (alive)
            {
                Matrix[] transforms = new Matrix[theModel.Bones.Count];
                theModel.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in theModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting(); //enables default lighting on the effect for the mesh besing considered
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale, scale, scale) * Matrix.CreateFromQuaternion(direction) * Matrix.CreateTranslation(position);
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                    }
                    mesh.Draw();
                }
            }
            base.Draw(gameTime);
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
            return velocity;
        }

        public void setVelocity(Vector3 v)
        {
            velocity = v;
        }
        #endregion

        public void collision(ICollidable other)
        {
            int soundNumber = new Random().Next(1, 3);
            String soundName = "rockImpact";
            soundName += soundNumber;
            game.getSoundPlayer().playSound(soundName);
            Vector3 newVelocity = position - other.getPosition(); // changes the direction of the asteroid
            newVelocity *= 3;
            velocity = newVelocity;
        }

        public bool isAlive()
        {
            return alive;
        }
    }
}
