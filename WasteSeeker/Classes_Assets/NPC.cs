using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Collisions;
using static WasteSeeker.Classes_Assets.Player;

namespace WasteSeeker.Classes_Assets
{
    public class NPC : Interfaces.ICharacter
    {
        public enum NPCState
        {
            Attacking,
            Idle,
            Walking,
            Running,
            Talking
        }

        private float _scaleFactor = 2.0f;

        private NPCState _npcState = NPCState.Idle;

        private NPCState _previousNPCState;

        private double _animationTimer;

        private short _animationFrame = 1;

        private SpriteEffects _directionFacing = SpriteEffects.None;

        private BoundingRectangle _bounds;

        private bool _isFollowingPlayer = false;

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
        /// The NPC's walk speed
        /// </summary>
        public float WalkSpeed { get; set; } = 230f;

        /// <summary>
        /// The NPC's Run Speed
        /// </summary>
        public float RunSpeed { get; set; }

        /// <summary>
        /// The NPC's position on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The NPC's texture
        /// </summary>
        public Texture2D Texture { get; set; }

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
        }

        /// <summary>
        /// Base Update Method in-case we only want to update what is happening to the NPC
        /// </summary>
        /// <param name="gameTime"></param>
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

            if (_previousNPCState != _npcState) { _animationFrame = 1; _animationTimer = 0; }

            switch (_npcState)
            {
                case NPCState.Walking:

                    Position += new Vector2(movementDirection * WalkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);

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
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            Rectangle source;

            switch (_npcState)
            {
                case NPCState.Idle:

                    if (_animationTimer > 0.15)
                    {
                        _animationFrame++;
                        if (_animationFrame > 5) { _animationFrame = 1; }
                        _animationTimer -= 0.15;
                    }
                    source = new Rectangle(_animationFrame * 48, 81, 48, 80);
                    spriteBatch.Draw(Texture, Position, source, Color.White, 0, new Vector2(24, 40), _scaleFactor, _directionFacing, 1);

                    break;

                case NPCState.Walking:

                    if (_animationTimer > 0.1)
                    {
                        _animationFrame++;
                        if (_animationFrame > 11) { _animationFrame = 1; }
                        _animationTimer -= 0.1;
                    }
                    source = new Rectangle(_animationFrame * 48, 2, 48, 80);
                    spriteBatch.Draw(Texture, Position, source, Color.White, 0, new Vector2(24, 40), _scaleFactor, _directionFacing, 1);

                    break;
            }
            
        }

        /// <summary>
        /// Method to set the direction of where the NPC is looking - in this case the player
        /// </summary>
        /// <param name="playerPosition">The player's position</param>
        public void TargetPlayer(Vector2 playerPosition)
        {
            if (playerPosition.X < Position.X) { _directionFacing = SpriteEffects.FlipHorizontally; }
            else {  _directionFacing = SpriteEffects.None; }
        }

        /// <summary>
        /// Method to update 
        /// </summary>
        /// <param name="playerPosition"></param>
        public int FollowPlayer(Vector2 playerPosition)
        {
            if (Position.X <= playerPosition.X && Position.X <= playerPosition.X - 150) // NPC is moving right towards player
            {
                _npcState = NPCState.Walking;
                return 1;

            }
            if (Position.X >= playerPosition.X - 150 && Position.X <= playerPosition.X + 150) // NPC is within "150" pixels of player, so we switch to idle
            {
                _npcState = NPCState.Idle;
                return 0;
            }
            if (Position.X >= playerPosition.X && Position.X >= playerPosition.X + 150) // NPC is moving left towards the player
            {
                _npcState = NPCState.Walking;
                return -1;
            }
            return 0;
        }
    }
}
