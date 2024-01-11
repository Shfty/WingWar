using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WingWar
{
    class Water 
    {
        Color waterColour;
        Vector3 topLeft, botLeft, topRight, botRight, normal;
        public BoundingBox waterBox;

        Vector4 color = new Vector4(0.1f, 0.6f, 0.8f, 0.6f);

        Vector3 windDirection = new Vector3(-1, 0, 0);
        Texture2D waterBumpMap;

        Effect effect;
        Vector3 position, scale;
        Quaternion rotation;

        VertexPositionTexture[] vertices;
        VertexBuffer objectBuffer;

        public Water(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            effect = Game.ContentMgr.Load<Effect>("Effects//WaterShader");
            waterBumpMap = Game.ContentMgr.Load<Texture2D>("Textures//watermap");

            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            waterBox = new BoundingBox(position, new Vector3(160 * scale.X, position.Y - 10, 160 * scale.Z));

            normal = new Vector3(0, 1, 0);
            waterColour = Color.Gray;
            BuildWater();
        }

        public void BuildWater()
        {
            int NUM_VERTICES = 4;

            vertices = new VertexPositionTexture[NUM_VERTICES];

            // Calculate the vertexPos of the vertices on the top face.
            topRight = new Vector3(0, 0, 0);
            topLeft = new Vector3(0, 0, 160);
            botRight = new Vector3(160, 0, 0);
            botLeft = new Vector3(160, 0, 160);

            AddVertices();
            CreateObjectBuffer();
        }

        public void CreateObjectBuffer()
        {
            objectBuffer = new VertexBuffer(
                Game.GraphicsMgr.GraphicsDevice,
                VertexPositionTexture.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            objectBuffer.SetData(vertices);
        }

        private void AddVertices()
        {
            int i = 0;

            vertices[i] = new VertexPositionTexture(topRight, new Vector2(0, 0)); i++;
            vertices[i] = new VertexPositionTexture(topLeft, new Vector2(0, 1)); i++;
            vertices[i] = new VertexPositionTexture(botRight, new Vector2(1, 0)); i++;
            vertices[i] = new VertexPositionTexture(botLeft, new Vector2(1, 1)); i++;
        }

        public void Draw(Camera camera, GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 500.0f;

            Matrix World = Matrix.CreateFromQuaternion(rotation) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["Color"].SetValue(color);
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
            effect.Parameters["xWaveLength"].SetValue(WeatherParameters.Instance.WaveLength());
            effect.Parameters["xWaveHeight"].SetValue(WeatherParameters.Instance.WaveHeight());
            effect.Parameters["xWindForce"].SetValue(WeatherParameters.Instance.WindForce());
            effect.Parameters["xWindDirection"].SetValue(WeatherParameters.Instance.WindDirection());
            effect.Parameters["xTime"].SetValue(time);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, vertices.Length / 3);
            }

            if (DebugDisplay.Instance.DrawDebug)
            {
                DebugDisplay.Instance.Draw(waterBox, camera.ViewMatrix(), camera.ProjectionMatrix());
            }
        }
    }
}
