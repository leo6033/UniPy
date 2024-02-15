import sys

NEED_RELOAD = False

def reload():
    modules_to_reload = []

    for key, value in sys.modules.items():
        if hasattr(value, "NEED_RELOAD") and getattr(value, "NEED_RELOAD"):
            modules_to_reload.append(key)
        elif hasattr(value, "__loader__") and hasattr(value.__loader__, "path") and value.__loader__.path.find("PyScripts") > 0:
            modules_to_reload.append(key)

    print("modules need to reload: ", modules_to_reload)

    for key in modules_to_reload:
        del sys.modules[key]
