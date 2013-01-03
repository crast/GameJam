using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IndieSpeedRun
{
    public class ViewArea
    {
        public Rectangle Rectangle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }

        private List<ViewAreaUpdatable> handlers = new List<ViewAreaUpdatable>();

        public ViewArea(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Update(0, 0);
        }

        public void Update(int top, int left)
        {
            this.Top = top;
            this.Left = left;
            this.Rectangle = new Rectangle(Top, Left, Width, Height);
            foreach (ViewAreaUpdatable handler in handlers)
            {
                handler.ViewAreaUpdated(this);
            }
        }

        public void Register(ViewAreaUpdatable handler)
        {
            handlers.Add(handler);
        }

    }
}
