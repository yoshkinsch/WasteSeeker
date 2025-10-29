using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using WasteSeeker.Classes_Assets;

namespace WasteSeeker
{
    /// <summary>
    /// The Waste Seeker game - Description will be added later in development
    /// </summary>
    public class WasteSeekerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //private GameState _previousGameState;
        private GameState _gameState = GameState.MainMenu;
        private InputHandler _inputHandler;

        private Dictionary<GameState, Song> _songs;

        private BattleSequence _battleSequence;

        #region Menu Objects

        // Music
        private Song _backgroundMenuMusic;

        // Sprite Fonts
        private SpriteFont _sedgwickAveDisplay;

        // Texture2D Assets
        private Texture2D _mainMenuEyes;

        private TitleBulletSprite _bulletIcon;

        private TitleGearSprite _leftGearSprite;
        
        private TitleGearSprite _rightGearSprite;

        // Buttons
        #region Buttons
        private Button _playButton;
        private Button _optionsButton;
        private Button _exitButton;
        #endregion

        #endregion

        #region Option Objects

        OptionsMenu _optionsMenu;

        #endregion

        #region Playing Objects

        #region Objects
        private Texture2D _worldGroundTexture;
        private Texture2D _worldBackGroundTexture;
        private Texture2D _worldFirstMidGroundTexture;
        private Texture2D _worldSecondMidGroundTexture;
        private Texture2D _worldForeGroundTexture;

        private Rectangle _worldGround;

        private SandParticleSystem _sandParticleSystem;

        private Tumbleweed _tumbleweed;
        #endregion

        #region Characters
        private Player _player;
        private NPC _soraNPC;
        #endregion

        #region Music
        private Song _backgroundPlayingMusic;
        #endregion

        #endregion

        #region BattleSequence Objects

        private BattleSequence _battleSequenceHandler;

        #endregion

        // Want to add a global scalar value used in the game code - Dependent on the graphics screen size of the player (will be later implemented)
        // Default Resolution => 1280 x 720

        /// <summary>
        /// Constructs the game
        /// </summary>
        public WasteSeekerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            #region Main Menu

            _leftGearSprite = new TitleGearSprite() { Position = new Vector2(270, 100), RotationDirection = 1, GearDirection = 0 };
            _rightGearSprite = new TitleGearSprite() { Position = new Vector2(1020, 100), RotationDirection = -1, GearDirection = (TitleGearSprite.Direction)1 };
            _bulletIcon = new TitleBulletSprite() { Graphics = _graphics };
            _playButton = new Button(new Vector2(640,200), 200) { GameStateLocation = GameState.MainMenu };
            _optionsButton = new Button(new Vector2(_playButton.Position.X, _playButton.Position.Y + 75), 175) { GameStateLocation = GameState.MainMenu };
            _exitButton = new Button(new Vector2(_optionsButton.Position.X, _optionsButton.Position.Y + 75), 160) { GameStateLocation = GameState.MainMenu };

            #endregion

            #region Options Menu

            _optionsMenu = new OptionsMenu();
            _optionsMenu.Initialize();

            #endregion

            // Here will initial and create the input handler
            _inputHandler = new InputHandler();
            
            #region Playing

            #region Objects
            _worldGround = new Rectangle(0, 540, 14000, GraphicsDevice.Viewport.Height - 540);
            _tumbleweed = new Tumbleweed(new Vector2(GraphicsDevice.Viewport.Width + 500, 540));
            
            #endregion 

            #region Characters
            _player = new Player("Kuzu", "TODO", 100, new Vector2(100, 480), _inputHandler);
            _soraNPC = new NPC("Sora", "TODO", 100, new Vector2(((GraphicsDevice.Viewport.Width/2) + 250), 470), false);
            #endregion

            #endregion

            _battleSequence = new BattleSequence();
            

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Sprite Fonts
            _sedgwickAveDisplay = Content.Load<SpriteFont>("sedgwickAveDisplay");

            // Main Menu Texture2D Assets
            #region Main Menu
            _mainMenuEyes = Content.Load<Texture2D>("Eyes_WasteSeeker_MainMenu");
            _bulletIcon.LoadContent(Content);
            _leftGearSprite.LoadContent(Content);
            _rightGearSprite.LoadContent(Content);
            _playButton.LoadContent(Content, "PlayButton_MainMenu-Sheet");
            _optionsButton.LoadContent(Content, "OptionsButton_MainMenu-Sheet");
            _exitButton.LoadContent(Content, "ExitButton_MainMenu-Sheet");
            #endregion

            #region Options Menu
            _optionsMenu.LoadContent(Content);
            #endregion

