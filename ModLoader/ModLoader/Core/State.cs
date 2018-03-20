namespace Core
{
    using ONI_Common;
    using ONI_Common.Data;
    using ONI_Common.IO;

    public static class State
    {
        private static Logger _logger;

        private static DraggableUIState _uiState;

        public static Logger Logger
        {
            get
            {
                return _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.CoreLogFileName));
            }
        }

        public static DraggableUIState UIState => _uiState ?? (_uiState = new DraggableUIState());
    }
}