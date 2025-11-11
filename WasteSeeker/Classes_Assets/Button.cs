using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Collisions;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Button class to represent a button on the Main Menu or anywhere in the game
    /// </summary>
    public class Button
    {
        private float _scale = 1;

        private Texture2D _buttonTexture;  // Button is 200x100 pixels

        private BoundingRectangle _bounds;

        private Vector2 _position;

        private MouseState _mouseState;

        private bool _isButtonActivated = true;

        private int _buttonHover; // 0 if NOT hovering, 1 otherwise - used in Draw and Update

        private bool _isSelected = false; // true if keyboard is on button, false otherwise

        private bool _isAnimated = true;

        public bool Animated
        {
            get { return _isAnimated; }
            set { _isAnimated = value; }
        }

        public bool ButtonSelect
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public GameState GameStateLocation { get; set; }

        public bool ButtonActivated
        {
            get { return _isButtonActivated; }
            set { _isButtonActivated = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
            }
        }

        /// <summary>
        /// The bounding "volume" for the Button
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// Public vector2 to get the button's position
        /// </summary>
        public Vector2 Position => _position;

        public Button(Vector2 position, int buttonWidth)
        {
            _position = position;
            _bounds = new BoundingRectangle(_position - new Vector2(100,30), buttonWidth, 60); // Position will usually be the origin of the button
        }

        /// <summary>
        /// Loads the texture(s) for the button
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content, string buttonTexture)
        {
            _buttonTexture = content.Load<Texture2D>(buttonTexture);
        }

        /// <summary>
        /// Updates the button state depending on where the mouse is
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(_mouseState.X, _mouseState.Y);

            _buttonHover = (_bounds.CollidesWith(mousePosition) || _isSelected) ? 1 : 0;
        }

        /// <summary>
        /// Draws the animated (or non-animated) texture(s) of the button
        /// </summary>
        /// <param name="spriteBatch">Sprite batch to draw button</param>
        /// <param name="gameTime">The game time</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Animated)
            {
                var source = new Rectangle(_buttonHover * 200, 0, 200, 100);
                spriteBatch.Draw(_buttonTexture, _position, source, Color.White, 0, new Vector2(100, 50), _scale, SpriteEffects.None, 1);
            }
            else
            {
                var source = new Rectangle(0 * 200, 0, 200, 100);
                spriteBatch.Draw(_buttonTexture, _position, source, Color.White, 0, new Vector2(100, 50), _scale, SpriteEffects.None, 1);
            }
        }
    }
}
