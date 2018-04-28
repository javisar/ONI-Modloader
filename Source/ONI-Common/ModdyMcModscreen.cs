using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ONI_Common
{
    public class ModdyMcModscreen
    {
        public KButton Button_ModOptions;

        public void OnPrefabInit(MainMenu __instance)
        {
            this.menu = __instance;
            this.Button_ModOptions.onClick += new System.Action(ModOptions);
        }

        private MainMenu menu;
        // MainMenu
        private void ModOptions()
        {
            if (ModScreen.Instance == null)
            {
                GameObject gameObject = Util.KInstantiateUI(ModScreen.Instance?.gameObject, this.menu.gameObject, true);
                ModScreen component  = gameObject.GetComponent<ModScreen>();
                component.requireConfirmation = false;
                component.SetBackgroundActive(true);
            }

            ModScreen.Instance?.gameObject.SetActive(true);
        }
    }
}
