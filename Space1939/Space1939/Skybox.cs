using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Space1939.Cameras;

namespace Space1939
{
    /// <summary>
    /// Adapted from Riemer's XNA series
    /// 
    /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/series2.php
    /// </summary>
    public class Skybox:DrawableGameComponent
    {
        Texture2D[] skyboxTextures;
        Model skyboxModel;
        Game1 game;
        Camera camera;
        /// <summary>
        /// Creates a new skybox
        /// </summary>
        /// <param name="skyboxTexture">the name of the skybox texture to use</param>
        public Skybox(Game1 game, Camera camera, String skyboxName)
            :base(game)
        {
            this.camera = camera;
            this.game = game;
            skyboxModel = LoadModel(game.Content, skyboxName, out skyboxTextures);
        }

        private Model LoadModel(ContentManager Content, String skyboxName, out Texture2D[] textures)
        {
            Effect effect = new BasicEffect(game.GraphicsDevice);
            Model newModel = Content.Load<Model>("Skyboxes/" + skyboxName+"/skybox");
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        /// <summary>
        /// Does the actual drawing of the skybox with our skybox effect.
        /// There is no world matrix, because we're assuming the skybox won't
        /// be moved around.  The size of the skybox can be changed with the size
        /// variable.
        /// </summary>
        /// <param name="view">The view matrix for the effect</param>
        /// <param name="projection">The projection matrix for the effect</param>
        /// <param name="cameraPosition">The position of the camera</param>
        public override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice device = game.GraphicsDevice;
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            device.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(camera.position);

                    currentEffect.World = worldMatrix;
                    currentEffect.View = camera.view;
                    currentEffect.Projection = camera.projection;
                    currentEffect.TextureEnabled=true;
                    currentEffect.Texture = skyboxTextures[i++];
                    
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }
         
    }
}
