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

        private float _scaleFactor = (float)2.5;

        //private NPCState _npcState = NPCState.Idle;

        //private NPCState _previousNPCState;

        private double _animationTimer;

        private short _animationFrame = 1;

        private SpriteEffects _directionFacing = SpriteEffects.None;

        private BoundingRectangle _bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

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
        public float WalkSpeed { get; set; }

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
        public NPC(string name, string description, int health, Vector2 position)
        {
            Name = name;
            Description = description;
            Health = health;
            Position = position;
            _bounds = new BoundingRectangle(position, 40, 80);
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
        /// Updates the NPC in the game
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(GameTime gameTime)
        {
            
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

            if (_animationTimer > 0.2)
            {
                _animationFrame++;
                if (_animationFrame > 5) { _animationFrame = 1; }
                _animationTimer -= 0.2;
            }
            source = new Rectangle(_animationFrame * 48, 81, 48, 80);
            spriteBatch.Draw(Texture, Position, source, Color.White, 0, new Vector2(24, 40), _scaleFactor, _directionFacing, 1);
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
    }
}
