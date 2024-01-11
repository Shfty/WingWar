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
    class VehicleSpawner
    {
        VerticesFactory verticesFac;

        public VertexBuffer jetBuffer;
        public VertexBuffer heliBuffer;
        public VertexBuffer aiBuffer;

        public VehicleSpawner()
        {
            verticesFac = new VerticesFactory();

            jetBuffer = BuildJetBuffer();
            heliBuffer = BuildHeliBuffer();
            aiBuffer = BuildAiBuffer();
        }

        private VertexBuffer BuildJetBuffer()
        {
            verticesFac.NUM_VERTICES = 216;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            //LEFT WING
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-3.4f, 0f, 0.5f);
            verticesFac.topLeftBack = new Vector3(-7.5f, 0.7f, 8f);
            verticesFac.topRightFront = new Vector3(-1.3f, 0f, -0.3f);
            verticesFac.topRightBack = new Vector3(-2.6f, 0.3f, 5.5f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-3f, -0.5f, -0.3f);
            verticesFac.btmLeftBack = new Vector3(-4f, 0f, 3.5f);
            verticesFac.btmRightFront = new Vector3(-0.8f, -0.8f, -4f);
            verticesFac.btmRightBack = new Vector3(-2f, -0.45f, 3.5f);

            verticesFac.AddVerticies();

            //RIGHT WING
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(1.3f, 0f, -0.3f);
            verticesFac.topLeftBack = new Vector3(2.6f, 0.3f, 5.5f);
            verticesFac.topRightFront = new Vector3(3.4f, 0f, 0.5f);
            verticesFac.topRightBack = new Vector3(7.5f, 0.7f, 8f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(0.8f, -0.8f, -4f);
            verticesFac.btmLeftBack = new Vector3(2f, -0.45f, 3.5f);
            verticesFac.btmRightFront = new Vector3(3f, -0.5f, -0.3f);
            verticesFac.btmRightBack = new Vector3(4f, 0f, 3.5f);

            verticesFac.AddVerticies();

            //SHIP2
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(0f, 0.7f, -0.3f);
            verticesFac.topLeftBack = new Vector3(0f, 0.3f, 3f);
            verticesFac.topRightFront = new Vector3(1.5f, 0.3f, 0f);
            verticesFac.topRightBack = new Vector3(3f, 0.7f, 7f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(0f, -0.8f, -8f);
            verticesFac.btmLeftBack = new Vector3(0f, -0.45f, 3f);
            verticesFac.btmRightFront = new Vector3(0.4f, -0.8f, -8f);
            verticesFac.btmRightBack = new Vector3(2f, -1.2f, 3f);

            verticesFac.AddVerticies();

            //SHIP1
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-1.5f, 0.3f, 0f);
            verticesFac.topLeftBack = new Vector3(-3f, 0.7f, 7f);
            verticesFac.topRightFront = new Vector3(0f, 0.7f, -0.3f);
            verticesFac.topRightBack = new Vector3(0f, 0.3f, 3f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-0.4f, -0.8f, -8f);
            verticesFac.btmLeftBack = new Vector3(-2f, -1.2f, 3f);
            verticesFac.btmRightFront = new Vector3(0f, -0.8f, -8f);
            verticesFac.btmRightBack = new Vector3(0f, -0.45f, 3f);

            verticesFac.AddVerticies();
            verticesFac.CreateObjectBuffer();

            return verticesFac.objectBuffer;
        }

        private VertexBuffer BuildHeliBuffer()
        {
            verticesFac.NUM_VERTICES = 384;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            //FRONT
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-0.851f, -0.834f, -4.959f);
            verticesFac.topLeftBack = new Vector3(0.851f, -0.0834f, -4.959f);
            verticesFac.topRightFront = new Vector3(-1.113f, 1.642f, -2.532f);
            verticesFac.topRightBack = new Vector3(1.113f, 1.642f, -2.532f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-0.967f, -1.522f, -5.824f);
            verticesFac.btmLeftBack = new Vector3(0.967f, -1.522f, -5.824f);
            verticesFac.btmRightFront = new Vector3(-1.662f, 1.64f, 0.781f);
            verticesFac.btmRightBack = new Vector3(1.662f, 1.64f, 0.781f);

            verticesFac.AddVerticies();

            //BACK
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-1.662f, 1.64f, 0.792f);
            verticesFac.topLeftBack = new Vector3(1.662f, 1.64f, 0.792f);
            verticesFac.topRightFront = new Vector3(-0.48f, 0.089f, 7.125f);
            verticesFac.topRightBack = new Vector3(0.48f, 0.089f, 7.125f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-0.967f, -1.522f, -5.812f);
            verticesFac.btmLeftBack = new Vector3(0.967f, -1.522f, -5.812f);
            verticesFac.btmRightFront = new Vector3(-0.16f, -0.76f, 9.637f);
            verticesFac.btmRightBack = new Vector3(0.16f, -0.76f, 9.637f);

            verticesFac.AddVerticies();

            //TAIL
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-1.086f, -0.973f, 5.781f);
            verticesFac.topLeftBack = new Vector3(1.086f, -0.973f, 5.781f);
            verticesFac.topRightFront = new Vector3(-0.0532f, 0.918f, 8.094f);
            verticesFac.topRightBack = new Vector3(0.0532f, 0.918f, 8.094f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-1.761f, -1.109f, 7.051f);
            verticesFac.btmLeftBack = new Vector3(1.761f, -1.109f, 7.051f);
            verticesFac.btmRightFront = new Vector3(-1.88f, -0.874f, 7.645f);
            verticesFac.btmRightBack = new Vector3(1.88f, -0.874f, 7.645f);

            verticesFac.AddVerticies();

            //WING
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-2.056f, -0.321f, -2.098f);
            verticesFac.topLeftBack = new Vector3(2.056f, -0.321f, -2.098f);
            verticesFac.topRightFront = new Vector3(-3.598f, 0.236f, -0.00526f);
            verticesFac.topRightBack = new Vector3(3.598f, 0.236f, -0.00526f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-1.049f, -0.919f, -3.753f);
            verticesFac.btmLeftBack = new Vector3(1.049f, -0.919f, -3.753f);
            verticesFac.btmRightFront = new Vector3(-4.09f, -0.0553f, 0.571f);
            verticesFac.btmRightBack = new Vector3(4.09f, -0.0553f, 0.571f);

            verticesFac.AddVerticies();

            verticesFac.CreateObjectBuffer();

            return verticesFac.objectBuffer;
        }

        private VertexBuffer BuildAiBuffer()
        {
            verticesFac.NUM_VERTICES = 500;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            //LEFT WING
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-3.4f, 0f, 0.5f);
            verticesFac.topLeftBack = new Vector3(-7.5f, 0.7f, 8f);
            verticesFac.topRightFront = new Vector3(-1.3f, 0f, -0.3f);
            verticesFac.topRightBack = new Vector3(-2.6f, 0.3f, 5.5f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-3f, -0.5f, -0.3f);
            verticesFac.btmLeftBack = new Vector3(-4f, 0f, 3.5f);
            verticesFac.btmRightFront = new Vector3(-0.8f, -0.8f, -4f);
            verticesFac.btmRightBack = new Vector3(-2f, -0.45f, 3.5f);

            verticesFac.AddVerticies();

            //RIGHT WING
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(1.3f, 0f, -0.3f);
            verticesFac.topLeftBack = new Vector3(2.6f, 0.3f, 5.5f);
            verticesFac.topRightFront = new Vector3(3.4f, 0f, 0.5f);
            verticesFac.topRightBack = new Vector3(7.5f, 0.7f, 8f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(0.8f, -0.8f, -4f);
            verticesFac.btmLeftBack = new Vector3(2f, -0.45f, 3.5f);
            verticesFac.btmRightFront = new Vector3(3f, -0.5f, -0.3f);
            verticesFac.btmRightBack = new Vector3(4f, 0f, 3.5f);

            verticesFac.AddVerticies();

            //SHIP2
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(0f, 0.7f, -0.3f);
            verticesFac.topLeftBack = new Vector3(0f, 0.3f, 3f);
            verticesFac.topRightFront = new Vector3(1.5f, 0.3f, 0f);
            verticesFac.topRightBack = new Vector3(3f, 0.7f, 7f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(0f, -0.8f, -8f);
            verticesFac.btmLeftBack = new Vector3(0f, -0.45f, 3f);
            verticesFac.btmRightFront = new Vector3(0.4f, -0.8f, -8f);
            verticesFac.btmRightBack = new Vector3(2f, -1.2f, 3f);

            verticesFac.AddVerticies();

            //SHIP1
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-1.5f, 0.3f, 0f);
            verticesFac.topLeftBack = new Vector3(-3f, 0.7f, 7f);
            verticesFac.topRightFront = new Vector3(0f, 0.7f, -0.3f);
            verticesFac.topRightBack = new Vector3(0f, 0.3f, 3f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-0.4f, -0.8f, -8f);
            verticesFac.btmLeftBack = new Vector3(-2f, -1.2f, 3f);
            verticesFac.btmRightFront = new Vector3(0f, -0.8f, -8f);
            verticesFac.btmRightBack = new Vector3(0f, -0.45f, 3f);

            verticesFac.CreateObjectBuffer();

            return verticesFac.objectBuffer;
        }
    }
}
