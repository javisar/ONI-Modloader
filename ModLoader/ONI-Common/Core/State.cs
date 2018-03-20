namespace ONI_Common.Core
{
    using ONI_Common.Data;

    public static class DraggableUI
    {
        private static DraggableUIState _uiState;

        public static DraggableUIState UIState => _uiState ?? (_uiState = new DraggableUIState());
    }
}