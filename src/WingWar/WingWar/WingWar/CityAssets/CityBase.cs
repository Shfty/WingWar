using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WingWar
{
    class CityBase : GameObject
    {
        public CityBase(GraphicsDevice device, Vector3 offset, Quaternion rotation, Vector3 scale)
        {
            verticesFac.NUM_VERTICES = 964;

            verticesFac.vertices = new VertexPositionNormal[verticesFac.NUM_VERTICES];

            base.device = device;
            base.rotation = rotation;
            base.scale = scale;

            StartPos();
            BuildHeli(device);
        }

        private void BuildHeli(GraphicsDevice device)
        {
            // Build the heli, setting up the vertices array            
            MiddleTop();
            MiddleBottom();
            YellowTS();
            YellowTR();
            YellowBS();
            YellowBR();
            BlueTS();
            BlueTR();
            BlueBS();
            BlueBR();
            RedTS();
            RedTR();
            RedBS();
            RedBR();
            GreenTS();
            GreenTR();
            GreenBS();
            GreenBR();
            verticesFac.CreateObjectBuffer(device);

            StartPos();
        }

        private void MiddleTop()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(5.325f, 0.500f, 5.325f);
            verticesFac.topLeftBack = new Vector3(-5.325f, 0.500f, 5.325f);
            verticesFac.topRightFront = new Vector3(5.325f, 0.500f, -5.325f);
            verticesFac.topRightBack = new Vector3(-5.325f, 0.500f, -5.325f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(5.000f, -0.500f, 5.000f);
            verticesFac.btmLeftBack = new Vector3(-5.000f, -0.500f, 5.000f);
            verticesFac.btmRightFront = new Vector3(5.000f, -0.500f, -5.000f);
            verticesFac.btmRightBack = new Vector3(-5.000f, -0.500f, -5.000f);

            verticesFac.AddVerticies();
        }

        private void MiddleBottom()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(5.000f, -0.500f, 5.000f);
            verticesFac.topLeftBack = new Vector3(-5.000f, -0.500f, 5.000f);
            verticesFac.topRightFront = new Vector3(5.000f, -0.500f, -5.000f);
            verticesFac.topRightBack = new Vector3(-5.000f, -0.500f, -5.000f);

            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(5.000f, -4.437f, 5.000f);
            verticesFac.btmLeftBack = new Vector3(-5.000f, -4.437f, 5.000f);
            verticesFac.btmRightFront = new Vector3(5.000f, -4.437f, -5.000f);
            verticesFac.btmRightBack = new Vector3(-5.000f, -4.437f, -5.000f);

            verticesFac.AddVerticies();
        }

        //Z = z axis, P = posative on axis, T = Top, S = Square
        private void YellowTS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(0.023f, 0.500f, -4.484f);
            verticesFac.topLeftBack = new Vector3(-6.016f, 0.500f, -10.494f);
            verticesFac.topRightFront = new Vector3(6.033f, 0.500f, -10.523f);
            verticesFac.topRightBack = new Vector3(-0.008f, 0.500f, -17.215f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(0.022f, -0.500f, -4.852f);
            verticesFac.btmLeftBack = new Vector3(5.648f, -0.500f, -10.495f);
            verticesFac.btmRightFront = new Vector3(5.665f, -0.500f, -10.522f);
            verticesFac.btmRightBack = new Vector3(-0.005f, -0.500f, -16.166f);

            verticesFac.AddVerticies();
        }

        private void YellowTR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(2.662f, 0.500f, -15.975f);
            verticesFac.topLeftBack = new Vector3(2.662f, 0.500f, -5.325f);
            verticesFac.topRightFront = new Vector3(-2.662f, 0.500f, -15.975f);
            verticesFac.topRightBack = new Vector3(-2.662f, 0.500f, -5.325f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(2.500f, -0.500f, -15.000f);
            verticesFac.btmLeftBack = new Vector3(2.500f, -0.500f, -5.000f);
            verticesFac.btmRightFront = new Vector3(-2.500f, -0.500f, -15.000f);
            verticesFac.btmRightBack = new Vector3(-2.500f, -0.500f, -5.000f);

            verticesFac.AddVerticies();
        }

        private void YellowBS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(5.657f, -0.500f, -10.500f);
            verticesFac.topLeftBack = new Vector3(0f, -0.500f, -4.843f);
            verticesFac.topRightFront = new Vector3(-0f, -0.500f, -16.157f);
            verticesFac.topRightBack = new Vector3(-5.657f, -0.500f, -10.500f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(5.657f, -4.437f, -10.500f);
            verticesFac.btmLeftBack = new Vector3(0f, -4.437f, -4.843f);
            verticesFac.btmRightFront = new Vector3(-0f, -4.437f, -16.157f);
            verticesFac.btmRightBack = new Vector3(-5.657f, -4.437f, -10.500f);

            verticesFac.AddVerticies();
        }

        private void YellowBR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(2.500f, -0.500f, -15.000f);
            verticesFac.topLeftBack = new Vector3(2.500f, -0.500f, -5.000f);
            verticesFac.topRightFront = new Vector3(-2.500f, -0.500f, -15.000f);
            verticesFac.topRightBack = new Vector3(-2.500f, -0.500f, -5.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(2.500f, -4.437f, -15.000f);
            verticesFac.btmLeftBack = new Vector3(2.500f, -4.437f, -5.000f);
            verticesFac.btmRightFront = new Vector3(-2.500f, -4.437f, -15.000f);
            verticesFac.btmRightBack = new Vector3(-2.500f, -4.437f, -5.000f);

            verticesFac.AddVerticies();
        }

        private void BlueTS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-4.475f, 0.500f, -0.000f);
            verticesFac.topLeftBack = new Vector3(-10.499f, 0.500f, 6.024f);
            verticesFac.topRightFront = new Vector3(-10.499f, 0.500f, -6.024f);
            verticesFac.topRightBack = new Vector3(-17.207f, 0.500f, 0.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-4.843f, -0.500f, -0.000f);
            verticesFac.btmLeftBack = new Vector3(-10.500f, -0.500f, 5.657f);
            verticesFac.btmRightFront = new Vector3(-10.500f, -0.500f, -5.657f);
            verticesFac.btmRightBack = new Vector3(-16.157f, -0.500f, 0.000f);

            verticesFac.AddVerticies();
        }

        private void BlueTR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-5.325f, 0.500f, 2.662f);
            verticesFac.topLeftBack = new Vector3(-15.975f, 0.500f, 2.662f);
            verticesFac.topRightFront = new Vector3(-5.325f, 0.500f, -2.662f);
            verticesFac.topRightBack = new Vector3(-15.975f, 0.500f, -2.662f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-5.000f, -0.500f, 2.500f);
            verticesFac.btmLeftBack = new Vector3(-15.000f, -0.500f, 2.500f);
            verticesFac.btmRightFront = new Vector3(-5.000f, -0.500f, -2.500f);
            verticesFac.btmRightBack = new Vector3(-15.000f, -0.500f, -2.500f);

            verticesFac.AddVerticies();
        }

        private void BlueBS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-4.843f, -0.500f, -0.000f);
            verticesFac.topLeftBack = new Vector3(-10.500f, -0.500f, 5.657f);
            verticesFac.topRightFront = new Vector3(-10.500f, -0.500f, -5.657f);
            verticesFac.topRightBack = new Vector3(-16.157f, -0.500f, 0.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-4.843f, -4.437f, -0.000f);
            verticesFac.btmLeftBack = new Vector3(-10.500f, -4.437f, 5.657f);
            verticesFac.btmRightFront = new Vector3(-10.500f, -4.437f, -5.657f);
            verticesFac.btmRightBack = new Vector3(-16.157f, -4.437f, 0.000f);

            verticesFac.AddVerticies();
        }

        private void BlueBR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(-5.000f, -0.500f, 2.500f);
            verticesFac.topLeftBack = new Vector3(-15.000f, -0.500f, 2.500f);
            verticesFac.topRightFront = new Vector3(-5.000f, -0.500f, -2.500f);
            verticesFac.topRightBack = new Vector3(-15.000f, -0.500f, -2.500f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(-5.000f, -4.437f, 2.500f);
            verticesFac.btmLeftBack = new Vector3(-15.000f, -4.437f, 2.500f);
            verticesFac.btmRightFront = new Vector3(-5.000f, -4.437f, -2.500f);
            verticesFac.btmRightBack = new Vector3(-15.000f, -4.437f, -2.500f);

            verticesFac.AddVerticies();
        }

        private void RedTS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(6.024f, 0.500f, 10.614f);
            verticesFac.topLeftBack = new Vector3(0f, 0.500f, 17.207f);
            verticesFac.topRightFront = new Vector3(-0f, 0.500f, 4.589f);
            verticesFac.topRightBack = new Vector3(-6.024f, 0.500f, 10.614f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(5.657f, -0.500f, 10.500f);
            verticesFac.btmLeftBack = new Vector3(0f, -0.500f, 16.157f);
            verticesFac.btmRightFront = new Vector3(-0f, -0.500f, 4.843f);
            verticesFac.btmRightBack = new Vector3(-5.657f, -0.500f, 10.500f);

            verticesFac.AddVerticies();
        }

        private void RedTR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(2.662f, 0.500f, 5.325f);
            verticesFac.topLeftBack = new Vector3(2.662f, 0.500f, 15.975f);
            verticesFac.topRightFront = new Vector3(-2.662f, 0.500f, 5.325f);
            verticesFac.topRightBack = new Vector3(-2.662f, 0.500f, 15.975f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(2.500f, -0.500f, 5.000f);
            verticesFac.btmLeftBack = new Vector3(2.500f, -0.500f, 15.000f);
            verticesFac.btmRightFront = new Vector3(-2.500f, -0.500f, 5.000f);
            verticesFac.btmRightBack = new Vector3(-2.500f, -0.500f, 15.000f);

            verticesFac.AddVerticies();
        }

        private void RedBS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(5.657f, -0.500f, 10.500f);
            verticesFac.topLeftBack = new Vector3(0f, -0.500f, 16.157f);
            verticesFac.topRightFront = new Vector3(-0f, -0.500f, 4.843f);
            verticesFac.topRightBack = new Vector3(-5.657f, -0.500f, 10.500f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(5.657f, -4.437f, 10.500f);
            verticesFac.btmLeftBack = new Vector3(0f, -4.437f, 16.157f);
            verticesFac.btmRightFront = new Vector3(-0f, -4.437f, 4.843f);
            verticesFac.btmRightBack = new Vector3(-5.657f, -4.437f, 10.500f);

            verticesFac.AddVerticies();
        }

        private void RedBR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(2.500f, -0.500f, 5.000f);
            verticesFac.topLeftBack = new Vector3(2.500f, -0.500f, 15.000f);
            verticesFac.topRightFront = new Vector3(-2.500f, -0.500f, 5.000f);
            verticesFac.topRightBack = new Vector3(-2.500f, -0.500f, 15.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(2.500f, -4.437f, 5.000f);
            verticesFac.btmLeftBack = new Vector3(2.500f, -4.437f, 15.000f);
            verticesFac.btmRightFront = new Vector3(-2.500f, -4.437f, 5.000f);
            verticesFac.btmRightBack = new Vector3(-2.500f, -4.437f, 15.000f);

            verticesFac.AddVerticies();
        }

        private void GreenTS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(17.207f, 0.500f, -0f);
            verticesFac.topLeftBack = new Vector3(10.486f, 0.500f, 6.024f);
            verticesFac.topRightFront = new Vector3(10.486f, 0.500f, -6.024f);
            verticesFac.topRightBack = new Vector3(4.426f, 0.500f, 0.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(16.157f, -0.500f, -0f);
            verticesFac.btmLeftBack = new Vector3(10.500f, -0.500f, 5.657f);
            verticesFac.btmRightFront = new Vector3(10.500f, -0.500f, -5.657f);
            verticesFac.btmRightBack = new Vector3(4.843f, -0.500f, 0f);

            verticesFac.AddVerticies();
        }

        private void GreenTR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(15.975f, 0.500f, 2.662f);
            verticesFac.topLeftBack = new Vector3(5.325f, 0.500f, 2.662f);
            verticesFac.topRightFront = new Vector3(15.975f, 0.500f, -2.662f);
            verticesFac.topRightBack = new Vector3(5.325f, 0.500f, -2.662f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(15.000f, -0.500f, 2.500f);
            verticesFac.btmLeftBack = new Vector3(5.000f, -0.500f, 2.500f);
            verticesFac.btmRightFront = new Vector3(15.000f, -0.500f, -2.500f);
            verticesFac.btmRightBack = new Vector3(5.000f, -0.500f, -2.500f);

            verticesFac.AddVerticies();
        }

        private void GreenBS()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(16.157f, -0.500f, -0f);
            verticesFac.topLeftBack = new Vector3(10.500f, -0.500f, 5.657f);
            verticesFac.topRightFront = new Vector3(10.500f, -0.500f, -5.657f);
            verticesFac.topRightBack = new Vector3(4.843f, -0.500f, 0.000f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(16.157f, -4.437f, -0.000f);
            verticesFac.btmLeftBack = new Vector3(10.500f, -4.437f, 5.657f);
            verticesFac.btmRightFront = new Vector3(10.500f, -4.437f, -5.657f);
            verticesFac.btmRightBack = new Vector3(4.843f, -4.437f, 0f);

            verticesFac.AddVerticies();
        }

        private void GreenBR()
        {
            // Calculate the vertexPos of the vertices on the top face.
            verticesFac.topLeftFront = new Vector3(15.000f, -0.500f, 2.500f);
            verticesFac.topLeftBack = new Vector3(15.000f, -0.500f, -2.500f);
            verticesFac.topRightFront = new Vector3(5.000f, -0.500f, 2.500f);
            verticesFac.topRightBack = new Vector3(5.000f, -0.500f, -2.500f);


            // Calculate the vertexPos of the vertices on the bottom face.
            verticesFac.btmLeftFront = new Vector3(15.000f, -4.437f, 2.500f);
            verticesFac.btmLeftBack = new Vector3(15.000f, -4.437f, -2.500f);
            verticesFac.btmRightFront = new Vector3(5.000f, -4.437f, 2.500f);
            verticesFac.btmRightBack = new Vector3(5.000f, -4.437f, -2.500f);

            verticesFac.AddVerticies();
        }

        private void StartPos()
        {
            this.scale = new Vector3(10, 10, 10);
        }

        //public override void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice device, float ambience)
        //{
        //    drawEffect.World = Matrix.CreateFromQuaternion(rotation) *

        //                Matrix.CreateScale(scale) *

        //                Matrix.CreateTranslation(position);

        //    drawEffect.View = viewMatrix;
        //    drawEffect.Projection = projectionMatrix;
        //    drawEffect.VertexColorEnabled = true;
        //    drawEffect.PreferPerPixelLighting = true;
        //    drawEffect.LightingEnabled = true;

        //    drawEffect.EnableDefaultLighting();

        //    foreach (EffectPass pass in drawEffect.CurrentTechnique.Passes)
        //    {
        //        pass.Apply();
        //        device.SetVertexBuffer(verticesFac.objectBuffer);
        //        device.DrawPrimitives(PrimitiveType.TriangleList, 0, verticesFac.vertices.Length);
        //    }
        //}
    }
}