using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndieSpeedRun
{
    class Parallax : ViewAreaUpdatable
    {
        private const float backFactor = 0.8f;
        private Texture2D backLayer;
        private Rectangle backSourceRect = Rectangle.Empty;

        private const float frontFactor = 0.6f;
        private Rectangle frontSourceRect = Rectangle.Empty;
        private Texture2D frontLayer;

        private Rectangle destRect;

        public Parallax(Game1 game)
        {
            this.backLayer = game.ConditionalLoadSprite("tiles/scene-01");
            this.frontLayer = game.ConditionalLoadSprite("tiles/scene2-01");
            this.destRect = new Rectangle(0, 0, game.mapWidth, game.mapHeight);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(backLayer, destRect, backSourceRect, Color.White);
            batch.Draw(frontLayer, destRect, frontSourceRect, Color.White);
        }

        public void ViewAreaUpdated(ViewArea area)
        {
            int w = destRect.Width;
            int h = destRect.Height;
            var OffsetV = area.Offset;
            frontSourceRect = new Rectangle(
                (int)(OffsetV.X * frontFactor),
                (int)(OffsetV.Y * frontFactor),
                w, h
            );

            backSourceRect = new Rectangle(
                (int)(OffsetV.X * backFactor),
                (int)(OffsetV.Y * backFactor),
                w, h
            );
        }
    }
}
