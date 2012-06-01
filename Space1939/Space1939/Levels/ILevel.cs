using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space1939.Levels
{
    public interface ILevel:IGameComponent
    {
        Player getPlayer();

        void Update(GameTime gameTime);

        void UnloadContent();

        void optionsHaveChanged();

        void changeCamera();

       // override void Draw(GameTime gameTime);
       
    }
}
