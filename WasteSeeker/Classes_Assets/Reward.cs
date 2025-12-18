using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WasteSeeker.Collisions;

namespace WasteSeeker.Classes_Assets
{
    public class Reward
    {
        private Texture2D _texture;

        private Vector2 _position;

        private BoundingCircle _bounds;

        public bool RewardGot
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public BoundingCircle Bounds
        {
            get { return _bounds; }
        }

        public Reward(Vector2 position)
        {
            _position = position;
            _bounds = new BoundingCircle(position, 125); 
        }

        public void LoadContent(ContentManager content, string texture)
        {
            _texture = content.Load<Texture2D>(texture);
        }

        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(125, 125), 1, SpriteEffects.None, 0f);
        }
    }
}
