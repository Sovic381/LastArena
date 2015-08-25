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
        public int frameIndex;

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
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (time > frameTime)
            {
                frameIndex++;
                time = 0f;
            }
            if (frameIndex > _totalFrames)
                frameIndex = 0;

            Source = new Rectangle(
                frameIndex * frameWidth,
                0,
                frameWidth,
                frameHeight);
        }




    }
}
