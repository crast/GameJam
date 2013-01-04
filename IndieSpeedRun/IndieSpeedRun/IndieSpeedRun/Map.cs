using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IndieSpeedRun.Blocks;
using IndieSpeedRun.Utilities;

namespace IndieSpeedRun
{
    /// <summary>
    /// A map is an isolated set of entities
    /// </summary>
    public class Map : ViewAreaUpdatable
    {
        //private Vector2 origin; //location of map
        private int mapHeight; //unused
        private string mapName; //unused
        protected Texture2D mapImage;

        private Player player; //reference to player
        private List<Block> mapBlocks; //holds all block
        private List<Block> drawableBlocks; // holds drawable only

        public List<Block> TopLayer { get; set; }
        private List<Block> drawableTopLayer;

        public List<Block> BottomLayer { get; set; }
        private List<Block> drawableBottomLayer;

        public List<Block> Interactables { get; set; }

        public List<CollisionBlock> CollisionBlocks { get; set; }
        public SortedDictionary<String, Vector2> Spawns { get; private set; }
        public List<CollisionBlock> ExtraCollisionBlocks { get; set; }

        private List<CollisionBlock> _allCollisionBlocks;
        private Dictionary<Rectangle, string> exits;
        public List<CollisionBlock> AllCollisionBlocks { 
            get {
                return _allCollisionBlocks;
            }
            set
            {
                CollisionBlocks = new List<CollisionBlock>(value);
                _allCollisionBlocks = value;
            }
        }

        //accessor methods
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public int Height
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        public int Width { get; set; }
        public string Name
        {
            get { return mapName; }
            set { mapName = value; }
        }
        public List<Block> Blocks
        {
            set {
                drawableBlocks = new List<Block>(value);
                mapBlocks = value; 
            }
            get { return mapBlocks; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Map()
        {
            mapName = "Default";
            mapHeight = 0;
            ExtraCollisionBlocks = new List<CollisionBlock>();
            BottomLayer = new List<Block>();
            drawableBottomLayer = new List<Block>();
            TopLayer = new List<Block>();
            drawableTopLayer = new List<Block>();
            mapBlocks = new List<Block>();
            drawableBlocks = new List<Block>();
            Interactables = new List<Block>();
            Spawns = new SortedDictionary<string, Vector2>();
            this.exits = new Dictionary<Rectangle, string>();
        }

        public bool ContainsCoordinate(float x, float y)
        {
            Rectangle r = new Rectangle((int)x, (int)y, 1, 1);
            // TODO optimize this using a bounding hierarchy
            foreach (CollisionBlock block in CollisionBlocks)
            {

                if (block.Rectangle.Intersects(r))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Draws map items
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (Block block in drawableBottomLayer)
            {
                block.Draw(spriteBatch, offset);
            }
            foreach (Block block in drawableBlocks)
            {
                block.Draw(spriteBatch, offset);
            }
            foreach (Block block in Interactables)
            {
                block.Draw(spriteBatch, offset);
            }
        }

        public void DrawTopLayer(SpriteBatch spriteBatch, Vector2 offset)
        {
            foreach (Block block in drawableTopLayer)
            {
                block.Draw(spriteBatch, offset);
            }
        }

        public void ReduceCollisionBlocks()
        {
            ShapeReducer.Reduce(this);
            AllCollisionBlocks.AddRange(ExtraCollisionBlocks);
        }

        public void ViewAreaUpdated(ViewArea area)
        {
            CollisionBlocks = new List<CollisionBlock>();
            filterBlocks(CollisionBlocks, AllCollisionBlocks, area.Rectangle);

            filterBlocks(drawableBlocks, mapBlocks, area.Rectangle);

            // Handle top/bottom layer
            filterBlocks(drawableTopLayer, TopLayer, area.Rectangle);
            filterBlocks(drawableBottomLayer, BottomLayer, area.Rectangle);
            /*
            Console.WriteLine(
                "ViewArea re-factor: Drawable blocks: {0}/{1}, Collision blocks: {2}/{3}, Top: {4}, Left: {5}",
                drawableBlocks.Count,
                mapBlocks.Count,
                CollisionBlocks.Count,
                AllCollisionBlocks.Count,
                area.Top,
                area.Left
            );
             */
        }

        private void filterBlocks<T> (List<T> dest, List<T> src, Rectangle rect) where T: Entity {
            dest.Clear();
            foreach (T item in src) 
            {
                if (rect.Intersects(item.Rectangle)) {
                    dest.Add(item);
                }
            }
        }

        internal void AddSpawn(float x, float y, string name)
        {
            Spawns.Add(name, new Vector2(x, y));
        }

        internal void AddExit(Rectangle rectangle, string destination)
        {
            exits.Add(rectangle, destination);
        }

        public string atExit(Player p)
        {
            Rectangle pr = p.Rectangle;
            foreach (var exit in exits)
            {
                if (exit.Key.Intersects(pr)) {
                    return exit.Value;
                }
            }
            return null;
        }

        public string CurrentLantern { get; set; }
    }
}
