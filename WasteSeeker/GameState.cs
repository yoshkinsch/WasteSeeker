using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteSeeker
{
    public enum GameState
    {
        MainMenu,
        Options,
        InGameOptions,
        Credits,
        GameOver,
        Playing,
        Paused,
        Cutscene,
        Transition,
        BattleSequence,
        Controls
    }
}
