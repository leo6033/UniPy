import sys

NEED_RELOAD = True

print("test python 112")
import gg
import clr
import Disc0ver
# import GamePlay
import UnityEngine
import System
from System import Action

gg.Log.Info(dir(clr))
gg.Log.Info(UnityEngine.GameObject)
# Disc0ver.Engine.Log.Info(Disc0ver.PythonPlugin.PythonModule.IsInitialized)

def callback():
    gg.Log.Info("save python")

def callback1(value):
    gg.Log.Info(f"save python, value {value}")



# Disc0ver.Engine.GameEvent.AddEventListener(GamePlay.GameEventKey.GameSave, Action(callback))
# Disc0ver.Engine.GameEvent.AddEventListener[bool](GamePlay.GameEventKey.GameSave, Action[bool](callback1))

import View.TestWindow as TestWindow


def get_clr_type(typ):
    name = typ.__module__ + "." + typ.__name__
    gg.Log.Info(f"get_clr_type type: {name}")
    return System.Type.GetType(name)

import Utils.Event.EngineEvent as EngineEvent
import Utils.Event.Event as Event
import UIModule.UIModule as UIModule
gg.EngineEvent = EngineEvent.Event()
gg.Event = Event.Event()
gg.UI = UIModule.UIModule()
gg.UI.Initialization()
# gg.Log.Info(Disc0ver.Engine.GameManager.Instance.GetModule[Disc0ver.Engine.UIModule]())
# uiModule = Disc0ver.Engine.GameManager.Instance.GetModule[Disc0ver.Engine.UIModule]()
# uiModule.ShowWindow[TestWindow.TestWindow]()


def test1():
    gg.Log.Info("test1")

def test2(temp):
    gg.Log.Info(f"test2 {temp}")

def test3(temp, temp1, temp2):
    gg.Log.Info(f"test3 {temp} {temp1} {temp2}")

gg.EngineEvent.AddListener("test", test1)
gg.EngineEvent.AddListener("test1", test2)
gg.EngineEvent.AddListener("test2", test3)

gg.EngineEvent.BroadCast("test")
gg.EngineEvent.BroadCast("test1", "temp")
gg.EngineEvent.BroadCast("test2", "temp", temp2 ="temp1", temp1 ="temp2")

gg.Log.Info("start test Event")

gg.Event.AddListener("test", test1)
gg.Event.AddListener("test1", test2)
gg.Event.AddListener("test2", test3)

gg.Event.BroadCast("test")
gg.Event.BroadCast("test1", "temp")
gg.Event.BroadCast("test2", "temp", temp2 ="temp1", temp1 ="temp2")

gg.UI.ShowWindow("TestWindow", {})
