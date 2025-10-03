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
    /// Tubmelweed class to hold a tumbleweed sprite (and apply physics)
    /// </summary>
    public class Tumbleweed
    {
        private Texture2D _tumbleweedTexture;

        private float _rotation;

        private Vector2 _tumbleweedSpeed = new Vector2(200, 0);

        private float _tumbleweedRotationSpeed = 3.5f;

        /// <summary>
        /// Position of the tumbleweed on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        public Tumbleweed(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            _tumbleweedTexture = content.Load<Texture2D>("Tumbleweed_Sprite");
        }

        public void Update(GameTime gameTime)
        {
            Position -= _tumbleweedSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _rotation -= _tumbleweedRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw( _tumbleweedTexture, Position, null, Color.White, _rotation, new Vector2(20, 20), 2.5f ,SpriteEffects.None, 0f );
        }
    }
}
