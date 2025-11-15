using Microsoft.Xna.Framework;
using System;
using WasteSeeker.Interfaces;

namespace WasteSeeker.Classes_Assets
{
    public class CirclingCamera : ICamera
    {
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        // Center point of where the model will be
        private Vector3 target = new Vector3(0, 0, -20);
        //private Vector3 target = Vector3.Zero;

        // Orbit radius distance from the model
        private float radius = 10f;

        // The angle for spinning
        private float angle = 0f;

        // The rotation speed of the camera
        public float RotationSpeed { get; set; } = 0.01f;

        public CirclingCamera(Game game)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio,
                1f,
                1000f
            );
        }

        public void Update(GameTime gameTime)
        {
            angle += RotationSpeed;

            // Orbit around Y-axis:
            float x = (float)Math.Sin(angle) * radius;   // left/right
            float z = (float)Math.Cos(angle) * radius;   // forward/back

            // Camera position (horizontal circle around model)
            Vector3 position = new Vector3(
                target.X + x,
                target.Y + 150f,     // height above the model
                target.Z + z
            );

            // Always look at the model
            View = Matrix.CreateLookAt(position, target, Vector3.Up);
        }
    }
}
