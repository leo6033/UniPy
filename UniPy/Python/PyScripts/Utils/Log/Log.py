class Log(object):

    def __init__(self) -> None:
        self._logImpl = None

    def SetLogImpl(self, logImpl):
        self._logImpl = logImpl

    def FormatMsg(self, msg):
        return f"[Python] - {msg}"

    def Debug(self, msg):
        new_msg = self.FormatMsg(msg)
        if self._logImpl:
            self._logImpl.Debug(new_msg)
        else:
            print(new_msg)
    
    def Info(self, msg):
        new_msg = self.FormatMsg(msg)
        if self._logImpl:
            self._logImpl.Info(new_msg)
        else:
            print(new_msg)
    
    def Error(self, msg):
        new_msg = self.FormatMsg(msg)
        if self._logImpl:
            self._logImpl.Error(new_msg)
        else:
            print(new_msg)

    def Warning(self, msg):
        new_msg = self.FormatMsg(msg)
        if self._logImpl:
            self._logImpl.Warning(new_msg)
        else:
            print(new_msg)
    
    def Fatal(self, msg):
        new_msg = self.FormatMsg(msg)
        if self._logImpl:
            self._logImpl.Fatal(new_msg)
        else:
            print(new_msg)
