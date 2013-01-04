using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndieSpeedRun;

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

        private bool isPad = true;
        public bool IsPad
        {
            get { return isPad; }
            set { isPad = value; }
        }

        public HeatBlock(int x, int y, int width, int height, int thermalAmount)
            :base(x,y, width, height)
        {
            addsHeat = (thermalAmount > 0);
            isPad = (Math.Abs(thermalAmount) > 1);
            this.Kill(); // Do not draw me
        }
    }
}
