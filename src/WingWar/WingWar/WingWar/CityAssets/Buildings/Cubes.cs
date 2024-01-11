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
    class Cubes : GameObject
    {
        //CONFIGURATION
        Vector2 gridDimensions = new Vector2(150, 150); //Total grid size (including border)
        Vector2 gridStride = new Vector2(30, 30); //How many blocks to skip before the next visualised (must be at least 1,1)
        Vector2 gridOffset = new Vector2(15, 15); //How many blocks to skip before the next visualised (must be at least 1,1)

        //Note: Sample values have a much lower update rate than frequency.
        float frequencyFactor = 50f; //Multiply frequency values (x) by this
        float sampleFactor = 100f; //Multiply sample values (y) by this
        float damping = 0.91f; //How much energy should waves have? (must be 0-1)

        /* Intrinsic wave algorithm multiplication factor. Makes crazy shit happen if you mess with it.
         * Known good values:
         * 0.5 - 'Normal'
         * 0.515 - Nice and Wavy
         * 0.52 - Still Nice and Wavy, but maybe a little much
         * 0.529 - WOULD YOU LIKE SOME SKYRIM? a.k.a GIANT MUSICAL APOCALYPSE-BRINGING DEATH CLAM
         * >= 0.53 - DO NOT USE: Resonance Cascade
         * 
         * Known bad values:
         * -0.5 - Bad trip. Lots of jittering.
         */
        //Min: -0.529f, Max: 0.529f. Makes crazy shit happen.
        float waveFactor = 0.5f;
        //END CONFIGURATION
        const int number_of_vertices = 8;
        const int number_of_indices = 36;
        Vector3 bounds = new Vector3(50f, 50f, 50f);

        GraphicsDevice graphics;
        ContentManager content;

        //Effect cubeEffect;

        Matrix World;
        Matrix worldInverseTransposeMatrix;

        //Vector3 position = new Vector3(16000, -510, -16000);

        VertexPositionNormal[] vertices;
        ushort[] indices;
        InstanceDataVertex[] vertexData;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        DynamicVertexBuffer instancedVertexBuffer;
        float[,] cubeHeightsBuffer1, cubeHeightsBuffer2;

        Vector3 LightDirection = new Vector3(1, -1, -1);
        Vector4 LightColor = new Vector4(0.8f, 0.8f, 0.8f, 1);

        public Cubes(GraphicsDevice graphics, ContentManager content, Vector3 position)
        {
            this.graphics = graphics;
            this.content = content;
            this.position = position;
            this.effect = effect;

            effect = content.Load<Effect>("Effects//InstanceShader");

            World = Matrix.Identity;

            CreateCubeVertexBuffer();
            CreateCubeIndexBuffer();
            CalculateNormals();

            vertexData = new InstanceDataVertex[(int)gridDimensions.X * (int)gridDimensions.Y];
            cubeHeightsBuffer1 = new float[(int)gridDimensions.X, (int)gridDimensions.Y];
            cubeHeightsBuffer2 = new float[(int)gridDimensions.X, (int)gridDimensions.Y];

            Random random = new Random();

            for (int x = 0; x < (int)gridDimensions.X; x++)
            {
                for (int z = 0; z < (int)gridDimensions.Y; z++)
                {
                    bool edge = (x == 0 || x == (int)gridDimensions.X - 1) || (z == 0 || z == (int)gridDimensions.Y - 1);

                    float xPos = x - ((int)gridDimensions.X / 2);
                    float zPos = z - ((int)gridDimensions.Y / 2);

                    vertexData[(z * (int)gridDimensions.X) + x] =
                        new InstanceDataVertex(
                            Matrix.CreateTranslation(new Vector3(xPos + position, 100.0f, zPos - position)),
                            SetColour(random.Next(0,9000), edge)
                        );
                }
            }

            // Create instanced VertexBuffer with some temporary data
            instancedVertexBuffer = new DynamicVertexBuffer(graphics, InstanceDataVertex.VertexDeclaration, vertexData.Length, BufferUsage.None);
            instancedVertexBuffer.SetData<InstanceDataVertex>(vertexData);
        }

        public void Update(VisualizationData visualizer)
        {
            for (int x = 1; x < (int)gridDimensions.X - 1; x++)
            {
                for (int z = 1; z < (int)gridDimensions.Y - 1; z++)
                {
                    //If the cube should be visualized, override it's position
                    if ((x - gridOffset.X) % gridStride.X == 0 && (z - gridOffset.Y) % gridStride.Y == 0)
                    {
                        cubeHeightsBuffer1[x, z] = (
                            (
                                (visualizer.Frequencies[x * (int)(visualizer.Frequencies.Count / gridDimensions.X)] * frequencyFactor) +
                                ((float)Math.Asin(visualizer.Samples[z * (int)(visualizer.Samples.Count / gridDimensions.Y)]) * sampleFactor)
                            ) / 2);
                    }
                    else
                    {
                        //Calculate new height based on surrounding cubes
                        cubeHeightsBuffer1[x, z] =
                           ((cubeHeightsBuffer2[x - 1, z] +
                            cubeHeightsBuffer2[x + 1, z] +
                            cubeHeightsBuffer2[x, z + 1] +
                            cubeHeightsBuffer2[x, z - 1]) * waveFactor) - cubeHeightsBuffer1[x, z];
                    }

                    //Reduce the new height by the damping factor
                    cubeHeightsBuffer1[x, z] = cubeHeightsBuffer1[x, z] * damping;
                    
                    float xPos = x - ((int)gridDimensions.X / 2);
                    float zPos = z - ((int)gridDimensions.Y / 2);

                    vertexData[(z * (int)gridDimensions.X) + x].World = Matrix.CreateScale(1.0f, cubeHeightsBuffer2[x, z], 1.0f) * Matrix.CreateTranslation(new Vector3(xPos, 0.0f, zPos));
                }
            }

            //Swap Buffers
            float[,] tempBuffer = cubeHeightsBuffer1;
            cubeHeightsBuffer1 = cubeHeightsBuffer2;
            cubeHeightsBuffer2 = tempBuffer;

            instancedVertexBuffer.SetData<InstanceDataVertex>(vertexData);
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            World = Matrix.CreateTranslation(position);
            worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            effect.CurrentTechnique = effect.Techniques["InstancePositionColour"];
            effect.Parameters["View"].SetValue(viewMatrix);
            effect.Parameters["Projection"].SetValue(projectionMatrix);

            graphics.SetVertexBuffers(vertexBuffer, new VertexBufferBinding(instancedVertexBuffer, 0, 1));
            graphics.Indices = indexBuffer;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3, instancedVertexBuffer.VertexCount);
        }

        void CreateCubeVertexBuffer()
        {
            vertices = new VertexPositionNormal[number_of_vertices];

            vertices[0].Position = new Vector3(-bounds.X, -bounds.Y, -bounds.Z);
            vertices[1].Position = new Vector3(-bounds.X, -bounds.Y, bounds.Z);
            vertices[2].Position = new Vector3(bounds.X, -bounds.Y, bounds.Z);
            vertices[3].Position = new Vector3(bounds.X, -bounds.Y, -bounds.Z);
            vertices[4].Position = new Vector3(-bounds.X, bounds.Y, -bounds.Z);
            vertices[5].Position = new Vector3(-bounds.X, bounds.Y, bounds.Z);
            vertices[6].Position = new Vector3(bounds.X, bounds.Y, bounds.Z);
            vertices[7].Position = new Vector3(bounds.X, bounds.Y, -bounds.Z);

            vertexBuffer = new VertexBuffer(graphics, VertexPositionNormal.VertexDeclaration, number_of_vertices, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionNormal>(vertices);
        }

        private Color SetColour(int randNum, bool edge)
        {
            if (edge)
            {
                return Color.White;
            }

            Color cubeColor = new Color();

            switch (randNum % 8)
            {
                case 0:
                    cubeColor = new Color(154, 205, 50, 255);
                    break;
                case 1:
                    cubeColor = new Color(20, 255, 20, 255);
                    break;
                case 2:
                    cubeColor = new Color(255, 60, 150, 255);
                    break;
                case 3:
                    cubeColor = new Color(120, 80, 255, 255);
                    break;
                case 4:
                    cubeColor = new Color(100, 190, 255, 255);
                    break;
                case 5:
                    cubeColor = new Color(20, 230, 230, 255);
                    break;
                case 6:
                    cubeColor = new Color(255, 140, 0, 255);
                    break;
                case 7:
                    cubeColor = new Color(255, 200, 0, 255);
                    break;
            }

            return cubeColor;
        }

        void CreateCubeIndexBuffer()
        {
            indices = new UInt16[number_of_indices];

            //bottom face
            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;

            //top face
            indices[6] = 4;
            indices[7] = 6;
            indices[8] = 5;
            indices[9] = 4;
            indices[10] = 7;
            indices[11] = 6;

            //front face
            indices[12] = 5;
            indices[13] = 2;
            indices[14] = 1;
            indices[15] = 5;
            indices[16] = 6;
            indices[17] = 2;

            //back face
            indices[18] = 0;
            indices[19] = 7;
            indices[20] = 4;
            indices[21] = 0;
            indices[22] = 3;
            indices[23] = 7;

            //left face
            indices[24] = 0;
            indices[25] = 4;
            indices[26] = 1;
            indices[27] = 1;
            indices[28] = 4;
            indices[29] = 5;

            //right face
            indices[30] = 2;
            indices[31] = 6;
            indices[32] = 3;
            indices[33] = 3;
            indices[34] = 6;
            indices[35] = 7;

            indexBuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
            indexBuffer.SetData<ushort>(indices);

        }

        private void CalculateNormals()
        {
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal = normal;
                vertices[index2].Normal = normal;
                vertices[index3].Normal = normal;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
            }
        }
    }
}