using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IndieSpeedRun
{
    /// <summary>
    /// Sprite.cs
    /// 
    /// Contains all the information for a 2D sprite minus position and rotation.
    /// Use an entity instead to create a sprite with a position and rotation.
    /// 
    /// </summary>
    public class Sprite
    {

        /// <summary>
        /// The texture of the sprite;
        /// </summary>
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// The width of the sprite.
        /// </summary>
        private int width;
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The height of the sprite.
        /// </summary>
        private int height;
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// The scale of the sprite.
        /// </summary>
        private float scale;
        public float Scale
        {
            get { return scale; }
        }

        /// <summary>
        /// The origin of the sprite.
        /// </summary>
        protected Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
        }

        /// <summary>
        /// Creates a sprite with a texture
        /// </summary>
        /// <param name="texture">The texture of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        public Sprite(Texture2D texture) : this(texture, Vector2.Zero)
        {
        }

        public Sprite(Texture2D texture, Vector2 origin)
            : this(texture, Game1.TILE_SIZE, Game1.TILE_SIZE, origin)
        { 
        }

        /// <summary>
        /// Creates a sprite with a texture
        /// </summary>
        /// <param name="texture">The texture of the sprite.</param>
        /// <param name="width">The width of the sprite.</param>
        /// <param name="height">The height of the sprite.</param>
        public Sprite(Texture2D texture, int width, int height, Vector2 origin)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            scale = 1;
            //origin = new Vector2(width / 2, height / 2); //sets the origin to the center of the sprite
            this.origin = origin;
        }

        public Sprite(Texture2D texture, int width, int height) : this(texture, width, height, Vector2.Zero) 
        { 
        }

        /// <summary>
        /// Draw the sprite to the screen at a specific position and rotation.
        /// </summary>
        /// <param name="spriteBatch">The settings used to draw the sprite.</param>
        /// <param name="position">The location of the sprite.</param>
        /// <param name="rotation">The rotation to draw the sprite.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
