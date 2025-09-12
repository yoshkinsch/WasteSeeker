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
        /// <summary>
        /// Deterimnes whether 
        /// </summary>
        public enum ButtonState
        {
            Idle,
            Hovering,
            Transitioning
        }

        private Texture2D _idleButtonTexture;  // idle button is 150x100 - not moving

        private Texture2D _buttonHoverTexture; // each frame is 200x150 pixels wide/tall

        private BoundingRectangle _bounds;

        private Vector2 _position;

        private MouseState _mouseState;

        private double _animationTimer;

        private short _animationFrame = 1;

        private ButtonState _buttonState;

        /// <summary>
        /// The bounding "volume" for the Button
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        public Button(Vector2 position)
        {
            _position = position;
            _bounds = new BoundingRectangle(_position, 150, 100); // Position will usually be the origin of the button
        }

        /// <summary>
        /// Loads the texture(s) for the button
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            // Load texture of static button 
            // Load texture of non-static button (will include transitions to and from static button)
        }

        /// <summary>
        /// Updates the button state depending on where the mouse is
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Vector2 mousePosition = new Vector2(_mouseState.X, _mouseState.Y);

            // Button state is idle if the mouse is not hovering on the button and the button is not transitioning
            if (!_bounds.CollidesWith(mousePosition) && _buttonState != ButtonState.Transitioning) { _buttonState = ButtonState.Idle; }

            // Button state is transitioning if the mouse is not on the button and the mouse is in the Hover state
            if (!_bounds.CollidesWith(mousePosition) && _buttonState == ButtonState.Hovering) { _buttonState = ButtonState.Transitioning; }
        }

        /// <summary>
        /// Draws the animated (or non-animated) texture(s) of the button
        /// </summary>
        /// <param name="spriteBatch">Sprite batch to draw button</param>
        /// <param name="gameTime">The game time</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {   
            switch (_buttonState)
            {
                case ButtonState.Idle:

                    spriteBatch.Draw(_idleButtonTexture, _position, Color.White);
                    break;

                case ButtonState.Hovering:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_animationTimer > 0.5)
                    {
                        _animationFrame++;
                        if (_animationFrame > 6) { _animationFrame = 1; }
                        _animationTimer -= 0.5;
                    }

                    var source = new Rectangle(_animationFrame * 200, 150, 200, 150);
                    spriteBatch.Draw(_buttonHoverTexture, _position, source, Color.White);
                    break;

                case ButtonState.Transitioning:
                    _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_animationTimer > 0.5)
                    {
                        _animationFrame++;
                        if (_animationFrame > 6) { _animationFrame = 1; _buttonState = ButtonState.Idle; }
                        _animationTimer -= 0.5;
                    }

                    source = new Rectangle(_animationFrame * 200, 0, 200, 150);
                    spriteBatch.Draw(_buttonHoverTexture, _position, source, Color.White);
                    break;
            }
        }
    }
}
