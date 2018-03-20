namespace ONI_Common.Json
{
    using ONI_Common.IO;

    public abstract class BaseManager
    {
        protected Logger _logger;

        protected JsonManager _manager;

        protected BaseManager(JsonManager manager, Logger logger = null)
        {
            this._logger  = logger;
            this._manager = manager;
        }
    }
}