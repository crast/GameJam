using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun.Blocks
{
    public class BreakableBlock : IndieSpeedRun.Block
    {
        public BreakableBlock(int x, int y, int width, int height)
            :base(x,y, width, height)
        {

            //this.Kill(); // Do not draw me
        }

        public void Destroy()
        {
            this.Kill(); //stop drawing
            //stop all collision
        }
    }
}
