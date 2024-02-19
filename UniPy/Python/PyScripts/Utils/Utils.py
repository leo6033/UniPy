NEED_RELOAD = True

import UnityEngine
import Disc0ver


def GetPyBehaviour(go):
    return go.GetComponent[Disc0ver.Engine.PyBehaviour]()

def IsEditorPlatform():
    platform =  UnityEngine.Application.platform
    return platform == UnityEngine.RuntimePlatform.WindowsEditor or\
            platform == UnityEngine.RuntimePlatform.OSXEditor or\
            platform == UnityEngine.RuntimePlatform.LinuxEditor

def EnableDebug():
    import ptvsd
    ptvsd.enable_attach(("127.0.0.1", 12345))

def BreakDebug():
    import ptvsd
    ptvsd.break_into_debugger()
