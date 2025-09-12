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
        private Texture2D _staticButtonTexture;

        private Texture2D _buttonHoverTexture; // each frame is 200x125 pixels wide

        private BoundingRectangle _bounds;

        private Vector2 _position;

        private MouseState _mouseState;

        private double _animationTimer;

        private short _animationFrame = 1;

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
        /// Draws the animated (or non-animated) texture(s) of the button
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">Sprite batch to draw button</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Will just draw the mouse-off texture unless the mouse is hovering on the button
            // Once the mouse hovers, the animated texture will start playing until the mouse is off
            // or the mouse clicks the button (in which case the game-state will change).
            
            Vector2 mousePosition = new Vector2(_mouseState.X, _mouseState.Y);

            if (!_bounds.CollidesWith(mousePosition))
            {
                spriteBatch.Draw(_staticButtonTexture, _position, Color.White);
            }
            else
            {
                _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_animationTimer > 0.5)
                {
                    _animationFrame++;
                    if (_animationFrame > 6) { _animationFrame = 1; }
                    _animationTimer -= 0.5;
                }

                var source = new Rectangle(_animationFrame * 200, 150, 200, 150);
                spriteBatch.Draw(_buttonHoverTexture, _position, source, Color.White);
            }
        }
    }
}
