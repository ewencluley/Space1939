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
    class TerrainCollision : GameComponent
    {
        Game1 game;
        Terrain terrain;

        public TerrainCollision(Game1 game, Terrain terrain)
            : base(game)
        {
            this.game = game;
            this.terrain = terrain;
        }

        public override void Update(GameTime gameTime)
        {
            terrainCollision();
            base.Update(gameTime);
        }

        private void terrainCollision()
        {
            
            Player player = game.currentLevel.getPlayer();

            float heightPoint = terrain.getTerrainHeight(player.getPosition());
            Console.WriteLine(heightPoint);
            Vector3 collisionPoint = new Vector3(player.getPosition().X, heightPoint, player.getPosition().Z);

            foreach (ModelMesh mesh in player.getModel().Meshes)
            {
                BoundingSphere playerBoundingSphere = mesh.BoundingSphere; // find the players bounding sphere for each mesh.
                playerBoundingSphere = playerBoundingSphere.Transform(mesh.ParentBone.Transform * Matrix.CreateScale(player.getScale(), player.getScale(), player.getScale()) * Matrix.CreateTranslation(player.getPosition()));

                BoundingSphere otherBoundingSphere = new BoundingSphere(collisionPoint, 10f);

                if ((player.getPosition().Y < heightPoint) || playerBoundingSphere.Intersects(otherBoundingSphere))
                {
                    player.destroy();
                }
            }
        }
    }
}
