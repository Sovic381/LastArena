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
        List<Shot> Shot = new List<Shot>();
        public float fTimeShot = 0.0f;
        //private Texture2D imgShot;
        //Ennemis
        Enemies Enemy1;
        Enemies Enemy2;
        

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
            //Tir
            #region tir
            // tout ce qui est dans cette boucle ne s'execute qu'une seule fois
            // pour créer un nouveau tir
            if(Player.IsPlayerShooting == true && fTimeShot >= 300.0f )
            {
                Shot.Add(new Shot(8, 8));
                Player.IsPlayerShooting = false;
                fTimeShot = 0.0f;

                for (int i = Shot.Count - 1; i < Shot.Count; i++)
                {
                    //postion des tirs
                    Shot[i].Position.X = Player.Position.X + 4;
                    Shot[i].Position.Y = Player.Position.Y + 4;
                    Shot[i].Texture = Content.Load<Texture2D>("shot");

                    //direction des tirs
                    if (Player.direction == Player.Direction.RIGHT)
                    {
                        Shot[i].iDirection = 2;
                    }
                    else if (Player.direction == Player.Direction.LEFT)
                    {
                        Shot[i].iDirection = 4;
                    }
                    else if (Player.direction == Player.Direction.TOP)
                    {
                        Shot[i].iDirection = 1;
                    }
                    else if (Player.direction == Player.Direction.BOTTOM)
                    {
                        Shot[i].iDirection = 3;
                    }
                }
            }
            //déplacement
            for (int i = 0; i < Shot.Count; i++)
            {
                if (Shot[i].iDirection == 2)
                {
                    Shot[i].Position.X += 4;
                }
                else if (Shot[i].iDirection == 4)
                {
                    Shot[i].Position.X -= 4;
                }
                else if (Shot[i].iDirection == 1)
                {
                    Shot[i].Position.Y -= 4;
                }
                else if (Shot[i].iDirection == 3)
                {
                    Shot[i].Position.Y += 4;
                }    
            }

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
            for (int i = 0; i < Shot.Count; i++)
            {
                if (Shot[i].Position.X >= Enemy1.Position.X && Shot[i].Position.X <= Enemy1.Position.X + 20 &&
                    Shot[i].Position.Y >= Enemy1.Position.Y && Shot[i].Position.Y <= Enemy1.Position.Y + 20)
                {
                    Enemy1.Position.X = 0;
                    Enemy1.Position.Y = 0;
                    Shot.RemoveAt(i);
                }
            }       
            //Ennemi 2
            for (int i = 0; i < Shot.Count; i++)
            {
                if (Shot[i].Position.X >= Enemy2.Position.X && Shot[i].Position.X <= Enemy2.Position.X + 20 &&
                      Shot[i].Position.Y >= Enemy2.Position.Y && Shot[i].Position.Y <= Enemy2.Position.Y + 20)
                {
                    Enemy2.Position.X = 0;
                    Enemy2.Position.Y = 0;
                    Shot.RemoveAt(i);
                }
            }
            //supprime les tirs
            for (int i = 0; i < Shot.Count; i++)
            {
                if (Shot[i].Position.X <= 0 || Shot[i].Position.X + 8 >= 800 ||
                    Shot[i].Position.Y <= 0 || Shot[i].Position.Y + 8 >= 480)
                {
                    Shot.RemoveAt(i);
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
            for (int i = 0; i < Shot.Count; i++)
            {
                spriteBatch.Draw(Shot[i].Texture, new Vector2(Shot[i].Position.X, Shot[i].Position.Y), Color.White);
            }
            //ennemis
            Enemy1.Draw(spriteBatch);
            Enemy2.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
