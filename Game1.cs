﻿using Microsoft.Xna.Framework;
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
        //tir ennemis
        List<EnemiesShot> EnemyShots1 = new List<EnemiesShot>();
        List<EnemiesShot> EnemyShots2 = new List<EnemiesShot>();
        float fTimeEnemiesShots1 = 0.0f;
        float fTimeEnemiesShots2 = 0.0f;
        //Texte
        private SpriteFont Font;
        
        
        

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
            Player.Position = new Vector2(100, 240);
            //Tir
            //Shot.Texture =
            //imgShot = Content.Load<Texture2D>("shot");
            //ennemis
            Enemy1.Texture = Content.Load<Texture2D>("enemy");
            Enemy1.Position = new Vector2(700, 140);
            Enemy2.Texture = Content.Load<Texture2D>("enemy");
            Enemy2.Position = new Vector2(750, 300);
            Enemy1.iRand = Enemy1.rand.Next(2);
            //Curseur
            MouseTexture = Content.Load<Texture2D>("Viseur");
            //Texte
            Font = Content.Load<SpriteFont>("Font");
            
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
            fTimeEnemiesShots1 += gameTime.ElapsedGameTime.Milliseconds;
            fTimeEnemiesShots2 += gameTime.ElapsedGameTime.Milliseconds;

            //récupération des touches
            Player.Move(Keyboard.GetState());
            //Curseur
            MouseState = Mouse.GetState();
            
            //Tir
            #region tir
            // tout ce qui est dans cette boucle ne s'execute qu'une seule fois
            // pour créer un nouveau tir
            if (Player.IsPlayerShooting && fTimeShot >= 400.0f)
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
                    if (Shots[i].ShotAngle == Math.PI / 2.0 || Shots[i].ShotAngle == 1.5 * Math.PI)
                    {
                        Shots[i].ShotAngle = Shots[i].ShotAngle + Math.PI;
                    }

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


            //Tir ennemis
            if (fTimeEnemiesShots1 >= 1200.0f)
            {
                EnemyShots1.Add(new EnemiesShot(8, 8));
                fTimeEnemiesShots1 = 0.0f;

                for (int i = EnemyShots1.Count - 1; i < EnemyShots1.Count; i++)
                {
                    //postion des tirs
                    EnemyShots1[i].Position.X = Enemy1.Position.X + 4;
                    EnemyShots1[i].Position.Y = Enemy1.Position.Y + 4;
                    EnemyShots1[i].Texture = Content.Load<Texture2D>("EnemyShot");
                    
                    //direction
                    double dblAutre = 1.0 * (Player.Position.Y - Enemy1.Position.Y) / (Player.Position.X - Enemy1.Position.X);
                    EnemyShots1[i].ShotAngle = (Player.Position.X - Enemy1.Position.X) > 0 ? Math.Atan(dblAutre) : Math.Atan(dblAutre) + Math.PI;
                    if (EnemyShots1[i].ShotAngle == Math.PI / 2.0 || EnemyShots1[i].ShotAngle == 1.5 * Math.PI)
                    {
                        EnemyShots1[i].ShotAngle = EnemyShots1[i].ShotAngle + Math.PI;
                    }
                }
            }
            //Deplacement
            for (int i = 0; i < EnemyShots1.Count; i++)
            {
                EnemyShots1[i].Position.X += (float)(EnemyShots1[i].ShotSpeed * Math.Cos(EnemyShots1[i].ShotAngle));
                EnemyShots1[i].Position.Y += (float)(EnemyShots1[i].ShotSpeed * Math.Sin(EnemyShots1[i].ShotAngle));
            }

            //Tir Ennemi 2
            if (fTimeEnemiesShots2 >= 1200.0f)
            {
                EnemyShots2.Add(new EnemiesShot(8, 8));
                fTimeEnemiesShots2 = 0.0f;

                for (int i = EnemyShots2.Count - 1; i < EnemyShots2.Count; i++)
                {
                    //postion des tirs
                    EnemyShots2[i].Position.X = Enemy2.Position.X + 4;
                    EnemyShots2[i].Position.Y = Enemy2.Position.Y + 4;
                    EnemyShots2[i].Texture = Content.Load<Texture2D>("EnemyShot");

                    //direction
                    double dblAutre = 1.0 * (Player.Position.Y - Enemy2.Position.Y) / (Player.Position.X - Enemy2.Position.X);
                    EnemyShots2[i].ShotAngle = (Player.Position.X - Enemy2.Position.X) > 0 ? Math.Atan(dblAutre) : Math.Atan(dblAutre) + Math.PI;
                    if (EnemyShots2[i].ShotAngle == Math.PI / 2.0 || EnemyShots2[i].ShotAngle == 1.5 * Math.PI)
                    {
                        EnemyShots2[i].ShotAngle = EnemyShots2[i].ShotAngle + Math.PI;
                    }
                }
            }
            //Deplacement
            for (int i = 0; i < EnemyShots2.Count; i++)
            {
                EnemyShots2[i].Position.X += (float)(EnemyShots2[i].ShotSpeed * Math.Cos(EnemyShots2[i].ShotAngle));
                EnemyShots2[i].Position.Y += (float)(EnemyShots2[i].ShotSpeed * Math.Sin(EnemyShots2[i].ShotAngle));
            }


            #endregion

            //Joueur
            Player.UpdateFrame(gameTime);

            //ennemis
            #region ennemis     
            //distance entre les ennemis et le joueur
            if (Enemy1.Position.X < Player.Position.X + Enemy1.SecureDistance && Enemy1.Position.X > Player.Position.X - Enemy1.SecureDistance &&
                Enemy1.Position.Y < Player.Position.Y + Enemy1.SecureDistance && Enemy1.Position.Y > Player.Position.Y - Enemy1.SecureDistance)
            {
                //rien
            }
            else
            {
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
                }
                else
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
            }
            if (Enemy2.Position.X < Player.Position.X + Enemy2.SecureDistance && Enemy2.Position.X > Player.Position.X - Enemy2.SecureDistance &&
                Enemy2.Position.Y < Player.Position.Y + Enemy2.SecureDistance && Enemy2.Position.Y > Player.Position.Y - Enemy2.SecureDistance)
            {
                //rien
            }
            else
            {
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
                    Enemy1.Position.X = 780;
                    Enemy1.Position.Y = 0;
                    Shots.RemoveAt(i);
                }
            }       
            //Ennemi 2
            for (int i = 0; i < Shots.Count; i++)
            {
                if (Shots[i].Position.X >= Enemy2.Position.X && Shots[i].Position.X <= Enemy2.Position.X + 20 &&
                      Shots[i].Position.Y >= Enemy2.Position.Y && Shots[i].Position.Y <= Enemy2.Position.Y + 20)
                {
                    Enemy2.Position.X = 780;
                    Enemy2.Position.Y = 460;
                    Shots.RemoveAt(i);
                }
            }

            //Tue le joueur
            for (int i = 0; i < EnemyShots1.Count; i++)
            {
                if (EnemyShots1[i].Position.X >= Player.Position.X && EnemyShots1[i].Position.X <= Player.Position.X + 20 &&
                      EnemyShots1[i].Position.Y >= Player.Position.Y && EnemyShots1[i].Position.Y <= Player.Position.Y + 20)
                {
                    if (Player.iLife <= 0)
                    {
                        Player.Position.X = 0;
                        Player.Position.Y = 0;
                        Player.iLife = 9;
                    }else
                    {
                        Player.iLife--;
                    }
                    
                    EnemyShots1.RemoveAt(i);
                }
            }
            for (int i = 0; i < EnemyShots2.Count; i++)
            {
                if (EnemyShots2[i].Position.X >= Player.Position.X && EnemyShots2[i].Position.X <= Player.Position.X + 20 &&
                      EnemyShots2[i].Position.Y >= Player.Position.Y && EnemyShots2[i].Position.Y <= Player.Position.Y + 20)
                {
                    if (Player.iLife <= 0)
                    {
                        Player.Position.X = 0;
                        Player.Position.Y = 0;
                        Player.iLife = 9;
                    }
                    else
                    {
                        Player.iLife--;
                    }

                    EnemyShots2.RemoveAt(i);
                }
            }

            //supprime les tirs
            //Ennemis
            for (int i = 0; i < EnemyShots1.Count; i++)
            {
                if (EnemyShots1[i].Position.X <= 0 || EnemyShots1[i].Position.X + 8 >= 800 ||
                    EnemyShots1[i].Position.Y <= 0 || EnemyShots1[i].Position.Y + 8 >= 480)
                {
                    EnemyShots1.RemoveAt(i);
                }
            }
            for (int i = 0; i < EnemyShots2.Count; i++)
            {
                if (EnemyShots2[i].Position.X <= 0 || EnemyShots2[i].Position.X + 8 >= 800 ||
                    EnemyShots2[i].Position.Y <= 0 || EnemyShots2[i].Position.Y + 8 >= 480)
                {
                    EnemyShots2.RemoveAt(i);
                }
            }
            //Joueur
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
            //tir
            for (int i = 0; i < Shots.Count; i++)
            {
                spriteBatch.Draw(Shots[i].Texture, new Vector2(Shots[i].Position.X, Shots[i].Position.Y), Color.White);
            }
            //ennemis
            Enemy1.Draw(spriteBatch);
            Enemy2.Draw(spriteBatch);
            //Joueur
            Player.DrawAnimation(spriteBatch);
            //Tir des ennemis
            for (int i = 0; i < EnemyShots1.Count; i++)
            {
                spriteBatch.Draw(EnemyShots1[i].Texture, new Vector2(EnemyShots1[i].Position.X, EnemyShots1[i].Position.Y), Color.White);
            }
            for (int i = 0; i < EnemyShots2.Count; i++)
            {
                spriteBatch.Draw(EnemyShots2[i].Texture, new Vector2(EnemyShots2[i].Position.X, EnemyShots2[i].Position.Y), Color.White);
            }

            //Texte
            spriteBatch.DrawString(Font, "VIE : " + Player.iLife , new Vector2(300, 10), Color.White);


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
