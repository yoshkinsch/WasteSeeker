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

namespace WasteSeeker
{
    public class OptionsMenu
    {
        Rectangle _sliderBar = new Rectangle(100, 200, 200, 10); 
        Rectangle _sliderHandle;

        float volume = 0.5f; // Starting at half volume
        bool isDragging = false;

        Texture2D _pixelTexture;

        public void Initialize()
        {
            _sliderHandle = new Rectangle(_sliderBar.X + (int)(_sliderBar.Width * volume) - 5, _sliderBar.Y - 5, 10, 20);
        }

        public void LoadContent(ContentManager content)
        {
            _pixelTexture = content.Load<Texture2D>("RedPixel");
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && _sliderHandle.Contains(mouse.Position))
            {
                isDragging = true;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                isDragging = false;
            }

            // If the mouse stops using the slider, then isDraggin will be false, otherwise it will continue to update
            // the background music currently playing (will be later implemented to update as it goes on in game)
            if (isDragging)
            {
                int clampedX = Math.Clamp(mouse.X, _sliderBar.X, _sliderBar.X + _sliderBar.Width);
                _sliderHandle.X = clampedX - _sliderHandle.Width / 2;

                volume = (float)(clampedX - _sliderBar.X) / _sliderBar.Width;

                MediaPlayer.Volume = volume;
                SoundEffect.MasterVolume = volume;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_pixelTexture, _sliderBar, Color.Gray);
            spriteBatch.Draw(_pixelTexture, _sliderHandle, Color.DarkGray);

            spriteBatch.End();
        }


    }
}
