using ONI_Common;
using ONI_Common.Data;

namespace Common.Json
{
    public class InjectorStateManager
    {
        public InjectorStateManager(JsonManager manager)
        {
            this._manager = manager;
        }

        private readonly JsonManager _manager;

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
