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

        // cutscene
        public int groupIndex { get; set; }
        public int groupMessageIndex { get; set; }
        public bool cutsceneOver {  get; set; }
    }
}
