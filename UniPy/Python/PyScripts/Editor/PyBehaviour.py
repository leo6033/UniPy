NEED_RELOAD = True

import UnityEngine
import importlib
import inspect
import os


def get_script_class(script_path):
    module = importlib.import_module(script_path)
    result = []
    for name, value in module.__dict__.items():
        if inspect.isclass(value):
            result.append(name)

    return result

def get_all_script_path(path):
    result = []
    for dirpath, dirnames, filenames in os.walk(path):
        if dirpath.find("Editor") > 0 or dirpath.find("__pycache__") > 0:
            continue
        for file in filenames:
            file_path = os.path.join(dirpath, file)
            result.append(file_path)
    result = [x.replace(path, "").replace("/", ".")[1:-3] for x in result]
    return result
