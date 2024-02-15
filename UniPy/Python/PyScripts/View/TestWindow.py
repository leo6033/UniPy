NEED_RELOAD = True

import gg
import UIModule.UIConfig as UIConfig
import UIModule.UIWindow as UIWindow
import UIModule.UINode as UINode


class UI_CLS(UIWindow.UIWindow):
    DataCls = UINode.UINodeData
    WIndowName: str = "TestWindow"
    FullScreen: bool = True
    WindowLayer: UIConfig.Layer = UIConfig.Layer.Normal
    PrefabPath = "UI/TestWindow"

    def __init__(self) -> None:
        super().__init__()

    def OnCreate(self):
        gg.Log.Debug("Create Test window")
