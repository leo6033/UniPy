import UnityEngine

class Test():
    
    def Awake(self):
        UnityEngine.Debug.Log("[Behaviour Test] Awake")
        
    def OnDestroy(self):
        UnityEngine.Debug.Log("[Behaviour Test] OnDestroy")
    
    def Start(self):
        UnityEngine.Debug.Log("[Behaviour Test] Start")

class Test1():
    
    def Awake(self):
        UnityEngine.Debug.Log("[Behaviour Test1] Awake")
        
    def OnDestroy(self):
        UnityEngine.Debug.Log("[Behaviour Test1] OnDestroy")
