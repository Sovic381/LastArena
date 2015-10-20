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
        //Nb vague d'ennemis
        int iNbWave = 1;
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
        int inbEnnemisBase = 1;
        List<Enemy> Enemies = new List<Enemy>();
        List<bigenemy> BigEnemies = new List<bigenemy>();
        //Curseur
        MouseState MouseState;
        private Texture2D MouseTexture;

        //Texte
        private SpriteFont Font;

        //texture
        Texture2D imgGameOver;
        
        
        

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
            foreach (Enemy iEnemy in BigEnemies)
            {
                iEnemy.Texture = Content.Load<Texture2D>("BigEnemy");
            }
            //Curseur
            MouseTexture = Content.Load<Texture2D>("Viseur");
            //Texte
            Font = Content.Load<SpriteFont>("Font");
            //GameOver
            imgGameOver = Content.Load<Texture2D>("GameOver");
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
            //ennemis
            foreach (Enemy iEnemy in Enemies)
            {
                iEnemy.ShotsTime -= gameTime.ElapsedGameTime.Milliseconds;
                iEnemy.PursuitTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
            //Gros ennemis
            foreach (Enemy iBigEnemy in Enemies)
            {
                iBigEnemy.ShotsTime -= gameTime.ElapsedGameTime.Milliseconds;
                iBigEnemy.PursuitTime -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //récupération des touches
            Player.Move(Keyboard.GetState());

            //Curseur
            MouseState = Mouse.GetState();

            //Tir Joueur
            #region tir
            // tout ce qui est dans cette boucle ne s'execute qu'une seule fois
            // pour créer un nouveau tir
            if (Player.IsPlayerShooting && fTimeShot >= 300.0f)
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
                    double dblTemp = 1.0 * (MouseState.Y - Player.Position.Y) / (MouseState.X - Player.Position.X);
                    Shots[i].ShotAngle = (MouseState.X - Player.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;
                    if (Shots[i].ShotAngle == Math.PI / 2.0 || Shots[i].ShotAngle == 1.5 * Math.PI)
                    {
                        Shots[i].ShotAngle = Shots[i].ShotAngle + Math.PI;
                    }
                }
            }

            //déplacement
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
            /*     //Vague d'ennemis
            if (Enemies.Count == 0)
            {
                iNbWave++;
                if (iNbWave >= 6)
                {
                    if (iNbWave >= 11)
                    {
                        inbEnnemisBase += 3;
                        if (iNbWave >= 21)
                        {
                            inbEnnemisBase += 5;
                        }
                    }
                    inbEnnemisBase++;
                }              
                inbEnnemisBase++;
                //Création des ennemis
                for (int i = 0; i < inbEnnemisBase; i++)
                {
                    Enemies.Add(new Enemy(1, 20, 20));
                    Enemies[i].Position.X = 780 - Rand.Next(200);
                    Enemies[i].Position.Y = 460 - Rand.Next(460);
                }
                foreach (Enemy iEnemy in Enemies)
                {
                    iEnemy.Texture = Content.Load<Texture2D>("enemy");
                }
            }
            */
            //Vague d'ennemis
            if (Enemies.Count == 0)
            {
                iNbWave++;
                if (iNbWave >= 6)
                {
                    if (iNbWave >= 11)
                    {
                        inbEnnemisBase += 3;
                        if (iNbWave >= 21)
                        {
                            inbEnnemisBase += 5;
                        }
                    }
                    inbEnnemisBase++;
                }              
                inbEnnemisBase++;
                //Création des ennemis
                //Gros Ennemis
                if (Rand.Next(5) == 1)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        BigEnemies.Add(new bigenemy(1, 20, 20));
                        BigEnemies[i].Position.X = 780 - Rand.Next(200);
                        BigEnemies[i].Position.Y = 460 - Rand.Next(460);
                    }
                    inbEnnemisBase--;
                }
                for (int i = 0; i < inbEnnemisBase; i++)
                {

                    Enemies.Add(new Enemy(1, 20, 20));
                    Enemies[i].Position.X = 780 - Rand.Next(200);
                    Enemies[i].Position.Y = 460 - Rand.Next(460);

                }
                foreach (Enemy iEnemy in Enemies)
                {
                    iEnemy.Texture = Content.Load<Texture2D>("enemy");
                }
                foreach (Enemy iEnemy in BigEnemies)
                {
                    iEnemy.Texture = Content.Load<Texture2D>("BigEnemy");
                }
            }

            //création des ennemis
            

            #region EnnemisNormaux
            for (int i = 0; i < Enemies.Count; i++)
            {

                if (Enemies[i].Position.X < Player.Position.X + Enemies[i].SecureDistance && Enemies[i].Position.X > Player.Position.X - Enemies[i].SecureDistance &&
                    Enemies[i].Position.Y < Player.Position.Y + Enemies[i].SecureDistance && Enemies[i].Position.Y > Player.Position.Y - Enemies[i].SecureDistance)
                {
                    //rien
                }
                else
                {
                    //Temps avant poursuite
                    if (Enemies[i].PursuitTime <= 0.0f)
                    {
                        Enemies[i].OldPlayerPositionX = Player.Position.X;
                        Enemies[i].OldPlayerPositionY = Player.Position.Y;
                        Enemies[i].PursuitTime = 3000.0f - Rand.Next(1000);

                    }
                    //type de déplacement 
                    if (i % 2 == 0)
                    {
                        //type 1 
                        if (Enemies[i].Position.X > Enemies[i].OldPlayerPositionX)
                        {
                            Enemies[i].Position.X--;
                        }
                        else if (Enemies[i].Position.X < Enemies[i].OldPlayerPositionX)
                        {
                            Enemies[i].Position.X++;
                        }
                        else if (Enemies[i].Position.Y > Enemies[i].OldPlayerPositionY)
                        {
                            Enemies[i].Position.Y--;
                        }
                        else if (Enemies[i].Position.Y < Enemies[i].OldPlayerPositionY)
                        {
                            Enemies[i].Position.Y++;
                        }
                    }
                    else
                    {
                        //type 2
                        if (Enemies[i].Position.Y > Enemies[i].OldPlayerPositionY)
                        {
                            Enemies[i].Position.Y--;
                        }
                        else if (Enemies[i].Position.Y < Enemies[i].OldPlayerPositionY)
                        {
                            Enemies[i].Position.Y++;
                        }
                        else if (Enemies[i].Position.X > Enemies[i].OldPlayerPositionX)
                        {
                            Enemies[i].Position.X--;
                        }
                        else if (Enemies[i].Position.X < Enemies[i].OldPlayerPositionX)
                        {
                            Enemies[i].Position.X++;
                        }
                    }
                }
            }
            #endregion
            
            #region GrosEnnemis
            for (int i = 0; i < BigEnemies.Count; i++)
            {

                if (BigEnemies[i].Position.X < Player.Position.X + BigEnemies[i].SecureDistance && BigEnemies[i].Position.X > Player.Position.X - BigEnemies[i].SecureDistance &&
                    BigEnemies[i].Position.Y < Player.Position.Y + BigEnemies[i].SecureDistance && BigEnemies[i].Position.Y > Player.Position.Y - BigEnemies[i].SecureDistance)
                {
                    //rien
                }
                else
                {
                    //Temps avant poursuite
                    if (BigEnemies[i].PursuitTime <= 0.0f)
                    {
                        BigEnemies[i].OldPlayerPositionX = Player.Position.X;
                        BigEnemies[i].OldPlayerPositionY = Player.Position.Y;
                        BigEnemies[i].PursuitTime = 3000.0f - Rand.Next(1000);

                    }
                    //type de déplacement 
                    if (i % 2 == 0)
                    {
                        //type 1 
                        if (BigEnemies[i].Position.X > BigEnemies[i].OldPlayerPositionX)
                        {
                            BigEnemies[i].Position.X--;
                        }
                        else if (BigEnemies[i].Position.X < BigEnemies[i].OldPlayerPositionX)
                        {
                            BigEnemies[i].Position.X++;
                        }
                        else if (BigEnemies[i].Position.Y > BigEnemies[i].OldPlayerPositionY)
                        {
                            BigEnemies[i].Position.Y--;
                        }
                        else if (BigEnemies[i].Position.Y < BigEnemies[i].OldPlayerPositionY)
                        {
                            BigEnemies[i].Position.Y++;
                        }
                    }
                    else
                    {
                        //type 2
                        if (BigEnemies[i].Position.Y > BigEnemies[i].OldPlayerPositionY)
                        {
                            Enemies[i].Position.Y--;
                        }
                        else if (BigEnemies[i].Position.Y < BigEnemies[i].OldPlayerPositionY)
                        {
                            BigEnemies[i].Position.Y++;
                        }
                        else if (BigEnemies[i].Position.X > BigEnemies[i].OldPlayerPositionX)
                        {
                            BigEnemies[i].Position.X--;
                        }
                        else if (BigEnemies[i].Position.X < BigEnemies[i].OldPlayerPositionX)
                        {
                            BigEnemies[i].Position.X++;
                        }
                    }
                }
            }


             #endregion

            //Tir ennemis
            #region EnnemisNormaux
            foreach (Enemy iEnemy in Enemies)
            {
                if (iEnemy.ShotsTime <= 0.0f)
                {
                    iEnemy.EnemyShots.Add(new EnemiesShot(8, 8));
                    iEnemy.ShotsTime = 2300.0f - Rand.Next(500);

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

            #region GrosEnnemis
            foreach (Enemy iEnemy in BigEnemies)
            {
                if (iEnemy.ShotsTime <= 0.0f)
                {
                    iEnemy.EnemyShots.Add(new EnemiesShot(8, 8));
                    iEnemy.EnemyShots.Add(new EnemiesShot(8, 8));
                    iEnemy.EnemyShots.Add(new EnemiesShot(8, 8));
                    iEnemy.ShotsTime = 2600.0f - Rand.Next(500);

                    for (int i = iEnemy.EnemyShots.Count - 3; i < iEnemy.EnemyShots.Count; i++)
                    {
                        //position des tirs
                        iEnemy.EnemyShots[i].Position.X = iEnemy.Position.X;
                        iEnemy.EnemyShots[i].Position.Y = iEnemy.Position.Y;
                        iEnemy.EnemyShots[i].Texture = Content.Load<Texture2D>("EnemyShot");
                        //Direction
                        if (i == iEnemy.EnemyShots.Count - 3)
                        {
                            double dblTemp = 1.0 * (Player.Position.Y - iEnemy.Position.Y) / (Player.Position.X - iEnemy.Position.X);
                            iEnemy.EnemyShots[i].ShotAngle = (Player.Position.X - iEnemy.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;
                            if (iEnemy.EnemyShots[i].ShotAngle == Math.PI / 2.0 || iEnemy.EnemyShots[i].ShotAngle == 1.5 * Math.PI)
                            {
                                iEnemy.EnemyShots[i].ShotAngle = iEnemy.EnemyShots[i].ShotAngle + Math.PI;
                            }
                            iEnemy.EnemyShots[i].ShotAngle -= 10.0;
                        }
                        else if (i == iEnemy.EnemyShots.Count - 2)
                        {
                            double dblTemp = 1.0 * (Player.Position.Y - iEnemy.Position.Y) / (Player.Position.X - iEnemy.Position.X);
                            iEnemy.EnemyShots[i].ShotAngle = (Player.Position.X - iEnemy.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;
                            if (iEnemy.EnemyShots[i].ShotAngle == Math.PI / 2.0 || iEnemy.EnemyShots[i].ShotAngle == 1.5 * Math.PI)
                            {
                                iEnemy.EnemyShots[i].ShotAngle = iEnemy.EnemyShots[i].ShotAngle + Math.PI;
                            }

                        }
                        else
                        {
                            double dblTemp = 1.0 * (Player.Position.Y - iEnemy.Position.Y) / (Player.Position.X - iEnemy.Position.X);
                            iEnemy.EnemyShots[i].ShotAngle = (Player.Position.X - iEnemy.Position.X) > 0 ? Math.Atan(dblTemp) : Math.Atan(dblTemp) + Math.PI;
                            if (iEnemy.EnemyShots[i].ShotAngle == Math.PI / 2.0 || iEnemy.EnemyShots[i].ShotAngle == 1.5 * Math.PI)
                            {
                                iEnemy.EnemyShots[i].ShotAngle = iEnemy.EnemyShots[i].ShotAngle + Math.PI;
                            }
                            iEnemy.EnemyShots[i].ShotAngle += 10.0;
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

            for (int i = 0; i < Enemies.Count; i++)
            {
                for (int j = 0; j < Shots.Count; j++)
                {

                    if (i == Enemies.Count)
                    {
                        //sécurité bizarre
                    }
                    else
                    {
                        if (Shots[j].Position.X >= Enemies[i].Position.X - 5 && Shots[j].Position.X <= Enemies[i].Position.X + 20 &&
                            Shots[j].Position.Y >= Enemies[i].Position.Y - 5 && Shots[j].Position.Y <= Enemies[i].Position.Y + 20)
                        {

                            Enemies.RemoveAt(i);
                            Shots.RemoveAt(j);
                        }
                    }
                }
            }
            //Tue les gros ennemis
            for (int i = 0; i < BigEnemies.Count; i++)
            {
                for (int j = 0; j < Shots.Count; j++)
                {

                    if (i == BigEnemies.Count)
                    {
                        //sécurité bizarre
                    }
                    else
                    {
                        if (Shots[j].Position.X >= BigEnemies[i].Position.X - 5 && Shots[j].Position.X <= BigEnemies[i].Position.X + 35 &&
                            Shots[j].Position.Y >= BigEnemies[i].Position.Y - 5 && Shots[j].Position.Y <= BigEnemies[i].Position.Y + 35)
                        {
                            //enleve un point de vie
                            BigEnemies[i].life--;
                            if (BigEnemies[i].life <= 0)
                            {
                            BigEnemies.RemoveAt(i);
                            Shots.RemoveAt(j);
                            }
                        }
                    }
                }
            }

            //tue le joueur
            foreach (Enemy iEnemy in Enemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    if (iEnemy.EnemyShots[i].Position.X >= Player.Position.X - 5 && iEnemy.EnemyShots[i].Position.X <= Player.Position.X + 20 &&
                      iEnemy.EnemyShots[i].Position.Y >= Player.Position.Y - 5 && iEnemy.EnemyShots[i].Position.Y <= Player.Position.Y + 20)
                    {
                        if (Player.iLife <= 0)
                        {
                            //meurt

                        }
                        else
                        {
                            Player.iLife--;
                        }

                        iEnemy.EnemyShots.RemoveAt(i);
                    }
                }
            }
            //Gros ennemis tue le joueur
            foreach (Enemy iEnemy in BigEnemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    if (iEnemy.EnemyShots[i].Position.X >= Player.Position.X - 5 && iEnemy.EnemyShots[i].Position.X <= Player.Position.X + 35 &&
                      iEnemy.EnemyShots[i].Position.Y >= Player.Position.Y - 5 && iEnemy.EnemyShots[i].Position.Y <= Player.Position.Y + 35)
                    {
                        if (Player.iLife <= 0)
                        {
                            //meurt

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
            foreach (Enemy iEnemy in BigEnemies)
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
            foreach (Enemy iEnemy in BigEnemies)
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
            foreach (Enemy iEnemy in BigEnemies)
            {
                for (int i = 0; i < iEnemy.EnemyShots.Count; i++)
                {
                    spriteBatch.Draw(iEnemy.EnemyShots[i].Texture, new Vector2(iEnemy.EnemyShots[i].Position.X, iEnemy.EnemyShots[i].Position.Y), Color.White);
                }
            }

            //Texte
            spriteBatch.DrawString(Font, "VIE : " + Player.iLife , new Vector2(300, 10), Color.White);
            spriteBatch.DrawString(Font, "Vague : " + iNbWave, new Vector2(300, 30),Color.White);

            //Curseur
            if (MouseState.X >= 0 || MouseState.X <= Window.ClientBounds.Width || MouseState.Y >= 0 || MouseState.Y <= Window.ClientBounds.Height)
            {
                spriteBatch.Draw(MouseTexture, new Vector2(MouseState.X, MouseState.Y), Color.White);
            }
            

            //GameOver
            if (Player.iLife <= 0)
            {
                spriteBatch.Draw(imgGameOver, new Vector2(0, 0), Color.White);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
