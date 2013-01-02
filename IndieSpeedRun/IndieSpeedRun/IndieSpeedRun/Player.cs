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

        private float speed;

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

            //velocity += acceleration;
            /*if (velocity.Length() >= 25f) //velocity clamping?
            {
                velocity.Normalize();
                Vector2.Multiply(velocity, 25);
            }*/
            //Position += velocity

            acceleration += new Vector2(0, 5); //gravity
            velocity += acceleration;

            /*if (PositionY + Sprite.Height > game.mapHeight)
            {
                PositionY = game.mapHeight - Sprite.Height;
                acceleration = Vector2.Zero;
                velocity.Y = 0;
            }*/

            Vector2 dPad = Vector2.Zero;
            speed = 20f;
            float maxSpeed = 300;
            if (Input.KeyDown(Keys.A) && velocity.X > -maxSpeed)
                dPad.X = -speed;
            else if (Input.KeyDown(Keys.D) && velocity.X < maxSpeed)
                dPad.X = speed;
            else
                dPad.X = 0;
            velocity += dPad;

            if (Input.KeyDown(Keys.W) || Input.KeyDown(Keys.K))
                velocity.Y = -200;

            Position += Vector2.Multiply(velocity, dt);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Sprite.Texture, new Rectangle((int)PositionX, (int)PositionY, 10, 10), Color.Black);

            base.Draw(spriteBatch);
        }


    }
}
