using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LastArena.Core;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;

namespace LastArena
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Déclaration
        //terrain
        Ground Ground;
        //Joueur
        Player Player;
        //tir
        List<Shot> Shots = new List<Shot>();
        public float fTimeShot = 0.0f;
        //private Texture2D imgShot;
        //Ennemis
        Enemies Enemy1;
        Enemies Enemy2;
        //Curseur
        MouseState MouseState;
        private Texture2D MouseTexture;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //initialisation
            //Terrain
            Ground = new Ground();
            //Joueur (NBanimation, TailleX,TailleY)
            Player = new Player(1, 20, 20);
            //ennemis
            Enemy1 = new Enemies(1, 20, 20);
            Enemy2 = new Enemies(1, 20, 20);
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Chargement du contenu
            //Terrain
            Ground.Texture = Content.Load<Texture2D>("Ground");
            Ground.Position = new Vector2(0, 0);
            //Joueur
            Player.Texture = Content.Load<Texture2D>("Player");
            Player.Position = new Vector2(400, 240);
            //Tir
            //Shot.Texture =
            //imgShot = Content.Load<Texture2D>("shot");
            //ennemis
            Enemy1.Texture = Content.Load<Texture2D>("enemy");
            Enemy1.Position = new Vector2(600, 140);
            Enemy2.Texture = Content.Load<Texture2D>("enemy");
            Enemy2.Position = new Vector2(550, 300);
            Enemy1.iRand = Enemy1.rand.Next(2);
            //Curseur
            MouseTexture = Content.Load<Texture2D>("Viseur");
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Temps
            fTimeShot += gameTime.ElapsedGameTime.Milliseconds;

            //récupération des touches
            Player.Move(Keyboard.GetState());
            //Curseur
            MouseState = Mouse.GetState();
            
            //Tir
            #region tir
            // tout ce qui est dans cette boucle ne s'execute qu'une seule fois
            // pour créer un nouveau tir
            if (Player.IsPlayerShooting && fTimeShot >= 200.0f)
            {
                Shots.Add(new Shot(8, 8));
                Player.IsPlayerShooting = false;
                fTimeShot = 0.0f;

                for (int i = Shots.Count - 1; i < Shots.Count; i++)
                {
                    //postion des tirs
                    Shots[i].Position.X = Player.Position.X + 4;
                    Shots[i].Position.Y = Player.Position.Y + 4;
                    Shots[i].Texture = Content.Load<Texture2D>("shot");

                    //direction des tirs
                    #region ancien
                    /*          if (MouseState.X >= Player.Position.X - 20 && MouseState.X <= Player.Position.X + 40 && MouseState.Y < Player.Position.Y)
                    {
                        //Haut
                        Shot[i].iDirection = 1;
                    }
                    else if (MouseState.X > Player.Position.X && MouseState.Y < Player.Position.Y - 20)
                    {
                        //diagonal haut droite
                        Shot[i].iDirection = 2;
                    }
                    else if(MouseState.Y >= Player.Position.Y - 20 && MouseState.Y <= Player.Position.Y +40 && MouseState.X > Player.Position.X)
                    {
                        //droite
                        Shot[i].iDirection = 3;
                    }
                    else if (MouseState.Y > Player.Position.Y + 20 && MouseState.X > Player.Position.X + 40)
                    {
                        //diagonal bas droite
                        Shot[i].iDirection = 4;
                    }
                    else if (MouseState.X >= Player.Position.X - 20 && MouseState.X <= Player.Position.X + 40 && MouseState.Y > Player.Position.Y + 40)
                    {
                        //bas
                        Shot[i].iDirection = 5;
                    }
                    else if (MouseState.X < Player.Position.X && MouseState.Y > Player.Position.Y + 40)
                    {
                        //diagonal gauche bas
                        Shot[i].iDirection = 6;
                    }
                    else if (MouseState.Y >= Player.Position.Y - 20 && MouseState.Y <= Player.Position.Y + 40 && MouseState.X < Player.Position.X)
                    {
                        //Gauche
                        Shot[i].iDirection = 7;
                    }
                    else if (MouseState.Y < Player.Position.Y && MouseState.X < Player.Position.X)
                    {
                        //diagonal haut gauche
                        Shot[i].iDirection = 8;
                    }
                    */
                    #endregion
                    double dblTemp = 1.0 * (MouseState.Y - Player.Position.Y) / (MouseState.X - Player.Position.X );
                    Shots[i].ShotAngle = (MouseState.X - Player.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;

                }
                   
            }
            //déplacement
            #region ancien
      /*      for (int i = 0; i < Shots.Count; i++)
            {
                if (Shots[i].iDirection == 1)
                {
                    //Haut
                    Shots[i].Position.Y -= 4;
                }
                else if (Shots[i].iDirection == 2)
                {
                    Shots[i].Position.X += 4;
                    Shots[i].Position.Y -= 4;
                }
                else if (Shots[i].iDirection == 3)
                {
                    //droite
                    Shots[i].Position.X += 4;
                }
                else if (Shots[i].iDirection == 4)
                {
                    Shots[i].Position.X += 4;
                    Shots[i].Position.Y += 4;
                }
                else if (Shots[i].iDirection == 5)
                {
                    //bas
                    Shots[i].Position.Y += 4;
                }
                else if (Shots[i].iDirection == 6)
                {
                    Shots[i].Position.X -= 4;
                    Shots[i].Position.Y += 4;
                }
                else if (Shots[i].iDirection == 7)
                {
                    //gauche
                    Shots[i].Position.X -= 4;
                }
                else if (Shots[i].iDirection == 8)
                {
                    Shots[i].Position.X -= 3;
                    Shots[i].Position.Y -= 3;
                }
            }*/
            #endregion
            for (int i = 0; i < Shots.Count; i++)
            {
                Shots[i].Position.X += (float)(Shots[i].ShotSpeed * Math.Cos(Shots[i].ShotAngle));
                Shots[i].Position.Y += (float)(Shots[i].ShotSpeed * Math.Sin(Shots[i].ShotAngle));
            }

            //Version propre
           /* foreach (Shot Shot in Shots)
            {
                Shot.Deplacement();
            }*/


            #endregion

            //Joueur
            Player.UpdateFrame(gameTime);

            //ennemis
            #region ennemis           

            if (Enemy1.iRand == 0)
            {
                if (Enemy1.Position.X > Player.Position.X)
                {
                    Enemy1.Position.X--;
                }
                else if (Enemy1.Position.X < Player.Position.X)
                {
                    Enemy1.Position.X++;
                }
                else if (Enemy1.Position.Y > Player.Position.Y)
                {
                    Enemy1.Position.Y--;
                }
                else if (Enemy1.Position.Y < Player.Position.Y)
                {
                    Enemy1.Position.Y++;
                }
            }else
            {
                if (Enemy1.Position.Y > Player.Position.Y)
                {
                    Enemy1.Position.Y--;
                }
                else if (Enemy1.Position.Y < Player.Position.Y)
                {
                    Enemy1.Position.Y++;
                }
                else if (Enemy1.Position.X > Player.Position.X)
                {
                    Enemy1.Position.X--;
                }
                else if (Enemy1.Position.X < Player.Position.X)
                {
                    Enemy1.Position.X++;
                }
            }

            if (Enemy1.iRand == 1)
            {
                if (Enemy2.Position.X > Player.Position.X)
                {
                    Enemy2.Position.X--;
                }
                else if (Enemy2.Position.X < Player.Position.X)
                {
                    Enemy2.Position.X++;
                }
                else if (Enemy2.Position.Y > Player.Position.Y)
                {
                    Enemy2.Position.Y--;
                }
                else if (Enemy2.Position.Y < Player.Position.Y)
                {
                    Enemy2.Position.Y++;
                }
            }
            else
            {
                if (Enemy2.Position.Y > Player.Position.Y)
                {
                    Enemy2.Position.Y--;
                }
                else if (Enemy2.Position.Y < Player.Position.Y)
                {
                    Enemy2.Position.Y++;
                }
                else if (Enemy2.Position.X > Player.Position.X)
                {
                    Enemy2.Position.X--;
                }
                else if (Enemy2.Position.X < Player.Position.X)
                {
                    Enemy2.Position.X++;
                }
            }

            #endregion

            //vérification des collisions
            #region collision
            //Joueur
            if (Player.Position.X <= 0)
            {
                Player.Position.X += 4;
            }

            if (Player.Position.X + 20 >= Window.ClientBounds.Width)
            {
                Player.Position.X-=4;
            }

            if (Player.Position.Y <= 0)
            {
                Player.Position.Y+=4;
            }

            if (Player.Position.Y + 20 >= Window.ClientBounds.Height)
            {
                Player.Position.Y-=4;
            }

            //Tue les ennemis
            //Ennemi 1
            for (int i = 0; i < Shots.Count; i++)
            {
                if (Shots[i].Position.X >= Enemy1.Position.X && Shots[i].Position.X <= Enemy1.Position.X + 20 &&
                    Shots[i].Position.Y >= Enemy1.Position.Y && Shots[i].Position.Y <= Enemy1.Position.Y + 20)
                {
                    Enemy1.Position.X = 600;
                    Enemy1.Position.Y = 140;
                    Shots.RemoveAt(i);
                }
            }       
            //Ennemi 2
            for (int i = 0; i < Shots.Count; i++)
            {
                if (Shots[i].Position.X >= Enemy2.Position.X && Shots[i].Position.X <= Enemy2.Position.X + 20 &&
                      Shots[i].Position.Y >= Enemy2.Position.Y && Shots[i].Position.Y <= Enemy2.Position.Y + 20)
                {
                    Enemy2.Position.X = 550;
                    Enemy2.Position.Y = 300;
                    Shots.RemoveAt(i);
                }
            }
            //supprime les tirs
            for (int i = 0; i < Shots.Count; i++)
            {
                if (Shots[i].Position.X <= 0 || Shots[i].Position.X + 8 >= 800 ||
                    Shots[i].Position.Y <= 0 || Shots[i].Position.Y + 8 >= 480)
                {
                    Shots.RemoveAt(i);
                }
            }
            
            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //affichage
            spriteBatch.Begin();
            //Terrain
            Ground.Draw(spriteBatch);
            //Joueur
            Player.DrawAnimation(spriteBatch);
            //tir
            for (int i = 0; i < Shots.Count; i++)
            {
                spriteBatch.Draw(Shots[i].Texture, new Vector2(Shots[i].Position.X, Shots[i].Position.Y), Color.White);
            }
            //ennemis
            Enemy1.Draw(spriteBatch);
            Enemy2.Draw(spriteBatch);
            //Curseur
            if (MouseState.X >= 0 || MouseState.X <= Window.ClientBounds.Width || MouseState.Y >= 0 || MouseState.Y <= Window.ClientBounds.Height)
            {
                spriteBatch.Draw(MouseTexture, new Vector2(MouseState.X, MouseState.Y), Color.White);
            }
            
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
