using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndieSpeedRun;

namespace IndieSpeedRun.Blocks
{
    public class BreakableBlock : IndieSpeedRun.Block
    {
        public CollisionBlock collisionBlock { get; set; }
        public BreakableBlock(int x, int y, Sprite sprite, Map map)
            :base(x,y, sprite)
        {
            //this.Kill(); // Do not draw me
            this.collisionBlock = new CollisionBlock(x, y, sprite.Width, sprite.Height);
            map.AllCollisionBlocks.Add(collisionBlock);
        }

        public void Destroy(Map map)
        {
            map.AllCollisionBlocks.Remove(this.collisionBlock);
            this.Kill(); //stop drawing
            //stop all collision
        }
    }
}
