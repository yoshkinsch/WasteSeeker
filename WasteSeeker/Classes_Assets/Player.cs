using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Collisions;
using WasteSeeker.Animation_Classes;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Class to represent the playable character(s) in the game
    /// </summary>
    public class Player : Interfaces.ICharacter
    {
        private float _scaleFactor = 2.0f;

        private InputHandler _inputHandler;

        private CharacterState _playerState = CharacterState.Idle;

        private CharacterState _previousPlayerState;

        private SoundEffect _walkingSfx;

        private SoundEffectInstance _walkingSfxInstance;

        private SoundEffectInstance _runningSfxInstance;

        private BoundingRectangle _bounds;

        private AnimatedSprite _animatedSprite; // Custom processor to process sprite sheets

        private Vector2 _velocity;

        #region Jumping

        private bool _onGround = true;

        private float _jumpPower = -500f;

        private float _gravity = 900;

        private float _currentSpeed = 250f;

        #endregion

        private Vector2 _position;

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
        /// Attack damage of the player
        /// </summary>
        public float AttackPower { get; set; }

        /// <summary>
        /// Ability of the player
        /// </summary>
        public Ability Ability { get; set; }

        /// <summary>
        /// Walk speed of the character
        /// </summary>
        public float WalkSpeed { get; set; } = 250f;

        /// <summary>
        /// Running speed of the character
        /// </summary>
        public float RunSpeed { get; set; } = 450f;

        /// <summary>
        /// Position in which the character is located on the screen
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Texture in which the character will use
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The battle texture used in a battle sequence
        /// </summary>
        public Texture2D BattleTexture { get; set; }

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
            Texture = content.Load<Texture2D>("Kuzu_Idle_Walk_Attack");
            _walkingSfx = content.Load<SoundEffect>("sand-walk");
            
            _walkingSfxInstance = _walkingSfx.CreateInstance();
            _walkingSfxInstance.Volume = 0.2f;
            _walkingSfxInstance.IsLooped = true;

            _runningSfxInstance = _walkingSfx.CreateInstance();
            _runningSfxInstance.Volume = 0.2f;
            _runningSfxInstance.Pitch = 0.5f;
            _runningSfxInstance.IsLooped = true;

            _animatedSprite = new AnimatedSprite(Texture, Position, 48, 80, SpriteEffects.None, _scaleFactor, 0.15f, 0.1f, 0.05f, 5, 11, 0.10f, 5);
            _animatedSprite.CharacterState = _playerState;
        }

        public void Update(GameTime gameTime)
        {
            
            // Sees if the current player state is not equal to the previous one
            // If true, then sets animation frame and timer to their default values
            if (_playerState == CharacterState.Idle && _previousPlayerState != CharacterState.Idle)
            {
                _animatedSprite.UpdateAnimationVariables(0, 1); // 0 = animation timer & 1 = animation frame
            }

            if (_position.Y >= 480) // Assuming _groundLevel is the y-coordinate of the ground
            {
                _position.Y = 480;
                _velocity.Y = 0;
                _onGround = true;
            }

            if (_onGround)
            {
                if (_inputHandler.JumpPressed)
                {
                    _velocity.Y = _jumpPower;
                    _onGround = false;
                }
            }
            else
            {
                _velocity.Y += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _walkingSfxInstance.Stop();
                _runningSfxInstance.Stop();
            }

            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle exiting the Attack state
            if (_playerState == CharacterState.Attacking)
            {
                // Check if the animation has reached frame 5 (as requested by user prompt)
                // You'll need a way to get the current frame index from your AnimatedSprite
                if (_animatedSprite.GetCurrentFrame() >= 5) // Use 5 for the 6th frame in 0-based indexing (0, 1, 2, 3, 4, 5) or adjust the logic
                {
                    // The attack is done. Determine the next state based on current input.
                    _inputHandler.Attacking = false; // Consume the input flag
                    if (_inputHandler.Running) { _playerState = CharacterState.Running; }
                    else if (_inputHandler.Direction != Vector2.Zero) { _playerState = CharacterState.Walking; }
                    else { _playerState = CharacterState.Idle; }
                }
                // If still attacking, we skip the rest of the input processing this frame.
            }
            else
            {
                if (!_onGround)
                {
                    _playerState = CharacterState.Jumping;
                    _animatedSprite.UpdateAnimationVariables(0, 0);
                }
                else if (_inputHandler.Attacking)
                {
                    _playerState = CharacterState.Attacking;
                    _animatedSprite.UpdateAnimationVariables(0, 0); // Reset animation timer and frame index to 0
                }
                else if (_inputHandler.JumpPressed)
                {
                    // Handle jump setup
                    _velocity.Y = _jumpPower;
                    _onGround = false;
                }
                else if (_inputHandler.Running)
                {
                    _playerState = CharacterState.Running;
                }
                else if (_inputHandler.Direction != Vector2.Zero)
                {
                    _playerState = CharacterState.Walking;
                }
                else
                {
                    _playerState = CharacterState.Idle;
                }
            }

            if (_playerState == CharacterState.Running)
            {
                _currentSpeed = RunSpeed;
            }
            else if (_playerState == CharacterState.Walking)
            {
                _currentSpeed = WalkSpeed;
            }

            // Stop movement during attack unless specifically designed otherwise
            if (_playerState == CharacterState.Walking)
            {
                _position.X += _inputHandler.Direction.X * WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_walkingSfxInstance.State != SoundState.Playing && _onGround)
                {
                    _runningSfxInstance.Stop();
                    _walkingSfxInstance.Play();
                }
            }
            else if (_playerState == CharacterState.Running)
            {
                _position.X += _inputHandler.Direction.X * RunSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_runningSfxInstance.State != SoundState.Playing && _onGround)
                {
                    _walkingSfxInstance.Stop();
                    _runningSfxInstance.Play();
                }
            }
            else if (_playerState == CharacterState.Jumping)
            {
                _position.X += _inputHandler.Direction.X * _currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else // Idle or Attacking (Attacking stops all lateral movement)
            {
                _walkingSfxInstance.Stop();
                _runningSfxInstance.Stop();
            }


            _animatedSprite.Position = Position; // Update the sprite component's position
            _animatedSprite.CharacterState = _playerState; // Inform the sprite component which animation to use

            // Get the last facing direction (Keep existing logic)
            if (_inputHandler.Direction.X > 0) { _animatedSprite.DirectionFacing = SpriteEffects.None; }
            else if (_inputHandler.Direction.X < 0) { _animatedSprite.DirectionFacing = SpriteEffects.FlipHorizontally; }

            // Updating the bounds to the new position (Keep existing logic)
            _bounds.X = _position.X;
            _bounds.Y = _position.Y;

            _previousPlayerState = _playerState;
        }
        /// <summary>
        /// A method to stop any and all sfx coming from the player class
        /// Is used when game is paused or changes state in game loop
        /// </summary>
        public void StopSFX()
        {
            _walkingSfxInstance.Stop();
            _runningSfxInstance.Stop();
        }

        /// <summary>
        /// Draws the character's sprite on screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch tool to draw texture</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            /*
             * This method will call animatedSprite to draw the sprite, 
             * if there is anything else other than the sprite to draw
             * it will be called here as well.
             */
            _animatedSprite.Draw(spriteBatch, gameTime);
        }
    }
}
