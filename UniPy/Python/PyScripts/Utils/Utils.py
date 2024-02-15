NEED_RELOAD = True

import UnityEngine
import Disc0ver


def GetPyBehaviour(go):
    return go.GetComponent[Disc0ver.Engine.PyBehaviour]()

