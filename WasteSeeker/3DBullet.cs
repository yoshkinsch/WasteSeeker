using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Interfaces;

namespace WasteSeeker
{
    public class _3DBullet
    {
        // The game this crate belongs to
        Game game;

        // The VertexBuffer of crate vertices
        VertexBuffer vertexBuffer;

        // The IndexBuffer defining the Crate's triangles
        IndexBuffer indexBuffer;

        // The effect to render the crate with
        BasicEffect effect1;
        BasicEffect effect2;
        BasicEffect effect3;

        // The texture to apply to the crate
        Texture2D texture;

        Model model;
        Model model2;
        Model model3;

        /// <summary>
        /// Creates a new crate instance
        /// </summary>
        /// <param name="game">The game this crate belongs to</param>
        /// <param name="type">The type of crate to use</param>
        /// <param name="world">The position and orientation of the crate in the world</param>
        public _3DBullet(Game game, Matrix world)
        {
            this.game = game;
            this.texture = game.Content.Load<Texture2D>("BaseColor");
            this.model = game.Content.Load<Model>("3DBullet");
            this.model2 = game.Content.Load<Model>("3DBullet");
            this.model3 = game.Content.Load<Model>("3DBullet");
            InitializeEffect();
            Matrix correctionRotation = Matrix.CreateRotationY(3*MathHelper.PiOver4); // Angling the First Model
            Matrix translation = Matrix.CreateTranslation(new Vector3(-20f, 0, 0f));
            effect1.World = correctionRotation * translation * world;
            
            Matrix correctionRotation2 = Matrix.CreateRotationY(5 * MathHelper.PiOver4); // Angling the Second Model
            Matrix translation2 = Matrix.CreateTranslation(new Vector3(20f, 0, 0f));
            effect2.World = correctionRotation2 * translation2 * world;

            Matrix correctionRotation3 = Matrix.CreateRotationY(8 * MathHelper.PiOver4); // Angling the Second Model
            Matrix translation3 = Matrix.CreateTranslation(new Vector3(0f, 0f, -50f));
            effect3.World = correctionRotation3 * translation3 * world;
        }

        /// <summary>
        /// Initializes the BasicEffect to render our crate
        /// </summary>
        void InitializeEffect()
        {
            // First Model
            effect1 = new BasicEffect(game.GraphicsDevice);
            effect1.TextureEnabled = true;
            effect1.Texture = texture;
            // Turn on lighting
            effect1.LightingEnabled = true;
            // Set up light 0
            effect1.DirectionalLight0.Enabled = true;
            effect1.DirectionalLight0.Direction = new Vector3(-1f, 0f, -1f);
            effect1.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0f, 0f);
            effect1.DirectionalLight0.SpecularColor = new Vector3(1f, 0.0f, 0.0f);

            // Second Model
            effect2 = new BasicEffect(game.GraphicsDevice);
            effect2.TextureEnabled = true;
            effect2.Texture = texture;

            effect2.LightingEnabled = true;
            effect2.DirectionalLight0.Enabled = true;
            effect2.DirectionalLight0.Direction = new Vector3(-1f, 0f, -1f);
            effect2.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0f, 0f);
            effect2.DirectionalLight0.SpecularColor = new Vector3(1f, 0.0f, 0.0f);

            effect3 = new BasicEffect(game.GraphicsDevice);
            effect3.TextureEnabled = true;
            effect3.Texture = texture;

            effect3.LightingEnabled = true;
            effect3.DirectionalLight0.Enabled = true;
            effect3.DirectionalLight0.Direction = new Vector3(-1f, 0f, -1f);
            effect3.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0f, 0f);
            effect3.DirectionalLight0.SpecularColor = new Vector3(1f, 0.0f, 0.0f);
            /*
            effect.DirectionalLight1.Enabled = true;
            effect.DirectionalLight1.Direction = new Vector3(1f, -1f, 0f);
            effect.DirectionalLight1.DiffuseColor = new Vector3(0.8f, 1f, 0f);
            effect.DirectionalLight1.SpecularColor = new Vector3(1f, 0f, 0f);

            effect.DirectionalLight2.Enabled = true;
            effect.DirectionalLight2.Direction = new Vector3(-1f, 0f, 1f);
            effect.DirectionalLight2.DiffuseColor = new Vector3(0.9f, 1f, 0f);
            effect.DirectionalLight2.SpecularColor = new Vector3(1f, 0f, 0f);*/
            //effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.9f);
        }

        /// <summary>
        /// Draws the bullet
        /// </summary>
        /// <param name="camera">The camera to use to draw the crate</param>
        public void Draw(ICamera camera)
        {
            // set the view and projection matrices
            effect1.View = camera.View;
            effect1.Projection = camera.Projection;

            // apply the effect 
            foreach (var mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = effect1;
                }
                mesh.Draw();
            }

            effect2.View = camera.View;
            effect2.Projection = camera.Projection;

            // apply the effect 
            foreach (var mesh in model2.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                }
                mesh.Draw();
            }

            effect3.View = camera.View;
            effect3.Projection = camera.Projection;

            // apply the effect 
            foreach (var mesh in model3.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = effect3;
                }
                mesh.Draw();
            }
        }
    }
}
