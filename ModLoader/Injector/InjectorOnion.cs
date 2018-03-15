using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using JetBrains.Annotations;
using Mono.Collections.Generic;

namespace Injector
{
    public class InjectorOnion
    {
        public InjectorOnion(ModuleDefinition onionModule, ModuleDefinition csharpModule, ModuleDefinition firstPassModule)
        {
            this.Initialize(onionModule, csharpModule, firstPassModule);
        }

        [NotNull]

        private ModuleDefinition _onionModule;

        private  ModuleDefinition _csharpModule;
        private  ModuleDefinition _firstPassModule;

        private  MethodInjector _onionToCSharpInjector;

        private  InstructionRemover _csharpInstructionRemover;
        private  Publisher _csharpPublisher;

        public  bool Failed { get; set; }

        private void Initialize(ModuleDefinition onionModule, ModuleDefinition csharpModule, ModuleDefinition firstPassModule)
        {
            this._onionModule = onionModule;

            this._csharpModule = csharpModule;
            this._firstPassModule = firstPassModule;

            this.InitializeHelpers();
        }

        private void InitializeHelpers()
        {
            this._csharpInstructionRemover = new InstructionRemover(this._csharpModule);
            this._csharpPublisher = new Publisher(this._csharpModule);
            this._csharpInstructionRemover = new InstructionRemover(this._csharpModule);

            this.InitializeMethodInjectors();
        }

        private void InitializeMethodInjectors()
        {
            this._onionToCSharpInjector = new MethodInjector(this._onionModule, this._csharpModule);
        }

