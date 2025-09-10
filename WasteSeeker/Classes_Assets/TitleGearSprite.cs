using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// A class to represent the gear sprite icon on the Main Menu screen
    /// </summary>
    public class TitleGearSprite
    {
        public enum Direction
        {
            Left,
            Right
        }

        private const float ANIMATION_SPEED = 0.025f;

        private Texture2D _gear;

        private float _rotation;

        private double _animationTimer = 2;

        private double _directionTimer;

        public Direction GearDirection { get; set; }

        /// <summary>
        /// The rotation direction of the gear icon.
        /// 1 => clockwise | -1 => counter-clockwise
        /// </summary>
        public int RotationDirection { get; set; } = 1; // 1 => Clockwise, -1 => Counter-Clockwise

        /// <summary>
        /// The position of where the Gear icon is located on the screen
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Loads the two animation frames needed for the gear icon to spin
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            _gear = content.Load<Texture2D>("Gear_WasteSeeker_MainMenu");
        }
        
        /// <summary>
        /// Udpates the gears on the main menu to move
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Update the direction timer
            _directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Switch directions every 2 seconds
            if (_directionTimer > 3.5)
            {
                switch (GearDirection)
                {
                    case Direction.Right:
                        GearDirection = Direction.Left;
                        break;
                    case Direction.Left:
                        GearDirection = Direction.Right;
                        break;
                }
                _directionTimer -= 3.5;
            }

            // Move the bat in the direction it is flying
            switch (GearDirection)
            {
                case Direction.Left:
                    Position += new Vector2(-1, 0) * 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }

        /// <summary>
        /// Draws the animated gear icon
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw</param>
        /// <param name="gameTime">The game time</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Updating the animation timer with the total game time
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer > ANIMATION_SPEED)
            {
                _rotation += RotationDirection * (float)0.0250;
                _animationTimer -= ANIMATION_SPEED;
            }
            
            spriteBatch.Draw(_gear, Position, null, Color.White, _rotation, new Vector2(150,150), 0.5f, SpriteEffects.None, 0);
        }
    }
}
