NEED_RELOAD = True

import UnityEngine
import Disc0ver
import System
import Python.Runtime as Runtime
from System import Action
import gg

class Event(object):

    def __init__(self) -> None:
        pass

    def AddListener(self, key, func):
        
        handler = Action[Runtime.PyTuple, Runtime.PyDict](lambda args, kwargs: func(*args, **kwargs))
        if Disc0ver.Engine.GameEvent.AddEventListener[Runtime.PyTuple, Runtime.PyDict](key, handler):
            return handler
        
        return None
    
    def RemoveListener(self, key, handler: Action):
        Disc0ver.Engine.GameEvent.RemoveListener[Runtime.PyTuple, Runtime.PyDict](key, handler)
    
    def BroadCast(self, key, *args, **kwargs):
        gg.Log.Info(args)
        gg.Log.Info(kwargs)
        Disc0ver.Engine.GameEvent.Send[Runtime.PyTuple, Runtime.PyDict](key, args, kwargs)
