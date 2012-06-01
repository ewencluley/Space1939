using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Space1939.Cameras;
using Space1939.ParticalSystem;
using Space1939.HUD;

namespace Space1939
{
    public class Player:DrawableGameComponent, ICollidable
    {
        Game1 game;

        Model theModel;

        bool alive = true;
        bool explosionHappened = false;

        public Vector3 player_position {get; private set; }
        public float MAX_SPEED {get; private set;}
        float speed = 0.3f;
        protected Quaternion direction = Quaternion.Identity;
        float scale;

        ParticleSystem funnelSmokePlume;
        ExplosionParticleSystem explosion;
        ExplosionSmokeParticleSystem explosionSmoke;

        SoundPlayer playerSfx;
        Cue trainSoundCue;

        Camera camera;
        Random random = new Random();
        
        public Player(Game1 game, Vector3 p, float s)
            :base(game)
        {
            this.game = game;
            MAX_SPEED = 10;
            theModel = game.Content.Load<Model>("Models\\Player\\ship");
            player_position = p;
            scale = s;

            funnelSmokePlume = new SmokePlumeParticleSystem(game, game.Content);
            funnelSmokePlume.DrawOrder=4;
            game.Components.Add(funnelSmokePlume);

            explosion = new ExplosionParticleSystem(game, game.Content);
            explosion.DrawOrder = 4;

            explosionSmoke = new ExplosionSmokeParticleSystem(game, game.Content);
            explosionSmoke.DrawOrder = 4;

            playerSfx = game.getSoundPlayer();
            trainSoundCue =  playerSfx.getCue("trainsSound");
            playerSfx.playCue(trainSoundCue);
            trainSoundCue.SetVariable("TrainSpeed", 0);


        }

        public override void Update(GameTime gameTime)
        {
            Vector3 funnelPosition = Vector3.Transform(new Vector3(0, 15, 12), Matrix.CreateFromQuaternion(direction));
            funnelPosition += player_position; //finds the position of the main funnel, where the smoke should come from.
            funnelSmokePlume.AddParticle(funnelPosition, Vector3.Zero);

            if (!alive && !explosionHappened)//if the player is not alive and the explosion has not already happened
            {
                game.getHealthMeter().setValue(0);
                UpdateExplosions(gameTime);
                explosionHappened = true; //only do the explosion once!
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Helper for updating the explosions effect.
        /// </summary>
        void UpdateExplosions(GameTime gameTime)
        {
            int numExplosionParticles =50, numExplosionSmokeParticles=30;
            for (int i = 0; i < numExplosionParticles; i++){
                    explosion.AddParticle(player_position, new Vector3(0,0,0));
            }
            for (int i = 0; i < numExplosionSmokeParticles; i++){
                   explosionSmoke.AddParticle(player_position, new Vector3(0,0,0));
            }
        }


        /// <summary>
        /// Sets the camera that the player object is being viewed by.
        /// </summary>
        /// <param name="c">Camera used to view the player</param>
        public void setCamera(Camera c)
        {
            camera = c;
        }

        protected override void UnloadContent()
        {
            trainSoundCue.Dispose();
            funnelSmokePlume.Dispose();
            explosion.Dispose();
            explosionSmoke.Dispose();

            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            UnloadContent();
            base.Dispose(disposing);
        }

        //DRAW METHOD FOR THE PLAYER OBJECT
        //Draws the model
        public override void Draw(GameTime gameTime)
        {
            if (alive || (!alive && !explosionHappened))
            {
                Matrix[] transforms = new Matrix[theModel.Bones.Count];
                theModel.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in theModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        // effect.EnableDefaultLighting(); //enables default lighting on the effect for the mesh besing considered
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale, scale, scale) * Matrix.CreateFromQuaternion(direction) * Matrix.CreateTranslation(player_position);
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                    }
                    mesh.Draw();
                }
                funnelSmokePlume.SetCamera(camera.view, camera.projection);
            }
            else
            {
                explosion.SetCamera(camera.view, camera.projection);
                explosionSmoke.SetCamera(camera.view, camera.projection);
            }
            base.Draw(gameTime);

        }

        public Vector3 getPosition()
        {
            return player_position;
        }

        public bool isAlive()
        {
            return alive;
        }

        public Quaternion getDirection()
        {
            return direction;
        }

        public Model getModel()
        {
            return theModel;
        }

        public float getScale()
        {
            return scale;
        }

        public Vector3 getVelocity()
        {
            return player_position;
        }

        public void changeDirection(float yawIn, float pitchIn, float rollIn)
        {
            if (!alive)
            {
                yawIn = 0; pitchIn = 0; rollIn = 0;
            }
            float yaw = (yawIn *speed) * MathHelper.ToRadians( -0.25f );

            float pitch = (pitchIn *speed) * MathHelper.ToRadians( -0.25f );

            float roll = (rollIn * speed) * MathHelper.ToRadians(-0.25f);

            Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.Right, pitch) * Quaternion.CreateFromAxisAngle(Vector3.Up, yaw) * Quaternion.CreateFromAxisAngle(Vector3.Backward, roll);

            direction *= rot;
        }


        public void accelerate(double acceleratingFor)
        {
            if(!alive)
                acceleratingFor =0;
            Vector3 changePos = Vector3.Transform(new Vector3(0, 0, 1), direction);
            player_position += changePos * (float)acceleratingFor * speed;
            
            //chnage pitch of train sound with acceleration.
            float trainSoundPitch = (float)acceleratingFor * 10;
            MathHelper.Clamp(trainSoundPitch, 0, 100);
            trainSoundCue.SetVariable("TrainSpeed", trainSoundPitch);

        }

        public void collision(ICollidable otherObject)
        {
            Record record = otherObject as Record;
            Planet planet = otherObject as Planet;
            if (record != null)
            {
                game.getScoreMeter().incrementScore();
            }
            else if (planet != null)
            {

            }
            else
            {
                if (game.getHealthMeter().getValue() <= 1)
                {
                    destroy();
                }
                else
                {
                    game.getHealthMeter().decrementValue(10);
                }
            }
        }

        public void destroy()
        {
            if (alive)
            {
                game.getHealthMeter().setValue(0);

                trainSoundCue.Stop(AudioStopOptions.Immediate);
                playerSfx.playSound("explosion-01");
                alive = false;

                if (game.Components.Contains(funnelSmokePlume))
                {
                    game.Components.Remove(funnelSmokePlume);
                }
                if (!game.Components.Contains(explosion))
                {
                    game.Components.Add(explosion);
                    game.Components.Add(explosionSmoke);
                }
            }
        }
    }
}
