using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ObjectMenu _objectmenu;
    private MusicMenuHandler m_MenuHandler;
    private bool IsOver = false;
    [SerializeField] TypeClick WhatShouldDo;

    private enum TypeClick { Directory, File, SetSelectedGameObject, SetSelectedEntity }

    public void OnPointerEnter(PointerEventData data)
    {
        IsOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IsOver = false;
    }

    private void Start()
    {
        m_MenuHandler = GameObject.Find("Canvas/Sidebar/Content/ListContent/MusicContent").GetComponent<MusicMenuHandler>();
        //_objectmenu = GameObject.Find("Canvas/Sidebar/Content/ListContent/ObjectMenuContent").GetComponent<ObjectMenu>();
    }

    private void Update()
    {
        if (!IsOver) { return; }
        if (Input.GetMouseButtonUp(0))
        {
            if (WhatShouldDo == TypeClick.Directory)
            {
                m_MenuHandler.OpenDirectory(this.gameObject.name);
                Debug.Log("Opened Directory: " + this.gameObject.name);
            }
            else if (WhatShouldDo == TypeClick.File)
            {
                m_MenuHandler.LoadAndPlay(this.gameObject.name);
                Debug.Log("Playing: " + this.gameObject.name);
            }
            else if (WhatShouldDo == TypeClick.SetSelectedGameObject)
            {
                _objectmenu.SetSelectedGameObject(this.gameObject.transform.parent.gameObject);
            }
            else if (WhatShouldDo == TypeClick.SetSelectedEntity)
            {
                _objectmenu.SetSelectedGameObject(this.gameObject.transform.parent.gameObject, true);
            }

        }
    }
}
