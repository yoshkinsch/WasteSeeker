using Microsoft.Xna.Framework;
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
    /// This class will be used to provide a cutscene
    /// - Functionality (what this class contains and does)
    ///     A cutscene object will contain:
    ///         X amount of images to display in sequential order (will be inside an array when initialized).
    ///         Dialogue box object (which will display text and handle when the user presses space to continue to the next box).
    ///     
    ///     There will also be a txt file that contains the information in regards to transitioning between backgrounds:
    ///         This is the format:
    ///             2 (num of backgrounds)
    ///             BackgroundImage.png, BackgroundImage2.png etc.. (background image filenames)
    ///             2, 1, etc.. (amount of text for the dialogue box to display)
    ///             Text to display first, Text to display second, Next Scene's text, etc.. (dialogue)
    ///             
    ///         First line: background images
    ///         Seoncd line: number of texts (connected with the background images
    ///         Third line: Text to be displayed
    /// </summary>
    public class Cutscene
    {
        private Texture2D[] _backgrounds;

        private int _backgroundIterator = 0;

        private DialogueBox _dialogueBox;

        string _filename;

        private bool _finished = false;

        public bool Finished
        {
            get { return _finished; }
            set { _finished = value; }
        }

        public Cutscene(string filename)
        {
            _filename = filename;
        }

        public void LoadContent(ContentManager content)
        {
            // format for text file data is commented above the class

            // Reading in all the data into this string
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));

            var lines = data.Split('\n');
            _backgrounds = new Texture2D[int.Parse(lines[0])]; // First line is amount of backgrounds there are

            var secondLine = lines[1].Split(","); // Second line is the filenames of the backgrounds

            // loading the backgrounds
            for (int i = 0; i < _backgrounds.Length; i++)
            {
                string backgroundTXT = secondLine[i].Trim();
                _backgrounds[i] = content.Load<Texture2D>(backgroundTXT);
            }

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

        /// <summary>
        /// Method to update the dialogue index when game is loaded
        /// </summary>
        public void UpdateDialogueIndex(int dialogueGroupIndex, int dialogueGroupMessageIndex)
        {
            _dialogueBox.DialogueGroup = dialogueGroupIndex;
            _dialogueBox.DialogueIndex = dialogueGroupMessageIndex;
        }

        /// <summary>
        /// Receive the group index
        /// </summary>
        /// <returns>group index</returns>
        public int GetDialogueGroupIndex()
        {
            return _dialogueBox.DialogueGroup;
        }

        /// <summary>
        /// Receive the dialogue message index
        /// </summary>
        /// <returns>group message index</returns>
        public int GetDialogueIndex()
        {
            return _dialogueBox.DialogueIndex;
        }

        public void Update(GameTime gameTime)
        {
            _dialogueBox.Update(gameTime);
            
            // Changes the background being displayed if the group changes
            int dialogueGroupIndex = _dialogueBox.RequestDialogueGroup();
            if (_backgroundIterator < dialogueGroupIndex)
            {
                _backgroundIterator = dialogueGroupIndex;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgrounds[_backgroundIterator], new Vector2(0, 0), Color.White);
            _dialogueBox.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
