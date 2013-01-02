using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IndieSpeedRun
{
    public class Player:Entity
    {

        private Vector2 startPosition;
        private Game1 game;

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { velocity = value; }
        }
        public Player(int x, int y, Sprite sprite, Game1 g)
            :base(x, y, sprite)
        {
            startPosition = new Vector2(x, y);
            Position = new Vector2(x, y);
            game = g;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public void LoadPlayer()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration = Vector2.Zero;

            velocity += acceleration;
            /*if (velocity.Length() >= 25f) //velocity clamping?
            {
                velocity.Normalize();
                Vector2.Multiply(velocity, 25);
            }*/
            Position += velocity;

            base.Update(gameTime);
        }


        public bool checkKickCollision(float x, float y)
        {
            /*
            Rectangle target1 = new Rectangle((int)(CenterX + kick.X*sm), (int)(CenterY + kick.Y*sm), 4, 4);
            Rectangle target2 = new Rectangle((int)(CenterX + kick.X*lg), (int)(CenterY + kick.Y*lg), 4, 4);

            foreach (Block block in game.currentMap.Blocks)
            {
                if (target1.Intersects(block.Rectangle) || target2.Intersects(block.Rectangle))
                {
                    return true;
                }
            }

            return false;*/

            return true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

           
            spriteBatch.Draw(Sprite.Texture, new Rectangle((int)(CenterX)-4, (int)(CenterY)-4, 10, 10), Color.Black);
            
        }


    }
}
