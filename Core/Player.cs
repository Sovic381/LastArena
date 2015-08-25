using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LastArena.Core
{
    class Player : GameObject
    {
        //Constructeur
        public Player(int totalAnimationFrames, int frameWidth, int frameHeight)
            : base(totalAnimationFrames, frameWidth, frameHeight)
        {
            //Image d'animation de base du joueur
            direction = Direction.RIGHT;
            frameIndex = framesIndex.RIGHT_1;      
        }

        //déclaration
        public Boolean IsPlayerShooting = false;

        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W))
            {
                direction = Direction.TOP;
                Position.Y -= 2;
            }
            if (state.IsKeyDown(Keys.A))
            {
                direction = Direction.LEFT;
                Position.X -= 2;
            }
            if (state.IsKeyDown(Keys.S))
            {
                direction = Direction.BOTTOM;
                Position.Y += 2;
            }
            if (state.IsKeyDown(Keys.D))
            {
                direction = Direction.RIGHT;
                Position.X += 2;
            }//Tir du joueur
            if(state.IsKeyDown(Keys.Space))
            {
                IsPlayerShooting = true;
            }
        }


    }
}
