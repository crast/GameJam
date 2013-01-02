using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IndieSpeedRun
{
    class MapParser
    {
        /// <summary>
        /// Read in map data from an XML file
        /// </summary>
        public static void ReadInMapData(Game1 game)
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
                    game.currentMap.Name = rdr.Value;
                    //Print it out, for debug purposes.
                    //Console.WriteLine(currentMap.Name);
                }
                else if (rdr.Name.ToUpper() == "BLOCKS")
                {
                    //Get the height value of the map
                    rdr.MoveToAttribute("height");
                    //..and set the map's height to whatever the file says.
                    game.currentMap.Height = Int32.Parse(rdr.Value);
                    //Keep readin.
                    rdr.Read();

                    //Run through the file, figure out where the blocks are, and place them where needed.
                    for (int y = 0; y < game.currentMap.Height; y++)
                    {
                        if (rdr.Name.ToUpper() == "ROW")
                        {
                            rdr.Read();
                        }
                        for (int x = 0; x < rdr.Value.Length; x++)
                        {
                            if (rdr.Value.Substring(x, 1) == "#")
                            {
                                game.currentMap.Blocks.Add(new Block(x * Game1.TILE_SIZE, y * Game1.TILE_SIZE, new Sprite(game.textures["tile1"])));
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

    }
}
