using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// This class is used to store information for a character's ability
    /// - An "ability" in this game will be considered an attack that does slightly more damage than the
    ///   base attack amount (i.e. a character with an attack of power 10 will have an ability of power 20.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// Name of the abilility
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the ability
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Attack power of the ability (ie - how much damage it does)
        /// </summary>
        public float AttackPower { get; set; }

        /// <summary>
        /// Effects of the ability when attacking
        /// </summary>
        public Texture2D animatedAttack {  get; set; }

        public Ability(string name, string description, float attackPower)
        {
            Name = name;
            Description = description;
            AttackPower = attackPower;
        }
    }
}
