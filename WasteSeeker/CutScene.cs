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

        public CutScene(List<Texture2D> frames, List<float> durations)
        {
            _frames = frames;
            _frameDurations = durations;
        }

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
                    //TODO: Finish Update method - start a draw method and add a method to 
                    //      make the boolean "Finished" false if a new game starts.
                    //      ALSO - make a possible skip button so players can skip the
                    //      cutscene.
                }
            }
        }
    }
}
