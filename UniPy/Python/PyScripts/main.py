NEED_RELOAD = True

import gg
import UnityEngine
import Disc0ver

def main():
    gg.Log.SetLogImpl(Disc0ver.Engine.Log)

    import UIModule.UIModule as UIModule
    gg.UI = UIModule.UIModule()
    gg.UI.Initialization()

    import Utils.Event.EngineEvent as EngineEventModule
    gg.EngineEvent = EngineEventModule.Event()

    import Utils.Event.Event as EventModule
    gg.Event = EventModule.Event()
    
    # import CombatView.CombatMgr as CombatMgr
    # gg.Combat = CombatMgr.CombatMgr()

    import test

