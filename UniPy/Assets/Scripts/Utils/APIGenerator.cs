using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GameTools
{
    public class APIGenerator
    {
        private static Dictionary<Type, List<MethodInfo>> type2ExtendMethod = new Dictionary<Type, List<MethodInfo>>();
        private static string ExportPath = "F:/Learn/Quantum-Tactics/QuantumTactics_Client/Python/PyScripts/lib/";

        private static Dictionary<string, string> TypeMap = new Dictionary<string, string>()
        {
            { "System.String", "str" }, 
            { "System.Void", "None" },
            { "System.Boolean", "bool"},
            { "System.Int8", "int"},
            { "System.Int16", "int"},
            { "System.Int32", "int"},
            { "System.Int64", "int"},
            { "System.Double", "float"},
            { "System.Single", "float"},
        };

        [MenuItem("Tools/GeneratorAPI")]
        public static void Test()
        {
            var types = GetTypes();
            ParseExtendMethod(types);
            
            for(int i = 0; i < types.Length; i ++)
                WriteTypeData(types[i]);
            
        }

        private static void WriteTypeData(Type type)
        {
            var nameSpace = type.Namespace;
            CreateFile(nameSpace, out string filePath);
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError($"no namespace define to type: {type}");
                return;
            }
            // var file = File.OpenWrite(filePath);
            
            // file.Write();
            string code = $"class {type.Name}:\n";
            int count = 0;
            
            // fields
            var fields = 
                type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var fi in fields)
            {
                code += $"\t{fi.Name}: {GetPythonType(nameSpace, fi.FieldType)}\n";
            }

            count += fields.Length;
            
            // properties
            var properties =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static| BindingFlags.DeclaredOnly);
            foreach (var pi in properties)
            {
                code += $"\t@property\n\tdef {pi.Name}(self) -> {GetPythonType(nameSpace, pi.PropertyType)}: ...\n";
            }
            count += properties.Length;

            
            // methods
            var methods =
                (from mi in type.GetMethods(BindingFlags.Public | BindingFlags.Instance| BindingFlags.Static | BindingFlags.DeclaredOnly)
                    where !mi.Name.StartsWith("get_") && !mi.Name.StartsWith("set_")
                    select mi).ToArray();
            List<MethodInfo> extendMethods = null;
            if (!type2ExtendMethod.TryGetValue(type, out extendMethods))
            {
                extendMethods=new List<MethodInfo>();
            }
				
            foreach (var mi in methods)
            {
                var methodCode = "";
                if (mi.IsStatic)
                    methodCode = $"\t@staticmethod\n\tdef {mi.Name}({{0}}) -> {GetPythonType(nameSpace, mi.ReturnType)}: ...\n";
                else
                    methodCode = $"\tdef {mi.Name}(self,{{0}}) -> {GetPythonType(nameSpace, mi.ReturnType)}: ...\n";
                var parameterCode = "";
                // parameters
                var parameterInfos = mi.GetParameters();
                foreach (var pi in parameterInfos)
                {
                    parameterCode += $" {pi.Name}: {GetPythonType(nameSpace, pi.ParameterType)},";
                }

                code += string.Format(methodCode, parameterCode);
            }

            count += methods.Length;
            if (count == 0)
                code += "\tpass\n";
            // foreach (var mi in extendMethods)
            // {
            //     // name
            //     WriteString(writer, mi.Name);
					       //
            //     // 必然视为方法
            //     writer.Write(false);
					       //
            //     // parameters 扩展方法忽略第一个参数
            //     var parameterInfos = mi.GetParameters().Skip(1).ToArray();
            //     writer.Write(parameterInfos.Length);
            //     foreach (var pi in parameterInfos)
            //     {
            //         WriteString(writer, pi.Name);
            //         WriteType(writer, pi.ParameterType);						
            //     }
            //
            //     // returns
            //     WriteType(writer, mi.ReturnType);
            // }
            
            Debug.Log(code);
            code += "\n\n";
            // var bytes = Encoding.Default.GetBytes(code);
            // file.Write(bytes);
            // file.Close();
            FileStream f = new FileStream(filePath, 
                FileMode.OpenOrCreate);
            f.Dispose();
            var w = File.AppendText(filePath);
            w.Write(code);
            w.Close();
        }

        private static string GetPythonType(string classNameSpace, Type type)
        {
            if (classNameSpace == type.Namespace)
                return GetPythonType(type.Name);
            else
            {
                return GetPythonType(type.ToString());
            }
        }

        private static string GetPythonType(string type)
        {
            type = type.Replace("`1", "").Replace("&", "");
            if (type.EndsWith("[]"))
                return $"List[{GetPythonType(type.Substring(0, type.Length - 2))}]";
            
            if (TypeMap.ContainsKey(type))
                return TypeMap[type];
            return type;
        }

        private static void CreateFile(string nameSpace, out string filePath)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                filePath = "";
                return;
            }
            filePath = Path.Join(ExportPath, nameSpace.Replace(".", "/") + ".pyi");
            var dirPath = "";
            var tmp = filePath.Split("/");
            for (int i = 0; i < tmp.Length - 1; i++)
            {
                dirPath = Path.Join(dirPath, tmp[i]);
            }

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            
            // if (!File.Exists(filePath))
            // {
            //     File.Create(filePath);
            // }
        }
        
        private static Type[] GetTypes()
        {
            var types = from assmbly in AppDomain.CurrentDomain.GetAssemblies()
                where !(assmbly.ManifestModule is ModuleBuilder)
                from type in assmbly.GetExportedTypes()
                where type.BaseType != typeof(MulticastDelegate)
                      && !type.IsInterface
                      && !type.IsEnum
                      && !type.IsNested
                      && !IsExcluded(type)
                select type;

            var arr = types.ToArray();
            return arr;
        }

        private static void ParseExtendMethod(Type[] types)
        {
            type2ExtendMethod.Clear();
            foreach (var type in types)
            {
                var extendsMethods =
                    from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    where method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false) 
                    select method;

                foreach (var extendsMethod in extendsMethods)
                {
                    var thisType = extendsMethod.GetParameters()[0].ParameterType;
                    if (type2ExtendMethod.ContainsKey(thisType))
                    {
                        var methods = type2ExtendMethod[thisType];
                        methods.Add(extendsMethod);
                    }
                    else
                    {
                        var methods = new List<MethodInfo>();
                        methods.Add(extendsMethod);
                        type2ExtendMethod.Add(thisType, methods);
                    }
                }
                
            }
        }

        private static bool IsExcluded(Type type)
        {
            // if (type.FullName.StartsWith("System") || type.FullName.StartsWith("Microsoft"))
            //     return true;
            return false;
        }
    }
}