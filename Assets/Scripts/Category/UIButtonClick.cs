using BuildingSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RightClickInfo _rightClickInfo;
    [SerializeField] BuildingSelector _buildingSelector;
    private bool IsOver = false;

    public void OnPointerEnter(PointerEventData data)
    {
        IsOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IsOver = false;
    }

    private void Update()
    {
        if (!IsOver) { return; }
        if (Input.GetMouseButton(0))
        {
            _buildingSelector.SetEntityToBuild(this.gameObject.name);
        }
        if (Input.GetMouseButtonDown(1))
        {
            EntityRightClick();
        }
    }

    private void Start()
    {
        _rightClickInfo = GameObject.FindGameObjectWithTag("RightClickInfo").GetComponent<RightClickInfo>();
        _buildingSelector = GameObject.Find("Player").gameObject.GetComponent<BuildingSelector>();
    }

    private void EntityRightClick()
    {
        _rightClickInfo.ShowMenu(new string[]{ "Delete"}, this.gameObject.name);
    }


}
