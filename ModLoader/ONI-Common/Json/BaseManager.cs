namespace Common.Json
{
    public abstract class BaseManager
    {
        protected BaseManager(JsonManager manager, ONI_Common.IO.Logger logger = null)
        {
            this._logger = logger;
            this._manager = manager;
        }

        protected ONI_Common.IO.Logger _logger;
        protected JsonManager _manager;
    }
}
