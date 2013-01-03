﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IndieSpeedRun
{
    public class Player:MovingEntity
    {

        private Vector2 startPosition;
        private Game1 game;

        private float speed;

        private float heat;
        private const float heatFromJump = 1.5f;
        private const float heatDamper = .02f;
        private const float minVelocity = 150;
        private const float heatForDoubleJump = 20;

        private bool canDoubleJump = false;
        public bool isSliding = false;

        private enum states {RUNNING=0, JUMPING=1};
        private int playerState = 0;
        public int PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

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

            heat = 0;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public void LoadPlayer()
        {
        }

        public override void Update(GameTime gameTime)
        {
            /*
            if (playerState == (int)states.RUNNING)
                Console.WriteLine("Running!");
            else if (playerState == (int)states.JUMPING)
                Console.WriteLine("Jumping!");
            */

   

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration = Vector2.Zero;


            acceleration += new Vector2(0, 10); //gravity
            velocity += acceleration;

            //MOVE with WASD
            speed = 10f;
            float maxSpeed = 250;
            Vector2 dPad = Vector2.Zero;
            if (playerState == (int)states.RUNNING)
            {
                if (Input.KeyDown(Keys.A) && velocity.X > -maxSpeed)
                {
                    dPad.X = -speed;
                }
                else if (Input.KeyDown(Keys.D) && velocity.X < maxSpeed)
                {
                    dPad.X = speed;
                }
                else
                {
                    //REDUCE the speed when not pressed
                    if (velocity.X < -2)
                        dPad.X = 3;
                    else if (velocity.X > 2)
                        dPad.X = -3;
                    else
                    {
                        velocity.X = 0;
                        dPad.X = 0;
                    }
                }
                velocity += dPad;
            }
            else if (playerState == (int)states.JUMPING)
            {
                float airDamper = .8f;

                if (Input.KeyDown(Keys.A) && velocity.X > -maxSpeed)
                    dPad.X = -speed*airDamper;
                else if (Input.KeyDown(Keys.D) && velocity.X < maxSpeed)
                    dPad.X = speed*airDamper;
                else
                {
                    dPad.X = 0;
                }
                velocity += dPad;
            }

            //JUMP code!
            if (Input.KeyPressed(Keys.K))
            {
                if(playerState == (int)states.RUNNING)
                {
                    velocity.Y = -400;
                    playerState = (int)states.JUMPING;
                    Console.WriteLine("START JUMP!");
                    heat += heatFromJump;

                    canDoubleJump = true;
                }
                else if(playerState == (int)states.JUMPING && heat>heatForDoubleJump && canDoubleJump && velocity.Y>0)
                {
                    velocity.Y = -400;
                    Console.WriteLine("DOUBLE JUMP!!!");
                    heat -= heatForDoubleJump;

                    canDoubleJump = false;
                }
            }

            Position += Vector2.Multiply(velocity, dt);

            //increase the heat!
            if (Math.Abs(Velocity.X) > minVelocity)
            {
                heat += Math.Abs(Vector2.Multiply(Vector2.Multiply(velocity, dt), heatDamper).X);
            }

            //clamp heat
            if (heat < 0)
                heat = 0;
            else if (heat > 100)
                heat = 100;//or activate SUPER!

            Console.WriteLine("heat: " + heat);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Sprite.Texture, new Rectangle((int)PositionX, (int)PositionY, 10, 10), Color.Black);

            base.Draw(spriteBatch);
        }


    }
}
