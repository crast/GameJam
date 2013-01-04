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
        private Rectangle screenBox;
        private Texture2D thermoFrame;
        private Texture2D thermoFill;
        private Texture2D warningCool;
        private Texture2D warningHot;

        public Hud(Game1 game, Player player) {
            this.screenBox = new Rectangle(0, 0, game.mapWidth, game.mapHeight);
            this.player = player;
            this.thermoFrame = game.ConditionalLoadSprite("thermoframe", "sprites/thermo_frame-01");
            this.thermoFill = game.ConditionalLoadSprite("thermofill", "sprites/thermo_fill-01");
            this.warningCool = game.ConditionalLoadSprite("tiles/warning_cool-01");
            this.warningHot = game.ConditionalLoadSprite("tiles/warning_heat-01");
        }

        public void Draw(SpriteBatch batch)
        {
            // Cool warning frame
            if (player.Heat < 10)
            {
                var alpha = (10 - player.Heat) * 100f / 10.0f;
                Color coldColor = new Color(255, 255, 255, alpha);
                Console.WriteLine("Alpha: {0}", alpha);
                batch.Draw(warningCool, screenBox, coldColor);
            }
            
            // Hot warning frame
            if (player.Heat > 90)
                batch.Draw(warningHot, screenBox, Color.White);
            

            // Thermo bar frame
            Rectangle frameBox = new Rectangle(0, 0, 40, 264);
            batch.Draw(thermoFrame, frameBox, frameBox, Color.White);

            // thermo bar
            int height = (int)(player.Heat * 248 / 100);
            Rectangle fillBox = new Rectangle(8, (248 - height) + 8, 24, height);
            Rectangle fillSourceBox = new Rectangle(0, 248 - height, 24, height);
            batch.Draw(thermoFill, fillBox, fillSourceBox, Color.White);
            
        }

    }
}
