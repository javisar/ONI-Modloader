using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Taken from LoadScreen; 
/// Intention: build upon it for the mod screen. What's needed: mod listing, custom mod settings, unique mod format;
/// 
/// Also advised: Each mod should use its own Harmony instance, not the main one from the mod loader. 
/// That way mods can interact with each other, like HarmonyBefore;
/// </summary>
public class ModScreen : KModalScreen
{
    private InspectSaveScreen inspectScreenInstance;

    [SerializeField]
    private KButton saveButtonPrefab;

    [SerializeField]
    private GameObject saveButtonRoot;

    [SerializeField]
    private LocText saveDetails;

    [SerializeField]
    private KButton closeButton;

    [SerializeField]
    private KButton loadButton;

    [SerializeField]
    private KButton deleteButton;

    [SerializeField]
    private KButton moreInfoButton;

    [SerializeField]
    private ColorStyleSetting validSaveFileStyle;

    [SerializeField]
    private ColorStyleSetting invalidSaveFileStyle;

    public Action<string> onClick;

    public bool requireConfirmation = true;

    private UIPool<KButton> saveButtonPool;

    private Dictionary<string, KButton> fileButtonMap = new Dictionary<string, KButton>();

    private ConfirmDialogScreen confirmScreen;

    private string selectedFileName;

    public static ModScreen Instance
    {
        get;
        private set;
    }

    protected override void OnPrefabInit()
    {
        ModScreen.Instance = this;
        base.OnPrefabInit();
        this.saveButtonPool = new UIPool<KButton>(this.saveButtonPrefab);
        if (SpeedControlScreen.Instance != null)
        {
            SpeedControlScreen.Instance.Pause(false);
        }
        if (this.onClick == null)
        {
            this.onClick = new Action<string>(this.SetSelectedGame);
        }
        if (this.closeButton != null)
        {
            this.closeButton.onClick += delegate
            {
                base.Show(false);
            };
        }
        if (this.loadButton != null)
        {
            this.loadButton.onClick += new System.Action(this.Load);
        }
        if (this.deleteButton != null)
        {
            this.deleteButton.onClick += new System.Action(this.Delete);
        }
        if (this.moreInfoButton != null)
        {
            this.moreInfoButton.onClick += new System.Action(this.MoreInfo);
        }
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        this.RefreshFiles();
    }

    // SaveLoader
    public static List<string> GetAllFiles()
    {
        return GetSaveFiles(GetSavePrefixAndCreateFolder());
    }

    // SaveLoader
    public static string GetSavePrefixAndCreateFolder()
    {
        string savePrefix = GetSavePrefix();
        if (!Directory.Exists(savePrefix))
        {
            Directory.CreateDirectory(savePrefix);
        }
        return savePrefix;
    }

    // SaveLoader
    public static string GetSavePrefix()
    {
        string path = Util.RootFolder();
        return Path.Combine(path, "save_files/");
    }

