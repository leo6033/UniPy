NEED_RELOAD = True

import gg
import UnityEngine
import UIModule.UINode as UINode
import UIModule.UIConfig as UIConfig


class UIWindow(UINode.UINode):
    WIndowName: str = ""
    FullScreen: bool = False
    WindowLayer: UIConfig.Layer = UIConfig.Layer.Normal

    def __init__(self) -> None:
        super().__init__()
        self._Canvas = None
        self._create = False
        self._Raycaster = None

        self.Interactable = property(self.GetInteractable, self.SetInteractable)
        self.Depth = property(self.GetDepth, self.SetDepth)
        
    def GetVisible(self):
        return self._Canvas != None and self.GameObject.activeSelf and self.GameObject.layer == UIConfig.UIModuleConfig.WindowShowLayer

    def SetVisible(self, value: bool):
        if self._Canvas == None:
            return
        
        setLayer = UIConfig.UIModuleConfig.WindowShowLayer if value else UIConfig.UIModuleConfig.WindowHideLayer
        if self.GameObject.layer == setLayer:
            return
        
        self.GameObject.layer = setLayer

        if value:
            self.OnVisible()
            self.RefreshUI()
        else:
            self.OnHidden()
    

    def GetInteractable(self):
        return self._Raycaster != None and self._Raycaster.enabled
    
    def SetInteractable(self, value: bool):
        if self._Raycaster == None:
            return
        self._Raycaster.enabled = value

    def GetDepth(self):
        return self._Canvas.sortingOrder if self._Canvas else 0
    
    def SetDepth(self, value: int):
        if self._Canvas == None or self._Canvas.sortingOrder == value:
            return
        self._Canvas.sortingOrder = value
    
    def _InternalCreate(self, uiRoot: UnityEngine.Transform):
        if self._create:
            return
        
        self._create = True
        self.GameObject = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load[UnityEngine.GameObject](self.PrefabPath), uiRoot)
        self.Name = self.WIndowName
        if self.GameObject == None:
            gg.Log.Error(f"[UI][Window][InternalCreate] Create Window {self.WIndowName} fail gameObject is null, path: {self.PrefabPath}")
            self.Close()
    
    def Close(self):
        gg.UI.CloseWIndow(self.WIndowName)
