using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Collisions
{
    public static class CollisionHelper
    {
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

        /// <summary>
        /// Detects a collision between two BoundingCircles
        /// </summary>
        /// <param name="a">The first bounding circle</param>
        /// <param name="b">The second bounding circle</param>
        /// <returns>true for collision, false otherwsie</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">The BoundingCircle</param>
        /// <param name="r">The BoundingRectangle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
        }

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
    }
}
