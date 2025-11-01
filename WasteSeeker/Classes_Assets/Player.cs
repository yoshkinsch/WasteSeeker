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
            Texture = content.Load<Texture2D>("Kuzu_Idle_Walk");
            _walkingSfx = content.Load<SoundEffect>("sand-walk");
            
            _walkingSfxInstance = _walkingSfx.CreateInstance();
            _walkingSfxInstance.Volume = 0.2f;
            _walkingSfxInstance.IsLooped = true;

            _runningSfxInstance = _walkingSfx.CreateInstance();
            _runningSfxInstance.Volume = 0.2f;
            _runningSfxInstance.Pitch = 0.5f;
            _runningSfxInstance.IsLooped = true;

            _animatedSprite = new AnimatedSprite(Texture, Position, 48, 80, SpriteEffects.None, _scaleFactor, 0.15f, 0.1f, 5, 11);
            _animatedSprite.CharacterState = _playerState;
        }

        /// <summary>
        /// Updates the character's movement - uses direction and animation to determine what part of the texture to use
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(GameTime gameTime)
        {
            _previousPlayerState = _playerState;
            
            // Updating Position of player
            if (_playerState == CharacterState.Walking)
            {
                Position += _inputHandler.Direction * WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animatedSprite.Position = Position;
                if (_walkingSfxInstance.State != SoundState.Playing)
                {
                    _runningSfxInstance.Stop();
                    _walkingSfxInstance.Play();
                }
            }
            else if (_playerState == CharacterState.Running)
            {
                Position += _inputHandler.Direction * RunSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animatedSprite.Position = Position;
                if (_runningSfxInstance.State != SoundState.Playing)
                {
                    _walkingSfxInstance.Stop();
                    _runningSfxInstance.Play();
                }
            }
            else
            {
                _walkingSfxInstance.Stop();
                _runningSfxInstance.Stop();
            }
            // Get the last facing direction
            if (_inputHandler.Direction.X > 0) { _animatedSprite.DirectionFacing = SpriteEffects.None; }
            else if (_inputHandler.Direction.X < 0) { _animatedSprite.DirectionFacing = SpriteEffects.FlipHorizontally; }

            //See if player is holding Left Shift (running)
            if (_inputHandler.Running == true) { _playerState = CharacterState.Running; }
            else if (_inputHandler.Idle == true) { _playerState = CharacterState.Idle; }
            else { _playerState = CharacterState.Walking; }

            _animatedSprite.CharacterState = _playerState; //setting the animated sprite's character state to reflect player

            // Sees if the current player state is not equal to the previous one
            // If true, then sets animation frame and timer to their default values
            if (_playerState != _previousPlayerState)
            {
                _animatedSprite.UpdateAnimationVariables(0, 1); // 0 = animation timer & 1 = animation frame
            }

            // Updating the bounds to the new position
            _bounds.X = Position.X;
            _bounds.Y = Position.Y;
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
