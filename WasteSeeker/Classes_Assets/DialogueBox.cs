using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// This class will be used to keep track of a Dialogue box object
    ///     It contains:
    ///         A background for the dialogue box
    ///         Text to display
    /// </summary>
    public class DialogueBox
    {
        private Texture2D _backgroundTexture; // Background of a dialogue box will be 400x200 (may need to change later)

        private Vector2 _position = new Vector2(50, 520);

        // Each row is a different set of strings that correspond to text to display
        private string[][] _dialogue;

        private int[] _dialogueTextPerGroup;

        private SpriteFont _schoolBell;

        private string _textDisplayed = "";

        private int _dialogueGroup = 0;

        private int _dialogueIndex = 0;

        private int _currentStringIndex = 0;

        private float _timer = 0f;

        private float _timerStep = 0.025f;

        private Rectangle _source = new Rectangle(0, 0, 1180, 200);

        private Vector2 _typingTextPosition;

        private Vector2 _continueTextPosition;

        public int DialogueGroup
        {
            get { return _dialogueGroup; }
            set { _dialogueGroup = value; }
        }

        public int DialogueIndex
        {
            get { return _dialogueIndex; }
            set { _dialogueIndex = value; }
        }

        /// <summary>
        /// Dialogue box to display text during cutscenes, gameplay, etc.
        /// </summary>
        /// <param name="dialogue">The dialogue text to be displayed in sequential order</param>
        public DialogueBox(string[][] dialogue, int[] dialogueTextPerGroup)
        {
            _dialogue = dialogue;
            _typingTextPosition = _position + new Vector2(20, 20);
            _continueTextPosition = _position + new Vector2(800 , 140);
        }

        public void LoadContent(ContentManager content)
        {
            _backgroundTexture = content.Load<Texture2D>("DialogueBox_Background");
            _schoolBell = content.Load<SpriteFont>("schoolBell-");
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Rough Explanation of this if statement
            // If the timer is greater than the delay && the current index of the string being displayed is 
            // less than the length of the string to display... then we set the displayed string as the substring of 
            // the current text to display at 0 to the current string index.
            if (_timer >= _timerStep && _currentStringIndex < _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {
                _currentStringIndex++;
                _textDisplayed = _dialogue[_dialogueGroup][_dialogueIndex].Substring(0, _currentStringIndex);
                _timer = 0f;
            }
        }

        /// <summary>
        /// Group stays the same, but index iterates to next text to display
        /// </summary>
        public bool UpdateDialogueBoxIndex()
        {
            // Text hasn't finished being written to screen
            // so we must display all of the text
            if (_currentStringIndex < _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {
                _currentStringIndex = _dialogue[_dialogueGroup][_dialogueIndex].Length;
                _textDisplayed = _dialogue[_dialogueGroup][_dialogueIndex];
            }

            // Text is already entirely written, so we must go onto the next text to write
            else if (_currentStringIndex >= _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {
                _timer = 0f;
                _currentStringIndex = 0;
                _textDisplayed = "";

                // Checking if next index of text to display within the group will be out of bounds
                int nextIndex = _dialogueIndex + 1;
                if (nextIndex >= _dialogue[_dialogueGroup].Length)
                {
                    _dialogueIndex = 0;

                    //Checking if next index of next group will be out of bounds
                    int nextGroup = _dialogueGroup + 1;
                    if (nextGroup < _dialogue.Length) { _dialogueGroup++; }
                    else { _dialogueGroup = 0; return true; }
                }
                else
                {
                    // More within the row of the dialogue group, so increase index to next text
                    _dialogueIndex++;
                }
            }

            return false; // return false indicating more is _dialogue[][] to be written
        }

        /// <summary>
        /// Method to return the position of which group of dialogue text is being displayed
        /// </summary>
        /// <returns>_dialogueIndex</returns>
        public int RequestDialogueGroup()
        {
            return _dialogueGroup;
        }

        /// <summary>
        /// Method to return whether or not the dialogue box is still writing
        /// </summary>
        /// <returns>True if dialogue is still being written, false otherwise</returns>
        public bool RequestIfWriting()
        {
            if (_currentStringIndex < _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {
                return true;
            }
            return false;
        }

        public bool RequestIfFinished()
        {
            int nextGroup = _dialogueGroup + 1;
            if (_currentStringIndex >= _dialogue[_dialogueGroup][_dialogueIndex].Length && nextGroup >= _dialogue.Length)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draw method for a normal dialogue box
        /// - Will mainly cover voice-overs or narration (not dialogue between characters)
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Background Texture
            spriteBatch.Draw(_backgroundTexture, _position, _source, Color.White);
            _textDisplayed = _textDisplayed.Replace('@', '\n');

            // Text to display
            spriteBatch.DrawString(
                        _schoolBell,
                        _textDisplayed,
                        _typingTextPosition,
                        Color.White,
                        0,
                        Vector2.Zero,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );

            // Continue button at bottom
            if (_currentStringIndex >= _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {

                spriteBatch.DrawString(
                        _schoolBell,
                        "Press 'Space' to Continue...",
                        _continueTextPosition,
                        Color.White,
                        0,
                        Vector2.Zero,
                        (float)0.75,
                        SpriteEffects.None,
                        1
                    );
            }
        }

        public void Draw(SpriteBatch spriteBatch, string characterName, Color characterNameColor)
        {
            //Background Texture (altered)
            Rectangle alteredSource = new Rectangle(0, 0, 980, 200);
            Vector2 alteredPosition = _position + new Vector2(200, 0);
            spriteBatch.Draw(_backgroundTexture, alteredPosition, alteredSource, Color.White);

            Vector2 namePosition = _typingTextPosition + new Vector2(200, 0);
            spriteBatch.DrawString(
                        _schoolBell,
                        characterName,
                        namePosition,
                        characterNameColor,
                        0,
                        Vector2.Zero,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );
            
            Vector2 colonPosition = namePosition;
            colonPosition.X += _schoolBell.MeasureString(characterName).X;
            spriteBatch.DrawString(
                        _schoolBell,
                        ":",
                        colonPosition,
                        Color.White,
                        0,
                        Vector2.Zero,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );

            _textDisplayed = _textDisplayed.Replace('@', '\n');

            Vector2 textPosition = namePosition + new Vector2(120, 0);
            // Text to display
            spriteBatch.DrawString(
                        _schoolBell,
                        _textDisplayed,
                        textPosition,
                        Color.White,
                        0,
                        Vector2.Zero,
                        (float)1,
                        SpriteEffects.None,
                        1
                    );

            // Continue button at bottom
            if (_currentStringIndex >= _dialogue[_dialogueGroup][_dialogueIndex].Length)
            {

                spriteBatch.DrawString(
                        _schoolBell,
                        "Press 'Space' to Continue...",
                        _continueTextPosition,
                        Color.White,
                        0,
                        Vector2.Zero,
                        (float)0.75,
                        SpriteEffects.None,
                        1
                    );
            }
        }
    }
}
