using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndieSpeedRun
{
    /// <summary>
    /// An entity is basically an object that is displayed onscreen with a position and rotation.
    /// Entities start out alive, but they can be killed so they are no longer displayed.
    /// 
    /// Authors: Anthony Reese and Robert Yates
    /// </summary>
    public class Entity
    {

        /// <summary>
        /// The image representing this entity.
        /// </summary>
        protected Sprite sprite;
        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// The location of the entity.
        /// </summary>
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        //The center location of the entity
        public float CenterX
        {
            get { return position.X + sprite.Width / 2; }
            //get { return position.X; }
        }
        public float CenterY
        {
            get { return PositionY + sprite.Height / 2; }
            //get { return position.Y; }
        }

        /// <summary>
        /// The rotation of the entity.
        /// </summary>
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Returns the rectangle with the bounding box of the entity.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)position.X, (int)position.Y,
                    (int)sprite.Width, (int)sprite.Height);
            }
        }

        /// <summary>
        /// True, if the entity is alive. False, if it is dead.
        /// (All entities start out being alive.)
        /// </summary>
        private bool alive;
        public bool Alive
        {
            get { return alive; }
        }

        /// <summary>
        /// Creates an entity with a sprite and position
        /// </summary>
        /// <param name="x">The x coordinate of the entity's position.</param>
        /// <param name="y">The y coordinate of the entity's position.</param>
        /// <param name="sprite">The sprite to display the entity.</param>
        public Entity(int x, int y, Sprite sprite)
        {
            position = new Vector2(x, y);
            this.sprite = sprite;
            alive = true;
        }

        /// <summary>
        /// Updates the entity over time.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the entity if it is alive.
        /// </summary>
        /// <param name="spriteBatch">The settings used to draw the entity's sprite.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                sprite.Draw(spriteBatch, Position, Rotation);
            }
        }

        /// <summary>
        /// Makes it so the entity is no longer alive.
        /// </summary>
        public void Kill()
        {
            alive = false;
        }

        /// <summary>
        /// Makes it so the entity is alive again and
        /// in a new location.
        /// </summary>
        /// <param name="x">The x coordinate of the entity's new position.</param>
        /// <param name="y">The y coordinate of the entity's new position.</param>
        public void Respawn(int x, int y)
        {
            position.X = x;
            position.Y = y;
            alive = true;
        }
    }
}
