namespace DraggablePanelMod
{
    using Harmony;

    [HarmonyPatch(typeof(KScreen), "OnPrefabInit")]
    public static class DraggablePanelModInit
    {
        public static void Prefix(KScreen __instance)
        {
            DraggablePanel.Attach(__instance);
        }
    }

 //   // KScreen
 //   protected override void OnPrefabInit()
 //   {
 //   DraggablePanel.Attach(this);
 //   if (this.fadeIn)
 //   {
 //       this.InitWidgetTransition();
 //   }
 //   }


    [HarmonyPatch(typeof(KScreen), "OnSpawn")]
    public static class DraggablePanelModSpawn
    {
        public static void Postfix(KScreen __instance)
        {
            DraggablePanel.SetPositionFromFile(__instance);
        }
    }

//    // KScreen
//    protected override void OnSpawn()
//    {
//    base.OnSpawn();
//    this._canvas = base.GetComponentInParent<Canvas>();
//    if (this._canvas != null)
//    {
//    this._rectTransform = this._canvas.GetComponentInParent<RectTransform>();
//    if (this.activateOnSpawn && global::KScreenManager.Instance != null)
//    {
//    this.Activate();
//    if (this.ConsumeMouseScroll && !this.activateOnSpawn)
//    {
//    global::Debug.LogWarning("ConsumeMouseScroll is true on" + base.gameObject.name + " , but activateOnSpawn is disabled. Mouse scrolling might not work properly on this screen.", null);
//    }
//}
//}
//DraggablePanel.SetPositionFromFile(this);
//}

}
