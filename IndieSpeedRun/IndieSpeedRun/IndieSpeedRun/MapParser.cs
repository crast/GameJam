using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;

namespace IndieSpeedRun
{
    class MapParser
    {
        public static void ReadInMapData(Game1 game)
        {
            ReadInMapDataTL(game);
            //ReadInMapDataOld(game);
        }

        public static void ReadInMapDataTL(Game1 game) {
            String filePath = @"..\..\..\..\IndieSpeedRunContent\maps\testmap.json";
            JObject root = getMapRoot(filePath);
            var tileinfo = parseTileSets((JArray)root["tilesets"]);
            handleLayer(game, (JObject)root["layers"][0], tileinfo);
       
        }

        private static void handleLayer(Game1 game, JObject layer, Dictionary<int, TileInfo> tileinfo)
        {
            game.currentMap.Height = (int)layer["height"];
            int width = game.currentMap.Width = (int)layer["width"];
            JArray blockData = (JArray)layer["data"];
            for (int y = 0; y < game.currentMap.Height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pos = y * width + x;
                    int curItem = (int)blockData[pos];
                    if (curItem == 0) continue;
                    TileInfo ti = tileinfo[curItem];
                    Sprite sprite = new Sprite(game.ConditionalLoadSprite(ti.Loc, ti.Path), ti.Origin);
                    game.currentMap.Blocks.Add(new Block(x * Game1.TILE_SIZE, y * Game1.TILE_SIZE, sprite));
                }
            }
            game.currentMap.ReduceCollisionBlocks();
        }

        private static Dictionary<int, TileInfo> parseTileSets(JArray tilesets)
        {
            var d = new Dictionary<int, TileInfo>();
            foreach (JObject o in tilesets) {
                String path = ((String) o["image"]).TrimStart(new char[] {'.', '\\', '/'}).Replace('/', '\\').Replace(".png", "");
                int imagewidth = (int) o["imagewidth"];
                int imageheight = (int) o["imageheight"];
                int gid = (int) o["firstgid"];
                for (int top = 0; top < imageheight; top += Game1.TILE_SIZE) {
                    for (int left= 0; left < imagewidth; left += Game1.TILE_SIZE) {
                        var origin = new Rectangle(left, top, Game1.TILE_SIZE, Game1.TILE_SIZE);
                        TileInfo info = new TileInfo(path, (String)o["name"], origin);
                        d.Add(gid, info);
                        gid++;
                    }
                }
            }
            return d;
        }

        private static JObject getMapRoot(String filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath, Encoding.UTF8));
        }
        

    }

    class TileInfo
    {
        public string Path { get; set; }
        public string Loc { get; set; }
        public Rectangle Origin {get; set; }
        public TileInfo(String path, String loc, Rectangle origin)
        {
            this.Path = path;
            this.Loc = loc;
            this.Origin = origin;
        }
    }
}
