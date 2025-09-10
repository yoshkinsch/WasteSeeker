using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WasteSeeker.Classes_Assets;

namespace WasteSeeker
{
    /// <summary>
    /// The Waste Seeker game - Description will be added later in development
    /// </summary>
    public class WasteSeekerGame : Game
    {
        #region GameState Enum
        /// <summary>
        /// Enumeration to hold the state of the game
        /// </summary>
        public enum GameState
        {
            MainMenu,
            Options,
            Credits,
            GameOver,
            Playing,
            Paused
        }
        #endregion

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState gameState = GameState.MainMenu;

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
            #region Main Menu
            _leftGearSprite = new TitleGearSprite() { Position = new Vector2(270, 100), RotationDirection = 1, GearDirection = 0 };
            _rightGearSprite = new TitleGearSprite() { Position = new Vector2(1020, 100), RotationDirection = -1, GearDirection = (TitleGearSprite.Direction)1 };
            _bulletIcon = new TitleBulletSprite() { Graphics = _graphics };
            _playButton = new Button(new Vector2(640,360));
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
            #endregion 


        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            // EXIT code
            if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed )
                Exit();
            
            // TODO: Add your update logic here

            /*
             * Game State is updated here if certain input is entered
             * Input-Handler will handle which input is sent and will communicate that back here - can determine the game state
            */

            switch (gameState)
            {
                case GameState.MainMenu:
                    _leftGearSprite.Update(gameTime);
                    _rightGearSprite.Update(gameTime);
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
            switch (gameState)
            {
                case GameState.MainMenu:
                    _spriteBatch.Begin();

                    // Texture2D Assets Main Menu
                    _spriteBatch.Draw(_mainMenuEyes, new Vector2(0, 0), null, Color.White);
                    _bulletIcon.Draw(_spriteBatch, gameTime, new Vector2(GraphicsDevice.Viewport.Width / 2 - 20, 90), new Vector2(256, 256), 0.125f);
                    _leftGearSprite.Draw(_spriteBatch, gameTime);
                    _rightGearSprite.Draw(_spriteBatch, gameTime);

                    // Sprite Fonts
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Black, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Waste Seeker", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 100), Color.White, 0, _sedgwickAveDisplay.MeasureString("Waste Seeker") / 2, 1, SpriteEffects.None, 1);
                    _spriteBatch.DrawString(_sedgwickAveDisplay, "Press 'Q' or 'ESC' to EXIT", new Vector2((GraphicsDevice.Viewport.Width / 2) + 10, 150), Color.White, 0, _sedgwickAveDisplay.MeasureString("Press 'Q' or 'ESC' to EXIT") / 2, 0.35f, SpriteEffects.None, 1);

                    _spriteBatch.End();
                    break;
                case GameState.Options:
                    // Options will contain volume, display options, langauge, and potentially more
                    break;
            }
            

            base.Draw(gameTime);
        }
    }
}
