using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun.Blocks
{
    class LanternBlock : Block
    {
        public LanternBlock(int x, int y, Sprite sprite)
            : base(x, y, sprite)
        {
            this.Alive = true;
        }
    }
}
