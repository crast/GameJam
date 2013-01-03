using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IndieSpeedRun
{
    public class MovingEntity : Entity
    {
        public enum Facing
        {
            LEFT,
            RIGHT
        };

        public MovingEntity(int x, int y, Sprite sprite)
            : base(x, y, sprite)
        {
        }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int) Position.Y, (int) sprite.Width, (int) sprite.Height);
            }
            set
            {
                base.Rectangle = value;
            }
        }
    }
}
