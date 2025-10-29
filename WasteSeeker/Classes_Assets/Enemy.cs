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
    /// Enemy class to represent an "enemy" in the game 
    /// - Will have similar setup (possibly) like the NPC class
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// Name of the Enemy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Enemy's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Enemy's Health
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The enemy's attack power (ie. how much damage they inflict on the character)
        /// </summary>
        public float AttackPower { get; set; }

        /// <summary>
        /// The NPC's position on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The NPC's texture
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The battle texture used in a battle sequence
        /// </summary>
        public Texture2D BattleTexture { get; set; }

        public Enemy(string name, string description, int health, Vector2 position, Texture2D texture)
        {
            Name = name;
            Description = description;
            Health = health;
            Position = position;
            Texture = texture;
        }
    }
}
