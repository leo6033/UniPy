# from collections.abc import Sequence
# from importlib.abc import Loader
# from types import CodeType
# import Disc0ver
# import sys
# import importlib.abc

# class UniPyFinder(importlib.abc.MetaPathFinder):
#     def __init__(self) -> None:
#         self.unipy_importer = Disc0ver.Engine.UniPyImporter

#     def find_module(self, fullname: str, path: Sequence[str] | None) -> Loader | None:
#         if self.unipy_importer.HasModule(fullname):
#             return UniPyLoader()
#         return None

# class UniPyLoader(importlib.abc.SourceLoader):
#     def __init__(self) -> None:
#         self.unipy_importer = Disc0ver.Engine.UniPyImporter

#     def get_code(self, fullname: str) -> CodeType | None:
#         return self.unipy_importer.GetCode(fullname)
    
#     def get_data(self, path: str):
#         pass

#     def get_filename(self, fullname: str) -> str:
#         return fullname + '.py'

# finder = UniPyFinder()
# sys.meta_path.append(finder)
# class UniPyImporter(importlib.abc.SourceLoader):
#     def __init__(self, path):
#         self.unipy_importer = Disc0ver.Engine.UniPyImporter

#     # def find_loader(self, fullname, path=None):
#     #     pass

#     def find_module(self, fullname, path=None):
#         if self.unipy_importer.HasModule(fullname):
#             return self
#         return None

#     # def find_spec(self, fullname, target=None):
#     #     pass

#     def get_code(self, fullname):
#         return self.unipy_importer.GetCode(fullname)

#     # def get_data(self, fullname):
#     #     pass

#     # def get_filename(self, fullname):
#     #     pass

#     # def get_source(self, fullname):
#     #     pass

#     # def is_package(self, fullname):
#     #     pass

#     def load_module(self, fullname):
#         code = self.get_code(fullname)
#         mod = sys.modules.setdefault(fullname, )

#     # def get_resource_reader(self, fullname):
#     #     pass

#     # def invalidate_caches(self):
#     #     pass
