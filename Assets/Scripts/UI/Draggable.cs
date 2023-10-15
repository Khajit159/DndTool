using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool CanMove = false;
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!CanMove) return;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    private void OnMouseDrag()
    {
        if (this.gameObject.GetComponent<MouseDetector>().IsSelected) { CanMove = true; }
    }

    private void OnMouseUp()
    {
        CanMove= false;
    }
}
