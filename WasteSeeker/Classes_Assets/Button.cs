using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private Texture2D _texture;

        private BoundingRectangle _bounds;

        private Vector2 _position;

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
            // Load texture of button here - will include the mouse-on texture
            // and the animated mouse-on textures (may be 5-10 frames)
        }

        /// <summary>
        /// Draws the animated (or non-animated) texture(s) of the button
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Will just draw the mouse-off texture unless the mouse is hovering on the button
            // Once the mouse hovers, the animated texture will start playing until the mouse is off
            // or the mouse clicks the button (in which case the game-state will change).
        }
    }
}
