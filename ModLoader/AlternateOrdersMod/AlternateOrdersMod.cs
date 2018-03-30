using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AlternateOrdersMod
{

    [HarmonyPatch(typeof(Fabricator), "OnCompleteWork", new Type[] { typeof(Worker) })]
    internal class AlternateOrdersMod2a
    {
        private static void Postfix(Fabricator __instance, Worker worker)
        {
            Debug.Log(" === AlternateOrdersMod2a Postfix === ");
            try
            {
                FieldInfo userOrders_ = AccessTools.Field(typeof(Fabricator), "userOrders");
                //FieldInfo operational_ = AccessTools.Field(typeof(Fabricator), "operational");
                FieldInfo isCancellingOrder_ = AccessTools.Field(typeof(Fabricator), "isCancellingOrder");
                FieldInfo machineOrders_ = AccessTools.Field(typeof(Fabricator), "machineOrders");
                //MethodInfo AlreadyMachineQueued = AccessTools.Method(typeof(Fabricator), "AlreadyMachineQueued", new Type[] { typeof(Fabricator.UserOrder) });
                //MethodInfo CompleteOrder = AccessTools.Method(typeof(Fabricator), "CompleteOrder", new Type[] { typeof(Fabricator.UserOrder) });
                //MethodInfo UpdateOrderQueue = AccessTools.Method(typeof(Fabricator), "UpdateOrderQueue", new Type[] { typeof(bool) });
                //MethodInfo OnCompleteWork = AccessTools.Method(typeof(Workable), "OnCompleteWork", new Type[] { typeof(Worker) });

                List<Fabricator.UserOrder> userOrders = (List<Fabricator.UserOrder>)userOrders_.GetValue(__instance);
                //Operational operational = (Operational)operational_.GetValue(__instance);
                bool isCancellingOrder = (bool)isCancellingOrder_.GetValue(__instance);
                List<Fabricator.MachineOrder> machineOrders = (List<Fabricator.MachineOrder>)machineOrders_.GetValue(__instance);


                if (!isCancellingOrder)
                {
                    if (machineOrders.Count > 0)
                    {
                        Fabricator.MachineOrder machineOrder = machineOrders[0];
                        if (machineOrder.parentOrder.infinite)
                        {
                            Fabricator.UserOrder last = userOrders[0];
                            userOrders.RemoveAt(0);
                            userOrders.Add(last);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                Debug.Log(ex.StackTrace);
            }
        }

    }

    [HarmonyPatch(typeof(Refinery), "OnCompleteWork", new Type[] { typeof(Worker) })]
    internal class AlternateOrdersMod2b
    {        
        private static void Postfix(Refinery __instance, Worker worker)
        {
            Debug.Log(" === AlternateOrdersMod2b Postfix === ");
            try
            {
                FieldInfo userOrders_ = AccessTools.Field(typeof(Refinery), "userOrders");
                //FieldInfo operational_ = AccessTools.Field(typeof(Refinery), "operational");
                FieldInfo isCancellingOrder_ = AccessTools.Field(typeof(Refinery), "isCancellingOrder");
                FieldInfo machineOrders_ = AccessTools.Field(typeof(Refinery), "machineOrders");
                //MethodInfo AlreadyMachineQueued = AccessTools.Method(typeof(Refinery), "AlreadyMachineQueued", new Type[] { typeof(Refinery.UserOrder) });
                //MethodInfo CompleteOrder = AccessTools.Method(typeof(Refinery), "CompleteOrder", new Type[] { typeof(Refinery.UserOrder) });
                //MethodInfo UpdateOrderQueue = AccessTools.Method(typeof(Refinery), "UpdateOrderQueue", new Type[] { typeof(bool) });
                //MethodInfo OnCompleteWork = AccessTools.Method(typeof(Workable), "OnCompleteWork", new Type[] { typeof(Worker) });

                List<Refinery.UserOrder> userOrders = (List<Refinery.UserOrder>)userOrders_.GetValue(__instance);
                //Operational operational = (Operational)operational_.GetValue(__instance);
                bool isCancellingOrder = (bool)isCancellingOrder_.GetValue(__instance);
                List<Refinery.MachineOrder> machineOrders = (List<Refinery.MachineOrder>)machineOrders_.GetValue(__instance);


                if (!isCancellingOrder)
                {
                    if (machineOrders.Count > 0)
                    {
                        Refinery.MachineOrder machineOrder = machineOrders[0];
                        if (machineOrder.parentOrder.infinite)
                        {
                            Refinery.UserOrder last = userOrders[0];
                            userOrders.RemoveAt(0);
                            userOrders.Add(last);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                Debug.Log(ex.StackTrace);
            }
        }
        
    }
    
}
