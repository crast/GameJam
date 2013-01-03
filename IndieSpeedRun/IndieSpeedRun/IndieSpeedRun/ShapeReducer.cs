using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IndieSpeedRun.Blocks;

namespace IndieSpeedRun
{
    class ShapeReducer
    {
        public static void Reduce(Map map)
        {
            List<CollisionBlock> cblocks;
            var lookup = new Dictionary<Point, Block>();
            foreach (Block block in map.Blocks) 
            {
                lookup[block.BlockCoordinate] = block;
            }

            for (int x = 0; x < map.Width; x++) { }

        }
    }
}
