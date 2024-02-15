NEED_RELOAD = True

import gg
import importlib
import UnityEngine
import Disc0ver
import UnityEngine.Rendering.Universal as URP
import UIModule.UIConfig as UIConfig
import UIModule.UIWindow as UIWindow


class UIModule(object):
    Config = UIConfig.UIModuleConfig
    
    def __init__(self):
        self._GameObject = None
        self._UICamera = None
        self._Canvas = None
        self._stack: list[UIWindow.UIWindow] = []

    def Initialization(self):
        self._GameObject = Disc0ver.Engine.GameManager.Instance.gameObject.transform.Find("UIRoot").gameObject
        self._UICamera = self._GameObject.GetComponentInChildren[UnityEngine.Camera]()
        self._Canvas = self._GameObject.GetComponentInChildren[UnityEngine.Canvas]()

        cameraData = URP.CameraExtensions.GetUniversalAdditionalCameraData(UnityEngine.Camera.main)
        cameraData.cameraStack.Add(self._UICamera)
        self._GameObject.layer = UIConfig.UIModuleConfig.WindowShowLayer

    def Update(self):
        for window in self._stack:
            if window.Visible:
                window.Update()
    
    def GetWindow(self, windowName: str) -> UIWindow.UIWindow:
        for window in self._stack:
            if window.Name == windowName:
                return window
        
        return None

    def HasWIndow(self, windowName) -> bool:
        for window in self._stack:
            if window.Name == windowName:
                return True
        
        return False

    def CloseWindow(self, windowName: str):
        window = self.GetWindow(windowName)
        if window == None:
            return
        
        window.__Destroy(True)
        self.Pop(window)
        self.OnSortWindowDepth(window.WindowLayer)
        self.OnSetWindowVisible()
    
    def ShowWindow(self, windowName, data: dict) -> UIWindow.UIWindow:
        window = self.GetWindow(windowName)

        if window != None:
            self.Pop(window)
            self.Push(window)
            self.OnSortWindowDepth(window.WindowLayer)
            window.RefreshUI(data)
            window.Visible = True
            self.OnSetWindowVisible()
            return window

        module = importlib.import_module(f"View.{windowName}")
        if module == None:
            gg.Log.Error(f"[Engien][UI][ShowWindow] show window fail, module not found, name: {windowName}")
            return None
        
        window: UIWindow.UIWindow = module.UI_CLS()
        gg.Log.Debug(window)
        if window != None:
            self.Push(window)
            window._InternalCreate(self._Canvas.transform)
            window._InitData(data)
            window.Visible = True
            self.OnSortWindowDepth(window.WindowLayer)
            self.OnSetWindowVisible()
        else:
            gg.Log.Error(f"[Engine][UIModule][ShowWindow] create window {windowName} fail")
        
        return window

    def Push(self, window: UIWindow.UIWindow):
        if self.HasWIndow(window.Name):
            gg.Log.Error(f"[Engine][UIModule][Push] window {window.Name} is exist.")
            return
        
        insertInex = -1
        for i in range(len(self._stack)):
            if window.WindowLayer == self._stack[i].WindowLayer:
                insertInex = i + 1
        
        if insertInex == -1:
            for i in range(len(self._stack)):
                if window.WindowLayer > self._stack[i].WindowLayer:
                    insertInex = i + 1
        
        insertInex = max(0, insertInex)
        self._stack.insert(insertInex, window)

    def Pop(self, window: UIWindow.UIWindow):
        self._stack.remove(window)

    def OnSortWindowDepth(self, windowLayer):
        depth = windowLayer * self.Config.LayerDeep
        for i in range(len(self._stack)):
            if self._stack[i].WindowLayer == windowLayer:
                self._stack[i].Depth = depth
                depth += self.Config.WindowDeep

    def OnSetWindowVisible(self):
        isHideNext = False
        for i in range(len(self._stack) - 1, 0, -1):
            window = self._stack[i]
            if isHideNext:
                window.Visible = False
            else:
                window.Visible = True
                if window.FullScreen:
                    isHideNext = True

    def CloseAll(self):
        for window in self._stack:
            window.Close()
