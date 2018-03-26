namespace DraggablePanelMod
{
    using DraggablePanelMod.Core;

    using UnityEngine;

    /// <summary>
    /// The draggable panel.
    /// </summary>
    public class DraggablePanel : MonoBehaviour
    {
        public Vector3 Offset;

        // Use GetComponent<KScreen>() instead?
        public KScreen Screen;

        private bool _isDragging;

        public static void Attach(KScreen screen)
        {
            DraggablePanel panel = screen.FindOrAddUnityComponent<DraggablePanel>();

            if (panel == null)
            {
                return;
            }

            panel.Screen = screen;
        }

        // TODO: call when position is set by game
        public static void SetPositionFromFile(KScreen screen)
        {
            Vector2 newPosition;

            DraggablePanel panel = screen.FindOrAddUnityComponent<DraggablePanel>();

            if (panel != null && panel.LoadPosition(out newPosition))
            {
                panel.SetPosition(newPosition);
            }
        }

        // Todo: fix the mashup with injection, move to Harmony
        public void Update()
        {
            if (this.Screen == null)
            {
                return;
            }

            Vector3 mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (this.Screen.GetMouseOver)
                {
                    this.Offset = Input.mousePosition - this.Screen.transform.position;

                    this._isDragging = true;
                }
            }

            if (this._isDragging && Input.GetMouseButtonUp(0))
            {
                this._isDragging = false;

                this.SavePosition(mousePos - this.Offset);
            }

            if (!this._isDragging)
            {
                return;
            }

            Vector3 newPosition = mousePos - this.Offset;

            this.SetPosition(newPosition);
        }

        private bool LoadPosition(out Vector2 position)
        {
            return DraggableUI.UIState.LoadWindowPosition(this.gameObject, out position);
        }

        // TODO: queue save to file
        private void SavePosition(Vector2 position)
        {
            DraggableUI.UIState.SaveWindowPosition(this.gameObject, position);
        }

        // use offset?
        private void SetPosition(Vector3 newPosition)
        {
            if (this.Screen == null)
            {
                return;
            }

            this.Screen.transform.position = newPosition;
        }
    }
}