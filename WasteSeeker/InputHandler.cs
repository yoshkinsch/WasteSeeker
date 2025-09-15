using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WasteSeeker
{
    /// <summary>
    /// Handles input from keyboard and mouse states in-game
    /// </summary>
    public class InputHandler
    {
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        /// <summary>
        /// The current direction an object/sprite is facing
        /// </summary>
        public Vector2 Direction { get; private set; }

        /// <summary>
        /// Whether or not the Player requested to exit the game - may be changed to go to main menu
        /// </summary>
        public bool Exit { get; private set; } = false;

        /// <summary>
        /// Used to update the previous and current input from the keyboard/mouse
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            #region Updating Input State
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            #endregion

            #region Mouse Input

            // Get Position from Mouse
            Vector2 _mousePosition = _currentMouseState.Position.ToVector2();

            #endregion

            #region Direction Input

            // Get Position from Keyboard - TODO: change "velocity" of sprites
            if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D))
            {
                Direction += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentKeyboardState.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentKeyboardState.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            #endregion

            #region EXIT

            if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit = true; }

            #endregion
        }
    }
}
