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
        Random Rand = new Random();
        int inbEnnemisBase = 2;
        List<Enemy> Enemies = new List<Enemy>();

        //Curseur
        MouseState MouseState;
        private Texture2D MouseTexture;

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
            //Création des ennemis de base
            for (int i = 0; i < inbEnnemisBase; i++)
            {
                Enemies.Add(new Enemy(1, 20, 20));
                Enemies[i].Position.X = 800 - Rand.Next(200);
                Enemies[i].Position.Y = 480 - Rand.Next(480);
            }
            
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
            foreach (Enemy iEnemy in Enemies)
            {
                iEnemy.Texture = Content.Load<Texture2D>("enemy");
            }
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
            foreach (Enemy iEnemy in Enemies)
            {
                iEnemy.ShotsTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            //récupération des touches
            Player.Move(Keyboard.GetState());

            //Curseur
            MouseState = Mouse.GetState();
            
            //Tir Joueur
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
            #endregion

            //Joueur
            Player.UpdateFrame(gameTime);

            //ennemis
            #region ennemis  
            if (Enemies.Count == 0)
            {
                
            }




            for (int i = 0; i < Enemies.Count; i++)
            {

                if (Enemies[i].Position.X < Player.Position.X + Enemies[i].SecureDistance && Enemies[i].Position.X > Player.Position.X - Enemies[i].SecureDistance &&
                    Enemies[i].Position.Y < Player.Position.Y + Enemies[i].SecureDistance && Enemies[i].Position.Y > Player.Position.Y - Enemies[i].SecureDistance)
                {
                    //rien
                }
                else
                {
                    //type de déplacement
                    if (i % 2 == 0)
                    {
                        //type 1
                        if (Enemies[i].Position.X > Player.Position.X)
                        {
                            Enemies[i].Position.X--;
                        }
                        else if (Enemies[i].Position.X < Player.Position.X)
                        {
                            Enemies[i].Position.X++;
                        }
                        else if (Enemies[i].Position.Y > Player.Position.Y)
                        {
                            Enemies[i].Position.Y--;
                        }
                        else if (Enemies[i].Position.Y < Player.Position.Y)
                        {
                            Enemies[i].Position.Y++;
                        }
                    }
                    else
                    {
                        //type 2
                        if (Enemies[i].Position.Y > Player.Position.Y)
                        {
                            Enemies[i].Position.Y--;
                        }
                        else if (Enemies[i].Position.Y < Player.Position.Y)
                        {
                            Enemies[i].Position.Y++;
                        }
                        else if (Enemies[i].Position.X > Player.Position.X)
                        {
                            Enemies[i].Position.X--;
                        }
                        else if (Enemies[i].Position.X < Player.Position.X)
                        {
                            Enemies[i].Position.X++;
                        }
                    }
                }
            }

            //Tir ennemis
            foreach (Enemy iEnemy in Enemies)
            {
                if (iEnemy.ShotsTime >= 1200.0f)
                {
                    iEnemy.EnemyShots.Add(new EnemiesShot(8, 8));
                    iEnemy.ShotsTime = 0.0f;

                    for (int i = iEnemy.EnemyShots.Count - 1; i < iEnemy.EnemyShots.Count; i++)
                    {
                        //position des tirs
                        iEnemy.EnemyShots[i].Position.X = iEnemy.Position.X;
                        iEnemy.EnemyShots[i].Position.Y = iEnemy.Position.Y;
                        iEnemy.EnemyShots[i].Texture = Content.Load<Texture2D>("EnemyShot");
                        //Direction
                        double dblTemp = 1.0 * (Player.Position.Y - iEnemy.Position.Y) / (Player.Position.X - iEnemy.Position.X);
                        iEnemy.EnemyShots[i].ShotAngle = (Player.Position.X - iEnemy.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;
                        if (iEnemy.EnemyShots[i].ShotAngle == Math.PI / 2.0 || iEnemy.EnemyShots[i].ShotAngle == 1.5 * Math.PI)
                        {
                            iEnemy.EnemyShots[i].ShotAngle = iEnemy.EnemyShots[i].ShotAngle + Math.PI;
                        }
                    }
                }

                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    iEnemy.EnemyShots[i].Position.X += (float)(iEnemy.EnemyShots[i].ShotSpeed * Math.Cos(iEnemy.EnemyShots[i].ShotAngle));
                    iEnemy.EnemyShots[i].Position.Y += (float)(iEnemy.EnemyShots[i].ShotSpeed * Math.Sin(iEnemy.EnemyShots[i].ShotAngle));
                }
            }
            
            #endregion

            //vérification des collisions
            #region collision
            //Joueur
            if (Player.Position.X < 0)
            {
                Player.Position.X = 0;
            }

            if (Player.Position.X + 20 > Window.ClientBounds.Width)
            {
                Player.Position.X = Window.ClientBounds.Width - 20;
            }

            if (Player.Position.Y < 0)
            {
                Player.Position.Y = 0;
            }

            if (Player.Position.Y + 20 > Window.ClientBounds.Height)
            {
                Player.Position.Y = Window.ClientBounds.Height - 20;
            }

            //Tue les ennemis

            foreach (Enemy iEnemy in Enemies)
            {
                for (int i = 0; i < Shots.Count; i++)
                {
                    if (Shots[i].Position.X >= iEnemy.Position.X && Shots[i].Position.X <= iEnemy.Position.X + 20 &&
                        Shots[i].Position.Y >= iEnemy.Position.Y && Shots[i].Position.Y <= iEnemy.Position.Y + 20)
                    {
                        iEnemy.Position.X = 800 - Rand.Next(200);
                        iEnemy.Position.Y = 480 - Rand.Next(480); 
                        
                        Shots.RemoveAt(i);
                    }
                }
            }

            //tue le joueur
            foreach (Enemy iEnemy in Enemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    if (iEnemy.EnemyShots[i].Position.X >= Player.Position.X && iEnemy.EnemyShots[i].Position.X <= Player.Position.X + 20 &&
                      iEnemy.EnemyShots[i].Position.Y >= Player.Position.Y && iEnemy.EnemyShots[i].Position.Y <= Player.Position.Y + 20)
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

                        iEnemy.EnemyShots.RemoveAt(i);
                    }
                }
            }

            //Supprime les tirs
            foreach (Enemy iEnemy in Enemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    if (iEnemy.EnemyShots[i].Position.X <= 0 || iEnemy.EnemyShots[i].Position.X + 8 >= 800 ||
                        iEnemy.EnemyShots[i].Position.Y <= 0 || iEnemy.EnemyShots[i].Position.Y + 8 >= 480)
                    {
                        iEnemy.EnemyShots.RemoveAt(i);
                    }
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
            foreach (Enemy iEnemy in Enemies)
            {
                iEnemy.Draw(spriteBatch);
            }

            //Joueur
            Player.DrawAnimation(spriteBatch);

            //Tir des ennemis
            foreach (Enemy iEnemy in Enemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    spriteBatch.Draw(iEnemy.EnemyShots[i].Texture, new Vector2(iEnemy.EnemyShots[i].Position.X, iEnemy.EnemyShots[i].Position.Y), Color.White);
                }
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
