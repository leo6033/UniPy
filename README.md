# UniPy
Unity runtime Python script plugin with [pythonnet](https://github.com/pythonnet/pythonnet). Write your game script with Python in Unity.

Currently it is possible to use under the windows and macos.

## How To Use
You can just copy [Scripts](UniPy/Assets/Scripts) and [Python](UniPy/Python) directory into your project. Run `PythonModule.Initialize()` to init python.

## TODOList:
  - [x] support script reload in editor. If you know Unity's package PythonScript you know that if you want to update a script that has been imported, you need to restart Unity Editor, which makes the development process very cumbersome.
  - [x] support macos
  - [x] c# code hints: Unity menu Tools/GeneratorAPI
  - [x] python debug support
  - [ ] research build for android & ios
  - [ ] support import python file by unity resource load method, such as Resource, BundleResource, AddressableAsset, if necessary
  - [x] Editor config support for python env set and site-packages set

## debug setting
Here I use [ptvsd](https://github.com/microsoft/ptvsd) to support python debugging. You can set config file like this if you use vscode.
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Python Debugger: Remote Attach",
            "type": "debugpy",
            "request": "attach",
            "connect": {
                "host": "127.0.0.1",
                "port": 12345
            },
            "pathMappings": [
                {
                    "localRoot": "${workspaceFolder}/Python/Windows",
                    "remoteRoot": "../../"
                }
            ]
        }
    ]
}
```
