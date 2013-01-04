using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun.Blocks
{
    class LanternBlock : Block
    {
        public LanternBlock(int x, int y, Sprite sprite, string name)
            : base(x, y, sprite)
        {
            this.Alive = false;
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
