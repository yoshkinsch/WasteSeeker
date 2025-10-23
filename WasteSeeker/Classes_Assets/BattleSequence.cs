using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private string _playerName;

        private float _playerHealth;
#nullable enable
        private string? _npcName1;

        private string? _npcName2;

        private string? _npcName3;

        private float? _npcHealth1;

        private float? _npcHealth2;

        private float? _npcHealth3;
#nullable disable
        private string _enemyName;

        private float _enemyHealth;
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


        public BattleSequence(
            string playerName, int playerHealth, Texture2D playerProfile,
            Button fightButton, Button switchButton, Button runButton,
            Button attack, Button ability, Button heal, Button back
            )
        {
            _playerName = playerName;
            _playerHealth = playerHealth;
            _playerProfile = playerProfile;
            _fightButton = fightButton;
            _switchButton = switchButton;
            _runButton = runButton;
            _attackButton = attack;
            _abilityButton = ability;
            _healButton = heal;
            _backButton = back;
        }

        public void Battle(
            int numAllies,
            string npcName1, float npcHealth1, Texture2D npcProfile1,
            string npcName2, float npcHealth2, Texture2D npcProfile2,
            string npcName3, float npcHealth3, Texture2D npcProfile3,
            string enemyName, float enemyHealth, Texture2D enemyProfile
            )
        {
            // Initialize all needed variables (npc names, textures, etc.)
            _npcName1 = npcName1;
            _npcHealth1 = npcHealth1;
            _npcName2 = npcName2;
            _npcHealth2 = npcHealth2;
            _npcName3 = npcName3;
            _npcHealth3 = npcHealth3;

            _npcProfile1 = npcProfile1;
            _npcProfile2 = npcProfile2;
            _npcProfile3 = npcProfile3;

        }
    }
}
