using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Class to represent the playable character(s) in the game
    /// </summary>
    public class Player : Interfaces.ICharacter
    {
        public enum PlayerState
        {
            Walking,
            Running
        }

        private InputHandler _inputHandler;

        private PlayerState _playerState;

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
        /// <param name="texture">Character's texture asset</param>
        public Player(string name, string description, int health, Vector2 position, InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            Name = name;
            Description = description;
            Health = health;
            Position = position;
        }

        /// <summary>
        /// Loads the texture of the character
        /// </summary>
        /// <param name="content">Content Manager to load content with</param>
        public void LoadContent(ContentManager content)
        {
            //Load content of the texture here "Texture = texture"
        }

        /// <summary>
        /// Updates the character's movement - uses direction and animation to determine what part of the texture to use
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public void Update(GameTime gameTime)
        {
            if (_playerState == PlayerState.Walking)
            {
                Position += _inputHandler.Direction * (WalkSpeed/100f);
            }
            else if (_playerState == PlayerState.Running)
            {
                Position += _inputHandler.Direction * (RunSpeed / 100f);
            }

            //Implement attacking
        }

        /// <summary>
        /// Draws the character's sprite on screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch tool to draw texture</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            //spriteBatch.End();
        }
    }
}
