using ONI_Common.Data;

namespace ONI_Common.Core
{
    public static class State
    {


        public static DraggableUIState UIState
            => _uiState ?? (_uiState = new DraggableUIState());

        private static DraggableUIState _uiState;
    }
}
