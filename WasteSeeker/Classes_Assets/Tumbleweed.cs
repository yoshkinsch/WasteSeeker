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
    /// Tubmelweed class to hold a tumbleweed sprite (and apply physics)
    /// </summary>
    public class Tumbleweed
    {
        private Texture2D _tumbleweedTexture;

        private float _rotation;

        private Vector2 _tumbleweedSpeed = new Vector2(200, 0);

        private float _tumbleweedRotationSpeed = 3.5f;

        private float _scale = 2.5f;

        private int _passings = 0;

        private BoundingCircle _bounds;

        /// <summary>
        /// This property is to tell the game class how many tumbleweeds have passed (will be updated in the game class)
        /// </summary>
        public int TumbleweedPassings
        {
            get { return _passings; }
            set { _passings = value; }
        }

        /// <summary>
        /// In case we need to disable the tumbleweed at any given point
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Position of the tumbleweed on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => _bounds;

        public Tumbleweed(Vector2 position)
        {
            Position = position;
            _bounds = new BoundingCircle(position, 20);
        }

        /// <summary>
        /// Used to update the scale of a new tumbleweed on screen
        /// </summary>
        public void UpdateScale()
        {
            _scale = RandomHelper.NextFloat(1.5f, 3.5f);
        }

        public void UpdateSpeed()
        {
            if (TumbleweedPassings > 15)
            {
                TumbleweedPassings = RandomHelper.Next(4, 12);
            }
            float speedIncrease = 50f * _passings;
            _tumbleweedSpeed.X = 200f + speedIncrease;
            _tumbleweedSpeed.X += RandomHelper.NextFloat(-50f, 50f);
        }

        public void LoadContent(ContentManager content)
        {
            _tumbleweedTexture = content.Load<Texture2D>("Tumbleweed_Sprite");
        }

        public void Update(GameTime gameTime)
        {
            Position -= _tumbleweedSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _rotation -= _tumbleweedRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _bounds.Center = Position;

            _bounds.Radius = 20 * _scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(_tumbleweedTexture, Position, null, Color.White, _rotation + _scale, new Vector2(20, 20), _scale, SpriteEffects.None, 0f);
            }
        }
    }
}
