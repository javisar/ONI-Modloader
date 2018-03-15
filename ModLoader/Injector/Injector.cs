using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Injector
{
    public static class Injector
    {

        public static void Inject(AssemblyDefinition game, string outputPath)
        {

            TypeDefinition planetRotate = game.MainModule.GetType("", "Global");

            if (planetRotate == null)
            {
                Console.WriteLine("Global not found");
                Console.Read();
                return; 
            }

            MethodDefinition planetStart = planetRotate.Methods.FirstOrDefault(
              method => method.Name == "Awake");

            if (planetStart == null)
            {
                Console.WriteLine( "Global.Awake not found");
                Console.Read();
                return;
            }

            ILProcessor p = planetStart.Body.GetILProcessor();
            Collection<Instruction> i = p.Body.Instructions;
            /*
            i.Insert(0, p.Create(OpCodes.Nop));
            i.Insert(1, p.Create(OpCodes.Nop));
            i.Insert(2, p.Create(OpCodes.Call, Util.ImportMethod<Assembly>(game, "GetExecutingAssembly")));
            i.Insert(3, p.Create(OpCodes.Callvirt, Util.ImportMethod<Assembly>(game, "get_Location", typeof(string))));
            i.Insert(4, p.Create(OpCodes.Call, Util.ImportMethod<System.IO.Path>(game, "GetDirectoryName")));
            */

            int index = 0;
            // Assembly.LoadFrom(Application.dataPath + "/Mods/ModLoader.dll")
            //i.Insert(0, p.Create(OpCodes.Call, Util.ImportMethod<Application>(game, "get_dataPath")));
            i.Insert(index++, p.Create(OpCodes.Call, spaar.ModLoader.Injector.Util.ImportMethod<Assembly>(game, "GetExecutingAssembly")));
            i.Insert(index++, p.Create(OpCodes.Callvirt, spaar.ModLoader.Injector.Util.ImportMethod<Assembly>(game, "get_Location")));
            //i.Insert(2, p.Create(OpCodes.Call, Util.ImportMethod<Path>(game, "GetDirectoryName", typeof(string))));
            Type[] types = { typeof(string) };
            i.Insert(index++, p.Create(OpCodes.Call, game.MainModule.ImportReference(typeof(Path).GetMethod("GetDirectoryName", types))));

            //i.Insert(3, p.Create(OpCodes.Stloc_0));

            i.Insert(index++, p.Create(OpCodes.Ldstr, "/ModLoader.dll"));
            i.Insert(index++, p.Create(OpCodes.Call, spaar.ModLoader.Injector.Util.ImportMethod<string>(game, "Concat", typeof(string), typeof(string))));
            i.Insert(index++, p.Create(OpCodes.Call, spaar.ModLoader.Injector.Util.ImportMethod<Assembly>(game, "LoadFrom", typeof(string))));
            // .GetType("spaar.ModLoader.Internal.Activator()
            i.Insert(index++, p.Create(OpCodes.Ldstr, "ModLoader.Activator"));
            i.Insert(index++, p.Create(OpCodes.Callvirt, spaar.ModLoader.Injector.Util.ImportMethod<Assembly>(game, "GetType", typeof(string))));
            // .GetMethod("Activate")
            i.Insert(index++, p.Create(OpCodes.Ldstr, "Activate"));
            i.Insert(index++, p.Create(OpCodes.Callvirt, spaar.ModLoader.Injector.Util.ImportMethod<Type>(game, "GetMethod", typeof(string))));
            // .Invoke(null, null);
            i.Insert(index++, p.Create(OpCodes.Ldnull));
            i.Insert(index++, p.Create(OpCodes.Ldnull));
            i.Insert(index++, p.Create(OpCodes.Callvirt, spaar.ModLoader.Injector.Util.ImportMethod<MethodBase>(game, "Invoke", typeof(object), typeof(object[]))));
            i.Insert(index++, p.Create(OpCodes.Pop));

            /*
            // Assembly.LoadFrom(Application.dataPath + "/Mods/SpaarModLoader.dll")
            i.Insert(0, p.Create(OpCodes.Call,      Util.ImportMethod<Application>(game, "get_dataPath")));
            i.Insert(1, p.Create(OpCodes.Ldstr,     "/Mods/SpaarModLoader.dll"));
            i.Insert(2, p.Create(OpCodes.Call,      Util.ImportMethod<string>(game, "Concat", typeof(string), typeof(string))));
            i.Insert(3, p.Create(OpCodes.Call,      Util.ImportMethod<Assembly>(game, "LoadFrom", typeof(string))));
            // .GetType("spaar.ModLoader.Internal.Activator()
            i.Insert(4, p.Create(OpCodes.Ldstr, "spaar.ModLoader.Internal.Activator"));
            i.Insert(5, p.Create(OpCodes.Callvirt,  Util.ImportMethod<Assembly>(game, "GetType", typeof(string))));
            // .GetMethod("Activate")
            i.Insert(6, p.Create(OpCodes.Ldstr, "Activate"));
            i.Insert(7, p.Create(OpCodes.Callvirt,  Util.ImportMethod<Type>(game, "GetMethod", typeof(string))));
            // .Invoke(null, null);
            i.Insert(8, p.Create(OpCodes.Ldnull));
            i.Insert(9, p.Create(OpCodes.Ldnull));
            i.Insert(10, p.Create(OpCodes.Callvirt, Util.ImportMethod<MethodBase>(game, "Invoke", typeof(object), typeof(object[]))));
            i.Insert(11, p.Create(OpCodes.Pop));
            */
            game.Write(outputPath);


        }
    }
}
