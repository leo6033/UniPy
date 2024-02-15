NEED_RELOAD = True
import enum
import UnityEngine

class UIModuleConfig:
    WindowShowLayer = UnityEngine.LayerMask.NameToLayer("UI")
    WindowHideLayer = UnityEngine.LayerMask.NameToLayer("Ignore Raycast")
    LayerDeep = 2000
    WindowDeep = 100


class WindowType():
    Normal = 1

class Layer():
    Bottom = 0
    Normal = 1
    Top = 2
    Tip = 3
    System = 4
