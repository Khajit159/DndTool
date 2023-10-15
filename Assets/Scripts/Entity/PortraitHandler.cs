using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PortraitHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Portrait")]
    [SerializeField] GameObject Highlight;

    public void OnPointerEnter(PointerEventData data) { Highlight.SetActive(true);}
    public void OnPointerExit(PointerEventData data) { Highlight.SetActive(false);}
}
