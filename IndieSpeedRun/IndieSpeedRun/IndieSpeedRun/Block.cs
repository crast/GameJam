using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IndieSpeedRun
{
    /// <summary>
    /// A block is a static entity
    /// </summary>
    public class Block:Entity
    {
        public Block(int x, int y, Sprite sprite): base(x, y, sprite)
        {
            BlockCoordinate = new Point(x / Game1.TILE_SIZE, y / Game1.TILE_SIZE);
        }

        public Point BlockCoordinate { get; set; }
    }
}
