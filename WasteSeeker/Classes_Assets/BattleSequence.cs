using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteSeeker.Importers_Processors;

namespace WasteSeeker.Classes_Assets
{
    /// <summary>
    /// Battle sequence class to hold a battle from the game
    /// -> Will hold Texture2D for Background (enemy), Player and NPC information, 
    ///    Texture2D for Player and NPC profiles, Textbox/Dialogue Box showing events that happened,
    ///    Buttons (Fight: Attack, Ability, Heal, Back // Switch // Run: Exits battle).
    ///    ^^^ (Switch will hover over a character to select, once all characters have selected something to do,
    ///             they will all do their attacks on the enemy)
    /// 
    /// -> When do battle sequences happen in the game?
    ///     - Battles occur when the player gets into a "fight" with an enemy
    ///         * This "fight" refers to any point an enemy decides to fight the player
    ///         * This could be through answering the wrong dialogue option, running into the enemy, etc.
    ///         
    /// -> What happens during a battle sequence?
    ///     - Players will control their MC and the NPCs that are along with the party.
    ///     - Players will automatically be hovering over the MC to decide what they should do
    ///         - They may decide to fight which allows the player to attack, use an ability, heal, or go back to the previous button options
    ///         - They may want to switch who is being hovered currently
    ///             * Until all characters present are ready to attack, they will have to decide each character's action
    ///             * Actions will always happen in order of MC -> NPC1 -> NPC2 -> NPC3
    ///         - If the player decides to run, they will have a base chance depending on the enemy present
    ///             * Good to note that they may not be able to run if the fight is required (most fights are required)
    ///     - Battle sequences will show the details of the attack through a "Dialogue Box" or "Text Box" on the top of the screen
    /// </summary>
    public class BattleSequence
    {
        // Implement Battle Sequence
        /*
         * FIELDS
         * 
         * Player name
         * Player HP
         * Player Mana
         * 
         * NPC1 name
         * NPC1 HP
         * NPC1 Mana
         * 
         * ETC
         * 
         * 
         * CONSTRUCTOR
         * 
         * BattleSequence -------------------------------------------------------------------------------------------
         * 
         * Sets up the player information
         * Sets up Texture2D for dialogue boxes, player profile boxes, enemy hp bar, and all buttons
         *  ^ Will probably use the Content Processor/Importer
         * METHODS
         * 
         * Battle Method --------------------------------------------------------------------------------------------
         * Initializes/Updates a battle by getting information relating to the battle
         *  - How many allies are in the party with the player
         *  - Player/NPC information (such as health, mana, attack (power), ability (power), heal (yes/no - how much)
         *  - Enemy info. (such as health, attack (power), ability (power), etc.)
         *  - Background of the battle Texture2D
         */


        #region character/enemy info
        private int _numAllies;

        private Player _player;
#nullable enable
        private NPC? _npc1;
        private NPC? _npc2;
        private NPC? _npc3;
#nullable disable
        private Enemy _enemy;

        // Statistical information of player, npcs, and enemy
        private float _playerHealth;
        private float _npcHealth1;
        private float _npcHealth2;
        private float _npcHealth3;
        private float _enemyHealth;

        private float _playerMana;
        private float _npcMana1;
        private float _npcMana2;
        private float _npcMana3;

        private float _playerAttack;
        private float _npcAttack1;
        private float _npcAttack2;
        private float _npcAttack3;
        private float _enemyAttack;

        private Ability _playerAbility;
        private Ability _npcAbility1;
        private Ability _npcAbility2;
        private Ability _npcAbility3;
        #endregion

        #region Texture2Ds
        // "Profile" will refer to the art of the character(s) and enemy
        private Texture2D _battleBackground;

        private Texture2D _enemyProfile;

        private Texture2D _playerProfile;

#nullable enable
        private Texture2D? _npcProfile1;

        private Texture2D? _npcProfile2;

        private Texture2D? _npcProfile3;
#nullable disable
        #endregion

        #region Buttons
        // Main Select
        private Button _fightButton;

        private Button _switchButton;

        private Button _runButton;

        // After "Fight" button selection
        private Button _attackButton;

        private Button _abilityButton;

        private Button _healButton;

        private Button _backButton;
        #endregion

        private Tilemap _tilemap;

        #region Tilemap

        /// <summary>
        /// - The tilemap here will have a size of 20x11 (to cover all facets of the screen
        /// The tiles present in "BattleTileset" are 13 tiles - meaning that tiles 14-16 are un-used(could be later)
        /// </summary>
        public Tilemap Tilemap
        {
            set { _tilemap = value; }
        }
        #endregion

        /// <summary>
        /// For testing purposes
        /// </summary>
        public BattleSequence()
        {
            _tilemap = new Tilemap("battle.txt");
        }

        public void LoadContent(ContentManager content)
        {
            _tilemap.LoadContent(content);
        }

        /// <summary>
        /// Constructor for setting up a new Battle sequence
        /// </summary>
        /// <param name="fightButton">The fight button</param>
        /// <param name="switchButton">The switch button</param>
        /// <param name="runButton">The run button</param>
        /// <param name="attack">The attack button</param>
        /// <param name="ability">The ability button</param>
        /// <param name="heal">The heal button</param>
        /// <param name="back">The back button</param>
        public BattleSequence(
            Button fightButton, Button switchButton, Button runButton,
            Button attack, Button ability, Button heal, Button back
            )
        {
            _fightButton = fightButton;
            _switchButton = switchButton;
            _runButton = runButton;
            _attackButton = attack;
            _abilityButton = ability;
            _healButton = heal;
            _backButton = back;
        }

        public void Battle(int numAllies, Player player, NPC npc1, NPC npc2, NPC npc3, Enemy enemy, Texture2D battleBackgroundTexture)
        {
            // Initialize all needed variables (player info, npc info, world texture, etc.)
            _numAllies = numAllies;
            _player = player;
            _npc1 = npc1;
            _npc2 = npc2;
            _npc3 = npc3;
            _enemy = enemy;
            _battleBackground = battleBackgroundTexture;

            LoadBattleInformation(); // Used to load names, textures, etc. of players and npcs


        }

        /// <summary>
        /// Used to load information received from the player and npc info
        /// </summary>
        public void LoadBattleInformation()
        {
            _playerProfile = _player.BattleTexture;
            _npcProfile1 = _npc1.BattleTexture;
            _npcProfile2 = _npc2.BattleTexture;
            _npcProfile3 = _npc3.BattleTexture;
            
        }

        /// <summary>
        /// Used to update what is happening in the battle
        /// </summary>
        public void UpdateBattle(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Draw method to update and draw the battle sequence of what is happening
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _tilemap.Draw(gameTime, spriteBatch);
        }
    }
}
