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

        public Game1()
        {
            Console.WriteLine(System.IO.Path.GetDirectoryName(@"..\..\..\..\"));
            if (System.IO.File.Exists(@"..\..\..\..\james.txt")) {
                GraphicsAdapter.UseReferenceDevice = true;
            }
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 35 * TILE_SIZE;
            graphics.PreferredBackBufferHeight = 20 * TILE_SIZE;
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
            // TODO: Add your initialization logic here
            textures = new Dictionary<string, Texture2D>();

            //initialize input
            Input.Initialize();

            //initialize map object
            currentMap = new Map();

            //initialize Phyiscs Engine 
            physics = new PhysicsEngine(currentMap);

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
            LoadSprite("char1", @"sprites\char1");
            LoadSprite("tile1", @"tiles\tile1");

            //reads in map data from XML
            MapParser.ReadInMapData(this);

            player = new Player(0, 0, new Sprite(textures["char1"], TILE_SIZE*1, TILE_SIZE*2), this);
            player.PositionX = 20 * TILE_SIZE;
            player.PositionY = 10 * TILE_SIZE;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Input.Update(gameTime); //update keyboard/mouse/gamepad states
            player.Update(gameTime); //update player info
            this.physics.CheckCollisions(player); //check for collisions and resolve

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray); //background color

            spriteBatch.Begin();//BEGIN
            currentMap.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();//END!

            base.Draw(gameTime);
        }
    }
}
