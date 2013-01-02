﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IndieSpeedRun
{
    /// <summary>
    /// The physics engine manages the movement, collision, and interaction of objects within a map
    /// </summary>
    public class PhysicsEngine
    {
        //current map, which holds all the available objects for collision
        private Map currentMap;

        //PHYSICS varaibles, not implemented in this build
        //public float gravity = 5f;
        //public float friction = 6f;

        public PhysicsEngine(Map _currentMap)
        {
            currentMap = _currentMap;
        }

        /// <summary>
        /// Checks collisions against the player object
        /// </summary>
        public bool CheckCollisions(Player p)
        {
            bool collided = false;
            List<Rectangle> test = new List<Rectangle>(); //list of colliding rectangles

            //test against BLOCKS in the map
            foreach (Block block in currentMap.Blocks)
            {
                Rectangle i = block.Rectangle;
                if (p.Rectangle.Intersects(i))
                {
                    test.Add(i); //add this block's Rectangle to the list
                    collided = true;
                }
            }

            //TODO: Handle special corner cases (to resolve current collision issues)//

            //handle different number of block collisions differently
            if (collided)
            {
                if (test.Count == 1)
                    FixCollisions((Rectangle)test[0], p);
                if (test.Count == 2)
                {
                    FixCollisions(combine(test[0], test[1]), p); //COMBINES into one block!!
                }
                if (test.Count == 3)
                {
                    //with three, separate collisions
                    FixCollisions((Rectangle)test[0], p);
                    FixCollisions((Rectangle)test[1], p);
                    FixCollisions((Rectangle)test[2], p);
                }
            }

            //Console.WriteLine("Colliding = " + collided);

            return collided;
        }

        /// <summary>
        /// Resolves player collisions with rectangles (using a minimum distance formula)
        /// </summary>
        public void FixCollisions(Rectangle rect, Player p)
        {
            Vector2 result = Vector2.Zero;

            
            float dx = 0.0f;
            float minDist = 0.0f; //min dist to move to not be colliding
            

            dx = rect.Right - p.PositionX; // check right side of block
            minDist = dx;
            int axis = 0; //axis at which it has to move
            int side = 1; //direction it has to move

            dx = p.PositionX + p.Sprite.Width - rect.Left; //check left side of block
            if (dx < minDist)
            {
                minDist = dx;
                axis = 0;
                side = -1;
            }

            dx = rect.Bottom - p.PositionY; // check bottom
            if (dx < minDist)
            {
                minDist = dx;
                axis = 1;
                side = 1;
            }

            dx = p.PositionY + p.Sprite.Height - rect.Top; // check Top
            if (dx < minDist)
            {
                minDist = dx;
                axis = 1;
                side = -1;
            }

            //axis, side, and minDist give us enough information to resolve the collision
            if (axis == 0) 
            {
                

                if (side == 1) //right
                {
                    Console.WriteLine("right");
                    p.PositionX += minDist;
                }
                else //left
                {
                    Console.WriteLine("left");
                    p.PositionX -= minDist-1;
                }

                p.Velocity *= new Vector2(0, 1); //stop all velocity in the X direction
            }
            else
            {
                

                if (side == 1) //bottom
                {
                    Console.WriteLine("bottom");
                    p.PositionY += minDist;
                }
                else //top
                {
                    Console.WriteLine("top");
                    p.PositionY -= minDist-1;
                }

                p.Velocity *= new Vector2(1,0); //stop all velocity in the Y direction
            }
        }

        /// <summary>
        /// Combine two adjacent rectangles into a single larger rectangle (assumes adjacency)
        /// </summary>
        public Rectangle combine(Rectangle a, Rectangle b)
        {
            int x = Math.Min(a.Left,b.Left);
            int y = Math.Min(a.Top,b.Top);
            int width = a.Width;
            int height = a.Height;

            if(a.Left != b.Left)
                width *=2;
            if(a.Top != b.Top)
                height *=2;

            return new Rectangle(x, y, width, height);
        }
    }
}