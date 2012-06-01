using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Space1939.Cameras;

namespace Space1939
{
    public struct VertexPositionNormalColored
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public static int SizeInBytes = 7 * 4;
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
              (
                  new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                  new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                  new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
              );
    }

    /// <summary>
    /// Adapted from Riemer's XNA series
    /// 
    /// http://www.riemers.net/eng/Tutorials/XNA/Csharp/series4.php
    /// </summary>
    class Terrain:DrawableGameComponent
    {
        Game1 game;
        GraphicsDevice device;
        Camera camera;
        Effect effect;

        Vector3 offset;

        int terrainWidth;
        int terrainLength;
        float[,] heightData;

        int sizeMod = 30;

        VertexBuffer terrainVertexBuffer;
        IndexBuffer terrainIndexBuffer;

        VertexPositionNormalColored[] vertices;
        int[] indices;

        Texture2D grassTexture;

       

        public Terrain(Game1 game)
            :base(game)
        {
            this.game = game;
            effect = game.Content.Load<Effect>("TerrainEffect");
            Texture2D heightMap = game.Content.Load<Texture2D>("Levels\\Level2\\heightmap"); 
            LoadHeightData(heightMap);
            LoadContent();
            offset = new Vector3(0, 0, 0);
        }

        protected override void LoadContent()
        {
            device = game.GraphicsDevice;

            effect = game.Content.Load<Effect>("TerrainEffect");

            LoadVertices();

            LoadTextures();
        }

        private void LoadVertices()
        {

            //LoadHeightData(heightMap);
            VertexPositionNormalTexture[] terrainVertices = SetUpTerrainVertices();
            int[] terrainIndices = SetUpTerrainIndices();
            terrainVertices = CalculateNormals(terrainVertices, terrainIndices);
            CopyToTerrainBuffers(terrainVertices, terrainIndices);
        }

        private void LoadTextures()
        {

            grassTexture = game.Content.Load<Texture2D>("Levels\\Level2\\moonTexture");
        }


        private void LoadHeightData(Texture2D heightMap)
        {
            float minimumHeight = float.MaxValue;
            float maximumHeight = float.MinValue;

            terrainWidth = heightMap.Width;
            terrainLength = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainLength];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainLength];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R;
                    if (heightData[x, y] < minimumHeight) minimumHeight = heightData[x, y];
                    if (heightData[x, y] > maximumHeight) maximumHeight = heightData[x, y];
                }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++){
                    heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 70.0f;
                    heightData[x, y] *= 10; // exaduarate terrain heights
                }
        }

        private VertexPositionNormalTexture[] SetUpTerrainVertices()
        {
            VertexPositionNormalTexture[] terrainVertices = new VertexPositionNormalTexture[terrainWidth * terrainLength];
            vertices = new VertexPositionNormalColored[terrainWidth * terrainLength];


            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    terrainVertices[x + y * terrainWidth].Position = new Vector3(-x*sizeMod, heightData[x, y], y*sizeMod);
                    terrainVertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 30.0f;
                }
            }

            return terrainVertices;
        }

        private int[] SetUpTerrainIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainLength - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainLength - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }

            return indices;
        }

        private VertexPositionNormalTexture[] CalculateNormals(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            return vertices;
        }

        private void CopyToTerrainBuffers(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            terrainVertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices.ToArray());

            terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            terrainIndexBuffer.SetData(indices);
        }

        public override void Draw(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            device.RasterizerState = rs;


            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawTerrain(camera.view);

            base.Draw(gameTime);
        }


        private void DrawTerrain(Matrix currentViewMatrix)
        {
            effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Parameters["xTexture"].SetValue(grassTexture);

            Matrix worldMatrix = Matrix.CreateTranslation(offset) * Matrix.Identity ;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(camera.view);
            effect.Parameters["xProjection"].SetValue(camera.projection);

            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(0.4f);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = terrainIndexBuffer;
                device.SetVertexBuffer(terrainVertexBuffer);

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);
            }
        }

        /// <summary>
        /// Sets the camera that the terrain is being viewed by.
        /// </summary>
        /// <param name="c">Camera used to view the terrain</param>
        public void setCamera(Camera c)
        {
            camera = c;
        }

        public float getTerrainHeight(Vector3 pos)
        {
            Vector2 translatedPos = new Vector2(-pos.X / sizeMod, pos.Z / sizeMod);
            Console.WriteLine("POS ON TERRAIN:"+translatedPos+":Z="+pos.Y);
            List<float> area = new List<float>();
            try
            {
                return heightData[(int)translatedPos.X, (int)translatedPos.Y];
            }
            catch (IndexOutOfRangeException e)
            {

                return pos.Y;
            }
        }
    }
}
