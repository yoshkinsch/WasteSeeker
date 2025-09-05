using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Collisions
{
    public class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between a BoundingRectangle and the mouse's position
        /// </summary>
        /// <param name="a">The rectangle</param>
        /// <param name="b">The mouse's XY coordinates</param>
        /// <returns>true for collision, false otherwsie</returns>
        public static bool Collides(BoundingRectangle a, Vector2 mousePosition)
        {
            if (mousePosition.X >= a.X && mousePosition.X <= a.X + a.Width && mousePosition.Y >= a.Y && mousePosition.Y <= a.Y + a.Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Detects a collision between two BoundingRectangles
        /// </summary>
        /// <param name="a">The first rectangle</param>
        /// <param name="b">The second rectangle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }
    }
}
