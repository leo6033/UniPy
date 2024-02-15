# UniPy
unity runtime python script plugin with pythonnet. Write your game script with Python in Unity.

Currently it is possible to build under the windows platform, if you put the Python interpreter and code in the Resource directory. Therefore I think it is also feasible under macos. If we can build Pythonnet for the corresponding platform.



TODOList:
  + support script reload in editor. If you know Unity's package PythonScript you know that if you want to update a script that has been imported, you need to restart Unity Editor, which makes the development process very cumbersome.
  + support macos
  + research build for mobile, android & ios
  + support import python file by unity resource load method, such as Resource, BundleResource, AddressableAsset.

