using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInput
{
    public class MouseUser : MonoBehaviour
    {
        public enum MouseButton
        {
            Left, Right
        }

        public Vector2 MousePosition {  get; private set; }
        public Vector2 MouseInWorldPosition => Camera.main.ScreenToWorldPoint(MousePosition);

        private bool _isLeftMouseButtonPressed;
        private bool _isRightMouseButtonPressed;

        private void Update()
        {
            MousePosition = Input.mousePosition;
        }


        public bool IsMouseButtonPressed(MouseButton button)
        {
            return button == MouseButton.Left ? _isLeftMouseButtonPressed : _isRightMouseButtonPressed;
        }
    }
}
