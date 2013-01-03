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

        public List<CollisionBlock> CollisionBlocks { get; set; }

        private List<CollisionBlock> _allCollisionBlocks;
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
            mapBlocks = new List<Block>();
        }

        /// <summary>
        /// Draws map items
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Block block in mapBlocks)
            {
                block.Draw(spriteBatch);
            }
        }

        public void ReduceCollisionBlocks()
        {
            ShapeReducer.Reduce(this);
        }

        public void ViewAreaUpdated(ViewArea area)
        {
            CollisionBlocks = new List<CollisionBlock>();
            foreach (CollisionBlock cb in AllCollisionBlocks) 
            {
                if (area.Rectangle.Intersects(cb.Rectangle)) 
                {
                    CollisionBlocks.Add(cb);
                }
            }
            drawableBlocks.Clear();
            foreach (Block block in mapBlocks)
            {
                if (area.Rectangle.Intersects(block.Rectangle))
                {
                    drawableBlocks.Add(block);
                }
            }
        }
    }
}
