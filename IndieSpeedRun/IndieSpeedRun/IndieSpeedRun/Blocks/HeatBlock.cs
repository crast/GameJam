using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndieSpeedRun.Blocks
{
    public class HeatBlock : IndieSpeedRun.Block
    {
        private bool addsHeat = true;
        public bool AddsHeat
        {
            get { return addsHeat; }
            set { addsHeat = value; }
        }

        public HeatBlock(int x, int y, Sprite sprite)
            :base(x,y,sprite)
        {
        }
    }
}
