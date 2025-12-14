using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WasteSeeker.Collisions;
using WasteSeeker.Animation_Classes;

namespace WasteSeeker.Classes_Assets
{
    public class NPC : Interfaces.ICharacter
    {

        private float _scaleFactor = 2.0f;

        private CharacterState _npcState = CharacterState.Idle;

        private CharacterState _previousNPCState;

        private BoundingRectangle _bounds;

        private bool _isFollowingPlayer = false;

        private AnimatedSprite _animatedSprite;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// Sets if the NPC is following the player
        /// </summary>
        public bool IsFollowingPlayer
        {
            get { return _isFollowingPlayer; }
            set { _isFollowingPlayer = value; }
        }

        /// <summary>
        /// Name of the NPC
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The NPC's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The NPC's Health
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
        /// The NPC's walk speed
        /// </summary>
        public float WalkSpeed { get; set; } = 245f;

        /// <summary>
        /// The NPC's Run Speed
        /// </summary>
        public float RunSpeed { get; set; } = 450f;

        /// <summary>
        /// The NPC's position on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The NPC's texture
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The NPC's battle texture for when a battle sequence happens
        /// </summary>
        public Texture2D BattleTexture { get; set; }

        public CharacterState NPCState
        {
            get { return _npcState; }
            set { _npcState = value; }
        }

        /// <summary>
        /// Constructor for a playable character
        /// </summary>
        /// <param name="name">Name of the character</param>
        /// <param name="description">Description of the character</param>
        /// <param name="health">Character's Hitpoints</param>
        /// <param name="position">Character's Position</param>
        public NPC(string name, string description, int health, Vector2 position, bool isFollowingPlayer)
        {
            Name = name;
            Description = description;
            Health = health;
            Position = position;
            _bounds = new BoundingRectangle(position, 40, 80);
            _isFollowingPlayer = isFollowingPlayer;
        }

        /// <summary>
        /// Loads the texture of the NPC
        /// </summary>
        /// <param name="content">Content Manager to load content with</param>
        public void LoadContent(ContentManager content, string texture)
        {
            //Load content of the texture here "Texture = texture"
            Texture = content.Load<Texture2D>(texture);

            _animatedSprite = new AnimatedSprite(Texture, Position, 48, 80, SpriteEffects.None, _scaleFactor, 0.15f, 0.1f, 0.05f, 5, 11, 0f, 0);
            _animatedSprite.CharacterState = _npcState;
        }

        /// <summary>
        /// Base Update Method in-case we only want to update what is happening to the NPC
        /// (required for interface)
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Implementation
        }

        /// <summary>
        /// Updates the NPC in the game
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            int movementDirection = 0;
            TargetPlayer(playerPosition);
            if (_isFollowingPlayer) { movementDirection = FollowPlayer(playerPosition); }

            if (_previousNPCState != _npcState) { _animatedSprite.UpdateAnimationVariables(0, 1); }

            switch (_npcState)
            {
                case CharacterState.Walking:

                    Position += new Vector2(movementDirection * WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    _animatedSprite.Position = Position;

                    break;
                case CharacterState.Running:

                    Position += new Vector2(movementDirection * RunSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    _animatedSprite.Position = Position;

                    break;
            }

            _bounds.X = Position.X;
            _bounds.Y = Position.Y;
            _previousNPCState = _npcState;
        }

        /// <summary>
        /// Draws the NPC on the screen
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        /// <param name="gameTime">Game Time</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animatedSprite.Draw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Method to set the direction of where the NPC is looking - in this case the player
        /// </summary>
        /// <param name="playerPosition">The player's position</param>
        public void TargetPlayer(Vector2 playerPosition)
        {
            if (playerPosition.X < Position.X) { _animatedSprite.DirectionFacing = SpriteEffects.FlipHorizontally; }
            else { _animatedSprite.DirectionFacing = SpriteEffects.None; }
        }

        /// <summary>
        /// Used to reset animation's variables
        /// </summary>
        public void ResetAnimation()
        {
            _animatedSprite.ResetAllAnimationVariables(Position);
        }

        /// <summary>
        /// Method to update 
        /// </summary>
        /// <param name="playerPosition"></param>
        public int FollowPlayer(Vector2 playerPosition)
        {
            float idleZone = 150f;
            float walkingZone = 250f;

            float distanceX = Position.X - playerPosition.X;

            if (Math.Abs(distanceX) <= idleZone) // NPC is in the radius of the player
            {
                _npcState = CharacterState.Idle;
                _animatedSprite.CharacterState = _npcState;
                return 0;
            }
            else if (Position.X < playerPosition.X) // NPC is moving right towards player
            {
                if (_npcState == CharacterState.Running)
                {
                    if (Position.X >= playerPosition.X - walkingZone + 25)
                    {
                        _npcState = CharacterState.Walking;
                    }
                }
                else
                {
                    if (Position.X >= playerPosition.X - walkingZone)
                    {
                        _npcState = CharacterState.Walking;
                    }
                    else
                    {
                        _npcState = CharacterState.Running;
                    }
                }

                _animatedSprite.CharacterState = _npcState;
                return 1;
            }
            else // NPC is moving left towards player
            {
                if (_npcState == CharacterState.Running)
                {
                    if (Position.X <= playerPosition.X + walkingZone - 25)
                    {
                        _npcState = CharacterState.Running;
                    }
                }
                else
                {
                    if (Position.X <= playerPosition.X + walkingZone)
                    {
                        _npcState = CharacterState.Walking;
                    }
                    else
                    {
                        _npcState = CharacterState.Running;
                    }
                }

                _animatedSprite.CharacterState = _npcState;
                return -1;
            }
        }
    }
}
