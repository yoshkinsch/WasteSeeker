using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WasteSeeker.Classes_Assets;
using WasteSeeker.Collisions;
using WasteSeeker.Data;

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
        private LevelState _levelState = LevelState.LevelOne;
        private InputHandler _inputHandler;

        private Dictionary<GameState, Song> _songs;

        private BattleSequence _battleSequence;

        #region Menu Objects

        private double _loadGameTime = 0;
        private bool _gameLoaded = false;
        private bool _gameFailedToLoad = false;

        // Music
        private Song _backgroundMenuMusic;

        // Sprite Fonts
        private SpriteFont _sedgwickAveDisplay;
        private SpriteFont _schoolBell;

        // Texture2D Assets
        private Texture2D _mainMenuEyes;

        private TitleBulletSprite _bulletIcon;

        private TitleGearSprite _leftGearSprite;
        
        private TitleGearSprite _rightGearSprite;

        // Buttons
        #region Buttons
        private Button _playButton;
        private Button _optionsButton;
        private Button _controlsButton;
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
        private _3DBullet _bulletModel;
        private CirclingCamera _camera;

        private Rectangle _worldGround;

        private SandParticleSystem _sandParticleSystem;

        private Reward _rewardOne;

        private Tumbleweed _tumbleweed;

        private bool _playerWasHit = false;

        private bool _rewardGot;
        #endregion

        #region Characters
        private Player _player;
        private NPC _soraNPC;
        #endregion

        #region Music
        private Song _backgroundPlayingMusic;
        private Song _battleMusic;
        #endregion

        #region Dialogue
        private Dialogue[] _dialogue; // Dialogue array to hold dialogues in the game
        #endregion

        #endregion

        #region BattleSequence Objects

        private BattleSequence _battleSequenceHandler;

        #endregion

        #region GameOver

        private bool _gameOver = false;

        #region Textures
        private Texture2D _kuzuDeadTexture;
        #endregion

        #region songs
        private Song _gameOverMusic;

        #endregion
        #endregion

        #region Cutscene
        // This region tailors to cutscenes in the game
        private Cutscene _cutsceneOne;

        private Song _cutsceneOneMusic;
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
            _bulletModel = new _3DBullet(this, Matrix.Identity);

            #region Main Menu

            _leftGearSprite = new TitleGearSprite() { Position = new Vector2(270, 100), RotationDirection = 1, GearDirection = 0 };
            _rightGearSprite = new TitleGearSprite() { Position = new Vector2(1020, 100), RotationDirection = -1, GearDirection = (TitleGearSprite.Direction)1 };
            _bulletIcon = new TitleBulletSprite() { Graphics = _graphics };
            _playButton = new Button(new Vector2(640, 200), 200) { Scale = 1.25f, GameStateLocation = GameState.MainMenu };
            _optionsButton = new Button(new Vector2(_playButton.Position.X, _playButton.Position.Y + 80), 175) { GameStateLocation = GameState.MainMenu };
            _controlsButton = new Button(new Vector2(_optionsButton.Position.X, _optionsButton.Position.Y + 75), 160) { GameStateLocation = GameState.MainMenu };
            _exitButton = new Button(new Vector2(_controlsButton.Position.X, _controlsButton.Position.Y + 65), 145) { Scale = 0.65f, GameStateLocation = GameState.MainMenu };

            _sandParticleSystem = new SandParticleSystem(this, new Rectangle(GraphicsDevice.Viewport.Width + 20, 0, 10, GraphicsDevice.Viewport.Height - 200));
            Components.Add(_sandParticleSystem);
            _sandParticleSystem.Enabled = true;
            

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
            _rewardOne = new Reward(new Vector2(14000, 300));
            #endregion 

            #region Characters
            _player = new Player("Kuzu", "TODO", 100, new Vector2(100, 480), _inputHandler);
            _soraNPC = new NPC("Sora", "TODO", 100, new Vector2(((GraphicsDevice.Viewport.Width/2) + 250), 470), false);
            #endregion

            #region Dialogue

            // Here we will load ALL the dialogues into the game in array format
            // Depending on which character at what point 
            _dialogue = new Dialogue[1]
            {
                new Dialogue("SoraInteraction01.txt")
            };

            _inputHandler.DialogueCompleted = new bool[_dialogue.Length];
            #endregion

            _battleSequence = new BattleSequence(); // May change into an inverntory/crafting system?
            #endregion

            #region GameOver
            // In case there is anything needed to be added here.
            #endregion

            #region Cutscene

            _cutsceneOne = new Cutscene("S1.txt");

            #endregion

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
            _schoolBell = Content.Load<SpriteFont>("schoolBell-");

            _camera = new CirclingCamera(this);
            // Main Menu Texture2D Assets
            #region Main Menu
            _mainMenuEyes = Content.Load<Texture2D>("Eyes_WasteSeeker_MainMenu");
            _bulletIcon.LoadContent(Content);
            _leftGearSprite.LoadContent(Content);
            _rightGearSprite.LoadContent(Content);
            _playButton.LoadContent(Content, "PlayButton_MainMenu-Sheet");
            _optionsButton.LoadContent(Content, "OptionsButton_MainMenu-Sheet");
            _controlsButton.LoadContent(Content, "ControlsButton_MainMenu-Sheet");
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
            _kuzuDeadTexture = Content.Load<Texture2D>("KuzuDead_Chibi");
            _rewardOne.LoadContent(Content, "ThumbsUP");
            #endregion

            #region Music
            _backgroundMenuMusic = Content.Load<Song>("MainMenuMusic");
            _backgroundPlayingMusic = Content.Load<Song>("TutorialMusic");
            _battleMusic = Content.Load<Song>("BattleMusic");
            _gameOverMusic = Content.Load<Song>("gameover");
            _cutsceneOneMusic = Content.Load<Song>("Cutscene01Music");

            // Loading all songs into a dictionary to determine what to play
            // Will change "playing" music later in terms of the screen (or level) being played
            _songs = new Dictionary<GameState, Song>()
            {
                {GameState.MainMenu,  _backgroundMenuMusic},
                {GameState.Options,  _backgroundMenuMusic},
                {GameState.Playing, _backgroundPlayingMusic},
                {GameState.BattleSequence, _battleMusic},
                {GameState.Controls, _backgroundMenuMusic},
                {GameState.GameOver,  _gameOverMusic},
                {GameState.Cutscene, _cutsceneOneMusic}
            };
            #endregion

            #region Cutscene
            _cutsceneOne.LoadContent(Content);
            #endregion

            #region Buttons
            // Using this section to send a list of the buttons loaded into the game to the input handler
            List<Button> mainMenuButtons = new List<Button>()
            {
                _playButton,
                _optionsButton,
                _controlsButton,
                _exitButton
            };

            List<Button> optionsMenuButtons = new List<Button>()
            {
                _optionsMenu.BackButton,
                _optionsMenu.ExitButton,
                _optionsMenu.SaveButton,
                _optionsMenu.LoadButton
            };


            _inputHandler.LoadButtons(GameState.MainMenu, mainMenuButtons);
            _inputHandler.LoadButtons(GameState.Options, optionsMenuButtons);
            #endregion

            #region Dialogue
            for (int i = 0; i < _dialogue.Length; i++)
            {
                _dialogue[0].LoadContent(Content);
            }
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
            _camera.Update(gameTime);

            #region DetermineDialogue

            if (_inputHandler.DialoguePlaying)
            {
                if (_gameState == GameState.Playing)
                {
                    switch (_levelState)
                    {
                        case LevelState.LevelOne:
                            // Now we check ALL bounds of player and interactable character in the level
                            if (!_player.Bounds.CollidesWith(_soraNPC.Bounds))
                            {
                                _inputHandler.StopDialogue = true;
                                _inputHandler.DialoguePlaying = false;
                            }
                            break;
                    }
                }
            }

            #endregion
            #region Saving_Loading
            if (_inputHandler.Exit == true) { Exit(); }

            if (_inputHandler.Save == true)
            {
                SaveGame("savegame.json");
                _optionsMenu.GameWasSaved = true;
                _inputHandler.Save = false;
            }

            if (_inputHandler.Load == true)
            {
                bool successfullyLoaded = LoadGame("savegame.json");

                if (successfullyLoaded) { _gameLoaded = true; _gameFailedToLoad = false; }
                else { _gameLoaded = false; _gameFailedToLoad = true; }
                
                _inputHandler.Load = false;
            }
            #endregion

            #region Button Selection
            // Selecting and De-Selecting Buttons
            if (_previousGameState == GameState.Options && _gameState != GameState.Options)
            {
                _optionsMenu.ExitButton.ButtonSelect = false;
                _optionsMenu.SaveButton.ButtonSelect = false;
                _optionsMenu.GameWasPaused = false;
                _optionsMenu.PlayNoise(false);
            }
            #endregion

            #region Music_Sounds
            // Checking if the options menu has popped open
            // opened => play noise in higher pitch
            // closed => play noise in lower pitch
            if (_previousGameState != GameState.Options && _gameState == GameState.Options || _previousGameState == GameState.MainMenu && _gameState == GameState.Playing)
            {
                _optionsMenu.PlayNoise(true);
            }

            // Checking if previous game state was Playing and current is not
            if (_previousGameState == GameState.Playing && _gameState != GameState.Playing)
            {
                _player.StopSFX();
            }

            // this if statement allows the options menu to attach itself to whatever background music is being played
            if (previousSongPlayed != null && _gameState == GameState.Options) { _songs[GameState.Options] = previousSongPlayed; }
            Song songToPlay = _songs[_gameState]; // Could contain a song that is not on

            if (songToPlay != null && MediaPlayer.Queue.ActiveSong != songToPlay)
            {
                MediaPlayer.Play(songToPlay);
                MediaPlayer.IsRepeating = true;
            }
            #endregion

            #region Particles
            // Sand Particle System - "turn off switch"
            if (_sandParticleSystem != null && _gameState != GameState.Playing)
            {
                //_sandParticleSystem.Enabled = false;
                _sandParticleSystem.Visible = false;
            }
            #endregion

            switch (_gameState)
            {
                case GameState.MainMenu:
                    _leftGearSprite.Update(gameTime);
                    _rightGearSprite.Update(gameTime);
                    _playButton.Update(gameTime);
                    _optionsButton.Update(gameTime);
                    _controlsButton.Update(gameTime);
                    _exitButton.Update(gameTime);
                    break;
                case GameState.Options:

                    if (_previousGameState == GameState.Playing || _previousGameState == GameState.BattleSequence || _previousGameState == GameState.Cutscene)
                    {

                        _optionsMenu.LoadButton.ButtonActivated = false;
                        _optionsMenu.SaveButton.ButtonActivated = true;
                        _optionsMenu.ExitButton.ButtonActivated = true;
                        _optionsMenu.GameWasPaused = true;
                    }
                    else if (_previousGameState == GameState.MainMenu)
                    {
                        _optionsMenu.LoadButton.ButtonActivated = true;
                        _optionsMenu.SaveButton.ButtonActivated = false;
                        _optionsMenu.ExitButton.ButtonActivated = false;
                    }


                    _optionsMenu.Update(gameTime);
                    break;
                case GameState.Playing:

                    switch (_levelState)
                    {
                        case LevelState.LevelOne:
                            #region DialoguePlaying
                            if (_inputHandler.DialoguePlaying)
                            {
                                if (_player.Bounds.CollidesWith(_soraNPC.Bounds))
                                {
                                    if (_dialogue[0].Finished)
                                    {
                                        _inputHandler.StopDialogue = true;
                                    }
                                    else
                                    {
                                        if (_inputHandler.ContinueDialogue)
                                        {
                                            if (_dialogue[0].UpdateDialogue())
                                            {
                                                _inputHandler.StopDialogue = true;
                                                _inputHandler.DialogueCompleted[0] = true;
                                                _dialogue[0].Finished = true;
                                            }
                                            _inputHandler.ContinueDialogue = false;
                                        }
                                        _dialogue[0].Update(gameTime);
                                    }
                                    break; // Break to avoid any further updates
                                }
                            }
                            #endregion

                            _player.Update(gameTime);
                            _soraNPC.TargetPlayer(_player.Position);
                            _soraNPC.Update(gameTime, _player.Position);

                            if (_player.Position.X >= _soraNPC.Position.X && !_soraNPC.IsFollowingPlayer)
                            {
                                _soraNPC.IsFollowingPlayer = true;
                            }

                            if (_sandParticleSystem.Enabled == false || _sandParticleSystem.Visible == false)
                            {
                                _sandParticleSystem.Enabled = true;
                                _sandParticleSystem.Visible = true;
                            }

                            #region Tumbleweed
                            if (_tumbleweed.IsEnabled)
                            {
                                _tumbleweed.Update(gameTime);

                                if (_tumbleweed.Bounds.CollidesWith(_player.Bounds) && !_playerWasHit)
                                {
                                    _playerWasHit = true;
                                    _player.Health -= 25;
                                    if (_player.Health <= 0)
                                    {
                                        _gameOver = true;
                                        _gameState = GameState.GameOver;
                                    }
                                }

                                if (!_tumbleweed.Bounds.CollidesWith(_player.Bounds) && _playerWasHit)
                                {
                                    _playerWasHit = false;
                                }

                                if (_tumbleweed.Position.X < _player.Position.X - 750)
                                {
                                    _tumbleweed.UpdateSpeed();
                                    _tumbleweed.UpdateScale();
                                    _tumbleweed.Position = new Vector2(_player.Position.X + GraphicsDevice.Viewport.Width + 200, 540);
                                    _tumbleweed.TumbleweedPassings++;
                                }
                            }

                            // Player has reached end of level
                            if (_player.Position.X >= 12500)
                            {
                                if (_tumbleweed.Position.X <= _player.Position.X - 600)
                                {
                                    _tumbleweed.IsEnabled = false;
                                    _tumbleweed.Position = new Vector2(_player.Position.X + GraphicsDevice.Viewport.Width + 200, 540);
                                }
                            }
                            else
                            {
                                _tumbleweed.IsEnabled = true;
                            }
                            #endregion

                            if (_rewardOne.Bounds.CollidesWith(_player.Bounds))
                            {
                                _rewardGot = true;
                                _gameState = GameState.MainMenu;
                                ResetVariables(gameTime);
                            }

                            // Player bounds on screen
                            if (_player.Position.X <= -10) { _player.Position = new Vector2(GraphicsDevice.Viewport.X - 10, _player.Position.Y); }
                            else if (_player.Position.X > 14000) { _player.Position = new Vector2(14000, _player.Position.Y); }

                            break;
                    }
                    break;

                case GameState.GameOver:

                    if (_gameOver) { ResetVariables(gameTime); }

                    break;
                case GameState.BattleSequence:

                    switch (_levelState)
                    {
                        case LevelState.LevelOne:

                            break;
                    }
                    break;
                case GameState.Cutscene:

                    if (_cutsceneOne.Finished)
                    {
                        _inputHandler.StopCutscene = true;
                    }
                    else
                    {
                        if (_inputHandler.ContinueDialogue)
                        {
                            if (_cutsceneOne.UpdateDialogue())
                            {
                                _inputHandler.StopCutscene = true;
                                _cutsceneOne.Finished = true;
                            }
                            _inputHandler.ContinueDialogue = false;
                        }
                        _cutsceneOne.Update(gameTime);
                    }
                    
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
                    //_model.Draw(worldMatrix, _camera.View, _camera.Projection);
                    //_bulletModel.Draw(_camera);
                    // Texture2D Assets Main Menu
                    _spriteBatch.Draw(_mainMenuEyes, new Vector2(0, 0), null, Color.White);
                    _bulletIcon.Draw(_spriteBatch, gameTime, new Vector2(GraphicsDevice.Viewport.Width / 2 - 20, 90), new Vector2(256, 256), 0.125f);
                    _leftGearSprite.Draw(_spriteBatch, gameTime);
                    _rightGearSprite.Draw(_spriteBatch, gameTime);
                    _playButton.Draw(_spriteBatch, gameTime);
                    _optionsButton.Draw(_spriteBatch, gameTime);
                    _controlsButton.Draw(_spriteBatch, gameTime);
                    _exitButton.Draw(_spriteBatch, gameTime);

                    // Sprite Fonts
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 100), Color.White, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    //_spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Space' to Play!", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 280), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Space' to Play!") / 2, 0.35f, SpriteEffects.None, 1);
                    
                    if (_rewardGot)
                    {
                        _spriteBatch.Draw(_rewardOne.Texture, new Vector2(GraphicsDevice.Viewport.Width / 2 + 200, 175), Color.White);
                    }

                    _spriteBatch.End();
                    break;
                case GameState.Options:
                    // Options will contain volume, display options, langauge, and potentially more
                    _loadGameTime += gameTime.ElapsedGameTime.TotalSeconds;

                    _spriteBatch.Begin();
                    if (_gameFailedToLoad || _gameLoaded && _loadGameTime < 1)
                    {
                        if (_gameFailedToLoad)
                        {
                            _spriteBatch.DrawString(_sedgwickAveDisplay, "Game Failed to Load.", new Vector2(GraphicsDevice.Viewport.Width / 2, 600), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Game Failed to Load.") / 2, 1, SpriteEffects.None, 1);
                        }

                        if (_gameLoaded)
                        {
                            _spriteBatch.DrawString(_sedgwickAveDisplay, "Game Loaded!", new Vector2(GraphicsDevice.Viewport.Width / 2, 600), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Game Loaded!") / 2, 1, SpriteEffects.None, 1);
                        }
                    }
                    else
                    {
                        _loadGameTime = 0;
                        _gameFailedToLoad = false;
                        _gameLoaded = false;
                    }
                    _spriteBatch.End();
                    _optionsMenu.Draw(_spriteBatch, gameTime);
                    break;
                case GameState.Playing:

                    switch (_levelState)
                    {
                        case LevelState.LevelOne:
                            
                            GraphicsDevice.Clear(Color.NavajoWhite);
                            // Calculate our offset vector 

                            float cameraCenterX = GraphicsDevice.Viewport.Width / 2 - 100;
                            float playerX = MathHelper.Clamp(_player.Position.X, cameraCenterX, 14000 - cameraCenterX);
                            float offsetX = cameraCenterX - playerX;

                            Matrix transform;
                            
                            transform = Matrix.CreateTranslation(offsetX * 0.100f, 0, 0);
                            _spriteBatch.Begin(transformMatrix: transform);
                            _spriteBatch.Draw(_worldBackGroundTexture, Vector2.Zero, Color.White);
                            _spriteBatch.End();

                            transform = Matrix.CreateTranslation(offsetX * 0.150f, 0, 0);
                            _spriteBatch.Begin(transformMatrix: transform);
                            _spriteBatch.Draw(_worldFirstMidGroundTexture, Vector2.Zero, Color.White);
                            _spriteBatch.End();

                            transform = Matrix.CreateTranslation(offsetX * 0.250f, 0, 0);
                            _spriteBatch.Begin(transformMatrix: transform);
                            _spriteBatch.Draw(_worldSecondMidGroundTexture, Vector2.Zero, Color.White);
                            _spriteBatch.End();

                            transform = Matrix.CreateTranslation(offsetX * 0.600f, 0, 0);
                            _spriteBatch.Begin(transformMatrix: transform);
                            _spriteBatch.Draw(_worldForeGroundTexture, Vector2.Zero, Color.White);
                            _spriteBatch.Draw(_worldGroundTexture, _worldGround, Color.White);
                            _spriteBatch.End();

                            _spriteBatch.Begin();
                            _sandParticleSystem.Draw(gameTime);
                            _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Backspace' on the keyboard, to return to the Menu.", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 700), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Backspace' on the keyboard, to return to the Menu.") / 2, 0.35f, SpriteEffects.None, 1);

                            // Tutorial
                            if (_player.Position.X <= 1280)
                            {
                                _spriteBatch.DrawString(
                                    _sedgwickAveDisplay,
                                    "Dodge the tumbleweeds! \n(Press 'T' to view the Tilemap)",
                                    new Vector2(GraphicsDevice.Viewport.Width / 2, 300),
                                    Color.Black,
                                    0,
                                    _sedgwickAveDisplay.MeasureString("Dodge the tumbleweeds! \n(Press 'T' to view the Tilemap)") / 2,
                                    (float)0.5,
                                    SpriteEffects.None,
                                    1
                                );
                            }
                            else if (_player.Position.X >= 1000 && _player.Position.X <= 2280)
                            {
                                _spriteBatch.DrawString(
                                    _sedgwickAveDisplay, 
                                    "Talk to an NPC by pressing 'F' while on top of them!", 
                                    new Vector2(GraphicsDevice.Viewport.Width / 2, 300), 
                                    Color.Black, 
                                    0, 
                                    _sedgwickAveDisplay.MeasureString("Talk to an NPC by pressing 'F' while on top of them!") / 2, 
                                    (float)0.5, 
                                    SpriteEffects.None, 
                                    1
                                );
                            }
                            else if (_player.Position.X <= 3280)
                            {
                                _spriteBatch.DrawString(
                                    _sedgwickAveDisplay,
                                    "(Reach the end of level one for a reward)",
                                    new Vector2(GraphicsDevice.Viewport.Width / 2, 300),
                                    Color.Black,
                                    0,
                                    _sedgwickAveDisplay.MeasureString("(Reach the end of level one for a reward)") / 2,
                                    (float)0.5,
                                    SpriteEffects.None,
                                    1
                                );
                            }
                            else if (_player.Position.X >= 13500)
                            {
                                _spriteBatch.DrawString(
                                    _sedgwickAveDisplay,
                                    "REWARD",
                                    new Vector2((GraphicsDevice.Viewport.Width / 2) + 200, 300),
                                    Color.Black,
                                    0,
                                    _sedgwickAveDisplay.MeasureString("REWARD") / 2,
                                    (float)0.5,
                                    SpriteEffects.None,
                                    1
                                );

                                //_rewardOne.Draw(_spriteBatch);
                            }
                            _spriteBatch.DrawString(
                                    _sedgwickAveDisplay,
                                    "Health: " + _player.Health,
                                    new Vector2(GraphicsDevice.Viewport.Width / 7 - 50, 650),
                                    Color.Black,
                                    0,
                                    _sedgwickAveDisplay.MeasureString("Health: " + _player.Health) / 2,
                                    (float)0.5,
                                    SpriteEffects.None,
                                    1
                                );
                            /* //Player position testing
                            _spriteBatch.DrawString(
                                    _sedgwickAveDisplay,
                                    "Player Position: " + _player.Position.X,
                                    new Vector2(GraphicsDevice.Viewport.Width / 2, 600),
                                    Color.Black,
                                    0,
                                    _sedgwickAveDisplay.MeasureString("Player Position: " + _player.Position.X) / 2,
                                    (float)0.5,
                                    SpriteEffects.None,
                                    1
                                    );
                            */
                            _spriteBatch.End();
                            transform = Matrix.CreateTranslation(offsetX, 0, 0);
                            _spriteBatch.Begin(transformMatrix: transform);
                            _soraNPC.Draw(_spriteBatch, gameTime);
                            _player.Draw(_spriteBatch, gameTime);
                            _tumbleweed.Draw(_spriteBatch);

                            if (_player.Position.X >= 13500)
                            {
                                _rewardOne.Draw(_spriteBatch);
                            }

                            _spriteBatch.End();

                            _spriteBatch.Begin();
                            if (_inputHandler.DialoguePlaying && _player.Bounds.CollidesWith(_soraNPC.Bounds))
                            {
                                // Draw dialogue[0]
                                _dialogue[0].Draw(_spriteBatch);
                                // Do NOT break here, because everything needs to be drawn still
                            }
                            _spriteBatch.End();

                            break;
                    } // END LevelState
                    break;
                case GameState.GameOver:
                    _spriteBatch.Begin();

                    _bulletModel.Draw(_camera);

                    _spriteBatch.DrawString(
                        _sedgwickAveDisplay,
                        "GAME OVER",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 + 225, 150),
                        Color.Black,
                        0,
                        _sedgwickAveDisplay.MeasureString("GAME OVER"),
                        (float)1,
                        SpriteEffects.None,
                        1
                    );

                    _spriteBatch.Draw(_kuzuDeadTexture, new Vector2(500, 200), Color.White);

                    _spriteBatch.DrawString(
                        _sedgwickAveDisplay,
                        "Press \"ENTER\" or \"SPACE\" to return to the main menu.",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 - 125, 150),
                        Color.Black,
                        0,
                        _sedgwickAveDisplay.MeasureString("Press \"ENTER\" or \"SPACE\" to return to the main menu.") / 3,
                        (float)0.33,
                        SpriteEffects.None,
                        1
                    );

                    _spriteBatch.End();

                    break;
                case GameState.BattleSequence:
                    GraphicsDevice.Clear(Color.NavajoWhite);

                    _spriteBatch.Begin();
                    _battleSequence.Draw(_spriteBatch, gameTime);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'F' to return to Overworld (or 'ESC' for options)", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Press 'F' to return to Overworld (or 'ESC' for options)") / 2, (float)0.5, SpriteEffects.None, 1);
                    _spriteBatch.End();
                    break;
                case GameState.Controls:
                    _spriteBatch.Begin();

                    _spriteBatch.DrawString(
                        _schoolBell,
                        "**Press BackSpace to go back to the main menu**",
                        new Vector2(GraphicsDevice.Viewport.Width / 3 + 70, 50),
                        Color.Black,
                        0,
                        _schoolBell.MeasureString("**Press BackSpace to go back to the main menu**") * (float)0.75,
                        (float)0.75,
                        SpriteEffects.None,
                        1
                    );

                    #region controls
                    _spriteBatch.DrawString(
                        _schoolBell,
                        "A / D - Move character left / right",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 - 40, 250),
                        Color.Black,
                        0,
                        _schoolBell.MeasureString("A / D - Move character left / right") * (float)1,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );
                    _spriteBatch.DrawString(
                        _schoolBell,
                        "SPACEBAR - Make character jump",
                        new Vector2(GraphicsDevice.Viewport.Width/2 - 65, 350),
                        Color.Black,
                        0,
                        _schoolBell.MeasureString("SPACEBAR - Make character jump") * (float)1,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );
                    _spriteBatch.DrawString(
                        _schoolBell,
                        "SHIFT - Make character 'speedup' (run)",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 + 35, 450),
                        Color.Black,
                        0,
                        _schoolBell.MeasureString("SHIFT - Make character 'speedup' (run)") * (float)1,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );
                    _spriteBatch.DrawString(
                        _schoolBell,
                        "J - Make character attack",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 - 175, 550),
                        Color.Black,
                        0,
                        _schoolBell.MeasureString("J - Make character attack") * (float)1,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );
                    #endregion
                    _spriteBatch.End();
                    
                    break;
                
                case GameState.Cutscene:
                    GraphicsDevice.Clear(Color.NavajoWhite);
                    _cutsceneOne.Draw(_spriteBatch);
                    break;
                
            } // END GameState
            
            base.Draw(gameTime);
        }

        /// <summary>
        /// This method is used when a game is over and
        /// all variables must be reset.
        /// </summary>
        public void ResetVariables(GameTime gameTime)
        {
            // Player
            _player.Position = new Vector2(100, 480);
            _inputHandler.UpdateDirection(1);
            _player.Health = 100;
            _playerWasHit = false;  
            _player.StopSFX();  

            // NPC
            _soraNPC.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 250, 470);
            _soraNPC.NPCState = CharacterState.Idle;
            _soraNPC.Health = 100;  
            _soraNPC.IsFollowingPlayer = false;

            _soraNPC.ResetAnimation();

            // Tumbleweed / Objects
            _tumbleweed.Position = new Vector2(GraphicsDevice.Viewport.Width + 500, 540);  
            _tumbleweed.IsEnabled = true;  
            _sandParticleSystem.Enabled = true;  
            _sandParticleSystem.Visible = true;  

            // Level State
            _levelState = LevelState.LevelOne;

            _gameOver = false;
        }

        /// <summary>
        /// Save game method used if player decided to save the game
        /// </summary>
        /// <param name="filePath">File Path</param>
        public void SaveGame(string filePath)
        {
            var gameData = new GameSaveData()
            {
                playerX = _player.Position.X,
                playerY = _player.Position.Y,
                playerHealth = _player.Health,
                npc1X = _soraNPC.Position.X,
                npc1Y = _soraNPC.Position.Y,
                npc1Health = _soraNPC.Health,
                groupIndex = _cutsceneOne.GetDialogueGroupIndex(),
                groupMessageIndex = _cutsceneOne.GetDialogueIndex(),
                cutsceneOver = _cutsceneOne.Finished
            };

            string json = JsonSerializer.Serialize(gameData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Load game method used if player decided to load a save
        /// </summary>
        /// <param name="filePath">File Path</param>
        public bool LoadGame(string filePath)
        {
            if (!File.Exists(filePath))
                return false; 

            string json = File.ReadAllText(filePath);
            GameSaveData gameData = JsonSerializer.Deserialize<GameSaveData>(json);

            //Player
            _player.Position = new Vector2(gameData.playerX, gameData.playerY);
            _player.Health = gameData.playerHealth;

            //Npc1
            _soraNPC.Position = new Vector2(gameData.npc1X, gameData.npc1Y);
            _soraNPC.Health = gameData.npc1Health;

            _cutsceneOne.UpdateDialogueIndex(gameData.groupIndex, gameData.groupMessageIndex);
            _cutsceneOne.Finished = gameData.cutsceneOver;
            return true;
        }
    }
}
