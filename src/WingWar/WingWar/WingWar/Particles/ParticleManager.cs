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
    class ParticleManager
    {
        //Singleton setup
        private static ParticleManager instance;

        public static ParticleManager Instance()
        {
            if (instance == null)
            {
                instance = new ParticleManager();
                return instance;
            }
            else
            {
                return instance;
            }
        }
        //End Singleton Setup

        List<BaseParticle> particles;
        Effect particleEffect;

        VertexBuffer particleGeometryBuffer;
        IndexBuffer particleIndexBuffer;
        DynamicVertexBuffer particleInstanceBuffer;
        Vector3 particleNormal = Vector3.Up;

        public ParticleManager()
        {
            particles = new List<BaseParticle>();
            SetupGeometry();
            particleEffect = Game.ContentMgr.Load<Effect>("Effects//ParticleShader");
        }

        private void SetupGeometry()
        {
            VertexPositionNormal[] vertices = new VertexPositionNormal[4];

            vertices[0] = new VertexPositionNormal(new Vector3(-1,-1, 0), particleNormal);
            vertices[1] = new VertexPositionNormal(new Vector3(-1, 1, 0), particleNormal);
            vertices[2] = new VertexPositionNormal(new Vector3( 1,-1, 0), particleNormal);
            vertices[3] = new VertexPositionNormal(new Vector3( 1, 1, 0), particleNormal);

            particleGeometryBuffer = new VertexBuffer(Game.GraphicsMgr.GraphicsDevice, VertexPositionNormal.VertexDeclaration, 4, BufferUsage.WriteOnly);
            particleGeometryBuffer.SetData<VertexPositionNormal>(vertices);

            UInt16[] indices = new UInt16[6];

            indices[5] = 0;
            indices[4] = 1;
            indices[3] = 2;
            indices[2] = 2;
            indices[1] = 1;
            indices[0] = 3;

            particleIndexBuffer = new IndexBuffer(Game.GraphicsMgr.GraphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);
            particleIndexBuffer.SetData<UInt16>(indices);

            particleInstanceBuffer = new DynamicVertexBuffer(Game.GraphicsMgr.GraphicsDevice, InstanceDataVertex.VertexDeclaration, 1, BufferUsage.WriteOnly);
        }

        public void Update()
        {
            for(int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();

                if (particles[i].lifespan <= 0) {
                    particles.RemoveAt(i);
                }
            }
        }

        public void Draw(Camera camera) {
            //No need to continue if we have nothing to draw
            if (particles.Count == 0)
            {
                return;
            }

            //Resize and repopulate buffers
            List<InstanceDataVertex> vertices = new List<InstanceDataVertex>();

            foreach (BaseParticle particle in particles)
            {
                //Manually build each world matrix and billboard
                Matrix world =
                    Matrix.CreateFromQuaternion(particle.Rotation) *
                    Matrix.CreateScale(particle.Scale) *
                    Matrix.Invert(camera.ViewMatrix());
                world.Translation = particle.Position;

                InstanceDataVertex vertex = new InstanceDataVertex(world, particle.Colour);
                vertices.Add(vertex);
            }

            InstanceDataVertex[] vertexArray = vertices.ToArray();
            particleInstanceBuffer = new DynamicVertexBuffer(Game.GraphicsMgr.GraphicsDevice, InstanceDataVertex.VertexDeclaration, vertexArray.Length, BufferUsage.WriteOnly);
            particleInstanceBuffer.SetData<InstanceDataVertex>(vertexArray);

            //Setup shaders
            particleEffect.CurrentTechnique = particleEffect.Techniques["InstancePositionColour"];
            particleEffect.Parameters["View"].SetValue(camera.ViewMatrix());
            particleEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            particleEffect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);

            //Setup graphics device
            Game.GraphicsMgr.GraphicsDevice.SetVertexBuffers(particleGeometryBuffer, new VertexBufferBinding(particleInstanceBuffer, 0, 1));
            Game.GraphicsMgr.GraphicsDevice.Indices = particleIndexBuffer;

            //Draw
            foreach (EffectPass pass in particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    particleGeometryBuffer.VertexCount,
                    0,
                    particleIndexBuffer.IndexCount / 3,
                    particleInstanceBuffer.VertexCount
                );
            }
        }

        //AddParticle + Overloads
        public void AddParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan)
        {
            particles.Add(new BaseParticle(colour, position, rotation, scale, lifespan));
        }

        public void AddParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan, Vector3 velocity, Vector3 damping)
        {
            particles.Add(new BaseParticle(colour, position, rotation, scale, lifespan, velocity, damping));
        }

        public void AddParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan, Vector3 velocity, Vector3 damping, Quaternion rotationSpeed, Quaternion rotationDamping)
        {
            particles.Add(new BaseParticle(colour, position, rotation, scale, lifespan, velocity, damping, rotationSpeed, rotationDamping));
        }

        public void AddParticle(Color colour, Vector3 position, Quaternion rotation, Vector3 scale, int lifespan, Vector3 velocity, Vector3 damping, Quaternion rotationSpeed, Quaternion rotationDamping, Vector3 scaleSpeed, Vector3 scaleDamping)
        {
            particles.Add(new BaseParticle(colour, position, rotation, scale, lifespan, velocity, damping, rotationSpeed, rotationDamping, scaleSpeed, scaleDamping));
        }
    }
}
