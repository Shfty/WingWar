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
    class CityBuilder : GameObject
    {
        List <Buildings> buildingList = new List<Buildings>();
        Random rand;


        public CityBuilder(GraphicsDevice device, ContentManager Content)
        {
            position = new Vector3(16000, 200, -16000);

            for (int i = 0; i < 5; i++)
            {
                rand.Next(0, 2);

                Buildings b = new Buildings(device, new Quaternion(0, 0, 0, 1), new Vector3(1), Content);

                //switch ()
                //{
                //    case 0:
                //        b.Building01();
                //        break;
                //    case 1:
                //        b.Building02();
                //        break;
                //    case 2:
                //        b.Building03();
                //        break;
                //}
                b.position = position;
                buildingList.Add(b);
                position.X += 100;
            }
        }
    }
}
