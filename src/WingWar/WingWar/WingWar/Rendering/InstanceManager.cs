using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace WingWar
{
    class InstanceManager : GameObject
    {
        // During the process of constructing a primitive model, vertex
        // and index data is stored on the CPU in these managed lists.
        List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
        List<ushort> indices = new List<ushort>();


        // Once all the geometry has been specified, the InitializePrimitive
        // method copies the vertex and index data into these buffers, which
        // store it on the GPU ready for efficient rendering.
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public DynamicVertexBuffer instancedVertexBuffer;

        public InstanceDataVertex[] vertexData;
        
        Matrix World;
        Matrix worldInverseTransposeMatrix;

        public InstanceManager(ContentManager content)
        {
            this.effect = effect;
            World = Matrix.Identity;
            effect = content.Load<Effect>("Effects//InstanceCityShader");
        }

        public void InstancedVertexBuffer()
        {
            // Create instanced VertexBuffer with some temporary data
            instancedVertexBuffer = new DynamicVertexBuffer
                (Game.GraphicsMgr.GraphicsDevice, InstanceDataVertex.VertexDeclaration, vertexData.Length, BufferUsage.None);
            instancedVertexBuffer.SetData<InstanceDataVertex>(vertexData);
        }

        /// <summary>
        /// Adds a new vertex to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        public void AddVertex(Vector3 position, Vector3 normal)
        {
            vertices.Add(new VertexPositionNormal(position, normal));
        }

        public void CreateIndex()
        {
            // Six indices (two triangles) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);
        }

        /// <summary>
        /// Adds a new index to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        public void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            indices.Add((ushort)index);
        }


        /// <summary>
        /// Queries the index of the current vertex. This starts at
        /// zero, and increments every time AddVertex is called.
        /// </summary>
        public int CurrentVertex
        {
            get { return vertices.Count; }
        }


        /// <summary>
        /// Once all the geometry has been specified by calling AddVertex and AddIndex,
        /// this method copies the vertex and index data into GPU format buffers, ready
        /// for efficient rendering.
        public void InitializePrimitive()
        {
            // Create a vertex declaration, describing the format of our vertex data.

            // Create a vertex buffer, and copy our vertex data into it.
            vertexBuffer = new VertexBuffer(Game.GraphicsMgr.GraphicsDevice,
                                            typeof(VertexPositionNormal),
                                            vertices.Count, BufferUsage.None);

            vertexBuffer.SetData(vertices.ToArray());

            // Create an index buffer, and copy our index data into it.
            indexBuffer = new IndexBuffer(Game.GraphicsMgr.GraphicsDevice, typeof(ushort),
                                          indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());
        }

        public override void Draw(Camera camera)
        {
            World = Matrix.CreateTranslation(position);
            worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            effect.CurrentTechnique = effect.Techniques["InstancePositionColour"];
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["lineThickness"].SetValue(8.0f);
            effect.Parameters["DiffuseLightDirection"].SetValue(Materials.Instance.LightDirection());
            effect.Parameters["DiffuseLightColor"].SetValue(Materials.Instance.DiffuseLightColor());

            Game.GraphicsMgr.GraphicsDevice.SetVertexBuffers(vertexBuffer, new VertexBufferBinding(instancedVertexBuffer, 0, 1));
            Game.GraphicsMgr.GraphicsDevice.Indices = indexBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            Game.GraphicsMgr.GraphicsDevice.DrawInstancedPrimitives
                (PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount,
                instancedVertexBuffer.VertexCount);
        }
    }
}

