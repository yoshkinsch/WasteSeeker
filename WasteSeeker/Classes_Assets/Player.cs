using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WasteSeeker.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Class to represent the playable character(s) in the game
    /// </summary>
    public class Player : Interfaces.ICharacter
    {
        public enum PlayerState
        {
            Attacking,
            Idle,
            Walking,
            Running
        }

        private float _scaleFactor = (float)2.5;

        private InputHandler _inputHandler;

        private PlayerState _playerState = PlayerState.Idle;

        private PlayerState _previousPlayerState;

        private double _animationTimer;

        private short _animationFrame = 1;

        private SpriteEffects _directionFacing = SpriteEffects.None;

        private BoundingRectangle _bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// The name of the playable character (in this case Kuzu)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the character
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The health of the character
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Walk speed of the character
        /// </summary>
        public float WalkSpeed { get; set; } = 100f;

        /// <summary>
        /// Running speed of the character
        /// </summary>
        public float RunSpeed { get; set; } = 200f;

        /// <summary>
        /// Position in which the character is located on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Texture in which the character will use
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Constructor for a playable character
        /// </summary>
        /// <param name="name">Name of the character</param>
        /// <param name="description">Description of the character</param>
        /// <param name="health">Character's Hitpoints</param>
        /// <param name="position">Character's Position</param>
        public Player(string name, string description, int health, Vector2 position, InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            Name = name;
            Description = description;
            Health = health;
            Position = position;
            _bounds = new BoundingRectangle(position, 40, 80);
        }

        /// <summary>
        /// Loads the texture of the character
        /// </summary>
        /// <param name="content">Content Manager to load content with</param>
        public void LoadContent(ContentManager content)
        {
            //Load content of the texture here "Texture = texture"
            Texture = content.Load<Texture2D>("Kuzu_Idle_Walk");
        }

        /// <summary>
        /// Updates the character's movement - uses direction and animation to determine what part of the texture to use
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(GameTime gameTime)
        {
            _previousPlayerState = _playerState;

            // Updating Position of player
            if (_playerState == PlayerState.Walking)
            {
                Position += _inputHandler.Direction * WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_playerState == PlayerState.Running)
            {
                Position += _inputHandler.Direction * RunSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Get the last facing direction
            if (_inputHandler.Direction.X > 0) { _directionFacing = SpriteEffects.None; }
            else if (_inputHandler.Direction.X < 0) { _directionFacing = SpriteEffects.FlipHorizontally; }

            //See if player is holding Left Shift (running)
            if (_inputHandler.Running == true) { _playerState = PlayerState.Running; }
            else if (_inputHandler.Idle == true) { _playerState = PlayerState.Idle; }
            else { _playerState = PlayerState.Walking; }

            // Sees if the current player state is equal to the previous one
            // If true, then sets animation frame and timer to their default values
            if (_playerState != _previousPlayerState)
            {
                _animationFrame = 1;
                _animationTimer = 0;
            }

            // Updating the bounds to the new position
            _bounds.X = Position.X;
            _bounds.Y = Position.Y;
        }

        /// <summary>
        /// Draws the character's sprite on screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch tool to draw texture</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            Rectangle source;

            switch (_playerState)
            {
                case PlayerState.Attacking:

                    break;
                case PlayerState.Idle:

                    if (_animationTimer > 0.15)
                    {
                        _animationFrame++;
                        if (_animationFrame > 5) { _animationFrame = 1; }
                        _animationTimer -= 0.15;
                    }
                    source = new Rectangle(_animationFrame * 48, 81, 48, 80);
                    spriteBatch.Draw(Texture, Position, source, Color.White, 0, new Vector2(24, 40), _scaleFactor, _directionFacing, 1);

                    break;
                case PlayerState.Walking:

                    if (_animationTimer > 0.1)
                    {
                        _animationFrame++;
                        if (_animationFrame > 11) { _animationFrame = 1; }
                        _animationTimer -= 0.1;
                    }
                    source = new Rectangle(_animationFrame * 48, 2, 48, 80);
                    spriteBatch.Draw(Texture, Position, source, Color.White, 0, new Vector2(24, 40), _scaleFactor, _directionFacing, 1);

                    break;
                case PlayerState.Running:

                    break;
            }
        }
    }
}
