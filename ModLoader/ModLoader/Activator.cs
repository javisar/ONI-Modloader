using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModLoader
{
    public static class Activator
    {
        private static bool activated = false;

        /// <summary>
        /// Activate the mod loader.
        /// Activation is only preformed the first time this is called.
        /// </summary>
        public static void Activate()
        {
            if (!activated)
            {
                ModLoader md = new ModLoader();
                activated = true;
                ModLoader.Start();                
            }
        }
    }
}