namespace ModLoader
{
    public static class Activator
    {
        private static bool activated;

        /// <summary>
        /// Activate the mod loader.
        /// Activation is only preformed the first time this is called.
        /// </summary>
        public static void Activate()
        {
            if (!activated)
            {
                activated = true;
                ModLoader.Start();                
            }
        }
    }
}