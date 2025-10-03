using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Classes_Assets;

namespace WasteSeeker
{
    public class OptionsMenu
    {
        private Rectangle _sliderBar = new Rectangle(360, 200, 600, 20); 
        private Rectangle _sliderHandle;

        private float _volume = 0.1f; 
        private bool _isDragging = false;

        private Texture2D _volumePixelTexture;
        private SpriteFont _sedgwickAveDisplay;

        // Sound effects for when the options menu pops up
        private SoundEffect _popup;
        private SoundEffectInstance _popupInstance;

        /// <summary>
        /// Public button to tell the inputhandler it's bounds
        /// </summary>
        public Button BackButton { get; private set; }

        public void Initialize()
        {
            BackButton = new Button(new Vector2(100, 50), 160);
            _sliderHandle = new Rectangle(_sliderBar.X + (int)(_sliderBar.Width * _volume) - 5, _sliderBar.Y - 5, 10, 20);
            MediaPlayer.Volume = _volume;
        }

        public void LoadContent(ContentManager content)
        {
            _volumePixelTexture = content.Load<Texture2D>("RedPixel");
            _sedgwickAveDisplay = content.Load<SpriteFont>("sedgwickAveDisplay");
            BackButton.LoadContent(content, "ButtonBack_OptionsMenu-Sheet");
            _popup = content.Load<SoundEffect>("Popup_Sound");
            _popupInstance = _popup.CreateInstance();
            _popupInstance.Volume = 0.25f;
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            BackButton.Update(gameTime);

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
            _popupInstance.Stop();
            _popupInstance.Pitch = isHighPitch ? 1.0f : 0.5f;
            _popupInstance.Play();
        }
    }
}
