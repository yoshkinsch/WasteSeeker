using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        private GameState _gameState = GameState.MainMenu;
        private InputHandler _inputHandler;

        #region Menu Objects

        // Sprite Fonts
        private SpriteFont _sedgwickAveDisplay;

        // Texture2D Assets
        private Texture2D _mainMenuEyes;

        private TitleBulletSprite _bulletIcon;

        private TitleGearSprite _leftGearSprite;
        
        private TitleGearSprite _rightGearSprite;

        // Buttons
        #region Buttons
        /* [TODO]
         * > Create Button class
         *      - BoundingRectangle class needs to be within the button class as a field
         *      - Each button below will need to have its texture loaded (add LoadContent method)
         *      - Each button will need an Update method to tell itself to switch the textures around
         * > Make buttons in CSP (should have a non-hover image and a hover-image
         *      - Only make the quit button for now
        */

        private Button _playButton;
        //private Rectangle _optionsButton;
        //private Rectangle _quitButton;
        #endregion

        #endregion

        #region Playing Objects

        private double _angryTimer = 0;

        #region Objects
        private Texture2D _worldGroundTexture;
        private Texture2D _worldBackGroundTexture;

        private Rectangle _worldGround;
        private Rectangle _worldBackGround;
        #endregion

        #region Characters
        private Player _player;
        private NPC _soraNPC;
        #endregion

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
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _inputHandler = new InputHandler();

            #region Main Menu
            _leftGearSprite = new TitleGearSprite() { Position = new Vector2(270, 100), RotationDirection = 1, GearDirection = 0 };
            _rightGearSprite = new TitleGearSprite() { Position = new Vector2(1020, 100), RotationDirection = -1, GearDirection = (TitleGearSprite.Direction)1 };
            _bulletIcon = new TitleBulletSprite() { Graphics = _graphics };
            //_playButton = new Button(new Vector2(640,360));
            #endregion

            #region Playing

            #region Objects
            _worldGround = new Rectangle(0, 540, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 540);
            _worldBackGround = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 180);
            #endregion 

            #region Characters
            _player = new Player("Kuzu", "TODO", 100, new Vector2(100, 480), _inputHandler);
            _soraNPC = new NPC("Sora", "TODO", 100, new Vector2(((GraphicsDevice.Viewport.Width/2) + 250), 470));

            #endregion

            #endregion
            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Sprite Fonts
            _sedgwickAveDisplay = Content.Load<SpriteFont>("sedgwickAveDisplay");

            // Main Menu Texture2D Assets
            #region Main Menu
            _mainMenuEyes = Content.Load<Texture2D>("Eyes_WasteSeeker_MainMenu");
            _bulletIcon.LoadContent(Content);
            _leftGearSprite.LoadContent(Content);
            _rightGearSprite.LoadContent(Content);
            //_playButton.LoadContent(Content);
            #endregion

            #region Characters
            _player.LoadContent(Content);
            _soraNPC.LoadContent(Content, "Sora_Idle_Walk");
            _worldGroundTexture = Content.Load<Texture2D>("SandGround");
            _worldBackGroundTexture = Content.Load <Texture2D>("SandBackground");
            #endregion
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
            _inputHandler.Update(gameTime, ref _gameState);

            if (_inputHandler.Exit == true) { Exit(); }

            switch (_gameState)
            {
                case GameState.MainMenu:
                    _leftGearSprite.Update(gameTime);
                    _rightGearSprite.Update(gameTime);
                    //_playButton.Update(gameTime);
                    break;
                case GameState.Playing:
                    _player.Update(gameTime);
                    _soraNPC.TargetPlayer(_player.Position);

                    // Teleports player to opposite end of the screen if outside the screen's bounds
                    if (_player.Position.X <= -28) { _player.Position = new Vector2(GraphicsDevice.Viewport.Width + 25, _player.Position.Y); }
                    else if (_player.Position.X > 1308) { _player.Position = new Vector2(GraphicsDevice.Viewport.X - 25, _player.Position.Y); }

                    // Make Sora ANGRY
                    if (_player.Bounds.CollidesWith(_soraNPC.Bounds)) { _angryTimer += gameTime.ElapsedGameTime.TotalSeconds; }

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
                    //_playButton.Draw(_spriteBatch, gameTime);

                    // Sprite Fonts
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 100), Color.White, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Q' or 'ESC' to EXIT", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 150), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Q' or 'ESC' to EXIT") / 2, 0.35f, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Space' to Play!", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 200), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Space' to Play!") / 2, 0.35f, SpriteEffects.None, 1);

                    _spriteBatch.End();
                    break;
                case GameState.Options:
                    // Options will contain volume, display options, langauge, and potentially more
                    break;
                case GameState.Playing:

                    GraphicsDevice.Clear(Color.NavajoWhite);

                    _spriteBatch.Begin();

                    _spriteBatch.Draw(_worldGroundTexture, _worldGround, Color.White);
                    _spriteBatch.Draw(_worldBackGroundTexture, _worldBackGround, Color.White);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Backspace' on the keyboard, to return to the Menu.", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 700), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Backspace' on the keyboard, to return to the Menu.") / 2, 0.35f, SpriteEffects.None, 1);


                    // Tutorial
                    if (_player.Position.X <= 1280 / 2)
                    {
                        _spriteBatch.DrawString(_sedgwickAveDisplay, "Press A, D, Left Arrow, or Right Arrow\nto move around!", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Press A, D, Left Arrow, or Right Arrow\nto move around!") / 2, (float)0.5, SpriteEffects.None, 1);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_sedgwickAveDisplay, $"Time Angry: {TimeSpan.FromSeconds(_angryTimer):mm\\:ss\\.fff}", new Vector2(400, 200), Color.Black, 0, _sedgwickAveDisplay.MeasureString($"Time Angry: {TimeSpan.FromSeconds(_angryTimer):mm\\:ss\\.fff}") / 2, (float)0.5, SpriteEffects.None, 1);
                        if (!_player.Bounds.CollidesWith(_soraNPC.Bounds))
                        {
                            _spriteBatch.DrawString(_sedgwickAveDisplay, "Run into the character to make them angry!", new Vector2(GraphicsDevice.Viewport.Width / 2, 300), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Run into the character to make them angry!") / 2, (float)0.5, SpriteEffects.None, 1);
                        }
                        else
                        {
                            _spriteBatch.DrawString(_sedgwickAveDisplay, "Watch it!", new Vector2(_soraNPC.Position.X, _soraNPC.Position.Y - 150), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Watch it!") / 2, (float)1.5, SpriteEffects.None, 1);
                        }
                    }
                    _soraNPC.Draw(_spriteBatch, gameTime);
                    _player.Draw(_spriteBatch, gameTime);
                    
                    _spriteBatch.End();
                    break;
            }
            

            base.Draw(gameTime);
        }
    }
}
