using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Space1939.Levels;

namespace Space1939
{
    class Collision:GameComponent
    {
        Game1 game;

        public Collision(Game1 game)
            : base(game)
        {
            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
            objectCollision();
            base.Update(gameTime);
        }

        private void objectCollision()
        {
            Player player = game.currentLevel.getPlayer();
            IGameComponent[] copy = new IGameComponent[game.Components.Count]; //component list is coppied to avoid concurrent modification exceptions when a collission is detected. i.e. adding explosion effect etc.
            game.Components.CopyTo(copy, 0);

            foreach (GameComponent component in copy)
            {
                ICollidable component3D = component as ICollidable; //tests if component implements ICollidable and thus is collidable
                if (component3D != null)
                {
                    Player componentPlayer = component3D as Player; //makes sure the component being looked at is not the player
                    if (componentPlayer == null)
                    {
                        foreach (ModelMesh mesh in player.getModel().Meshes)
                        {
                            foreach (ModelMesh currentMesh in component3D.getModel().Meshes)
                            {
                                BoundingSphere otherBoundingSphere = currentMesh.BoundingSphere; // set the bounding sphere to being the mesh's bounding sphere.
                                Planet planet = component3D as Planet; // test if the component being checked is a planet. 
                                if (planet != null)
                                {
                                    otherBoundingSphere.Radius += 1; //if it is, its bounding sphere needs to be above the surface of the planet.
                                }
                                otherBoundingSphere = otherBoundingSphere.Transform(currentMesh.ParentBone.Transform * Matrix.CreateScale(component3D.getScale(), component3D.getScale(), component3D.getScale()) * Matrix.CreateTranslation(component3D.getPosition()));

                                BoundingSphere playerBoundingSphere = mesh.BoundingSphere; // find the players bounding sphere for each mesh.
                                playerBoundingSphere = playerBoundingSphere.Transform(mesh.ParentBone.Transform * Matrix.CreateScale(player.getScale(), player.getScale(), player.getScale()) * Matrix.CreateTranslation(player.getPosition()));

                                if (playerBoundingSphere.Intersects(otherBoundingSphere) && component3D.isAlive())
                                {
                                    component3D.collision(player);
                                    player.collision(component3D);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
