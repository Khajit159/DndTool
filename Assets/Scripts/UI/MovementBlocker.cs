using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] WhatToDo type;
    [SerializeField] CameraMovement _cammovement;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type == WhatToDo.BlockScrolling) { _cammovement.BlockScrolling = true; }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (type == WhatToDo.BlockScrolling) { _cammovement.BlockScrolling = false; }
    }
    private enum WhatToDo { BlockScrolling}
}
