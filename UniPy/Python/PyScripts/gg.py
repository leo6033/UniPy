from __future__ import annotations
NEED_RELOAD = True
TYPE_CHECKING = False

from typing import TYPE_CHECKING
if TYPE_CHECKING:
    import UIModule.UIModule as _UI
    import Utils.Event.Event as EventModule
    import Utils.Event.EngineEvent as EngineEventModule
    # import CombatView.CombatMgr as CombatMgr


import Utils.Log.Log as LogModule

Log: LogModule.Log = LogModule.Log()
UI: _UI.UIModule = None
EngineEvent: EngineEventModule.Event = None
Event: EventModule.Event = None
# Combat: CombatMgr.CombatMgr = None
