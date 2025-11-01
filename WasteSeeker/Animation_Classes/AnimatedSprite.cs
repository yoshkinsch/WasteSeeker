using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WasteSeeker.Classes_Assets.Player;

namespace WasteSeeker.Animation_Classes
{
    /// <summary>
    /// Class used to simply display an animated sprite on the screen
    /// </summary>
    public class AnimatedSprite
    {
        private Texture2D _spriteSheet;

        private int _idleMaxFrames;

        private int _walkMaxFrames;

        private int _spriteWidth;

        private int _spriteHeight;

        float _idleFrameStep;

        float _walkFrameStep;

        private float _scaleFactor;

        private double _animationTimer;

        private short _animationFrame = 1;

        private CharacterState _spriteState;

        private SpriteEffects _directionFacing = SpriteEffects.None;

        private Vector2 _position;

        /// <summary>
        /// Public variable to update the position of the sprite on the game screen
        /// </summary>
        public Vector2 Position
        {
            set { _position = value; }
        }

        /// <summary>
        /// Public variable to set the direction facing
        /// </summary>
        public SpriteEffects DirectionFacing
        {
            set { _directionFacing = value; }
        }

        /// <summary>
        /// Public character state to set the state in which the character should be drawn
        /// </summary>
        public CharacterState CharacterState
        {
            set { _spriteState = value; }
        }

        public AnimatedSprite(
            Texture2D spriteSheet, 
            Vector2 Position, 
            int individualSpriteWidth,
            int individualSpriteHeight,
            SpriteEffects directionFacing,
            float scaleFactor,
            float idleFrameStep,
            float walkFrameStep,
            int idleMaxFrames,
            int walkMaxFrames
            )
        {
            _spriteSheet = spriteSheet;
            _position = Position;
            _spriteWidth = individualSpriteWidth;
            _spriteHeight = individualSpriteHeight;
            _directionFacing = directionFacing;
            _scaleFactor = scaleFactor;
            _idleFrameStep = idleFrameStep;
            _walkFrameStep = walkFrameStep;
            _idleMaxFrames = idleMaxFrames;
            _walkMaxFrames = walkMaxFrames;
        }

        /// <summary>
        /// Method used to update the animation timer
        /// </summary>
        /// <param name="updatedTimer">Animation Timer</param>
        public void UpdateAnimationVariables(double updatedTimer, short animationFrame)
        {
            _animationTimer = updatedTimer;
            _animationFrame = animationFrame;
        }

        /// <summary>
        /// Draws the character's sprite on screen
        /// </summary>
        /// <param name="spriteBatch">Sprite batch tool to draw texture</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            Rectangle source;


            switch (_spriteState)
            {
                case CharacterState.Attacking:

                    break;
                case CharacterState.Idle:

                    if (_animationTimer > _idleFrameStep) // usually 0.15
                    {
                        _animationFrame++;
                        if (_animationFrame > _idleMaxFrames) { _animationFrame = 1; } // if _animationFrame > 5
                        _animationTimer -= _idleFrameStep;
                    }
                    source = new Rectangle(_animationFrame * _spriteWidth, 81, _spriteWidth, _spriteHeight); // 48 80
                    spriteBatch.Draw(_spriteSheet, _position, source, Color.White, 0, new Vector2(_spriteWidth/2, _spriteHeight/2), _scaleFactor, _directionFacing, 1);

                    break;
                case CharacterState.Walking: 

                    if (_animationTimer > _walkFrameStep) // usually 0.1
                    {
                        _animationFrame++;
                        if (_animationFrame > _walkMaxFrames) { _animationFrame = 1; } //if  _animationFrame > 11
                        _animationTimer -= _walkFrameStep;
                    }
                    source = new Rectangle(_animationFrame * _spriteWidth, 2, _spriteWidth, _spriteHeight);
                    spriteBatch.Draw(_spriteSheet, _position, source, Color.White, 0, new Vector2(_spriteWidth / 2, _spriteHeight / 2), _scaleFactor, _directionFacing, 1);

                    break;
                case CharacterState.Running:

                    break;
            }
        }
    }
}
