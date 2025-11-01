using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        #region Buttons
        private int _selectedButtonIndex = 0;
        private Dictionary<GameState, List<Button>> _buttonsDict = new Dictionary<GameState, List<Button>>();
        #endregion

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
        /// This is used whenever the player will want to save the game (in the options screen)
        /// </summary>
        public bool Save { get; set; } = false;

        /// <summary>
        /// This is used whenever the player will want to load the game (in the main menu screen)
        /// </summary>
        public bool Load { get; set; } = false;

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

        /// <summary>
        /// Method to load in the buttons used in the game.
        /// </summary>
        /// <param name="buttons"></param>
        public void LoadButtons(GameState gameState, List<Button> buttons)
        {
            _buttonsDict[gameState] = buttons;
            if (gameState == GameState.MainMenu) { _buttonsDict[GameState.MainMenu][0].ButtonSelect = true; }
            if (gameState == GameState.Options) { _buttonsDict[GameState.Options][0].ButtonSelect = true; }
        }

        private GameState? HandleButtonClick(Button button, GameState currentGameState)
        {
            #region MainMenu
            if (currentGameState == GameState.MainMenu)
            {
                if (button.GameStateLocation == GameState.MainMenu && button == _buttonsDict[currentGameState][0]) // Play Button
                {
                    return GameState.Playing;
                }
                if (button.GameStateLocation == GameState.MainMenu && button == _buttonsDict[currentGameState][1]) // Options Button
                {
                    _previousGameState = currentGameState;
                    return GameState.Options;
                }
                if (button.GameStateLocation == GameState.MainMenu && button == _buttonsDict[currentGameState][2]) // Exit Button
                {
                    Load = true;
                    return GameState.MainMenu;
                }
                if (button.GameStateLocation == GameState.MainMenu && button == _buttonsDict[currentGameState][3]) // Exit Button
                {
                    Exit = true;
                    return null;
                }
            }
            #endregion

            #region Options
            if (currentGameState == GameState.Options)
            {
                if (button.GameStateLocation == GameState.Options && button == _buttonsDict[currentGameState][0]) // Back Button
                {
                    return _previousGameState;
                }
                if (button.GameStateLocation == GameState.Options && button == _buttonsDict[currentGameState][1]) // Exit Button
                {
                    return GameState.MainMenu;
                }
                if (button.GameStateLocation == GameState.Options && button == _buttonsDict[currentGameState][2]) // Save Button
                {
                    Save = true;
                    return GameState.Options;
                }
            }
            #endregion

            return 0;
        }

        /// <summary>
        /// Private method to handle button select going up
        /// </summary>
        /// <param name="buttons">List of buttons</param>
        private void MoveSelectionUpwards(List<Button> buttons)
        {
            _selectedButtonIndex = buttons.FindIndex(button => button.ButtonSelect);
            if (_selectedButtonIndex == -1) _selectedButtonIndex = 0;

            if (_selectedButtonIndex > 0)
            {
                buttons[_selectedButtonIndex].ButtonSelect = false; // Deselecting button
                buttons[_selectedButtonIndex - 1].ButtonSelect = true; // Selecting new button
            }
            else
            {
                buttons[_selectedButtonIndex].ButtonSelect = false; // Deselecting button
                buttons[buttons.Count - 1].ButtonSelect = true; // Loop back to bottom side of list of buttons
            }
        }

        /// <summary>
        /// Private method to handle button select going down
        /// </summary>
        /// <param name="buttons">List of buttons</param>
        private void MoveSelectionDownwards(List<Button> buttons)
        {
            _selectedButtonIndex = buttons.FindIndex(button => button.ButtonSelect);
            if (_selectedButtonIndex == -1) _selectedButtonIndex = 0;

            if (_selectedButtonIndex < buttons.Count - 1)
            {
                buttons[_selectedButtonIndex].ButtonSelect = false; // Deselecting button
                buttons[_selectedButtonIndex + 1].ButtonSelect = true; // Selecting new button
            }
            else
            {
                buttons[_selectedButtonIndex].ButtonSelect = false; // Deselecting button
                buttons[0].ButtonSelect = true; // Loop back to top of list of buttons
            }
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
                    if (_buttonsDict.ContainsKey(GameState.MainMenu))
                    {
                        // Handling keybaord selection on Main Menu
                        if (_currentKeyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up) || _currentKeyboardState.IsKeyDown(Keys.W) && !_previousKeyboardState.IsKeyDown(Keys.W))
                        {
                            MoveSelectionUpwards(_buttonsDict[GameState.MainMenu]);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down) || _currentKeyboardState.IsKeyDown(Keys.S) && !_previousKeyboardState.IsKeyDown(Keys.S))
                        {
                            MoveSelectionDownwards(_buttonsDict[GameState.MainMenu]);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter) || _currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
                        {
                            var selectedButton = _buttonsDict[GameState.MainMenu].FirstOrDefault(b => b.ButtonSelect);
                            if (selectedButton != null)
                            {
                                GameState? changedGameState = HandleButtonClick(selectedButton, GameState.MainMenu);
                                if (changedGameState != null)
                                {
                                    gameState = (GameState)changedGameState;
                                }
                            }
                        }

                        // Handing mouse selection on Main Menu
                        foreach (var button in _buttonsDict[GameState.MainMenu])
                        {
                            // Checks the main menu buttons
                            if (button.GameStateLocation == GameState.MainMenu && button.Bounds.CollidesWith(new Vector2(_currentMouseState.X, _currentMouseState.Y)))
                            {
                                if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
                                {
                                    GameState? changedGameState = HandleButtonClick(button, gameState);
                                    if (changedGameState != null) { gameState = (GameState)changedGameState; }
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case GameState.Options:

                    if (_buttonsDict[GameState.Options][1].ButtonActivated)
                    {
                        // Handling keybaord selection on Main Menu
                        if (_currentKeyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up)
                            || _currentKeyboardState.IsKeyDown(Keys.W) && !_previousKeyboardState.IsKeyDown(Keys.W))
                        {
                            MoveSelectionUpwards(_buttonsDict[GameState.Options]);
                        }

                        if (_currentKeyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down)
                            || _currentKeyboardState.IsKeyDown(Keys.S) && !_previousKeyboardState.IsKeyDown(Keys.S))
                        {
                            MoveSelectionDownwards(_buttonsDict[GameState.Options]);
                        }
                    }
                    else
                    {
                        _buttonsDict[GameState.Options][0].ButtonSelect = true;
                    }

                    // Button iteration for options buttons - Mouse Iterations
                    if (_buttonsDict.ContainsKey(GameState.Options))
                    {
                        foreach (Button button in _buttonsDict[GameState.Options])
                        {
                            // Checks the Options buttons
                            if (button.GameStateLocation == GameState.Options && button.Bounds.CollidesWith(new Vector2(_currentMouseState.X, _currentMouseState.Y)))
                            {
                                if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
                                {
                                    GameState? changedGameState = HandleButtonClick(button, gameState);
                                    if (changedGameState != null)
                                    {
                                        gameState = (GameState)changedGameState;
                                    }
                                }
                            }
                        }
                    }

                    if (_currentKeyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter) || _currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        var selectedButton = _buttonsDict[GameState.Options].FirstOrDefault(b => b.ButtonSelect);
                        if (selectedButton != null)
                        {
                            GameState? changedGameState = HandleButtonClick(selectedButton, GameState.Options);
                            if (changedGameState != null)
                            {
                                gameState = (GameState)changedGameState;
                            }
                        }
                    }

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

                    // Temporary for testing and showing off tilemap
                    #region TO BATTLESEQUENCE
                    if (Keyboard.GetState().IsKeyDown(Keys.T)) { _previousGameState = GameState.Playing; gameState = GameState.BattleSequence; }
                    #endregion

                    #region TO MAIN MENU

                    if (Keyboard.GetState().IsKeyDown(Keys.Back)) { gameState = GameState.MainMenu; }

                    #endregion

                    #region TO OPTIONS

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { _previousGameState = GameState.Playing; gameState = GameState.Options; }
                    #endregion

                    break;

                case GameState.BattleSequence:
                    // Handles input on the "BattleSequence" Screen

                    if (Keyboard.GetState().IsKeyDown(Keys.F)) { _previousGameState = GameState.BattleSequence; gameState = GameState.Playing; }

                    #region TO OPTIONS
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { _previousGameState = GameState.BattleSequence; gameState = GameState.Options; }
                    #endregion
                    break;
            }
        }
    }
}
