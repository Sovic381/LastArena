using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastArena.Core
{
    class GameObject
    {

        // Rectangle permettant de définir la zone de l'image à afficher
        public Rectangle Source;
        // Durée depuis laquelle l'image est à l'écran
        public float time;
        // Durée de visibilité d'une image
        public float frameTime = 0.1f;
        // Indice de l'image en cours
        //public int frameIndex;
        //selon tuto
        public framesIndex frameIndex;

        //Index des Images pour animation (pour l'instant selon tuto)
        public enum framesIndex
        {
            RIGHT_1 = 0,
            RIGHT_2 = 1,
            BOTTOM_1 = 2,
            BOTTOM_2 = 3,
            LEFT_1 = 4,
            LEFT_2 = 5,
            TOP_1 = 6,
            TOP_2 = 7
        }
        //Direction du joueur

        public Direction direction;

        public enum Direction
        {
            LEFT = 0,
            RIGHT = 1,
            TOP = 2,
            BOTTOM = 3
        }


        //code selon tuto pour propriété des images d'animation
        private int _totalFrames;
        public int totalFrames
        {
            get { return _totalFrames; }
        }
        private int _frameWidth;
        public int frameWidth
        {
            get { return _frameWidth; }
        }
        private int _frameHeight;
        public int frameHeight
        {
            get { return _frameHeight; }
        }


        //Constructeur
        public GameObject()
        {
        }
        public GameObject(int totalAnimationFrames, int frameWidth, int frameHeight)
        {
            _totalFrames = totalAnimationFrames;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
        }




        //Déclaration
        public Vector2 Position;
        public Texture2D Texture;

        //affichage
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        //affichage animation
        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color.White);

        }

        //calcul du temps passé pour savoir quand changer d'animation
        public void UpdateFrame(GameTime gameTime)
        {
            switch (direction)
            {
                case Direction.TOP:
                    if (frameIndex == framesIndex.TOP_1)
                        frameIndex = framesIndex.TOP_2;
                    else
                        frameIndex = framesIndex.TOP_1;
                    break;
                case Direction.LEFT:
                    if (frameIndex == framesIndex.LEFT_1)
                        frameIndex = framesIndex.LEFT_2;
                    else
                        frameIndex = framesIndex.LEFT_1;
                    break;
                case Direction.BOTTOM:
                    if (frameIndex == framesIndex.BOTTOM_1)
                        frameIndex = framesIndex.BOTTOM_2;
                    else
                        frameIndex = framesIndex.BOTTOM_1;
                    break;
                case Direction.RIGHT:
                    if (frameIndex == framesIndex.RIGHT_1)
                        frameIndex = framesIndex.RIGHT_2;
                    else
                        frameIndex = framesIndex.RIGHT_1;
                    break;
            }
            //selon tuto
            Source = new Rectangle((int)frameIndex * frameWidth, 0, frameWidth, frameHeight);


        }
    }
}
