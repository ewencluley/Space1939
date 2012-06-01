using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space1939
{
    public interface ICollidable
    {
        Model getModel();
        Vector3 getPosition();
        float getScale();
        Vector3 getVelocity();

        bool isAlive();

        void collision(ICollidable component);

        void setCamera(Cameras.Camera camera);
    }
}
