NEED_RELOAD = True

import gg
import collections

class Event(object):

    def __init__(self):
        self.__subscript_dict: dict[str, list[function]] = collections.defaultdict()

    def AddListener(self, eventType, func):
    
        if not self.__subscript_dict.get(eventType):
            self.__subscript_dict[eventType] = []
    
        self.__subscript_dict[eventType].append(func)
    
    def BroadCast(self, eventType, *args, **kwargs):
        subcript_list = self.__subscript_dict.get(eventType)

        if subcript_list:
            for subcript_func in subcript_list:
                subcript_func(*args, **kwargs)
    
    def RemoveListener(self, eventType, func):

        if self.__subscript_dict.get(eventType):
            self.__subscript_dict[eventType].remove(func)
