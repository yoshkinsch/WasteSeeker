using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Classes_Assets;

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
        /// Gets the current key that is pressed
        /// </summary>
        public Keys KeyPressed { get; private set; }

        /// <summary>
        /// Whether or not the Player requested to exit the game - may be changed to go to main menu
        /// </summary>
        public bool Exit { get; private set; } = false;

        /// <summary>
        /// Whether or not the player is wanting to run in the game (players holds shift)
        /// </summary>
        public bool Running { get; private set; } = false;

        /// <summary>
        /// Whether or not the player is inputting nothing into the game (player presses nothing)
        /// </summary>
        public bool Idle { get; private set; } = false;

        /// <summary>
        /// Used to update the previous and current input from the keyboard/mouse
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, ref GameState gameState)
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

            #region EXIT

            if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit = true; }

            #endregion

            switch (gameState)
            {
                case GameState.MainMenu:

                    //TESTING FOR PLAYING
                    if (_currentKeyboardState.IsKeyDown(Keys.Space)) { gameState = GameState.Playing; }

                    break;
                case GameState.Playing:

                    #region Direction Input

                    Vector2 direction = Vector2.Zero; // Making a zero vector to help with direction input on keyboard

                    if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A)) { direction.X -= 1; }
                    if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D)) { direction.X += 1; }

                    //Direction = currentGamePadState.ThumbSticks.Right * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Direction = direction;


                    // Get Position from Keyboard - TODO: change "velocity" of sprites
                    if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentKeyboardState.IsKeyDown(Keys.A))
                    {
                        Direction += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    }
                    if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentKeyboardState.IsKeyDown(Keys.D))
                    {
                        Direction += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    }

                    #endregion

                    #region RUNNING IDLE

                    if (_currentKeyboardState.IsKeyDown(Keys.LeftShift)) { Running = true; }
                    else { Running = false; }

                    if (_currentKeyboardState.GetPressedKeys().Length == 0 || (Direction.X == 0 && Direction.Y == 0)) { Idle = true; }
                    else { Idle = false; }

                    #endregion

                    break;
            }
        }
    }
}
