using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastArena.Core
{
    class Enemies : GameObject
    {
        public Enemies(int totalAnimationFrames, int frameWidth, int frameHeight)
            : base(totalAnimationFrames, frameWidth, frameHeight)
        {
            //Image d'animation de base des ennemis
            direction = Direction.RIGHT;
            frameIndex = framesIndex.RIGHT_1;      
        }
        //random
        public Random rand = new Random();
        public int iRand;
    }
}
