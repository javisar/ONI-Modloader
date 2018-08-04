namespace Injector
{
	using Klei.AI;
	using Mono.Cecil;
	using Mono.Cecil.Cil;
	using Mono.Collections.Generic;
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public static class Injector
	{
		public static void Inject(ModuleDefinition module, AssemblyDefinition game, string className, string methodName, string outputPath)
		{
			TypeDefinition launchInitializer = game.MainModule.GetType(string.Empty, className);

			if (launchInitializer == null)
			{
				ModLogger.WriteLine(ConsoleColor.Red, className + " not found");
				Console.Read();
				return;
			}

			MethodDefinition launchInitializerAwake = launchInitializer.Methods.FirstOrDefault(method => method.Name == methodName);

			if (launchInitializerAwake == null)
			{
				ModLogger.WriteLine(ConsoleColor.Red, className+"."+ methodName+" not found");
				Console.Read();
				return;
			}

			ILProcessor p = launchInitializerAwake.Body.GetILProcessor();
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
			// i.Insert(0, p.Create(OpCodes.Call, Util.ImportMethod<Application>(game, "get_dataPath")));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Call,
							  ImportMethod<Assembly>(game, "GetExecutingAssembly")));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Callvirt,
							  ImportMethod<Assembly>(game, "get_Location")));

			// i.Insert(2, p.Create(OpCodes.Call, Util.ImportMethod<Path>(game, "GetDirectoryName", typeof(string))));
			Type[] types = { typeof(string) };
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Call,
							  game.MainModule.ImportReference(typeof(Path).GetMethod("GetDirectoryName", types))));

			// i.Insert(3, p.Create(OpCodes.Stloc_0));
			i.Insert(index++, p.Create(OpCodes.Ldstr, Path.DirectorySeparatorChar+"ModLoader.dll"));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Call,
							  ImportMethod<string>(
																				 game,
																				 "Concat",
																				 typeof(string),
																				 typeof(string))));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Call,
							  ImportMethod<Assembly>(game, "LoadFrom", typeof(string))));

			// .GetType("spaar.ModLoader.Internal.Activator()
			i.Insert(index++, p.Create(OpCodes.Ldstr, "ModLoader.Activator"));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Callvirt,
							  ImportMethod<Assembly>(game, "GetType", typeof(string))));

			// .GetMethod("Activate")
			i.Insert(index++, p.Create(OpCodes.Ldstr, "Activate"));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Callvirt,
							 ImportMethod<Type>(game, "GetMethod", typeof(string))));

			// .Invoke(null, null);
			i.Insert(index++, p.Create(OpCodes.Ldnull));
			i.Insert(index++, p.Create(OpCodes.Ldnull));
			i.Insert(
					 index++,
					 p.Create(
							  OpCodes.Callvirt,
							  ImportMethod<MethodBase>(
																					 game,
																					 "Invoke",
																					 typeof(object),
																					 typeof(object[]))));
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


			//game.MainModule.Types.Add(CecilHelper.GetTypeDefinition(CecilHelper.GetModule("Injector2.exe", Directory.GetCurrentDirectory()+Path.DirectorySeparatorChar), "Radioactive"));


			/*
			MethodDefinition ctor = diseases.Methods.FirstOrDefault(method => method.Name == ".ctor");			
			ILProcessor p2 = ctor.Body.GetILProcessor();
			Collection<Instruction> ix = p2.Body.Instructions;
			
			index = 0;
			ix.Insert(
					 index++,
					 p.Create(
							  OpCodes.Ldarg_0));

			
			ix.Insert(
					 index++,
					 p.Create(
							  OpCodes.Newobj,
							  ImportMethod<string>(
																				 game,
																				 ".ctor",
																				 typeof(ResourceSet))));
			ix.Insert(
					 index++,
					 p.Create(
							  OpCodes.Call,
							  ImportMethod<string>(
																				 game,
																				 "Add",
																				 typeof(Klei.AI.Disease))));
			ix.Insert(
					 index++,
					 p.Create(
							  OpCodes.Stfld,
							  ImportField<string>(
																				 game,
																				 "Radioactive")));

			ix.Insert(
					 index++,
					 p.Create(
							  OpCodes.Ldarg_0));
			*/
			game.Write(outputPath);
		}
		public static MethodReference ImportMethod<T>(AssemblyDefinition assembly, string name)
		{
			return assembly.MainModule.ImportReference(typeof(T).GetMethod(name, Type.EmptyTypes));
		}

		public static MethodReference ImportMethod<T>(AssemblyDefinition assembly, string name, params Type[] types)
		{
			return assembly.MainModule.ImportReference(typeof(T).GetMethod(name, types));
		}

		public static FieldReference ImportField<T>(AssemblyDefinition assembly, string name)
		{
			return assembly.MainModule.ImportReference(typeof(T).GetField(name));
		}

		public static MethodReference ImportMethod(
		AssemblyDefinition assembly,
		string type,
		string method,
		params Type[] types)
		{
			TypeReference reference = assembly.MainModule.Types.First(t => t.Name == type);
			return assembly.MainModule.ImportReference(reference.Resolve().Methods.First(m => m.Name == method));
		}
	}

}