            #region Characters/Objects
            _player.LoadContent(Content);
            _soraNPC.LoadContent(Content, "Sora_Idle_Walk");
            _worldGroundTexture = Content.Load<Texture2D>("SandGround");
            _worldBackGroundTexture = Content.Load <Texture2D>("SandBackground_BackGround");
            _worldFirstMidGroundTexture = Content.Load<Texture2D>("SandBackground_FirstMidGround");
            _worldSecondMidGroundTexture = Content.Load<Texture2D>("SandBackground_SecondMidGround");
            _worldForeGroundTexture = Content.Load<Texture2D>("SandBackground_ForegroundGround");
            _tumbleweed.LoadContent(Content);
            #endregion

            #region Music
            _backgroundMenuMusic = Content.Load<Song>("MainMenuMusic");
            _backgroundPlayingMusic = Content.Load<Song>("TutorialMusic");
            
            // Loading all songs into a dictionary to determine what to play
            // Will change "playing" music later in terms of the screen (or level) being played
            _songs = new Dictionary<GameState, Song>()
            {
                {GameState.MainMenu,  _backgroundMenuMusic},
                {GameState.Options,  _backgroundMenuMusic},
                {GameState.Playing, _backgroundPlayingMusic }
            };
            #endregion

            #region Buttons
            // Using this section to send a list of the buttons loaded into the game to the input handler
            List<Button> mainMenuButtons = new List<Button>()
            {
                _playButton,
                _optionsButton,
                _exitButton
            };

            List<Button> optionsMenuButtons = new List<Button>()
            {
                _optionsMenu.BackButton,
                _optionsMenu.ExitButton
            };

            _inputHandler.LoadButtons(GameState.MainMenu, mainMenuButtons);
            _inputHandler.LoadButtons(GameState.Options, optionsMenuButtons);
            #endregion

            _battleSequence.LoadContent(Content);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            /*
             * Game State is updated here if certain input is entered
             * Input-Handler will handle which input is sent and will communicate that back here - can determine the game state
            */
            var previousSongPlayed = _songs[_gameState];
            var _previousGameState = _gameState;
            _inputHandler.Update(gameTime, ref _gameState);

            if (_inputHandler.Exit == true) { Exit(); }

            // Checking if the options menu has popped open
            // opened => play noise in higher pitch
            // closed => play noise in lower pitch
            if (_previousGameState != GameState.Options && _gameState == GameState.Options)
            {
                _optionsMenu.PlayNoise(true);
            }
            
            if (_previousGameState == GameState.Options && _gameState != GameState.Options)
            {
                _optionsMenu.GameWasPaused = false;
                _optionsMenu.PlayNoise(false);
            }

            // this if statement allows the options menu to attach itself to whatever background music is being played
            if (previousSongPlayed != null && _gameState == GameState.Options) { _songs[_gameState] = previousSongPlayed; }
            Song songToPlay = _songs[_gameState];
            
            if (MediaPlayer.Queue.ActiveSong != songToPlay)
            {
                MediaPlayer.Play(songToPlay);
                MediaPlayer.IsRepeating = true;
            }

            // Sand Particle System - "turn off switch"
            if (_sandParticleSystem != null && _gameState != GameState.Playing)
            {
                _sandParticleSystem.Enabled = false;
                _sandParticleSystem.Visible = false;
            }

