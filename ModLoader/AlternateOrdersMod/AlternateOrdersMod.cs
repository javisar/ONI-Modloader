using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AlternateOrdersMod
{    
    
    [HarmonyPatch(typeof(Fabricator), "UpdateOrderQueue", new Type[] { typeof(bool)})]
    internal class AlternateOrdersMod1
    {
        private static void Prefix(Fabricator __instance, bool force_update = false)
        {
            Debug.Log(" === AlternateOrdersMod1 Prefix === "+__instance.ToString());

			FieldInfo userOrders_ = AccessTools.Field(typeof(Fabricator), "userOrders");
            FieldInfo operational_ = AccessTools.Field(typeof(Fabricator), "operational");
            FieldInfo machineOrders_ = AccessTools.Field(typeof(Fabricator), "machineOrders");
            MethodInfo AlreadyMachineQueued = AccessTools.Method(typeof(Fabricator), "AlreadyMachineQueued", new Type[] { typeof(Fabricator.UserOrder) });

            List<Fabricator.UserOrder> userOrders = (List <Fabricator.UserOrder>) userOrders_.GetValue(__instance);
            Operational operational = (Operational) operational_.GetValue(__instance);
            List<Fabricator.MachineOrder> machineOrders = (List<Fabricator.MachineOrder>) machineOrders_.GetValue(__instance);


            if (!force_update && !operational.IsOperational)
			{
				return;
			}
			int num = 0;
			while (num < userOrders.Count && machineOrders.Count < 3)
			{
				Fabricator.UserOrder userOrder = userOrders[num];
				if (!(bool) AlreadyMachineQueued.Invoke(__instance, new object[] { userOrder}) || userOrder.infinite)
				{
					Fabricator.MachineOrder machineOrder = new Fabricator.MachineOrder();
					machineOrder.parentOrder = userOrder;
					machineOrders.Add(machineOrder);
				}
				/*
				if (!userOrder.infinite)
				{
					num++;
				}
				*/
				num++;
			}
			
		}

    }


	[HarmonyPatch(typeof(Refinery), "UpdateOrderQueue", new Type[] { typeof(bool) })]
	internal class AlternateOrdersMod2
	{
		private static void Prefix(Refinery __instance, bool force_update = false)
		{
			Debug.Log(" === AlternateOrdersMod2 Prefix === ");

			PropertyInfo userOrders_ = AccessTools.Property(typeof(Refinery), "userOrders");
			PropertyInfo operational_ = AccessTools.Property(typeof(Refinery), "operational");
			PropertyInfo machineOrders_ = AccessTools.Property(typeof(Refinery), "machineOrders");
			MethodInfo AlreadyMachineQueued = AccessTools.Method(typeof(Refinery), "AlreadyMachineQueued", new Type[] { typeof(Refinery.UserOrder) });

			List<Refinery.UserOrder> userOrders = (List<Refinery.UserOrder>)userOrders_.GetValue(__instance, null);
			Operational operational = (Operational)operational_.GetValue(__instance, null);
			List<Refinery.MachineOrder> machineOrders = (List<Refinery.MachineOrder>)machineOrders_.GetValue(__instance, null);


			if (!force_update && !operational.IsOperational)
			{
				return;
			}
			int num = 0;
			while (num < userOrders.Count && machineOrders.Count < 3)
			{
				Refinery.UserOrder userOrder = userOrders[num];
				if (!(bool)AlreadyMachineQueued.Invoke(__instance, new object[] { userOrder }) || userOrder.infinite)
				{
					Refinery.MachineOrder machineOrder = new Refinery.MachineOrder();
					machineOrder.parentOrder = userOrder;
					machineOrders.Add(machineOrder);
				}
				/*
				if (!userOrder.infinite)
				{
					num++;
				}
				*/
				num++;
			}

		}

	}


}
