using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastArena.Core
{
    class Shot : GameObject
    {
        //constructeur
        public Shot(int frameWidth, int frameHeight)
        {
            
        }
        //Direction des tirs (1 = haut, 2 = droite etc...)
        public int iDirection = 1;

        //Angle
        public double ShotAngle;

        //vitesse projectile
        public double ShotSpeed = 4.0;

        public void Deplacement()
        {
            //Version propre
            /*
            Position.X += (float)(ShotSpeed * Math.Cos(ShotAngle));
            Position.Y += (float)(ShotSpeed * Math.Sin(ShotAngle));
             * */
        }

    }
}
