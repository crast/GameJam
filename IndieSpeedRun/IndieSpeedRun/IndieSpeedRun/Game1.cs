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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //holds all textures after loading for easy access
        public Map currentMap;
        private Dictionary<String, Texture2D> textures;
        private Player player;

        public Game1()
        {
            GraphicsAdapter.UseReferenceDevice = true;
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

            //load all textures into a dictionary
            string[] textureNames = { "tile1"};
            for (int i = 0; i < textureNames.Length; i++)
            {
                textures.Add(textureNames[i], this.Content.Load<Texture2D>(textureNames[i]));
            }

            //reads in map data from XML
            readInMapData();
        }

        /// <summary>
        /// Read in map data from an XML file
        /// </summary>
        private void readInMapData()
        {
            //Creates XML Reader
            XmlReader rdr;
            XmlReaderSettings settings = new XmlReaderSettings();

            //Various settings for the XML reader
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            rdr = XmlReader.Create(@"..\..\..\..\IndieSpeedRunContent\test.xml", settings);

            while (rdr.Read())
            {
                //Print out what's being loaded (for debug business)
                //Console.WriteLine(rdr.LocalName);

                //If nothing is present, go to the first element
                if (rdr.IsEmptyElement)
                {
                    rdr.ReadStartElement();
                }

                //If it's a map..
                if (rdr.Name.ToUpper() == "MAP")
                {
                    //Skip to name, and set the currentMap's current name to whatever the XML file says it is.
                    rdr.MoveToAttribute("name");
                    currentMap.Name = rdr.Value;
                    //Print it out, for debug purposes.
                    //Console.WriteLine(currentMap.Name);
                }
                else if (rdr.Name.ToUpper() == "BLOCKS")
                {
                    //Get the height value of the map
                    rdr.MoveToAttribute("height");
                    //..and set the map's height to whatever the file says.
                    currentMap.Height = Int32.Parse(rdr.Value);
                    //Keep readin.
                    rdr.Read();

                    //Run through the file, figure out where the blocks are, and place them where needed.
                    for (int y = 0; y < currentMap.Height; y++)
                    {
                        if (rdr.Name.ToUpper() == "ROW")
                        {
                            rdr.Read();
                        }
                        for (int x = 0; x < rdr.Value.Length; x++)
                        {
                            if (rdr.Value.Substring(x, 1) == "#")
                            {
                                currentMap.Blocks.Add(new Block(x * TILE_SIZE, y * TILE_SIZE, new Sprite(textures["tile1"])));
                            }
                            /*
                            else if (rdr.Value.Substring(x, 1) == "P")
                            {
                                player = new Player(x * TILE_SIZE, y * TILE_SIZE, new Sprite(textures["HeroBall"]), this);
                            }*/
                        }
                        rdr.Read();
                        if (rdr.Name.ToUpper() == "ROW")
                        {
                            rdr.Read();
                        }
                    }
                    rdr.Read();
                }
            }
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
            //player.Update(gameTime); //update player info

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); //background color

            spriteBatch.Begin();//BEGIN
            //player.Draw(spriteBatch);
            currentMap.Draw(spriteBatch);
            spriteBatch.End();//END!

            base.Draw(gameTime);
        }
    }
}
