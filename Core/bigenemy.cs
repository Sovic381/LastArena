using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LastArena.Core
{
    class BigEnemy : Enemy
    {
        public BigEnemy(int totalAnimationFrames, int frameWidth, int frameHeight)
            : base(totalAnimationFrames, frameWidth, frameHeight)
        {
              
        }


        public int life = 5;
        public float BigShotsTime = 2500.0f;
        public int BigSecureDistance = 180;
    }
}
