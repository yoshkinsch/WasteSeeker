using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WasteSeeker
{
    /// <summary>
    /// A cut-scene class to store and manipulate frames to display cut-scenes in the game
    /// </summary>
    public class CutScene
    {
        private List<Texture2D> _frames;

        private List<float> _frameDurations;

        private int _currentFrame = 0;

        private float _timer = 0f;

        /// <summary>
        /// A bool to check if the cutscene has been played
        /// </summary>
        public bool Finished { get; private set; } = false;

        /// <summary>
        /// Cutscene constructor to initialize a Cutscene object
        /// </summary>
        /// <param name="frames">frames of the cut-scene</param>
        /// <param name="durations">durations of each frame in the cut-scene</param>
        public CutScene(List<Texture2D> frames, List<float> durations)
        {
            _frames = frames;
            _frameDurations = durations;
        }

        /// <summary>
        /// Update method to update which frames should be shown
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            if (Finished) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _frameDurations[_currentFrame])
            {
                _timer = 0f;
                _currentFrame++;

                if (_currentFrame >= _frames.Count)
                {
                    Finished = true;
                    _currentFrame = _frames.Count - 1;
                }
            }
        }

        /// <summary>
        /// Draw method to draw cutscenes
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw frames</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Finished)
            {
                spriteBatch.Draw(_frames[_currentFrame], Vector2.Zero, Color.White);
            }
        }

        /// <summary>
        /// Reset method in case a new game is started
        /// </summary>
        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
            Finished = false;
        }
    }
}
