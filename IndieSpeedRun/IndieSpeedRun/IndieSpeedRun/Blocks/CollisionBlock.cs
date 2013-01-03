using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun.Blocks
{
    public class CollisionBlock: IndieSpeedRun.Entity
    {
        public CollisionBlock(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
        }

        public int Right
        {
            get
            {
                return Rectangle.Left + Rectangle.Width;
            }
        }

        public int Bottom
        {
            get
            {
                return Rectangle.Top + Rectangle.Height;
            }
        }
    }
}
