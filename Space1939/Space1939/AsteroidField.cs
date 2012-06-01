using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;

namespace Space1939
{
    class AsteroidField:DrawableGameComponent
    {
        Game1 game;
        List<Asteroid> asteroids;
        List<Record> records;
        BoundingBox asteroidField;
        Camera currentCamera;

        Random random = new Random();

        public AsteroidField(Game1 game, Camera c)
            : base(game)
        {
            this.game = game;
            currentCamera = c;
            asteroidField = new BoundingBox(new Vector3(-1200, -300, 400), new Vector3(1200, 300, 1000));
            generateAsteroidField();
        }

        private void generateAsteroidField()
        {
            const int MAX_ASTEROIDS = 400;
            const int MAX_RECORDS = 7;
            asteroids = new List<Asteroid>();
            for (int i = 0; i < MAX_ASTEROIDS; i++)
            {
                Vector3 pos = new Vector3(random.Next((int)asteroidField.Min.X, (int)asteroidField.Max.X), random.Next((int)asteroidField.Min.Y, (int)asteroidField.Max.Y), random.Next((int)asteroidField.Min.Z, (int)asteroidField.Max.Z));
                Vector3 direction = new Vector3(random.Next(), random.Next(), random.Next());
                Vector3 velocity = new Vector3(random.Next(-20,20), random.Next(-20,20), random.Next(-20,20));
                Asteroid newRoid = new Asteroid(game, pos, random.Next(10,60), direction, velocity);
                asteroids.Add(newRoid);
                newRoid.setCamera(currentCamera);
                newRoid.DrawOrder = 3;
                game.Components.Add(newRoid);
            }
            records = new List<Record>();
            for (int i = 0; i < MAX_RECORDS; i++)
            {
                Vector3 pos = new Vector3(random.Next((int)asteroidField.Min.X, (int)asteroidField.Max.X), random.Next((int)asteroidField.Min.Y, (int)asteroidField.Max.Y), random.Next((int)asteroidField.Min.Z, (int)asteroidField.Max.Z));
                Record record = new Record(game, pos);
                records.Add(record);
                record.setCamera(currentCamera);
                record.DrawOrder = 3;
                game.Components.Add(record);

            }
        }

        protected override void UnloadContent()
        {
            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.Dispose();
                Game.Components.Remove(asteroid);
            }
            foreach (Record record in records)
            {
                record.Dispose();
                Game.Components.Remove(record);
            }
            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            UnloadContent();
            base.Dispose(disposing);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Asteroid roid in asteroids)
            {
                if (asteroidField.Contains(roid.getPosition()) == ContainmentType.Disjoint)
                {

                    roid.setVelocity(Vector3.Negate(roid.getVelocity()));
                }
            }
            base.Update(gameTime);
        }

        public void setCamera(Camera c)
        {
            currentCamera = c;
        }

    }
}
