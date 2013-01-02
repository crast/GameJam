using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun
{
    /// <summary>
    /// A block is a static entity
    /// </summary>
    public class Block:Entity
    {
        public Block(int x, int y, Sprite sprite)
            : base(x, y, sprite)
        {
        }
    }
}
