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
    ///     S, K, S ...                             (Keeps track of who is currently talking - first letter of each name)
    ///     PlayerSprite.png, NPCSprite.png, ...    (player and npc sprites - animated illustration of them talking)
    ///     2, 3, 1, ...                            (similar to the cutscene class, determines how many lines the player/npc will use before switching)
    ///     "text"\ "text"\ "text"\ ...             (the actual dialogue text that is provided)
    ///     1.0, 1.1, ...                           (the pitch of the different characters)
    ///     player_name, npc_name, ...              (player/npc names)
    /// 
    /// - Example text using the @ symbol to indicate a new line in the dialogue (43 characters until a new line each time):
    ///     I'm doing alright, let's just head to the hideout@first.
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

        private string[] _names;
        private Texture2D[] _textures;
        private float[] _pitches;
        private string[] _conversationOrder;

        private short _animationFrame = 0;
        private float _noiseTimer = 0.15f;
        private int _backgroundIterator;
        private int _characterSpeaking;

        private Vector2 _position = new Vector2(50, 1040);
        private Rectangle _source;

        private string _filename;

        private SoundEffect _pitchSoundEffect;
        private SoundEffectInstance _pitchSoundEffectInstance;

        private bool _finished = false;

        public bool Finished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        public Dialogue(string filename)
        {
            _filename = filename;
        }

        public void LoadContent(ContentManager content)
        {
            _pitchSoundEffect = content.Load<SoundEffect>("dialogueSFX");
            _pitchSoundEffectInstance = _pitchSoundEffect.CreateInstance();
            _pitchSoundEffectInstance.Volume = 0.15f;
            //_pitchSoundEffectInstance.Pitch = 1.0f; - use to change pitch to w/e

            // Reading in all the data into this string
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));

            var lines = data.Split('\n');

            // Fisrt line contains the dialogue/conversation order
            var firstLine = lines[0].Split(",");
            _conversationOrder = new string[firstLine.Length];
            for (int i = 0; i < firstLine.Length; i++)
            {
                _conversationOrder[i] = firstLine[i].Trim();
            }

            // Sprite Textures for the characters when they talk
            var secondLine = lines[1].Split(",");
            _textures = new Texture2D[secondLine.Length];    // Contains the entity's textures

            for (int i = 0; i < _textures.Length; i++)
            {
                _textures[i] = content.Load<Texture2D>(secondLine[i].Trim());
            }

            #region Dialogue Box
            // Third line is for when to display the next background image after X amount of dialogue boxes have been displayed
            var thirdLine = lines[2].Split(",");

            int[] temp = new int[thirdLine.Length];
            for (int i = 0; i < thirdLine.Length; i++)
            {
                temp[i] = int.Parse(thirdLine[i]);
            }

            // The fourth line is the text to display - here we create a dialogue box with all the text from this line
            var fourthLine = lines[3].Split("\\");

            string[][] dialogueStrings = new string[thirdLine.Length][];
            int textStep = 0;
            for (int i = 0; i < thirdLine.Length; i++) // Group
            {
                int groupSize = int.Parse(thirdLine[i].Trim());
                dialogueStrings[i] = new string[groupSize];
                for (int j = 0; j < int.Parse(thirdLine[i].Trim()); j++) // Text to display
                {
                    // Fourth line contains ALL text that will be displayed in the scene
                    // so we need to keep track of where we are in the fourth line (with textStep)
                    dialogueStrings[i][j] = fourthLine[textStep].Trim();
                    textStep++;
                }
            }

            _dialogueBox = new DialogueBox(dialogueStrings, temp);
            _dialogueBox.LoadContent(content);
            #endregion

            // Pitches of the characters when they talk
            var fifthLine = lines[4].Split(",");
            _pitches = new float[fifthLine.Length];           // Contains the entity's pitches
            for (int i = 0; i < _pitches.Length; i++)
            {
                _pitches[i] = float.Parse(fifthLine[i].Trim());
                
            }

            var sixthLine = lines[5].Split(",");
            _names = new string[sixthLine.Length];     // Contains the entity's names
            for (int i = 0; i < _names.Length; i++)
            {
                _names[i] = sixthLine[i].Trim();
            }
        }

        public void Update(GameTime gameTime)
        {
            _dialogueBox.Update(gameTime);

            // Changes the background being displayed if the group changes
            int dialogueGroupIndex = _dialogueBox.RequestDialogueGroup();
            if (_backgroundIterator < dialogueGroupIndex)
            {
                _backgroundIterator = dialogueGroupIndex;
                _noiseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Setting whoever the speaker is at the moment
            for (int i = 0; i < _names.Length; i++)
            {
                if (_conversationOrder[_backgroundIterator][0] == _names[i][0])
                {
                    _characterSpeaking = i;
                }
            }
        }

        /// <summary>
        /// Method to update the dialogue box when spacebar is hit from inputhandler
        /// </summary>
        public bool UpdateDialogue()
        {
            // if true -> then last group has been displayed
            if (_dialogueBox.UpdateDialogueBoxIndex())
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            // Choosing the color the character's name should be
            Color nameColor = Color.White;
            if (_names[_characterSpeaking].Equals("Kuzu")) { nameColor = Color.Red; }
            else if (_names[_characterSpeaking].Equals("Sora")) { nameColor = Color.Beige; }

            // Drawing the dialogue box with the name to be highlighted inside
            _dialogueBox.Draw(spriteBatch, _names[_characterSpeaking], nameColor);

            // Drawing the character sprites talking
            // If true - character's mouth should be open, otherwise their mouth should be closed
            // Also if true - play noise
            if (_dialogueBox.RequestIfWriting()){ _animationFrame = 1; PlayNoise(_pitches[_characterSpeaking]); }
            else{ _animationFrame = 0; }

            _source = new Rectangle(_animationFrame * 200, 0, 200, 200); // sprites talking will be of size 200x200
            spriteBatch.Draw(_textures[_characterSpeaking], _position, _source, Color.White, 0, new Vector2(0, 520), 1, SpriteEffects.None, 1);

            //spriteBatch.End();
        }

        public void StopNoise()
        {
            _pitchSoundEffectInstance.Stop();
        }

        /// <summary>
        /// Helper method to play the menu's character speaking sound
        /// </summary>
        /// <param name="pitch">Pitch of the character speaking</param>
        public void PlayNoise(float pitch)
        {
            _pitchSoundEffectInstance.Pitch = pitch;
            _pitchSoundEffectInstance.Play();
        }
    }
}
