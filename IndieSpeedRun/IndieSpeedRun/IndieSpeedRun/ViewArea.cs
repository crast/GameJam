using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndieSpeedRun
{
    public class ViewArea
    {
        public Rectangle Rectangle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }
        public Vector2 Offset { get; private set; }

        private List<ViewAreaUpdatable> handlers = new List<ViewAreaUpdatable>();

        public ViewArea(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Update(0, 0);
        }

        public void Update(int left, int top)
        {
            this.Top = top;
            this.Left = left;
            this.Rectangle = new Rectangle(Left, Top, Width, Height);
            this.Right = left + Width;
            this.Bottom = top + Height;
            foreach (ViewAreaUpdatable handler in handlers)
            {
                handler.ViewAreaUpdated(this);
            }
            this.Offset = new Vector2(left, top);
        }

        public void Register(ViewAreaUpdatable handler)
        {
            handlers.Add(handler);
            handler.ViewAreaUpdated(this);
        }


        internal void Unregister(ViewAreaUpdatable handler)
        {
            handlers.Remove(handler);
        }
    }
}
