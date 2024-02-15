import sys
import UnityEngine

_orig_stdout = None

class FakeStdout(object):
    def __init__(self, write_method, flush_method):
        super().__init__()
        self.write_method = write_method
        self.flush_method = flush_method
    
    def write(self, msg):
        self.write_method(msg)
    
    def flush(self):
        self.flush_method()


class Logger(object):
    def __init__(self):
        try:
            import clr
            clr.AddReference("System")
            import System as cssys
            import Disc0ver
            self.ternimal = FakeStdout(cssys.Console.Out.Write, cssys.Console.Out.Flush)
        except ModuleNotFoundError:
            self.ternimal = sys.stdout
        self.log = Disc0ver.Engine.Log.Info
    
    def write(self, message):
        try:
            self.ternimal.write(message)
        except IOError:
            pass
        self.log(message)
    
    def flush(self):
        try:
            self.ternimal.flush()
        except IOError:
            pass

def undo_redirection():
    global _orig_stdout
    sys.stdout = _orig_stdout

def redirect_stdout():
    global _orig_stdout
    _orig_stdout = sys.stdout
    # sys.stdout = Logger()

