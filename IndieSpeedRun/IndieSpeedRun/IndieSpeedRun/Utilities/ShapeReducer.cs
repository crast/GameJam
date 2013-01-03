using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IndieSpeedRun.Blocks;

namespace IndieSpeedRun.Utilities
{
    class ShapeReducer
    {
        public static void Reduce(Map map)
        {
            var cblocks = new List<CollisionBlock>();
            var lookup = new Dictionary<Point, Block>();
            foreach (Block block in map.Blocks) 
            {
                lookup[block.BlockCoordinate] = block;
            }

            for (int x = 0; x < map.Width; x++)
            {
                var consecutive = new List<Block>();
                for (int y = 0; y < map.Width; y++)
                {
                    Point p = new Point(x, y);
                    if (lookup.ContainsKey(p))
                    {
                        consecutive.Add(lookup[p]);
                    }
                    else if (consecutive.Count != 0)
                    {
                        cblocks.Add(DoMerge(consecutive));
                        consecutive.Clear();
                    }

                }
                if (consecutive.Count != 0)
                {
                    cblocks.Add(DoMerge(consecutive));
                }
            }

            cbStats(map, cblocks);
            cblocks = HorizontalMerge(cblocks);
            cbStats(map, cblocks);
            map.AllCollisionBlocks = cblocks;
        }

        private static void cbStats(Map map, List<CollisionBlock> cblocks)
        {
            foreach (CollisionBlock cb in cblocks)
            {
                Rectangle cr = cb.Rectangle;
                Console.WriteLine("collisionBlock x={0}, y={1}, w={2}, h={3}", cr.Left, cr.Top, cr.Width, cr.Height);
            }
            Console.WriteLine("Blocks length: {0} Collision Length: {1}", map.Blocks.Count, cblocks.Count);
        }

        private static List<CollisionBlock> HorizontalMerge(List<CollisionBlock> cblocks)
        {
            var nblocks = new List<CollisionBlock>(cblocks);
            foreach (CollisionBlock cblock in nblocks) {
                foreach (CollisionBlock other in nblocks)
                {
                }
            }
            return cblocks;
        }

        private static CollisionBlock DoMerge(List<Block> consecutive)
        {
            Block first = consecutive[0];
            Block last = consecutive[consecutive.Count - 1];
            Rectangle lastR = last.Rectangle;
            return new CollisionBlock(
                (int)first.PositionX,
                (int)first.PositionY, 
                (lastR.Width + lastR.Left) - first.Rectangle.Left,
                (lastR.Height + lastR.Top) - first.Rectangle.Top
            );
        }
    }
}
