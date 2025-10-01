using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Classes_Assets;
using WasteSeeker.Collisions;

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

        private BoundingRectangle _playButtonBounds;
        private BoundingRectangle _optionsButtonBounds;
        private BoundingRectangle _exitButtonBounds;

        private GameState _previousGameState;

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
        /// Input Handler initialization
        /// </summary>
        public InputHandler()
        {
            // If needed can fill in
        }

        public void InitializeMenuButtons(BoundingRectangle play, BoundingRectangle options, BoundingRectangle exit)
        {
            _playButtonBounds = play;
            _optionsButtonBounds = options;
            _exitButtonBounds = exit;
        }

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

            switch (gameState)
            {
                case GameState.MainMenu:

                    #region Buttons
                    // play button check
                    if (_playButtonBounds.CollidesWith(new Vector2(_currentMouseState.X, _currentMouseState.Y)))
                    {
                        if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            gameState = GameState.Playing;
                        }
                    }

                    // options button check
                    if (_optionsButtonBounds.CollidesWith(new Vector2(_currentMouseState.X, _currentMouseState.Y)))
                    {
                        if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            gameState = GameState.Options;
                        }
                    }

                    // exit button check
                    if (_exitButtonBounds.CollidesWith(new Vector2(_currentMouseState.X, _currentMouseState.Y)))
                    {
                        if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            Exit = true;
                        }
                    }
                    #endregion

                    break;
                case GameState.Options:

                    //TODO: Change Y to mouse state - and make it so the previous keyboard is not down (so gameplay doesn't go to menu)
                    if (Keyboard.GetState().IsKeyDown(Keys.Y)) { gameState = _previousGameState; }

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

                    #region TO MAIN MENU

                    if (Keyboard.GetState().IsKeyDown(Keys.Back)) { gameState = GameState.MainMenu; }

                    #endregion

                    #region TO OPTIONS

                    if (Keyboard.GetState().IsKeyDown(Keys.T)) { _previousGameState = GameState.Playing; gameState = GameState.Options; }
                    #endregion

                    break;
            }
        }
    }
}
