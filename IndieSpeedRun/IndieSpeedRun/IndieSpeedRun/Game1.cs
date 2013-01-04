using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace IndieSpeedRun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int TILE_SIZE = 32;
        private ViewArea viewArea;

        public enum GameState { START, GAME, DEAD, END }
        public GameState gameState;

        GraphicsDeviceManager graphics;
        public int mapWidth
        {
            get { return graphics.PreferredBackBufferWidth; }
        }
        public int mapHeight
        {
            get { return graphics.PreferredBackBufferHeight; }
        }

        SpriteBatch spriteBatch;

        //holds all textures after loading for easy access
        public Map currentMap;
        public PhysicsEngine physics;
        public Dictionary<String, Texture2D> textures;
        private Player player;
        private Hud hud;
        private int chosenSpawn = 0;
        private Parallax parallax;
        private int chosenMapNum = -1;
        private List<String> MapList = new List<String>{"tutorial1", "level2-1", "level3"};

        private Sprite startScreen;
        private Rectangle screenRect;
        private Texture2D deadScreen;

        public Game1()
        {

            if (System.IO.File.Exists(@"..\..\..\..\james.txt")) {
                GraphicsAdapter.UseReferenceDevice = true;
            }
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 35 * TILE_SIZE;
            graphics.PreferredBackBufferHeight = 20 * TILE_SIZE;
            screenRect = new Rectangle(0, 0, mapWidth, mapHeight);
            //graphics.ApplyChanges(); //only needed outside of constructor!
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
            gameState = GameState.START;

            //initialize input
            Input.Initialize();

            // TODO: Add your initialization logic here
            textures = new Dictionary<string, Texture2D>();

            viewArea = new ViewArea(mapWidth, mapHeight);

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

            //LEGACY
            //load all textures into a dictionary
            //LoadSprite("char1", @"sprites\ninja_large");
            LoadSprite("char1", @"sprites\ninja_01-01");
            LoadSprite("tile1", @"tiles\tile1");
            LoadSprite("startScreen", @"sprites/titlescreen-01");

            startScreen = new Sprite(textures["startScreen"]);
            player = new Player(0, 0, new Sprite(textures["char1"], TILE_SIZE*1, TILE_SIZE*2), this, viewArea);
            this.hud = new Hud(this, player);
            LoadNextMap();

            //player.PositionX = 5 * TILE_SIZE;
            //player.PositionY = 10 * TILE_SIZE;

            this.parallax = new Parallax(this);

            viewArea.Register(parallax);
        }

        public void LoadNextMap()
        {
            chosenMapNum = (chosenMapNum + 1) % MapList.Count;
            LoadMap(MapList[chosenMapNum]);
        }

        public void LoadMap(String mapName) {
            if (currentMap != null) viewArea.Unregister(currentMap);
            //initialize map object
            currentMap = new Map();

            //initialize Phyiscs Engine 
            physics = new PhysicsEngine(currentMap);

            //reads in map data from XML
            MapParser.ReadInMapData(this, mapName);
            viewArea.Register(currentMap);

            ChangeSpawn(0);
        }

        public void ChangeSpawn(int p)
        {
            Vector2 spawn = currentMap.Spawns[p];
            player.PositionX = spawn.X;
            player.PositionY = spawn.Y;
            player.Velocity = Vector2.Zero;
        }

        public void LoadSprite(string destName, string src)
        {
            textures.Add(destName, this.Content.Load<Texture2D>(src));
        }

        public Texture2D ConditionalLoadSprite(String destName, String src)
        {
            if (!textures.ContainsKey(destName)) {
                LoadSprite(destName, src);
            }
            return textures[destName];  
        }

        public Texture2D ConditionalLoadSprite(String name)
        {
            return ConditionalLoadSprite(name, name);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            Input.Update(gameTime); //update keyboard/mouse/gamepad states

            if (gameState == GameState.START)
            {
                //Console.WriteLine("START SCREEN");
                if (Input.KeyPressed(Keys.Space))
                {
                    gameState = GameState.GAME;
                }
            }
            else if (gameState == GameState.GAME)
            {
                //Console.WriteLine("GAME IS PLAYING");

                if (Input.KeyPressed(Keys.M))
                {
                    LoadNextMap();
                }

                // Allows the game to exit
                if (Input.KeyPressed(Keys.Escape))
                    gameState = GameState.END;

                // Swap Spawn points
                if (Input.KeyPressed(Keys.O))
                {
                    chosenSpawn = (chosenSpawn + 1) % currentMap.Spawns.Count;
                    ChangeSpawn(chosenSpawn);
                }

                player.Update(gameTime); //update player info
                //this.physics.isHeatingUp(player);
                this.physics.CheckCollisions(player); //check for collisions and resolve

                base.Update(gameTime);
            }
            else if (gameState == GameState.END)
            {
                //Console.WriteLine("GAME IS OVER");
                if (Input.KeyPressed(Keys.Space)||Input.KeyPressed(Keys.Escape))
                {
                    this.Exit();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();//BEGIN

            switch(gameState) {
                case GameState.GAME:
                    GraphicsDevice.Clear(Color.LightGray); //background color
                    parallax.Draw(spriteBatch);
                    currentMap.Draw(spriteBatch, viewArea.Offset);
                    player.Draw(spriteBatch, viewArea.Offset);
                    currentMap.DrawTopLayer(spriteBatch, viewArea.Offset);
                    hud.Draw(spriteBatch);
                    break;
                case GameState.START:
                    GraphicsDevice.Clear(Color.Gray);
                    spriteBatch.Draw(startScreen.Texture, screenRect, Color.White);
                    break;
                case GameState.DEAD:
                    GraphicsDevice.Clear(Color.Wheat);
                    spriteBatch.Draw(deadScreen, screenRect, Color.White);
                    break;
                default:
                    GraphicsDevice.Clear(Color.Red);
                    break;
            }

            spriteBatch.End();//END!\
            base.Draw(gameTime);
        }
    }
}
