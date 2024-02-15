from __future__ import annotations

NEED_RELOAD = True
'''
@Description: UINode
@Author: lmw
@Date: 2023-12-3 13:13:40
'''

import UnityEngine
import Disc0ver
import gg
import importlib

class UINodeData(object):

    def __init__(self) -> None:
        self.input_data = {}
        self.output_data = {}

    def RefreshData(self, data):
        self.input_data = data
        self.OnRefreshDataAuto()
        self.OnRefreshData()
    
    def OnRefreshDataAuto(self):
        pass

    def OnRefreshData(self):
        pass

class UINode(object):

    DataCls: UINodeData = None
    PrefabPath = ""  # 预制体路径
    Updateable = False  # 是否更新

    def __init__(self) -> None:
        self.__destroyed = False
        self._index = 0
        self.GameObject = None
        self.RectTransform = None
        self.Name:str = ""
        self.parent: UINode = None
        self.child_nodes: dict[str, UINode] = {}
        self.update_nodes: list[UINode] = []
        self.__visible = False
        self.data: UINodeData = self.DataCls()

        self.Visible = property(self.GetVisible, self.SetVisible)

    def GetVisible(self):
        return self.__visible and self.gameObject.activeSelf
    
    def SetVisible(self, value):
        if self.Visible == value:
            return
        self.GameObject.SetActive(value)
        self.__visible = value
        if value:
            self.OnVisible()
            self.RefreshUI()
        else:
            self.OnHidden()

    def _InitData(self, data: dict):
        self.RefreshData(data)
        if self.Visible:
            self.OnRefreshUI()
            self.OnRefreshUIAuto()

    def __Destroy(self, destroyGo=False):
        if self.__destroyed:
            return
        
        for node in self.child_nodes.values():
            node.__Destroy()
        
        self.OnDestroy()
        self.child_nodes.clear()

        if destroyGo and self.GameObject != None:
            UnityEngine.Object.Destroy(self.GameObject)
        
        self.__destroyed = True

    def OnVisible(self):
        pass

    def OnHidden(self):
        pass

    def OnCreate(self):
        pass

    def OnRefreshUI(self):
        pass

    def OnRefreshUIAuto(self):
        pass

    def OnDestroy(self):
        pass

    def OnUpdate(self):
        pass

    def BindProperty(self):
        pass

    def Update(self):
        if not self.Visible or self.__destroyed:
            return
        
        for ui_node in self.update_nodes:
            ui_node.Update()

        self.OnUpdate()

    def RefreshUI(self, data:dict=None):
        if data:
            self.RefreshData(data)
        if self.Visible:
            self.OnRefreshUIAuto()
            self.OnRefreshUI()

    def RefreshData(self, data: dict):
        self.data.RefreshData(data)

    def CreateUINode(self, nodeName: str, go: UnityEngine.GameObject, name: str, visible=True):
        if go == None:
            gg.Log.Error(f"[Engine][UINode][CreateUINode] create node fail, game object is null, node: {nodeName}")
            return None
        
        module = importlib.import_module(f"View.{nodeName}")
        if module == None:
            gg.Log.Error(f"[Engine][UINode][CreateUINoe] create node fail, module is None, node: {nodeName}")
            return None
        
        cls = module.__getattr__("UI_CLS")
        if cls == None:
            gg.Log.Error(f"[Engine][UINode][CreateUINoe] create node fail, cls is None, node: {nodeName}")
            return None
        
        node: UINode = cls()
        name = name if name else go.name
        if self.child_nodes.get(name):
            gg.Log.Warning(f"[Engine][UIElement][AddUIElement] element key {go.name} already exist, check is any child game object with same name")
            name = f"{name}_{self._index}"
            self._index += 1
        
        node.Name = name
        node.GameObject = go
        node.parent = self
        node.__visible = go.activeSelf

        self.child_nodes[name] = node
        if(node.Updateable):
            self.update_nodes.append(node)
        
        node.OnCreate()
        node.Visible = visible
        return node

    def RemoveNodeWithName(self, nodeName: str):
        node = self.child_nodes.get(nodeName, None)
        if not node:
            gg.Log.Warning(f"[Engine][UINode][CreateUINode] {nodeName} not exist in child, this element {self.GameObject.name}")
            return
        
        self.child_nodes[nodeName].__Destroy()
        del self.child_nodes[nodeName]
        self.update_nodes.remove(node)
    
    def RemoveNode(self, node: UINode):
        self.RemoveNodeWithName(node.Name)

