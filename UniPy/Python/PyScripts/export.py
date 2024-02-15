import shutil
import os


def export(path):
    for dirpath, dirnames, filenames in os.walk(path):
        print(dirpath, dirnames, filenames)
        dst_dirpath = "../../Assets/Resources/PyScripts/" + dirpath
        if not os.path.exists(dst_dirpath):
            os.makedirs(dst_dirpath)
        for filename in filenames:
            src_filepath = os.path.join(dirpath, filename)
            dst_filepath = "../../Assets/Resources/PyScripts/" +  src_filepath + ".txt"
            shutil.copy(src_filepath, dst_filepath)

        for dirname in dirnames:
            export(os.path.join(dirpath, dirname))
            
export(".")