    // SaveLoader
    public static List<string> GetSaveFiles(string save_dir)
    {
        if (!Directory.Exists(save_dir))
        {
            Directory.CreateDirectory(save_dir);
        }
        string[]                       files = Directory.GetFiles(save_dir, "*.sav", SearchOption.AllDirectories);
        List<SaveLoader.SaveFileEntry> list  = new List<SaveLoader.SaveFileEntry>();
        string[]                       array = files;
        for (int i = 0; i < array.Length; i++)
        {
            string text = array[i];
            try
            {
                System.DateTime lastWriteTime = File.GetLastWriteTime(text);
                SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
                                                {
                                                path      = text,
                                                timeStamp = lastWriteTime
                                                };
                list.Add(item);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString(), null);
            }
        }
        list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
        List<string> list2 = new List<string>();
        foreach (SaveLoader.SaveFileEntry current in list)
        {
            list2.Add(current.path);
        }
        return list2;
    }

    private void RefreshFiles()
    {
        this.saveButtonPool?.ClearAll();
        this.fileButtonMap?.Clear();
        List<string> allFiles = GetAllFiles();
        if (allFiles.Count > 0)
        {
            for (int i = 0; i < allFiles.Count; i++)
            {
                this.AddExistingMod(allFiles[i]);
            }
            this.SetSelectedGame(allFiles[0]);
            this.deleteButton.isInteractable = true;
        }
        else
        {
            this.saveDetails.text = "";
            this.deleteButton.isInteractable = false;
            this.loadButton.isInteractable = false;
        }
    }

    protected override void OnShow(bool show)
    {
        base.OnShow(show);
        this.RefreshFiles();
    }

    protected override void OnDeactivate()
    {
        if (SpeedControlScreen.Instance != null)
        {
            SpeedControlScreen.Instance.Unpause(false);
        }
        this.selectedFileName = null;
        base.OnDeactivate();
    }

    private void AddExistingMod(string filename)
    {
        KButton freeElement = this.saveButtonPool.GetFreeElement(this.saveButtonRoot, true);
        freeElement.ClearOnClick();
        LocText componentInChildren = freeElement.GetComponentInChildren<LocText>();
        System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
        componentInChildren.text = string.Format("{0}\n{1:H:mm:ss}\n" + Localization.GetFileDateFormat(1), Path.GetFileNameWithoutExtension(filename), lastWriteTime);
        freeElement.onClick += delegate
        {
            this.onClick(filename);
        };
        bool flag = false;
        try
        {
            SaveGame.Header header;
            flag = (SaveLoader.LoadHeader(filename, out header).saveMajorVersion >= 7);
        }
        catch (Exception ex)
        {
            global::Debug.LogWarning("Corrupted save file: " + filename + "\n" + ex.ToString(), null);
        }
        if (flag)
        {
            freeElement.onDoubleClick += delegate
            {
                this.onClick(filename);
                this.DoLoad();
            };
        }
        ImageToggleState component = freeElement.GetComponent<ImageToggleState>();
        component.colorStyleSetting = ((!flag) ? this.invalidSaveFileStyle : this.validSaveFileStyle);
        component.RefreshColorStyle();
        component.SetState(ImageToggleState.State.Inactive);
        component.ResetColor();
        freeElement.transform.SetAsLastSibling();
        this.fileButtonMap.Add(filename, freeElement);
    }

    public static void ForceStopGame()
    {
        ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
        Game.Instance.SetIsLoading();
        Grid.CellCount = 0;
        Sim.Shutdown();
    }

    private static bool IsSaveFileFromUnsupportedFutureBuild(SaveGame.Header header)
    {
        return header.buildVersion > 260525u;
    }

    private void SetSelectedGame(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            global::Debug.LogError("The filename provided is not valid.", null);
        }
        else
        {
            KButton kButton = (this.selectedFileName == null) ? null : this.fileButtonMap[this.selectedFileName];
            if (kButton != null)
            {
                kButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
            }
            this.selectedFileName = filename;
            kButton = this.fileButtonMap[this.selectedFileName];
            kButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
            this.moreInfoButton.gameObject.SetActive(false);
            try
            {
                SaveGame.Header header;
                SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
                string format = UI.FRONTEND.LOADSCREEN.SAVEDETAILS;
                string text = string.Format("{0:H:mm:ss}\n" + Localization.GetFileDateFormat(0), File.GetLastWriteTime(filename));
                string text2 = Path.GetFileName(filename);
                if (gameInfo.isAutoSave)
                {
                    text2 += UI.FRONTEND.LOADSCREEN.AUTOSAVEWARNING;
                }
                string text3 = string.Format(format, new object[]
                {
                    text2,
                    text,
                    gameInfo.baseName,
                    gameInfo.numberOfDuplicants.ToString(),
                    gameInfo.numberOfCycles.ToString()
                });
                this.saveDetails.text = text3;
                if (ModScreen.IsSaveFileFromUnsupportedFutureBuild(header))
                {
                    this.saveDetails.text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, filename, header.buildVersion, 260525u);
                    this.loadButton.isInteractable = false;
                    this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
                }
                else if (gameInfo.saveMajorVersion < 7)
                {
                    this.saveDetails.text = string.Format(UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, new object[]
                    {
                        filename,
                        gameInfo.saveMajorVersion,
                        gameInfo.saveMinorVersion,
                        7,
                        3
                    });
                    this.loadButton.isInteractable = false;
                    this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
                }
                else if (!this.loadButton.isInteractable)
                {
                    this.loadButton.isInteractable = true;
                    this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
                }
            }
            catch (Exception obj)
            {
                global::Debug.LogWarning(obj, null);
                this.saveDetails.text = string.Format(UI.FRONTEND.LOADSCREEN.CORRUPTEDSAVE, filename);
                if (this.loadButton.isInteractable)
                {
                    this.loadButton.isInteractable = false;
                    this.loadButton.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
                }
            }
        }
    }

    private void Load()
    {
        LoadingOverlay.Load(new System.Action(this.DoLoad));
    }

    private void DoLoad()
    {
        ReportErrorDialog.MOST_RECENT_SAVEFILE = this.selectedFileName;
        bool flag = true;
        SaveGame.Header header;
        SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(this.selectedFileName, out header);
        string arg = null;
        string arg2 = null;
        if (header.buildVersion > 260525u)
        {
            arg = header.buildVersion.ToString();
            arg2 = 260525u.ToString();
        }
        else if (gameInfo.saveMajorVersion < 7)
        {
            arg = string.Format("v{0}.{1}", gameInfo.saveMajorVersion, gameInfo.saveMinorVersion);
            arg2 = string.Format("v{0}.{1}", 7, 3);
        }
        if (!flag)
        {
            GameObject parent = (!(FrontEndManager.Instance == null)) ? FrontEndManager.Instance.gameObject : GameScreenManager.Instance.ssOverlayCanvas;
            ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>();
            component.PopupConfirmDialog(string.Format(UI.CRASHSCREEN.LOADFAILED, "Version Mismatch", arg, arg2), null, null, null, null, null, null, null);
        }
        else
        {
            if (Game.Instance != null)
            {
                ModScreen.ForceStopGame();
            }
            SaveLoader.SetActiveSaveFilePath(this.selectedFileName);
            Time.timeScale = 0f;
            App.LoadScene("backend");
            this.Deactivate();
        }
    }

    private void MoreInfo()
    {
        Application.OpenURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
    }

    private void Delete()
    {
        if (string.IsNullOrEmpty(this.selectedFileName))
        {
            global::Debug.LogError("The path provided is not valid and cannot be deleted.", null);
        }
        else
        {
            this.ConfirmDoAction(string.Format(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, Path.GetFileName(this.selectedFileName)), delegate
            {
                this.fileButtonMap[this.selectedFileName].GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
                this.fileButtonMap[this.selectedFileName].isInteractable = true;
                this.saveButtonPool.ClearElement(this.fileButtonMap[this.selectedFileName]);
                File.Delete(this.selectedFileName);
                this.selectedFileName = null;
                this.RefreshFiles();
            });
        }
    }

    private void ConfirmDoAction(string message, System.Action action)
    {
        if (this.confirmScreen == null)
        {
            this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
            this.confirmScreen.PopupConfirmDialog(message, action, delegate
            {
            }, null, null, null, null, null);
            this.confirmScreen.gameObject.SetActive(true);
        }
    }

    public override void OnKeyUp(KButtonEvent e)
    {
        if (e.TryConsume(global::Action.Escape))
        {
            this.Deactivate();
        }
        base.OnKeyUp(e);
    }
}
