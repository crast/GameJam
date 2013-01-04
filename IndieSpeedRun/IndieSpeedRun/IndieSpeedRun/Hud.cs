using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndieSpeedRun
{
    class Hud
    {

        private Player player;
        private Texture2D thermoFrame;
        private Texture2D thermoFill;

        public Hud(Game1 game, Player player) {
            this.player = player;
            this.thermoFrame = game.ConditionalLoadSprite("thermoframe", "sprites/thermo_frame-01");
            this.thermoFill = game.ConditionalLoadSprite("thermofill", "sprites/thermo_fill-01");

        }

        public void Draw(SpriteBatch batch)
        {
            Rectangle frameBox = new Rectangle(0, 0, 40, 264);
            batch.Draw(thermoFrame, frameBox, frameBox, Color.White);


            int height = (int)(player.Heat * 248 / 100);
            Rectangle fillBox = new Rectangle(8, (248 - height) + 8, 24, height);
            Rectangle fillSourceBox = new Rectangle(0, 248 - height, 24, height);
            batch.Draw(thermoFill, fillBox, fillSourceBox, Color.White);
            
        }

    }
}
