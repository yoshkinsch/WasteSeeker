using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Classes_Assets;

namespace WasteSeeker
{
    public class OptionsMenu
    {
        private Rectangle _sliderBar = new Rectangle(360, 300, 600, 20); 
        private Rectangle _sliderHandle;

        private double _gameTimeCount = 0f;
        private float _volume = 0.1f; 
        private bool _isDragging = false;

        private Texture2D _volumePixelTexture;
        private SpriteFont _sedgwickAveDisplay;

        // Sound effects for when the options menu pops up
        private SoundEffect _popup;
        private SoundEffectInstance _popupInstance;

        /// <summary>
        /// Public bool to indicate that the game was paused
        /// </summary>
        public bool GameWasPaused { get; set; }

        /// <summary>
        /// Public bool to indicate that the game was saved
        /// </summary>
        public bool GameWasSaved { get; set; }

        /// <summary>
        /// Public button to tell the inputhandler it's bounds
        /// </summary>
        public Button BackButton { get; private set; }

        /// <summary>
        /// Public button to Savee content into the game
        /// </summary>
        public Button SaveButton { get; private set; }

        /// <summary>
        /// Public button to exit back to the main menu
        /// </summary>
        public Button ExitButton { get; private set; }

        /// <summary>
        /// Public button to Load content into the game
        /// </summary>
        public Button LoadButton { get; private set; }

        public void Initialize()
        {
            BackButton = new Button(new Vector2(100, 50), 160) { GameStateLocation = GameState.Options };
            ExitButton = new Button(new Vector2(100, 125), 160) { GameStateLocation = GameState.Options, ButtonActivated = false };
            SaveButton = new Button(new Vector2(100, 200), 160) { GameStateLocation = GameState.Options };
            LoadButton = new Button(new Vector2(100, 125), 160) { GameStateLocation = GameState.Options };
            _sliderHandle = new Rectangle(_sliderBar.X + (int)(_sliderBar.Width * _volume) - 5, _sliderBar.Y - 5, 10, 20);
            MediaPlayer.Volume = _volume;
        }

        public void LoadContent(ContentManager content)
        {
            _volumePixelTexture = content.Load<Texture2D>("RedPixel");
            _sedgwickAveDisplay = content.Load<SpriteFont>("sedgwickAveDisplay");
            BackButton.LoadContent(content, "ButtonBack_OptionsMenu-Sheet");
            SaveButton.LoadContent(content, "ButtonSave_OptionsMenu-Sheet");
            ExitButton.LoadContent(content, "ExitButton_MainMenu-Sheet");
            LoadButton.LoadContent(content, "ButtonLoad_MainMenu-Sheet");
            _popup = content.Load<SoundEffect>("gunshot_sfx");
            _popupInstance = _popup.CreateInstance();
            _popupInstance.Volume = 0.25f;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            if (GameWasSaved)
            {
                _gameTimeCount += gameTime.ElapsedGameTime.TotalSeconds;
                if (_gameTimeCount > 1) { _gameTimeCount = 0; GameWasSaved = false; }
            }

            BackButton.Update(gameTime);
            if (GameWasPaused) { ExitButton.ButtonActivated = true; ExitButton.Update(gameTime); SaveButton.Update(gameTime); }
            else { ExitButton.ButtonActivated = false; LoadButton.Update(gameTime); }

            if (mouse.LeftButton == ButtonState.Pressed && _sliderHandle.Contains(mouse.Position))
            {
                _isDragging = true;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                _isDragging = false;
            }

            // If the mouse stops using the slider, then isDraggin will be false, otherwise it will continue to update
            // the background music currently playing (will be later implemented to update as it goes on in game)
            if (_isDragging)
            {
                int clampedX = Math.Clamp(mouse.X, _sliderBar.X, _sliderBar.X + _sliderBar.Width);
                _sliderHandle.X = clampedX - _sliderHandle.Width / 2;

                _volume = (float)(clampedX - _sliderBar.X) / _sliderBar.Width;

                MediaPlayer.Volume = _volume;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            BackButton.Draw(spriteBatch, gameTime);
            if (GameWasPaused) { ExitButton.Draw(spriteBatch, gameTime); SaveButton.Draw(spriteBatch, gameTime); }
            else { LoadButton.Draw(spriteBatch, gameTime); }
            if (GameWasSaved) { spriteBatch.DrawString(_sedgwickAveDisplay, "Game Saved!", new Vector2(640, 480), Color.White, 0, _sedgwickAveDisplay.MeasureString("Game Saved!") / 2, 1f, SpriteEffects.None, 1); }
            spriteBatch.DrawString(_sedgwickAveDisplay, "Options", new Vector2(650, 50), Color.White, 0, _sedgwickAveDisplay.MeasureString("Options") / 2, 1f, SpriteEffects.None, 1);

            #region BGM volume
            spriteBatch.DrawString(_sedgwickAveDisplay, "Volume", new Vector2(_sliderBar.X - 200, _sliderBar.Y + 20), Color.White, 0, _sedgwickAveDisplay.MeasureString("Volume") / 2, 1f, SpriteEffects.None, 1);
            spriteBatch.Draw(_volumePixelTexture, _sliderBar, Color.Gray);
            spriteBatch.Draw(_volumePixelTexture, _sliderHandle, Color.DarkGray);
            #endregion


            spriteBatch.End();
        }
        
        /// <summary>
        /// Helper method to play the menu's popup sound
        /// </summary>
        public void PlayNoise(bool isHighPitch)
        {
            _popupInstance.Pitch = isHighPitch ? 1.0f : 0.75f;
            _popupInstance.Play();
        }
    }
}
