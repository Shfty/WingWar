using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WingWar
{
    class GameType
    {
        private static GameType instance;
        private float bulletDamage;
        private float missileDamage;

        private bool normalMode = true;
        private bool turboMode = false;

        public GameType() { }

        public static GameType Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameType();
                }
                return instance;
            }
        }

        public float BulletDamage
        {
            get
            {
                return bulletDamage;
            }

            set
            {
                bulletDamage = value;
            }
        }

        public float MissileDamage
        {
            get
            {
                return missileDamage;
            }

            set
            {
                missileDamage = value;
            }
        }

        public bool NormalMode
        {
            get
            {
                return normalMode;
            }

            set
            {
                normalMode = value;
            }
        }

        public bool TurboMode
        {
            get
            {
                return turboMode;
            }

            set
            {
                turboMode = value;
            }
        }

        public void ToggleNormal(bool on)
        {
            if (on)
            {
                NormalMode = true;
                TurboMode = false;
            }
            else
            {
                NormalMode = false;
                TurboMode = true;
            }
        }

        public void SetGameType()
        {
            if (TurboMode)
            {
                BulletDamage = 0.5f;
                MissileDamage = 6.0f;
            }

            if (NormalMode)
            {
                BulletDamage = 0.25f;
                MissileDamage = 3f;
            }
        }
    }
}
