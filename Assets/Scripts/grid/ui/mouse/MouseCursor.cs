using grid.utils;
using UnityEngine;

namespace grid.ui.mouse
{
    public class MouseCursor: Singleton<MouseCursor>
    {
        
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        
        private void Start()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        
        public void OnMouseEnter()
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }

        public void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
}