class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
{
    public SimViewMode simView;

    public string requiredTechItem;

    public OverlayToggleInfo(string text, string icon_name, SimViewMode sim_view, string required_tech_item = "", Action hotKey = Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hotKey, tooltip, tooltip_header)
    {
        this.simView          = sim_view;
        this.requiredTechItem = required_tech_item;
    }

    public bool IsUnlocked()
    {
        return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
    }
}