        // TODO: refactor
        public void Inject()
        {
            //added
            // InjectCore();

            // added
            // if (injectorState.FixLogicBridges)
            //          {
            //      MakeLogicBridgesPlaceableOnLogicGates();
            //            }

            // added
            // if (injectorState.EnableImprovedOxygenOverlay)
            // {
            //     try
            //     {
            //         _csharpInstructionRemover.ClearAllButLast("SimDebugView", "GetOxygenMapColour");
            //         _materialToCSharpInjector.InjectAsFirst("InjectionEntry", "EnterGasOverlay", "SimDebugView", "GetOxygenMapColour", includeArgumentCount: 1);
            //     }
            //     catch (Exception e)
            //     {
            //         Console.WriteLine("Improved gas overlay injection failed");
            //         Console.WriteLine(e);
            //
            //         Failed = true;
            //     }
            // }

            //    if (injectorState.EnableDraggableGUI)

            try
            {
                MethodInjector methodInjector = new MethodInjector(this._onionModule, this._firstPassModule);

                methodInjector.InjectAsFirst("DraggablePanel", "Attach", "KScreen", "OnPrefabInit",
                                             includeCallingObject: true);

                TypeDefinition kScreen     = this._firstPassModule.Types.First(t => t.Name == "KScreen");
                MethodBody     onSpawnBody = kScreen.Methods.First(m => m.Name == "OnSpawn").Body;

                Instruction lastInstruction = onSpawnBody.Instructions.Last();

                methodInjector.InjectBefore("DraggablePanel", "SetPositionFromFile", onSpawnBody, lastInstruction,
                                            includeCallingObject: true);

                Instruction injectedCallFirstInstruction =
                onSpawnBody.Instructions.Last(i => i.OpCode == OpCodes.Ldarg_0);

                foreach (Instruction branch in onSpawnBody.Instructions.Where(i => i.OpCode == OpCodes.Brtrue ||
                                                                                   i.OpCode == OpCodes.Brfalse))
                {
                    branch.Operand = injectedCallFirstInstruction;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Draggable GUI injection failed");
                Console.WriteLine(e);

                Failed = true;
            }

            //  if (injectorState.InjectMaterialColor)

            //InjectMain();
            // MakeFieldPublic intact!
            this.InjectCellColorHandling();

            // Must remain!
            this.InjectBuildingsSpecialCasesHandling();

            // try
            // {
            //     _materialToCSharpInjector.InjectAsFirst("InjectionEntry", "OverlayChangedEntry", "OverlayMenu", "OnOverlayChanged");
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine("OverlayChangedEntry injection failed");
            //     Console.WriteLine(e);
            //
            //     Failed = true;
            // }

            //  if (injectorState.InjectMaterialColorOverlayButton)

            try
            {
                this.InjectToggleButton();
            }
            catch (Exception e)
            {
                {
                    Console.WriteLine("Overlay menu button injection failed");
                    Console.WriteLine(e);

                    Failed = true;
                }
            }

            //   if (injectorState.InjectOnion)
            {
                try
                {
                    this.InjectOnionPatcher();
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnionPatcher injection failed");
                    Console.WriteLine(e);

                    Failed = true;
                }
            }

            this.InjectPatchedSign();
            this.FixGameUpdateExceptionHandling();
        }


        private  void InjectCellColorHandling()
        {
           // _csharpInstructionRemover.ClearAllButLast("BlockTileRenderer", "GetCellColour");

          //  _materialToCSharpInjector.InjectAsFirst(
          //      "InjectionEntry", "EnterCell",
          //      "BlockTileRenderer", "GetCellColour",
          //      true, 1);

            this._csharpPublisher.MakeFieldPublic("BlockTileRenderer", "selectedCell");
            this._csharpPublisher.MakeFieldPublic("BlockTileRenderer", "highlightCell");
            this._csharpPublisher.MakeFieldPublic("BlockTileRenderer", "invalidPlaceCell");

            /*
            var deconstructable = _csharpModule.Types.FirstOrDefault(t => t.Name == "Deconstructable");

            if (deconstructable != null)
            {
                var onCompleteWorkBody = deconstructable.Methods.FirstOrDefault(m => m.Name == "OnCompleteWork")?.Body;

                if (onCompleteWorkBody != null)
                {
                    var lastInstruction = onCompleteWorkBody.Instructions.LastOrDefault();

                    if (lastInstruction != null)
                    {
                        var inserter = new InstructionInserter(onCompleteWorkBody);

                        inserter.InsertBefore(lastInstruction, Instruction.Create(OpCodes.Ldloc_3));
                        _materialToCSharpInjector.InjectBefore("InjectionEntry", "ResetCell", onCompleteWorkBody, lastInstruction);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find last instruction at Deconstructable.OnCompleteWork method");

                        Failed = true;
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't find method at Deconstructable.OnCompleteWork");

                    Failed = true;
                }
            }
            else
            {
                Console.WriteLine("Couldn't find type: Deconstructable");

                Failed = true;
            }
            */
        }

        private  void InjectBuildingsSpecialCasesHandling()
        {
            this._csharpPublisher.MakeFieldPublic("Ownable", "unownedTint");
            this._csharpPublisher.MakeFieldPublic("Ownable", "ownedTint");

            this._csharpPublisher.MakeMethodPublic("Ownable", "UpdateTint");

            this._csharpPublisher.MakeFieldPublic("FilteredStorage", "filterTint");
            this._csharpPublisher.MakeFieldPublic("FilteredStorage", "noFilterTint");

            this._csharpPublisher.MakeFieldPublic("FilteredStorage", "filterable");

            this._csharpPublisher.MakeFieldPublic("StorageLocker", "filteredStorage");
            this._csharpPublisher.MakeFieldPublic("Refrigerator", "filteredStorage");
            this._csharpPublisher.MakeFieldPublic("RationBox", "filteredStorage");
        }

        private  void InjectToggleButton()
        {
            this.MakeTogglePublic();
            this.ExtendMaxActionCount();
          //  this.AddOverlayButtonSprite();
         //   this.InjectKeybindings();
        }

        // TODO: notify user when injection fails
        private  void ExtendMaxActionCount()
        {
            const int MaxActionCount = 1000;

            TypeDefinition kInputController = this._firstPassModule.Types.FirstOrDefault(type => type.Name == "KInputController");

            if (kInputController != null)
            {
                TypeDefinition keyDef = kInputController.NestedTypes.FirstOrDefault(nestedType => nestedType.Name == "KeyDef");

                if (keyDef != null)
                {
                    MethodBody keyDefConstructorBody = keyDef.Methods.First(method => method.Name == ".ctor").Body;

                    keyDefConstructorBody.Instructions.Last(instruction => instruction.OpCode == OpCodes.Ldc_I4)
                        .Operand = MaxActionCount;

                    MethodBody kInputControllerConstructorBody = CecilHelper.GetMethodDefinition(this._firstPassModule, kInputController, ".ctor").Body;

                    kInputControllerConstructorBody.Instructions.First(instruction => instruction.OpCode == OpCodes.Ldc_I4)
                        .Operand = MaxActionCount;
                }
                else
                {
                    Console.WriteLine("Can't find type KInputController.KeyDef");

                    Failed = true;
                }
            }
            else
            {
                Console.WriteLine("Can't find type KInputController");

                Failed = true;
            }
        }

        // TODO: use other sprite, refactor
        private  void MakeTogglePublic()
        {
            this._csharpPublisher.MakeFieldPublic("OverlayMenu", "overlay_toggle_infos");

            var overlayMenu = _csharpModule.Types.First(type => type.Name == "OverlayMenu");

            overlayMenu.NestedTypes.First(nestedType => nestedType.Name == "OverlayToggleInfo").IsPublic = true;


        }

        private  void AddOverlayButtonSprite()
        {
            try
            {
                MethodBody body = this._csharpModule.Types.FirstOrDefault(t => t.Name == "KIconToggleMenu")?.Methods.FirstOrDefault(m => m.Name == "RefreshButtons")?.Body;
                Instruction targetInstruction = body?.Instructions.Reverse().Skip(5).FirstOrDefault(i => i.OpCode == OpCodes.Callvirt);

                InstructionInserter inserter = new InstructionInserter(body);

                inserter.InsertBefore(targetInstruction, Instruction.Create(OpCodes.Ldloc, body?.Variables[5]));

                this._onionToCSharpInjector.InjectBefore("OverlayMenuManager", "EnterToggleSprite", body, targetInstruction, includeCallingObject: true);

            }
            catch (Exception e)
            {
                Console.WriteLine("Toggle change sprite failed");
                Console.WriteLine(e);

                Failed = true;
            }
        }

        /*
         * is disabled in:
         * FrontEndManager.LateUpdate()
         * Game.Update()
         * GraphicsOptionsScreen.Update()
         * OptionsMenuScreen.Update()
         * ReportErrorDialog.Update()
         
        //TODO: make it more flexible for future versions
        // Doesn't work on Tubular Upgrade
        private void EnableConsole()
        {
            try
            {
                _csharpModule
                    .Types.FirstOrDefault(t => t.Name == "Game")
                        .Methods.FirstOrDefault(m => m.Name == "Update")
                            .Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldc_I4_0)
                               .OpCode = OpCodes.Ldc_I4_1;

                _csharpModule
                    .Types.FirstOrDefault(t => t.Name == "FrontEndManager")
                        .Methods.FirstOrDefault(m => m.Name == "LateUpdate")
                            .Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldc_I4_0)
                               .OpCode = OpCodes.Ldc_I4_1;

                _csharpModule
                    .Types.FirstOrDefault(t => t.Name == "GraphicsOptionsScreen")
                        .Methods.FirstOrDefault(m => m.Name == "Update")
                            .Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldc_I4_0)
                               .OpCode = OpCodes.Ldc_I4_1;

                _csharpModule
                    .Types.FirstOrDefault(t => t.Name == "OptionsMenuScreen")
                        .Methods.FirstOrDefault(m => m.Name == "Update")
                            .Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldc_I4_0)
                               .OpCode = OpCodes.Ldc_I4_1;

                _csharpModule
                    .Types.FirstOrDefault(t => t.Name == "ReportErrorDialog")
                        .Methods.FirstOrDefault(m => m.Name == "Update")
                            .Body.Instructions.FirstOrDefault(i => i.OpCode == OpCodes.Ldc_I4_0)
                               .OpCode = OpCodes.Ldc_I4_1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Enable console failed");
                Console.WriteLine(e);

                Failed = true;
            }
        }
        */

        private  void FixGameUpdateExceptionHandling()
        {
            ExceptionHandler handler = new ExceptionHandler(ExceptionHandlerType.Finally);
            MethodBody methodBody = CecilHelper.GetMethodDefinition(this._csharpModule, "Game", "Update").Body;
            Collection<Instruction> methodInstructions = methodBody.Instructions;

            handler.TryStart = methodInstructions.First(instruction => instruction.OpCode == OpCodes.Ldsfld);

            Instruction tryEndInstruction = methodInstructions.Last(instruction => instruction.OpCode == OpCodes.Ldloca_S);

            handler.TryEnd = tryEndInstruction;
            handler.HandlerStart = tryEndInstruction;
            handler.HandlerEnd = methodInstructions.Last();
            handler.CatchType = this._csharpModule.Import(typeof(Exception));

            methodBody.ExceptionHandlers.Clear();
            methodBody.ExceptionHandlers.Add(handler);
        }


        private  void InjectOnionPatcher()
        {
            this.InjectOnionDoWorldGen();
            this.InjectOnionCameraController();
            this.InjectOnionDebugHandler();
            this.InjectOnionInitRandom();
        }

        private  void InjectOnionInitRandom()
        {
            this._onionToCSharpInjector.InjectAsFirst(
                "Hooks", "OnInitRandom",
                "WorldGen", "InitRandom",
                false, 4, true
                );
        }

        private  void InjectOnionDoWorldGen()
        {
            try
            {
                MethodBody doWorldGenInitialiseBody = CecilHelper.GetMethodDefinition(this._csharpModule, "OfflineWorldGen", "DoWordGenInitialise").Body;

                Instruction callResetInstruction = doWorldGenInitialiseBody
                    .Instructions
                    .Where(instruction => instruction.OpCode == OpCodes.Call)
                    .Reverse()
                    .Skip(3)
                    .First();

                InstructionInserter instructionInserter = new InstructionInserter(doWorldGenInitialiseBody);

                instructionInserter.InsertBefore(callResetInstruction, Instruction.Create(OpCodes.Pop));
                instructionInserter.InsertBefore(callResetInstruction, Instruction.Create(OpCodes.Pop));

                this._onionToCSharpInjector.InjectBefore(
                    "Hooks", "OnDoOfflineWorldGen",
                    doWorldGenInitialiseBody,
                    callResetInstruction);

                this._csharpInstructionRemover.ReplaceByNop(doWorldGenInitialiseBody, callResetInstruction);
            }
            catch (Exception e)
            {
                Console.WriteLine("World generation injection failed");
                Console.WriteLine(e);

                Failed = true;
            }
        }

        private  void InjectOnionDebugHandler()
        {

            TypeDefinition debugHandler = this._csharpModule
                .Types
                .FirstOrDefault(type => type.Name == "DebugHandler");

            if (debugHandler != null)
            {
                PropertyDefinition debugHandlerEnabledProperty = debugHandler
                    .Properties
                    .FirstOrDefault(property => property.Name == "enabled");

                if (debugHandlerEnabledProperty != null)
                {
                    debugHandlerEnabledProperty
                        .SetMethod
                        .IsPublic = true;
                }
            }
            else
            {
                Console.WriteLine("Can't find type DebugHandler");

                Failed = true;
            }

            MethodBody debugHandlerConstructorBody = CecilHelper.GetMethodDefinition(this._csharpModule, debugHandler, ".ctor").Body;

            Instruction lastInstruction = debugHandlerConstructorBody.Instructions.Last();

            this._onionToCSharpInjector.InjectBefore("Hooks", "OnDebugHandlerCtor", debugHandlerConstructorBody, lastInstruction);
        }

        private  void InjectOnionCameraController()
        {
            const string TypeName = "CameraController";

            this._csharpPublisher.MakeFieldPublic(TypeName, "maxOrthographicSize");
            this._csharpPublisher.MakeFieldPublic(TypeName, "maxOrthographicSizeDebug");

            MethodBody cameraControllerOnSpawnBody = CecilHelper.GetMethodDefinition(this._csharpModule, TypeName, "OnSpawn").Body;
            Instruction restoreCall = cameraControllerOnSpawnBody.Instructions.Last(instruction => instruction.OpCode == OpCodes.Call);

            this._onionToCSharpInjector.InjectBefore(
                "Hooks",
                "OnCameraControllerCtor",
                cameraControllerOnSpawnBody,
                restoreCall,
                true);
        }




        private  void InjectPatchedSign()
        {
            this._csharpModule.Types.Add(new TypeDefinition("Mods", "Patched", TypeAttributes.Class, this._csharpModule.TypeSystem.Object));
            this._firstPassModule.Types.Add(new TypeDefinition("Mods", "Patched", TypeAttributes.Class, this._firstPassModule.TypeSystem.Object));
        }
    }
}
