using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastArena.Core
{
    class EnemiesShot : GameObject
    {
        public EnemiesShot(int frameWidth, int frameHeight)
        {
            
        }

        //Angle
        public double ShotAngle;

        //vitesse projectile
        public double ShotSpeed = 2.0;
    }
}
