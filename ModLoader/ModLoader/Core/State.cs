using ONI_Common;
using ONI_Common.Data;

namespace Core
{
    public static class State
    {
        public static ONI_Common.IO.Logger Logger
        {
            get { return _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.CoreLogFileName)); }
        }

        private static ONI_Common.IO.Logger _logger;

        public static DraggableUIState UIState
            => _uiState ?? (_uiState = new DraggableUIState());

        private static DraggableUIState _uiState;
    }
}
