using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseDetector : MonoBehaviour
{
    public bool IsMouseOver {  get; private set; }


    public ObjectMenu _objectmenu;
    [SerializeField] private string WhatDoOnMouseClick = null;
    public bool IsSelected = false;
    private void Awake()
    {
        IsMouseOver = true;
        _objectmenu = GameObject.Find("Canvas/GameObjectEditor").GetComponent<ObjectMenu>();
    }
    private void OnMouseEnter()
    {
        IsMouseOver = true;
    }

    private void OnMouseExit()
    {
        IsMouseOver = false;
    }

    private void OnMouseDown()
    {
        if (WhatDoOnMouseClick == "SetSelectedGameObject")
        {
            _objectmenu.SetSelectedGameObject(this.gameObject.transform.parent.gameObject);
        }
        if (WhatDoOnMouseClick == "SetSelectedEntity")
        {
            _objectmenu.SetSelectedGameObject(this.gameObject.transform.parent.gameObject, true);
        }
    }
}
