/* Controls.cs
 *
 * Abstracts Keyboard and GamePad input into a single class
 */

//#define ARCADE
#undef ARCADE

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace WingWar
{
    class Controls
    {
        //Singleton setup
        private static Controls instance;

        public Controls() {}

        public static Controls Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Controls();
                }
                return instance;
            }
        }

        //Control checking methods
        public Vector2 LeftStick(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).ThumbSticks.Left;
            }
            else
            {
                Vector2 output = Vector2.Zero;

#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        output.X = -1.0f;
                    else if (Keyboard.GetState().IsKeyDown(Keys.G))
                        output.X = 1.0f;

                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                        output.Y = 1.0f;
                    else if (Keyboard.GetState().IsKeyDown(Keys.F))
                        output.Y = -1.0f;

                    return output;
                }
#endif

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    output.X = -1.0f;
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    output.X = 1.0f;

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    output.Y = 1.0f;
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    output.Y = -1.0f;

                return output;
            }
        }

        public Vector2 RightStick(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).ThumbSticks.Right;
            }
            else
            {
                Vector2 output = Vector2.Zero;

#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        output.X = -1.0f;
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                        output.X = 1.0f;

                    return output;
                }
#endif

                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    output.X = -1.0f;
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    output.X = 1.0f;

                return output;
            }
        }

        public float Thrust(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).Triggers.Right;
            }
            else
            {
#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    return Keyboard.GetState().IsKeyDown(Keys.W) ? 1.0f : 0.0f;
                }
#endif
                return Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 1.0f : 0.0f;
            }
        }

        public float Airbrake(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).Triggers.Left;
            }
            else
            {
#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    return Keyboard.GetState().IsKeyDown(Keys.I) ? 1.0f : 0.0f;
                }
#endif
                return Keyboard.GetState().IsKeyDown(Keys.Z) ? 1.0f : 0.0f;
            }
        }

        public bool PrimaryFire(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightShoulder);
            }
            else
            {
#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    return Keyboard.GetState().IsKeyDown(Keys.Q);
                }
#endif
                return Keyboard.GetState().IsKeyDown(Keys.Space);
            }
        }

        public bool SecondaryFire(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.LeftShoulder);
            }
            else
            {
#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    return Keyboard.GetState().IsKeyDown(Keys.K);
                }
#endif
                return Keyboard.GetState().IsKeyDown(Keys.X);
            }
        }

        public bool FirstPerson(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadUp);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Enter);
            }
        }

        //public bool RemovePlayer(PlayerIndex playerIndex)
        //{
        //    /*
        //    if (GamePad.GetState(playerIndex).IsConnected)
        //    {
        //        return GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightShoulder);
        //    }
        //    else
        //    {
        //     */
        //    return Keyboard.GetState().IsKeyDown(Keys.D1);
        //    //}
        //}

        //public bool AddJet(PlayerIndex playerIndex)
        //{
        //    /*
        //    if (GamePad.GetState(playerIndex).IsConnected)
        //    {
        //        return GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightShoulder);
        //    }
        //    else
        //    {
        //     */
        //    return Keyboard.GetState().IsKeyDown(Keys.D2);
        //    //}
        //}

        //public bool AddHeli(PlayerIndex playerIndex)
        //{
        //    /*
        //    if (GamePad.GetState(playerIndex).IsConnected)
        //    {
        //        return GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightShoulder);
        //    }
        //    else
        //    {
        //     */
        //    return Keyboard.GetState().IsKeyDown(Keys.D3);
        //    //}
        //}

        public bool Up(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadUp) || (GamePad.GetState(playerIndex).ThumbSticks.Left.Y > 0.5f);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Up);
            }
        }

        public bool Down(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadDown) || (GamePad.GetState(playerIndex).ThumbSticks.Left.Y < -0.5f);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Down);
            }
        }

        public bool Left(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadLeft) || (GamePad.GetState(playerIndex).ThumbSticks.Left.X < -0.5f);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Left);
            }
        }

        public bool Right(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadRight) || (GamePad.GetState(playerIndex).ThumbSticks.Left.X > 0.5f);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Right);
            }
        }

        public bool Accept(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.A);
            }
            else
            {
#if ARCADE
                if (playerIndex == PlayerIndex.Two)
                {
                    return Keyboard.GetState().IsKeyDown(Keys.W);
                }
                else
                {
                    return Keyboard.GetState().IsKeyDown(Keys.LeftShift);
                }
#else
                return Keyboard.GetState().IsKeyDown(Keys.Enter);
#endif

            }
        }

        public bool Start(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start) || (Keyboard.GetState().IsKeyDown(Keys.C) && Keyboard.GetState().IsKeyDown(Keys.J));
            }
        }

        public bool Wireframe(PlayerIndex playerIndex)
        {
            /*
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightShoulder);
            }
            else
            {
             */
            return Keyboard.GetState().IsKeyDown(Keys.F1);
            //}
        }

        public bool ActionButtons(PlayerIndex playerIndex)
        {
            return PrimaryFire(playerIndex)
                || SecondaryFire(playerIndex)
                || Accept(playerIndex)
                || Up(playerIndex)
                || Down(playerIndex)
                || Left(playerIndex)
                || Right(playerIndex);
        }

        public bool AttractModeTimer()
        {
            if (Controls.Instance.ActionButtons(PlayerIndex.One)
                            || Controls.Instance.ActionButtons(PlayerIndex.Two)
                            || Controls.Instance.ActionButtons(PlayerIndex.Three)
                            || Controls.Instance.ActionButtons(PlayerIndex.Four))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemovePlayer(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.Back);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.D1);
            }
        }

        public bool AddPlayer(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.D2);
            }
        }

        public bool FreezeCam(PlayerIndex playerIndex)
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                return GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadDown);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(Keys.F1);
            }
        }
    }
}
