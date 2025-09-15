using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WasteSeeker.Interfaces
{
    /// <summary>
    /// Interface for a moving character in the game (Player, NPC, Enemy, etc.)
    /// </summary>
    public interface ICharacter
    {
        public string Name { get; }

        public string Description { get; }

        public int Health { get; set; }

        public float WalkSpeed { get; set; }

        public float RunSpeed { get; set; }

        public Vector2 Position { get; set; }

        public Texture2D Texture { get; set; }

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
