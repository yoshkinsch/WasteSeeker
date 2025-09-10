using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// The bullet sprite/icon
    /// </summary>
    public class TitleBulletSprite
    {
        private Texture2D _bullet;

        public GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// Loads the Texture2D into the Bullet class
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            _bullet = contentManager.Load<Texture2D>("Bullet");
        }

        /// <summary>
        /// Drawing method for bulletIcon
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, Vector2 position, Vector2 origin, float scale)
        {
            _spriteBatch.Draw(_bullet, new Vector2(Graphics.GraphicsDevice.Viewport.Width / 2 - 20, 90), null, Color.White, 0, new Vector2(256, 256), 0.125f, SpriteEffects.None, 0);
        }
    }
}
