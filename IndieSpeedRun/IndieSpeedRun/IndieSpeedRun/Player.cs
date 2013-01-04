using System;
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
        const int HScrollThreshold = 350;
        const int VScrollThreshold = 200;
            
        private Vector2 startPosition;
        private Game1 game;

        private Facing facing = Facing.LEFT;
        private float speed;

        //heat values
        private float heat;
        public float Heat { 
            get {return heat; }
        }
        private const float heatFromJump = 3f;
        private const float heatDamper = .02f;
        private const float heatToDoubleJump = 15;
        private const float heatToPunch = 10;
        private const float heatBuffer = 2;
        private List<Sprite> animationSprites = new List<Sprite>();

        private const float minVelocity = 150;

        private Sprite punchSprite;
        private float punchSeconds = .3f;
        private float punchCounter = 0;

        private bool canDoubleJump = false;
        public bool isSliding = false;

        private enum states {RUNNING=0, JUMPING=1, PUNCHING=2};
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
        private ViewArea viewArea;

        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { velocity = value; }
        }

        public Player(int x, int y, Sprite sprite, Game1 g, ViewArea viewArea)
            :base(x, y, sprite)
        {
            startPosition = new Vector2(x, y);
            Position = new Vector2(x, y);
            game = g;

            heat = 20;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            this.viewArea = viewArea;
            this.punchSprite = new Sprite(g.ConditionalLoadSprite("punch", "sprites/fireball"));
            this.SetupAnimationSprites();
        }

        private void SetupAnimationSprites()
        {
            var tex = game.ConditionalLoadSprite("sprites/ninja-09");
            // TODO
        }

        public override void Update(GameTime gameTime)
        {
            /*
            if (playerState == (int)states.RUNNING)
                Console.WriteLine("Running!");
            else if (playerState == (int)states.JUMPING)
                Console.WriteLine("Jumping!");
            else if (playerState == (int)states.PUNCHING)
                Console.WriteLine("Punching!");
            */

            UpdateAnimation(gameTime);

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
                    sprite.Effects = SpriteEffects.None;
                    facing = Facing.LEFT;
                }
                else if (Input.KeyDown(Keys.D) && velocity.X < maxSpeed)
                {
                    dPad.X = speed;
                    sprite.Effects = SpriteEffects.FlipHorizontally;
                    facing = Facing.RIGHT;
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

                //increase the heat with movement!!
                if (Math.Abs(Velocity.X) > minVelocity)
                {
                    heat += Math.Abs(Vector2.Multiply(Vector2.Multiply(velocity, dt), heatDamper).X);
                }
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

            //PUNCH code
            if (Input.KeyPressed(Keys.J) && !(playerState == (int)states.PUNCHING) && heat >= heatToPunch + heatBuffer)
            {
                Console.WriteLine("punch start");
                playerState = (int)states.PUNCHING;
                heat -= heatToPunch;

                punch();
            }
            if (playerState == (int)states.PUNCHING)
            {
                punchCounter += dt;

                if (punchCounter >= punchSeconds)
                {
                    Console.WriteLine("punch finished");
                    punchCounter = 0;
                    playerState = (int)states.JUMPING;
                }
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
                else if(playerState == (int)states.JUMPING)
                {
                    //while jumping, can only perform a walljump or double jump

                    if (game.currentMap.ContainsCoordinate(PositionX + sprite.Width + 3, PositionY + sprite.Height))
                    {
                        PositionX -= 1;
                        velocity.Y = -400;
                        velocity.X = -300;
                        Console.WriteLine("wallJump Left");
                        heat += heatFromJump;
                        sprite.Effects = SpriteEffects.None;
                        facing = Facing.LEFT;
                    }
                    else if (game.currentMap.ContainsCoordinate(PositionX - 2, PositionY + sprite.Height))
                    {
                        PositionX += 1;
                        velocity.Y = -400;
                        velocity.X = 300;
                        Console.WriteLine("wallJump Right");
                        heat += heatFromJump;
                        sprite.Effects = SpriteEffects.FlipHorizontally;
                        facing = Facing.RIGHT;
                    }
                    else if (heat > heatToDoubleJump+heatBuffer && canDoubleJump && velocity.Y > -20)
                    {
                        
                        velocity.Y = -400;
                        Console.WriteLine("DOUBLE JUMP!!!");
                        heat -= heatToDoubleJump;
                        canDoubleJump = false;
                    }
                }
            }

            Position += Vector2.Multiply(velocity, dt);

            //decrease the heat all the time!
            heat -= .03f;

            heat += game.physics.isHeatingUp(this);

            //clamp heat
            if (heat < 0)
            {
                heat = 0;
                this.Die();
            }
            else if (heat > 100)
            {
                heat = 100;
                this.Die();
            }

            // Console.WriteLine("heat: " + heat);
            recenterView();
            base.Update(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // TODO
        }

        public void Die()
        {
            if (heat == 0)
            {
                //cold death!
                Console.WriteLine("Cold Death");
            }
            else if (heat == 100)
            {
                //overheat death!
                Console.WriteLine("Heat Death");
            }

            Kill();
            game.gameState = Game1.GameState.END;
            //end ze game
        }

        public void punch()
        {
            int thickness = 10;
            Rectangle testRectangle = new Rectangle(0, 0, thickness, thickness);
            if (facing == Facing.RIGHT)
            {
                testRectangle.X = (int)PositionX + sprite.Width + 20;
                testRectangle.Y = (int)PositionY + 20;
            }
            else
            {
                testRectangle.X = (int)PositionX - 20 - thickness;
                testRectangle.Y = (int)PositionY + 20;
            }
        }

        protected void recenterView()
        {
            int offsetX = 0;
            int offsetY = 0;
            /** Deal with X scrolling */

            int diffX = ((int)PositionX) - viewArea.Left;
            int boxRight = game.mapWidth - HScrollThreshold;
            if (diffX > boxRight)
            {
                offsetX = diffX - boxRight;
            }
            else if (diffX < HScrollThreshold)
            {
                offsetX = diffX - HScrollThreshold;
            }

            /* Deal with Y scrolling */
            int diffY = ((int) PositionY) - viewArea.Top;
            int boxBottom = game.mapHeight - VScrollThreshold;
            if (diffY > boxBottom) {
                offsetY = diffY - boxBottom;
            }
            else if (diffY < VScrollThreshold)
            {
                offsetY = diffY - VScrollThreshold;
            }
            /* Now re-center */
            if (offsetX != 0 || offsetY != 0)
            {
                viewArea.Update(viewArea.Left + offsetX, viewArea.Top + offsetY);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (playerState == (int)states.PUNCHING)
            {
                Vector2 merp = Position - offset;
                if (facing == Facing.RIGHT)
                    spriteBatch.Draw(punchSprite.Texture, new Rectangle((int)merp.X + sprite.Width, (int)merp.Y + 20, 20, 20),Color.White);
                else
                    spriteBatch.Draw(punchSprite.Texture, new Rectangle((int)merp.X - 20, (int)merp.Y + 20, 20, 20), Color.White);
            }
            base.Draw(spriteBatch, offset);
        }

    }
}
