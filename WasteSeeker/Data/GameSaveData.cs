using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker.Data
{
    public class GameSaveData
    {
        /// player
        public float playerX { get; set; }
        public float playerY { get; set; }
        public int playerHealth { get; set; }
        
        // npc1
        public float npc1X { get; set; }
        public float npc1Y { get; set; }
        public int npc1Health { get; set; }

        // cutscenes
        // C1
        public int groupOneIndex { get; set; }
        public int groupOneMessageIndex { get; set; }
        public bool cutsceneOneOver {  get; set; }

        // C2
        public int groupTwoIndex { get; set; }
        public int groupTwoMessageIndex { get; set; }
        public bool cutsceneTwoOver { get; set; }

        // Rewards
        public bool rewardOne { get; set; }
        public bool rewardTwo { get; set; }

        // Dialogues
        public bool dialogueOneFinish { get; set; }
        public bool dialogueTwoFinish { get;set; }

        // Level State
        public int levelState { get; set; }
    }
}
