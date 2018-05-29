using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;


namespace InstantResearch
{

	[HarmonyPatch(typeof(ResearchEntry), "OnResearchClicked")]
	internal class InstantResearchMod
	{
		private static bool instantBuildMode = false;

		private static void Prefix(ResearchEntry __instance)
		{
			Debug.Log(" === ResearchEntry.OnResearchClicked Prefix === ");
			instantBuildMode = DebugHandler.InstantBuildMode;
			DebugHandler.InstantBuildMode = true;
		}

		private static void Postfix(ResearchEntry __instance)
		{
			Debug.Log(" === ResearchEntry.OnResearchClicked Postfix === ");
			DebugHandler.InstantBuildMode = instantBuildMode;

		}
		/*
		ManagementMenu.Instance.CheckResearch(null);
		 * 
		 * */

		[HarmonyPatch(typeof(ManagementMenu), "ConfigureToggle", new Type[] {typeof(KToggle), typeof(bool), typeof(bool), typeof(string), typeof(TextStyleSetting) })]
		internal class ManagementMenuMod
		{
			
			private static void Prefix(ManagementMenu __instance, ref KToggle toggle, ref bool disabled, ref bool active, ref string tooltip, ref TextStyleSetting tooltip_style)
			{
				Debug.Log(" === ManagementMenu.ConfigureToggle Postfix === ");

				//__instance.CheckResearch(null);
				//this.ConfigureToggle(this.researchInfo.toggle, flag, active, tooltip, base.ToggleToolTipTextStyleSetting);
				disabled = false;
				//active = true;
				tooltip = UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH + " " + GameUtil.GetHotkeyString(Action.ManageResearch);
			}

		}
		/*
		[HarmonyPatch(typeof(ManagementMenu), "ReseachAvailable")]
		internal class ManagementMenuMod2
		{

			private static bool Prefix(ManagementMenu __instance, ref bool __result)
			{
				Debug.Log(" === ManagementMenu.ReseachAvailable Prefix === ");
				__result = true;
				return false;
			}

		}
		*/
	}
}
	
	
	