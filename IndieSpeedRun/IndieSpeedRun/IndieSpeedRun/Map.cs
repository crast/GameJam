﻿using System;
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
            drawableBlocks = new List<Block>();
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
            foreach (Block block in drawableBlocks)
            {
                block.Draw(spriteBatch, offset);
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
            Console.WriteLine(
                "ViewArea re-factor: Drawable blocks: {0}/{1}, Collision blocks: {2}/{3}, Top: {4}, Left: {5}",
                drawableBlocks.Count,
                mapBlocks.Count,
                CollisionBlocks.Count,
                AllCollisionBlocks.Count,
                area.Top,
                area.Left
            );
        }
    }
}
