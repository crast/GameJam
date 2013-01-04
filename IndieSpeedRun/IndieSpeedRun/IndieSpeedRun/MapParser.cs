using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;
using IndieSpeedRun.Blocks;

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

            String filePath = @"..\..\..\..\IndieSpeedRunContent\maps\testmap3.json";
            JObject root = getMapRoot(filePath);
            var tileinfo = parseTileSets((JArray)root["tilesets"]);
            JArray layers = (JArray)root["layers"];
            if (layers.Count >= 3)
            {
                for (int i = 0; i < layers.Count; i++) {
                    JObject layer = (JObject)layers[i];
                    List<Block> destination = null;
                    switch ((String) layer["name"]) {
                        case "Background":
                            destination = game.currentMap.BottomLayer;
                            break;
                        case "Gameplay":
                            destination = game.currentMap.Blocks;
                            break;

                        case "Interactive":
                            handleInteractive(game, layer, tileinfo, game.currentMap.Interactables);
                            destination = null;
                            break;

                        case "Foreground":
                            destination = game.currentMap.TopLayer;
                            break;
                        default:
                            throw new Exception("Unknown layer type");
                    }

                    if (destination != null)
                        handleLayer(game, layer, tileinfo, destination);

                }
            }
            else
            {
                handleLayer(game, (JObject)layers[0], tileinfo, game.currentMap.Blocks);
            }
            
            game.currentMap.ReduceCollisionBlocks();
       
        }

        private static void handleInteractive(Game1 game, JObject layer, Dictionary<int,TileInfo> tileinfo, List<Block> list)
        {
            JArray objects = (JArray)layer["objects"];
            foreach (JObject obj in objects) {
                int x = (int) obj["x"];
                int y = (int) obj["y"];
                var kind = (string)obj["type"];
                var name = (string)obj["name"];
                Block createdBlock = null;
                if (obj["gid"] != null)
                {

                    createdBlock = handleInteractiveTile(game, tileinfo, obj, name, kind, x, y);
                }
                else
                {
                    createdBlock = handleInteractiveEntity(game, tileinfo, obj, name, kind, x, y);
                }
                if (createdBlock == null)
                {
                    Console.WriteLine("bad block x={0} y={1} name={2}", x, y, name);
                }
                else
                {

                    list.Add(createdBlock);
                }
            }
        }

        private static Block handleInteractiveEntity(Game1 game, Dictionary<int, TileInfo> tileinfo, JObject obj, string name, string kind, int x, int y)
        {
            int width = (int)obj["width"];
            int height = (int)obj["height"];
            switch (kind)
            {
                case "spawn":
                    game.currentMap.AddSpawn(x, y);
                    return null;
                case "thermal":
                    int amount = int.Parse((string)obj["properties"]["value"]);
                    return new HeatBlock(x, y, width, height, amount);
            }
            return null;
        }

        private static Block handleInteractiveTile(Game1 game, Dictionary<int, TileInfo> tileinfo, JObject obj, string name, string kind, int x, int y)
        {
            
            int gid = (int)obj["gid"];
            var ti = tileinfo[gid];
            Sprite sprite = new Sprite(game.ConditionalLoadSprite(ti.Loc, ti.Path), ti.Origin);
            switch (kind)
            {
                case "door":
                    return new SwitchBlock(x, y, sprite); // FIXME
                case "switch":
                    return new SwitchBlock(x, y, sprite);
                case "breakable":
                    var b =  new BreakableBlock(x, y, sprite, game.currentMap);
                    return b;
            }
            return null;
        } 
        private static void handleLayer(Game1 game, JObject layer, Dictionary<int, TileInfo> tileinfo, List<Block> blocks)
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

                    blocks.Add(new Block(x * Game1.TILE_SIZE, y * Game1.TILE_SIZE, sprite));
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