            switch (_gameState)
            {
                case GameState.MainMenu:
                    _leftGearSprite.Update(gameTime);
                    _rightGearSprite.Update(gameTime);
                    _playButton.Update(gameTime);
                    _optionsButton.Update(gameTime);
                    _exitButton.Update(gameTime);
                    break;
                case GameState.Options:
                    if (_previousGameState == GameState.Playing)
                    {
                        _optionsMenu.GameWasPaused = true;
                    }
                    _optionsMenu.Update(gameTime);
                    break;
                case GameState.Playing:

                    if (_sandParticleSystem == null)
                    {
                        _sandParticleSystem = new SandParticleSystem(this, new Rectangle(GraphicsDevice.Viewport.Width + 20, 0, 10, GraphicsDevice.Viewport.Height - 100));
                        Components.Add(_sandParticleSystem);
                    }

                    if (_sandParticleSystem.Enabled == false || _sandParticleSystem.Visible == false)
                    {
                        _sandParticleSystem.Enabled = true;
                        _sandParticleSystem.Visible = true;
                    }

                    _player.Update(gameTime);
                    _soraNPC.TargetPlayer(_player.Position);
                    _soraNPC.Update(gameTime, _player.Position);

                    if (_player.Position.X >= _soraNPC.Position.X && !_soraNPC.IsFollowingPlayer)
                    {
                        _soraNPC.IsFollowingPlayer = true;
                    }

                    // TumbleWeed
                    _tumbleweed.Update(gameTime);
                    if (_tumbleweed.Position.X < _player.Position.X - 500)
                    {
                        _tumbleweed.UpdateScale();
                        _tumbleweed.Position = new Vector2(_player.Position.X + GraphicsDevice.Viewport.Width + 200, 540);
                    }
                    // Player bounds on screen
                    if (_player.Position.X <= -10) { _player.Position = new Vector2(GraphicsDevice.Viewport.X - 10, _player.Position.Y); }
                    else if (_player.Position.X > 14000) { _player.Position = new Vector2(14000, _player.Position.Y); }
                    
                    break;
                case GameState.BattleSequence:



                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            // TODO: Add your drawing code here
            switch (_gameState)
            {
                case GameState.MainMenu:
                    _spriteBatch.Begin();
                    
                    // Texture2D Assets Main Menu
                    _spriteBatch.Draw(_mainMenuEyes, new Vector2(0, 0), null, Color.White);
                    _bulletIcon.Draw(_spriteBatch, gameTime, new Vector2(GraphicsDevice.Viewport.Width / 2 - 20, 90), new Vector2(256, 256), 0.125f);
                    _leftGearSprite.Draw(_spriteBatch, gameTime);
                    _rightGearSprite.Draw(_spriteBatch, gameTime);
                    _playButton.Draw(_spriteBatch, gameTime);
                    _optionsButton.Draw(_spriteBatch, gameTime);
                    _exitButton.Draw(_spriteBatch, gameTime);

                    // Sprite Fonts
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 100), Color.White, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    //_spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Space' to Play!", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 280), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Space' to Play!") / 2, 0.35f, SpriteEffects.None, 1);

                    _spriteBatch.End();
                    break;
                case GameState.Options:
                    // Options will contain volume, display options, langauge, and potentially more
                    _optionsMenu.Draw(_spriteBatch, gameTime);
                    break;
                case GameState.Playing:

                    GraphicsDevice.Clear(Color.NavajoWhite);

                    // Calculate our offset vector 
                    float playerX = MathHelper.Clamp(_player.Position.X, 500, 14000);
                    float offsetX = 500 - playerX;

                    Matrix tranform;

                    //Background Textures
                    tranform = Matrix.CreateTranslation(offsetX * 0.100f, 0, 0);
                    _spriteBatch.Begin(transformMatrix: tranform);
                    _spriteBatch.Draw(_worldBackGroundTexture, Vector2.Zero, Color.White);
                    _spriteBatch.End();

                    tranform = Matrix.CreateTranslation(offsetX * 0.150f, 0, 0);
                    _spriteBatch.Begin(transformMatrix: tranform);
                    _spriteBatch.Draw(_worldFirstMidGroundTexture, Vector2.Zero, Color.White);
                    _spriteBatch.End();

                    tranform = Matrix.CreateTranslation(offsetX * 0.250f, 0, 0);
                    _spriteBatch.Begin(transformMatrix: tranform);
                    _spriteBatch.Draw(_worldSecondMidGroundTexture, Vector2.Zero, Color.White);
                    _spriteBatch.End();

                    tranform = Matrix.CreateTranslation(offsetX * 0.600f, 0, 0);
                    _spriteBatch.Begin(transformMatrix: tranform);
                    _spriteBatch.Draw(_worldForeGroundTexture, Vector2.Zero, Color.White);
                    _spriteBatch.Draw(_worldGroundTexture, _worldGround, Color.White);
                    _spriteBatch.End();

                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Backspace' on the keyboard, to return to the Menu.", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 700), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Backspace' on the keyboard, to return to the Menu.") / 2, 0.35f, SpriteEffects.None, 1);
                    

                    // Tutorial
                    if (_player.Position.X <= 1280)
                    {
                        _spriteBatch.DrawString(_sedgwickAveDisplay, "Press A, D, Left Arrow, or Right Arrow\nto move around!", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Press A, D, Left Arrow, or Right Arrow\nto move around!") / 2, (float)0.5, SpriteEffects.None, 1);
                    }
                    else if (_player.Position.X <= 1600)
                    {
                        _spriteBatch.DrawString(_sedgwickAveDisplay, "Certain NPCs may follow you\n after crossing paths with them!", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Certain NPCs may follow you\n after crossing paths with them!") / 2, (float)0.5, SpriteEffects.None, 1);
                    }
                    else 
                    {
                        _spriteBatch.DrawString(_sedgwickAveDisplay, "Gameplay coming soon!", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Gameplay coming soon!") / 2, (float)0.5, SpriteEffects.None, 1);
                    }
                    _spriteBatch.End();
                    tranform = Matrix.CreateTranslation(offsetX, 0, 0);
                    _spriteBatch.Begin(transformMatrix: tranform);
                    _soraNPC.Draw(_spriteBatch, gameTime);
                    _player.Draw(_spriteBatch, gameTime);
                    _tumbleweed.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                case GameState.BattleSequence:
                    _spriteBatch.Begin();
                    _battleSequence.Draw(_spriteBatch, gameTime);
                    _spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
