using System.IO;
using Harmony;

namespace MainMenuResumeGame
{
    [HarmonyPatch(typeof(MainMenu), "RefreshResumeButton")]
    public static class MainMenuResumeGame
    {
        public static void Postfix(MainMenu __instance)
        {
            string currentSave = __instance.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text;
            const int not_Auto_Save = -1;

            if (!string.IsNullOrEmpty(currentSave))
            {
                string latestSaveFile = SaveLoader.GetLatestSaveFile();
                string auto_save_path = Path.GetDirectoryName(SaveLoader.GetAutosaveFilePath());

                if (latestSaveFile.IndexOf(auto_save_path) == not_Auto_Save && latestSaveFile.IndexOf("auto_save") == not_Auto_Save)
                {
                    //Debug.Log("================== MANUAL SAVE DETECTED =================== \n" + latestSaveFile);               
                    __instance.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = "Manual Save - " + currentSave;
                }
                else
                {
                    __instance.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = "Auto Save - " + currentSave;
                    //Debug.Log("================== AUTO SAVE DETECTED =================== \n" + latestSaveFile);
                };
            }
        }
    }
}
