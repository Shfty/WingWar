#define ARCADE_MODE
#undef ARCADE_MODE

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
    class Aircraft : GameObject
    {
        public int NUM_VERTICES;
        public int playerIndex;

        public Vector3 velocity = Vector3.Zero;
        public Vector3 totalRot = Vector3.Zero;
        public Vector3 rotSpeed = Vector3.Zero;
        public Vector3 stickRotCap = Vector3.Zero;
        public Vector3 modelOffset = new Vector3(0, 0, Camera.FAR_PLANE);
        public Quaternion modelRot = Quaternion.Identity;

        private Vector4 ColorBase, ColorStripe;

        PlayerIndex gamePad;

        public BoundingSphere aircraftSphere;
        public int aircraftType;

        public bool[] cameraView = new bool[4];

        public Vector3 Velocity
        {
            get { return velocity; }
        }

        public Aircraft(Vector3 offset, Quaternion rotation, int playerIndex)
        {
            base.verticesFac = new VerticesFactory();

            base.rotation = rotation;
            this.playerIndex = playerIndex;

            StartPos(offset);

            switch (playerIndex)
            {
                case 1:
                    gamePad = PlayerIndex.One;
                    ColorBase = new Vector4(0.8f, 0.1f, 0.1f, 1f);
                    ColorStripe = new Vector4(1f, 1f, 0f, 1f);
                    break;
                case 2:
                    gamePad = PlayerIndex.Two;
                    ColorBase = new Vector4(1f, 1f, 0.2f, 1f);
                    ColorStripe = new Vector4(0.0f, 0.0f, 0.0f, 1f);
                    break;
                case 3:
                    gamePad = PlayerIndex.Three;
                    ColorBase = new Vector4(0.1f, 0.1f, 1f, 1f);
                    ColorStripe = new Vector4(1f, 1f, 1f, 1f);
                    break;
                case 4:
                    gamePad = PlayerIndex.Four;
                    ColorBase = new Vector4(0.0f, 0.0f, 0.0f, 1f);
                    ColorStripe = new Vector4(0.2f, 1f, 0f, 1f);
                    break;
            }
        }
        
        public void Update(Player player)
        {
            if (player.Active)
            {
                getInput();
                applyPhysics();
                applyMovement();

                aircraftSphere.Center = position;
            }
        }

        private void getInput()
        {            
            Thrust(Controls.Instance.Thrust(gamePad));

            //Airbrake
            Airbrake(Controls.Instance.Airbrake(gamePad));

            Rotate(Controls.Instance.LeftStick(gamePad).X, Controls.Instance.LeftStick(gamePad).Y, Controls.Instance.RightStick(gamePad).X);

            //Rotate(padState.ThumbSticks.Right.X, padState.ThumbSticks.Left.Y, padState.ThumbSticks.Left.X);
        }

        public virtual void Thrust(float thrust)
        {
            //No implementation for base
        }

        public virtual void Airbrake(float intensity)
        {
            //No implementation for base
        }

        public virtual void Rotate(float yaw, float pitch, float roll)
        {
            //No implementation for base
        }

        public virtual void applyMovement()
        {
            //No implementation for base
        }

        public virtual void applyPhysics()
        {
            //No implementation for base
        }

        public virtual void Reset()
        {
            //No implementation for base
        }

        public void StartPos(Vector3 newSpawn)
        {
            position = Vector3.Zero;

            Vector3 offset = Vector3.Transform(newSpawn, rotation);
            position += offset;

            Quaternion startRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), rotation.X) *
                Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), rotation.Y) *
                Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation.Z);

            rotation *= startRot;

            startRot = Quaternion.Identity;
            offset = Vector3.Zero;
        }

        public override void Draw(Camera camera)
        {
            Matrix World = Matrix.CreateFromQuaternion(modelRot) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position + modelOffset);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            if (aircraftType == 0)
                effect.CurrentTechnique = effect.Techniques["Technique1"];

            if (aircraftType == 1)
                effect.CurrentTechnique = effect.Techniques["Technique2"];

            effect.Parameters["Line"].SetValue(0.23f);
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(camera.ViewMatrix());
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix());
            effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            effect.Parameters["AircraftType"].SetValue((float)aircraftType);
            effect.Parameters["PlayerIndex"].SetValue((float)playerIndex);
            effect.Parameters["ColorStripe"].SetValue(ColorStripe);
            effect.Parameters["ColorBase"].SetValue(ColorBase);
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["DiffuseLightDirection"].SetValue(Materials.Instance.LightDirection());
            effect.Parameters["DiffuseLightColor"].SetValue(Materials.Instance.DiffuseLightColor());
            effect.Parameters["LightPosition"].SetValue(position);


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, NUM_VERTICES - 2);
            }

            if (DebugDisplay.Instance.DrawDebug)
            {
                DebugDisplay.Instance.Draw(aircraftSphere, camera.ViewMatrix(), camera.ProjectionMatrix());
            }
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix World = Matrix.CreateFromQuaternion(modelRot) *

                        Matrix.CreateScale(scale) *

                        Matrix.CreateTranslation(position);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

            if (aircraftType == 0)
                effect.CurrentTechnique = effect.Techniques["Technique1"];

            if (aircraftType == 1)
                effect.CurrentTechnique = effect.Techniques["Technique2"];

            effect.Parameters["Line"].SetValue(0.15f);
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(viewMatrix);
            effect.Parameters["Projection"].SetValue(projectionMatrix);
            effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            effect.Parameters["AircraftType"].SetValue((float)aircraftType);
            effect.Parameters["PlayerIndex"].SetValue((float)playerIndex);
            effect.Parameters["ColorStripe"].SetValue(ColorStripe);
            effect.Parameters["ColorBase"].SetValue(ColorBase);
            effect.Parameters["ambientIntensity"].SetValue(Materials.Instance.Ambience);
            effect.Parameters["DiffuseLightDirection"].SetValue(Materials.Instance.LightDirection());
            effect.Parameters["DiffuseLightColor"].SetValue(Materials.Instance.DiffuseLightColor());
            effect.Parameters["LightPosition"].SetValue(position);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsMgr.GraphicsDevice.SetVertexBuffer(verticesFac.objectBuffer);
                Game.GraphicsMgr.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, NUM_VERTICES - 2);
            }
        }
    }
}