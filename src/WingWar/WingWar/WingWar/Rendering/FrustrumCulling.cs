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
    class FrustrumCulling
    {
        private PerlinCity city;
        private List<Player> playerList;
        private ContainmentType currentContainmentType = ContainmentType.Disjoint;

        public FrustrumCulling(PerlinCity city, List<Player> playerList)
        {
            this.city = city;
            this.playerList = playerList;
        }

        public void Update()
        {
            //Culling for buildings checks vs each camera and sets a draw check to true if in view.
            for (int i = 0; i < playerList.Count; i++)
            {
                foreach (Buildings bui in city.buildingList)
                {
                    currentContainmentType = new BoundingFrustum(playerList[i].camera.ViewMatrix() * playerList[i].camera.ProjectionMatrix()).Contains(bui.collisionBox);

                    if (currentContainmentType != ContainmentType.Disjoint)
                    {
                        bui.cameraView[i] = true;
                    }
                }
            }
        }
    }
}
