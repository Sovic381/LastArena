using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastArena.Core
{
    class Enemy : GameObject
    {
        public Enemy(int totalAnimationFrames, int frameWidth, int frameHeight)
            : base(totalAnimationFrames, frameWidth, frameHeight)
        {
            //Image d'animation de base des ennemis
            direction = Direction.RIGHT;
            frameIndex = framesIndex.RIGHT_1;      
        }
        //random
        Random rand = new Random();
        //public int iRand;
        private int iSecureDistance = 130;
        //Temps entre les tirs
        private float fShotsTime = 2000.0f;
        //Tirs
        List<EnemiesShot> lEnemyShots = new List<EnemiesShot>();
        //Poursuite de l'ennemi
        public float PursuitTime = 3000.0f;
        public double OldPlayerPositionX;
        public double OldPlayerPositionY;

        public int SecureDistance
        {
            get
            {
                return iSecureDistance;
            }
            set
            {
                if (value < 0)
                {
                    iSecureDistance = 0;
                }
                else
                {
                    iSecureDistance = value;
                }
            }
        }

        public float ShotsTime
        {
            get
            {
                return fShotsTime;
            }
            set
            {
                fShotsTime = value;
            }
        }

        public List<EnemiesShot> EnemyShots
        {
            get
            {
                return lEnemyShots;
            }
            set
            {
                lEnemyShots = value;
            }
        }

        
    }
}
