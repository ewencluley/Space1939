using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space1939.Cameras
{
    class FirstPersonCam:Camera
    {
        public FirstPersonCam(Game1 game, Vector3 pos, Vector3 up, float aspectRatio)
            : base(game, pos, pos + new Vector3(0,0,10), up, aspectRatio)
        {

        }
    }
}
