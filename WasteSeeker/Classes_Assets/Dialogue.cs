using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Dialogue class to hold dialogue interaction between character/npcs in game
    /// 
    /// - NPC and Enemy classes will contain a Dialogue object to reference their own dialogue with the player
    /// 
    /// - Probably will extract text dialogue from a txt file and store all possible sources of text inside the main game loop (in an array)
    /// - File format for a dialogue text:
    ///     
    ///     player_name, npc_name           (player/npc names)
    ///     PlayerSprite.png, NPCSprite.png (player and npc sprites - animated illustration of them talking)
    ///     2, 3, 1,                        (similar to the cutscene class, determines how many lines the player/npc will use before switching)
    ///     "text"\ "text"\ "text"\ ...     (the actual dialogue text that is provided)
    ///     1.0, 1.1                        (the pitch of the different characters)    
    /// 
    /// - For the illustrations of the characters talking; the player will be on the side in which they are positioned relative to the 
    ///     NPC. They also will be hand drawn by me and talk through this dialogue class.
    /// 
    /// - If I want more dynamic dialogue options (ie. character selecting choices and npc responding based on responses) this 
    ///   may need to be changed.
    /// </summary>
    public class Dialogue
    {
        private DialogueBox _dialogueBox;

        private string[] names;

        private Texture2D[] _textures;

        // These float values will determine the pitch of the noise in which the text appears on screen
        private float[] _pitches;

        // Filename of the dialogue
        private string _filename;

        private SoundEffect _pitchSoundEffect;
        private SoundEffectInstance _pitchSoundEffectInstance;

        public Dialogue(string filename)
        {
            _filename = filename;
        }

        public void LoadContent(ContentManager content)
        {
            _pitchSoundEffect = content.Load<SoundEffect>("dialogueSFX");
            _pitchSoundEffectInstance = _pitchSoundEffect.CreateInstance();
            //_pitchSoundEffectInstance.Pitch = 1.0f; - use to change pitch to w/e

            // Reading in all the data into this string
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));

            var lines = data.Split('\n');

            var firstLine = lines[0];
            names[int.Parse(firstLine)] = firstLine;        // Contains the entity's names
            _textures = new Texture2D[firstLine.Length];    // Contains the entity's textures
            _pitches = new float[firstLine.Length];           // Contains the entity's pitches
        

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        /// <summary>
        /// Helper method to play the menu's character speaking sound
        /// </summary>
        public void PlayNoise(bool isHighPitch)
        {
            _pitchSoundEffectInstance.Pitch = isHighPitch ? 1.0f : 0.75f; // Need to change to reflect what is in pitches array
            _pitchSoundEffectInstance.Play();
        }
    }
}
