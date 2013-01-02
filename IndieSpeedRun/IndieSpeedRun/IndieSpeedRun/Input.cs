using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IndieSpeedRun
{
    /// <summary>
    /// Handles all player input (mouse, keyboard, and gamepad)
    /// </summary>
    public static class Input
    {
        private const float TILT_THRESHOLD = .1f;
        private const float TRIGGER_THRESHOLD = .5f;

        /// <summary>
        /// The current state of the keyboard buttons.
        /// </summary>
        private static KeyboardState keyboard;

        /// <summary>
        /// The current state of the mouse buttons and position.
        /// </summary>
        private static MouseState mouse;

        /// <summary>
        /// The last state of the mouse buttons and position.
        /// </summary>
        private static MouseState previousMouse;

        /// <summary>
        /// The current state of the gamepad
        /// </summary>
        public static GamePadState gamePad;

        private static GamePadState previousGamePad;
        


        /// <summary>
        /// Initialize the input state.
        /// </summary>
        public static void Initialize() {
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            mouse = Mouse.GetState();
            previousMouse = mouse;
            gamePad = GamePad.GetState(PlayerIndex.One);
            previousGamePad = gamePad;
        }

        /// <summary>
        /// Update the mouse and keyboard states.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public static void Update(GameTime gameTime) {
            previousMouse = mouse;
            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();

            previousGamePad = gamePad;
            gamePad = GamePad.GetState(PlayerIndex.One);
        }


        //******KEYBOARD INPUT******//
        /// <summary>
        /// Checks if a key is down.
        /// </summary>
        public static bool KeyDown(Keys key) {
            return keyboard.IsKeyDown(key);
        }


        //******MOUSE INPUT******//
        /// <summary>
        /// Returns the position of the mouse cursor.
        /// </summary>
        public static Vector2 MousePosition() {
            return new Vector2(mouse.X, mouse.Y);
        }

        /// <summary>
        /// Determine if the left mouse button was just pressed.
        /// </summary>
        public static bool LeftMouseClick() {
            return previousMouse.LeftButton == ButtonState.Pressed &&
                mouse.LeftButton == ButtonState.Released; 
        }

        /// <summary>
        /// Determine if the right mouse button was just pressed.
        /// </summary>
        public static bool RightMouseClick() {
            return previousMouse.RightButton == ButtonState.Pressed &&
                mouse.RightButton == ButtonState.Released;
        }

        //******GAME PAD INPUT******//
        /// <summary>
        /// Determine if a gamepad button is pressed
        /// </summary>
        public static bool BackButton()
        {
            return gamePad.Buttons.Back == ButtonState.Pressed;
        }

        public static bool rightBumper()
        {
            return gamePad.Buttons.RightShoulder == ButtonState.Pressed && previousGamePad.Buttons.RightShoulder == ButtonState.Released;
        }

        public static bool leftBumper()
        {
            return gamePad.Buttons.LeftShoulder == ButtonState.Pressed && previousGamePad.Buttons.LeftShoulder == ButtonState.Released;
        }

        /// <summary>
        /// Returns X coordinate of the left analog stick
        /// </summary>
        public static float leftX()
        {
            return gamePad.ThumbSticks.Left.X;
        }

        /// <summary>
        /// Returns the negative Y coordinate of the left analogic stick
        /// </summary>
        public static float leftY()
        {
            return -gamePad.ThumbSticks.Left.Y;
        }

        /// <summary>
        /// Returns the X coordiante of the right analog stick
        /// </summary>
        public static float rightX()
        {
            return gamePad.ThumbSticks.Right.X;
        }
        
        /// <summary>
        /// Returns the -Y coordinate of the right analog stick
        /// </summary>
        public static float rightY()
        {
            return -gamePad.ThumbSticks.Right.Y;
        }

        public static bool leftTilted()
        {
            return Math.Abs(leftX()) > TILT_THRESHOLD || Math.Abs(leftY()) > TILT_THRESHOLD;
        }
        public static bool rightTilted()
        {
            return Math.Abs(rightX()) > TILT_THRESHOLD || Math.Abs(rightY()) > TILT_THRESHOLD;
        }

        public static bool leftTriggerDown()
        {
            return Input.gamePad.Triggers.Left > TRIGGER_THRESHOLD;
        }
        public static bool rightTriggerDown()
        {
            return Input.gamePad.Triggers.Right > TRIGGER_THRESHOLD;
        }

        

    }
}
