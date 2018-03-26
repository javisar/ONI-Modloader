namespace DraggablePanelMod.Core
{
    using DraggablePanelMod.Data;

    public static class DraggableUI
    {
        private static DraggableUIState _uiState;

        public static DraggableUIState UIState => _uiState ?? (_uiState = new DraggableUIState());
    }
}