namespace ONI_Common.Json
{
    using ONI_Common.Data;

    public class InjectorStateManager
    {
        private readonly JsonManager _manager;

        public InjectorStateManager(JsonManager manager)
        {
            this._manager = manager;
        }

        public InjectorState LoadState()
        {
            return this._manager.Deserialize<InjectorState>(Paths.InjectorStatePath);
        }

        public void SaveState(InjectorState state)
        {
            this._manager.Serialize(state, Paths.InjectorStatePath);
        }
    }
}